using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace PixelEditor
{
    public enum PixelTool
    {
        Selection,
        Pen,
        MirrorPen,
        Eraser,
        PaintBucket,
        ColorPicker,
        Line,
        Curve,
        Rectangle,
        FilledRectangle,
        Circle,
        FilledCircle,
        LightenDarken
    }

    public enum EditorLanguage
    {
        Vietnamese,
        English
    }

    [System.Serializable]
    public class PixelLayer
    {
        public string name = "Layer 1";
        public bool isVisible = true;
        public bool isLocked = false;
        public float opacity = 1.0f;

        public PixelLayer(string name = "Layer 1")
        {
            this.name = name;
            this.isVisible = true;
            this.isLocked = false;
            this.opacity = 1.0f;
        }
    }

    [System.Serializable]
    public class LayerCel
    {
        public Color32[] pixels;
        public Texture2D thumbnail;

        public LayerCel(int width, int height, Color32[] initialPixels = null)
        {
            pixels = new Color32[width * height];
            if (initialPixels != null && initialPixels.Length == pixels.Length)
            {
                Array.Copy(initialPixels, pixels, pixels.Length);
            }
            else
            {
                for (int i = 0; i < pixels.Length; i++) pixels[i] = new Color32(0, 0, 0, 0);
            }
            UpdateThumbnail(width, height);
        }

        public bool HasContent()
        {
            if (pixels == null) return false;
            for (int i = 0; i < pixels.Length; i++)
            {
                if (pixels[i].a > 0) return true;
            }
            return false;
        }

        public void UpdateThumbnail(int width, int height)
        {
            if (thumbnail == null || thumbnail.width != width || thumbnail.height != height)
            {
                if (thumbnail != null) UnityEngine.Object.DestroyImmediate(thumbnail);
                thumbnail = new Texture2D(width, height, TextureFormat.RGBA32, false)
                {
                    filterMode = FilterMode.Point,
                    wrapMode = TextureWrapMode.Clamp
                };
            }
            thumbnail.SetPixels32(pixels);
            thumbnail.Apply();
        }

        public void Dispose()
        {
            if (thumbnail != null)
            {
                UnityEngine.Object.DestroyImmediate(thumbnail);
                thumbnail = null;
            }
        }
    }

    [System.Serializable]
    public class AnimationFrame
    {
        public List<LayerCel> cels = new List<LayerCel>();
        public Texture2D compositeThumbnail;
        public int durationMs = 125; // Default 125ms (~8 FPS)

        public AnimationFrame(int layerCount, int width, int height, int duration = 125)
        {
            durationMs = duration;
            cels = new List<LayerCel>();
            for (int i = 0; i < layerCount; i++)
            {
                cels.Add(new LayerCel(width, height));
            }
        }

        public void UpdateCompositeThumbnail(int width, int height, List<PixelLayer> layersList)
        {
            if (compositeThumbnail == null || compositeThumbnail.width != width || compositeThumbnail.height != height)
            {
                if (compositeThumbnail != null) UnityEngine.Object.DestroyImmediate(compositeThumbnail);
                compositeThumbnail = new Texture2D(width, height, TextureFormat.RGBA32, false)
                {
                    filterMode = FilterMode.Point,
                    wrapMode = TextureWrapMode.Clamp
                };
            }

            Color32[] comp = CompositePixels(width, height, layersList);
            compositeThumbnail.SetPixels32(comp);
            compositeThumbnail.Apply();
        }

        public Color32[] CompositePixels(int width, int height, List<PixelLayer> layersList)
        {
            Color32[] output = new Color32[width * height];
            for (int i = 0; i < output.Length; i++) output[i] = new Color32(0, 0, 0, 0);

            // Composite bottom to top (layer 0 is bottom, layer N-1 is top)
            for (int l = 0; l < layersList.Count; l++)
            {
                if (!layersList[l].isVisible) continue;
                if (l >= cels.Count) continue;

                var celPixels = cels[l].pixels;
                float layerOp = layersList[l].opacity;

                for (int i = 0; i < output.Length; i++)
                {
                    Color32 top = celPixels[i];
                    if (top.a == 0) continue;

                    float topA = (top.a / 255f) * layerOp;
                    if (topA <= 0f) continue;

                    Color32 bot = output[i];
                    float botA = bot.a / 255f;

                    float outA = topA + botA * (1f - topA);
                    if (outA > 0f)
                    {
                        float r = (top.r * topA + bot.r * botA * (1f - topA)) / outA;
                        float g = (top.g * topA + bot.g * botA * (1f - topA)) / outA;
                        float b = (top.b * topA + bot.b * botA * (1f - topA)) / outA;

                        output[i] = new Color32((byte)Mathf.Clamp(Mathf.RoundToInt(r), 0, 255),
                                                (byte)Mathf.Clamp(Mathf.RoundToInt(g), 0, 255),
                                                (byte)Mathf.Clamp(Mathf.RoundToInt(b), 0, 255),
                                                (byte)Mathf.Clamp(Mathf.RoundToInt(outA * 255f), 0, 255));
                    }
                }
            }
            return output;
        }

        public void Dispose()
        {
            if (compositeThumbnail != null)
            {
                UnityEngine.Object.DestroyImmediate(compositeThumbnail);
                compositeThumbnail = null;
            }
            foreach (var c in cels) c.Dispose();
        }
    }

    public class PixelArtEditorWindow : EditorWindow
    {
        [MenuItem("Tools/Pixel Art Editor (Piskel & Aseprite) %#p")]
        [MenuItem("Window/2D/Pixel Art Editor (Aseprite Style)")]
        public static void OpenWindow()
        {
            var window = GetWindow<PixelArtEditorWindow>("Pixel Art Editor");
            window.minSize = new Vector2(920, 640);
            window.Show();
        }

        // --- Preferences Keys ---
        private const string PREF_LANG_KEY = "PixelArtEditor_Lang";
        private const string PREF_LEFT_WIDTH = "PixelArtEditor_LeftWidth";
        private const string PREF_RIGHT_WIDTH = "PixelArtEditor_RightWidth";
        private const string PREF_TIMELINE_HEIGHT = "PixelArtEditor_TimelineHeight";

        private EditorLanguage currentLang = EditorLanguage.Vietnamese;

        // --- Canvas & Texture Data ---
        private int canvasWidth = 32;
        private int canvasHeight = 32;
        private Texture2D previewTexture;
        private Texture2D checkerTexture;
        private Texture2D onionPrevTexture;
        private Texture2D onionNextTexture;
        private Texture2D targetAsset;
        private string targetAssetPath = "";

        // --- Multi-Layer & Frame Animation System ---
        private readonly List<PixelLayer> layers = new List<PixelLayer>();
        private readonly List<AnimationFrame> frames = new List<AnimationFrame>();
        private int currentLayerIndex = 0;
        private int currentFrameIndex = 0;
        private bool isPlaying = false;
        private bool isLooping = true;
        private float globalFps = 8f;
        private double lastAnimTick = 0;
        private int playbackFrame = 0;

        // --- Onion Skinning (Aseprite Style) ---
        private bool onionSkinning = false;
        private float onionAlpha = 0.35f;

        // --- Layout & Panels ---
        private bool showTimeline = true;
        private float timelineHeight = 180f;
        private float leftPanelWidth = 190f;
        private float rightPanelWidth = 220f;
        private bool isResizingLeft = false;
        private bool isResizingRight = false;
        private bool isResizingTimeline = false;
        private const float MinPanelWidth = 140f;
        private const float MaxPanelWidth = 380f;
        private const float SplitterWidth = 5f;
        private Vector2 timelineScroll;

        // --- View / Pan & Zoom ---
        private Vector2 panOffset = Vector2.zero;
        private float zoomLevel = 16f;
        private bool isPanning = false;
        private Vector2 lastMousePanPos;
        private Rect canvasScreenRect;

        // --- Tool Settings ---
        private PixelTool currentTool = PixelTool.Pen;
        private int brushSize = 1;
        private Color primaryColor = Color.black;
        private Color secondaryColor = Color.white;
        private bool mirrorHorizontal = true;
        private bool mirrorVertical = false;
        private bool showGrid = true;
        private float lightenDarkenAmount = 0.1f;

        // --- Undo / Redo System ---
        private readonly Stack<Color32[]> undoStack = new Stack<Color32[]>();
        private readonly Stack<Color32[]> redoStack = new Stack<Color32[]>();
        private const int MaxUndoSteps = 50;

        // --- Drawing States ---
        private bool isDrawing = false;
        private Vector2Int lastDrawnPixel = new Vector2Int(-1, -1);
        private Vector2Int strokeStartPixel = new Vector2Int(-1, -1);
        private Vector2Int currentHoverPixel = new Vector2Int(-1, -1);

        // --- MS Paint Curve Tool State Machine ---
        private int curveStep = 0;
        private Vector2Int curveP0, curveP1, curveP2, curveP3;

        // --- Selection & 360° Transform System ---
        private enum SelectionDragMode
        {
            None,
            CreatingBox,
            MovingSelection,
            RotatingSelection
        }

        private bool hasSelection = false;
        private bool isFloatingSelection = false;
        private RectInt selectionBox = new RectInt(0, 0, 0, 0);
        private Color32[] floatingPixels = null;
        private Vector2 floatingOffset = Vector2.zero;
        private float selectionRotation = 0f; // 0° - 360°
        private SelectionDragMode selectionDragMode = SelectionDragMode.None;
        private Vector2Int selDragStartPixel = new Vector2Int(-1, -1);
        private Vector2 selDragStartOffset = Vector2.zero;
        private float selDragStartAngle = 0f;
        private float selDragStartRotation = 0f;

        // --- Clipboard (Copy / Cut / Paste) ---
        private static Color32[] clipboardPixels = null;
        private static int clipboardWidth = 0;
        private static int clipboardHeight = 0;

        // --- Color Palette ---
        private List<Color> customPalette = new List<Color>()
        {
            Color.black, Color.white, new Color(0.2f, 0.2f, 0.2f), new Color(0.5f, 0.5f, 0.5f),
            new Color(0.85f, 0.85f, 0.85f), new Color(0.85f, 0.15f, 0.15f), new Color(1f, 0.45f, 0.45f),
            new Color(1f, 0.6f, 0f), new Color(1f, 0.92f, 0.05f), new Color(0.2f, 0.8f, 0.2f),
            new Color(0.08f, 0.48f, 0.15f), new Color(0.1f, 0.8f, 1f), new Color(0.15f, 0.4f, 0.95f),
            new Color(0.55f, 0.15f, 0.85f), new Color(0.95f, 0.35f, 0.75f), new Color(0.55f, 0.3f, 0.1f)
        };

        // --- GUI Styles ---
        private GUIStyle headerStyle;
        private GUIStyle toolButtonStyle;
        private GUIStyle activeToolButtonStyle;
        private GUIStyle infoLabelStyle;
        private GUIStyle asepriteHeaderStyle;
        private GUIStyle asepriteActiveHeaderStyle;
        private Vector2 leftScroll;
        private Vector2 rightScroll;
        private int newWidthInput = 32;
        private int newHeightInput = 32;

        public Color32[] CurrentActiveCelPixels
        {
            get
            {
                if (layers.Count == 0 || frames.Count == 0)
                {
                    CreateNewCanvas(canvasWidth, canvasHeight);
                }
                currentFrameIndex = Mathf.Clamp(currentFrameIndex, 0, frames.Count - 1);
                currentLayerIndex = Mathf.Clamp(currentLayerIndex, 0, layers.Count - 1);

                var frame = frames[currentFrameIndex];
                while (frame.cels.Count <= currentLayerIndex)
                {
                    frame.cels.Add(new LayerCel(canvasWidth, canvasHeight));
                }

                return frame.cels[currentLayerIndex].pixels;
            }
        }

        private void OnEnable()
        {
            currentLang = (EditorLanguage)EditorPrefs.GetInt(PREF_LANG_KEY, (int)EditorLanguage.Vietnamese);
            leftPanelWidth = EditorPrefs.GetFloat(PREF_LEFT_WIDTH, 190f);
            rightPanelWidth = EditorPrefs.GetFloat(PREF_RIGHT_WIDTH, 220f);
            timelineHeight = Mathf.Max(EditorPrefs.GetFloat(PREF_TIMELINE_HEIGHT, 180f), 140f);
            showTimeline = true;

            InitCheckerTexture();
            if (layers.Count == 0 || frames.Count == 0)
            {
                CreateNewCanvas(32, 32);
            }
            else
            {
                RebuildPreviewTexture();
            }
            CenterCanvas();
        }

        private void OnDisable()
        {
            EditorPrefs.SetInt(PREF_LANG_KEY, (int)currentLang);
            EditorPrefs.SetFloat(PREF_LEFT_WIDTH, leftPanelWidth);
            EditorPrefs.SetFloat(PREF_RIGHT_WIDTH, rightPanelWidth);
            EditorPrefs.SetFloat(PREF_TIMELINE_HEIGHT, timelineHeight);

            if (previewTexture != null) DestroyImmediate(previewTexture);
            if (checkerTexture != null) DestroyImmediate(checkerTexture);
            if (onionPrevTexture != null) DestroyImmediate(onionPrevTexture);
            if (onionNextTexture != null) DestroyImmediate(onionNextTexture);

            foreach (var f in frames) f.Dispose();
        }

        private void Update()
        {
            if (isPlaying && frames.Count > 1)
            {
                double now = EditorApplication.timeSinceStartup;
                float interval = 1.0f / Mathf.Max(globalFps, 1f);

                if (now - lastAnimTick >= interval)
                {
                    lastAnimTick = now;
                    if (isLooping)
                    {
                        playbackFrame = (playbackFrame + 1) % frames.Count;
                    }
                    else
                    {
                        if (playbackFrame + 1 < frames.Count) playbackFrame++;
                        else isPlaying = false;
                    }
                    Repaint();
                }
            }
        }

        private void InitStyles()
        {
            if (headerStyle == null)
            {
                headerStyle = new GUIStyle(EditorStyles.boldLabel)
                {
                    fontSize = 11,
                    alignment = TextAnchor.MiddleLeft
                };
            }

            if (toolButtonStyle == null)
            {
                toolButtonStyle = new GUIStyle(GUI.skin.button)
                {
                    fontSize = 11,
                    fixedHeight = 28,
                    alignment = TextAnchor.MiddleLeft,
                    imagePosition = ImagePosition.ImageLeft,
                    padding = new RectOffset(6, 6, 2, 2)
                };
            }

            if (activeToolButtonStyle == null)
            {
                activeToolButtonStyle = new GUIStyle(GUI.skin.button)
                {
                    fontSize = 11,
                    fixedHeight = 28,
                    alignment = TextAnchor.MiddleLeft,
                    imagePosition = ImagePosition.ImageLeft,
                    fontStyle = FontStyle.Bold,
                    padding = new RectOffset(6, 6, 2, 2)
                };
                activeToolButtonStyle.normal.textColor = new Color(1f, 0.85f, 0.1f);
            }

            if (infoLabelStyle == null)
            {
                infoLabelStyle = new GUIStyle(EditorStyles.miniLabel)
                {
                    alignment = TextAnchor.MiddleLeft
                };
            }

            if (asepriteHeaderStyle == null)
            {
                asepriteHeaderStyle = new GUIStyle(GUI.skin.box)
                {
                    fontSize = 10,
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleCenter,
                    padding = new RectOffset(2, 2, 2, 2)
                };
            }

            if (asepriteActiveHeaderStyle == null)
            {
                asepriteActiveHeaderStyle = new GUIStyle(GUI.skin.box)
                {
                    fontSize = 10,
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleCenter,
                    padding = new RectOffset(2, 2, 2, 2)
                };
                asepriteActiveHeaderStyle.normal.textColor = new Color(1f, 0.85f, 0.2f);
            }
        }

        private void InitCheckerTexture()
        {
            if (checkerTexture != null) return;
            checkerTexture = new Texture2D(16, 16, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Repeat
            };

            Color32 c1 = new Color32(195, 195, 195, 255);
            Color32 c2 = new Color32(240, 240, 240, 255);
            Color32[] cols = new Color32[16 * 16];

            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    bool isEven = ((x / 8) + (y / 8)) % 2 == 0;
                    cols[y * 16 + x] = isEven ? c1 : c2;
                }
            }
            checkerTexture.SetPixels32(cols);
            checkerTexture.Apply();
        }

        private void CreateNewCanvas(int width, int height, bool fillTransparent = true)
        {
            width = Mathf.Clamp(width, 1, 512);
            height = Mathf.Clamp(height, 1, 512);

            canvasWidth = width;
            canvasHeight = height;
            newWidthInput = width;
            newHeightInput = height;

            foreach (var f in frames) f.Dispose();
            frames.Clear();
            layers.Clear();

            // Default Layer 1
            layers.Add(new PixelLayer("Layer 1"));
            currentLayerIndex = 0;

            var f1 = new AnimationFrame(1, width, height);
            if (!fillTransparent)
            {
                for (int i = 0; i < f1.cels[0].pixels.Length; i++) f1.cels[0].pixels[i] = new Color32(255, 255, 255, 255);
                f1.cels[0].UpdateThumbnail(width, height);
            }

            frames.Add(f1);
            currentFrameIndex = 0;
            playbackFrame = 0;
            curveStep = 0;

            hasSelection = false;
            isFloatingSelection = false;
            floatingPixels = null;
            floatingOffset = Vector2.zero;
            selectionRotation = 0f;
            selectionDragMode = SelectionDragMode.None;
            selectionBox = new RectInt(0, 0, 0, 0);

            undoStack.Clear();
            redoStack.Clear();

            RebuildPreviewTexture();
            CenterCanvas();
            Repaint();
        }

        private void RebuildPreviewTexture()
        {
            if (previewTexture == null || previewTexture.width != canvasWidth || previewTexture.height != canvasHeight)
            {
                if (previewTexture != null) DestroyImmediate(previewTexture);
                previewTexture = new Texture2D(canvasWidth, canvasHeight, TextureFormat.RGBA32, false)
                {
                    filterMode = FilterMode.Point,
                    wrapMode = TextureWrapMode.Clamp
                };
            }

            if (frames.Count > 0 && currentFrameIndex >= 0 && currentFrameIndex < frames.Count)
            {
                var curFrame = frames[currentFrameIndex];
                Color32[] comp = curFrame.CompositePixels(canvasWidth, canvasHeight, layers);

                // If floating selection is active, composite the transformed floating pixels on top
                if (hasSelection && isFloatingSelection && floatingPixels != null && selectionBox.width > 0 && selectionBox.height > 0)
                {
                    comp = CompositeFloatingSelection(comp);
                }

                previewTexture.SetPixels32(comp);
                previewTexture.Apply();

                // Update active cel and composite thumbnail
                if (currentLayerIndex >= 0 && currentLayerIndex < curFrame.cels.Count)
                {
                    curFrame.cels[currentLayerIndex].UpdateThumbnail(canvasWidth, canvasHeight);
                }
                curFrame.UpdateCompositeThumbnail(canvasWidth, canvasHeight, layers);
            }

            UpdateOnionSkinTextures();
        }

        private void UpdateOnionSkinTextures()
        {
            if (!onionSkinning || frames.Count <= 1) return;

            // Past frames (Blue tint)
            int pastIdx = (currentFrameIndex - 1 + frames.Count) % frames.Count;
            if (pastIdx != currentFrameIndex)
            {
                if (onionPrevTexture == null || onionPrevTexture.width != canvasWidth || onionPrevTexture.height != canvasHeight)
                {
                    if (onionPrevTexture != null) DestroyImmediate(onionPrevTexture);
                    onionPrevTexture = new Texture2D(canvasWidth, canvasHeight, TextureFormat.RGBA32, false)
                    {
                        filterMode = FilterMode.Point
                    };
                }
                var pBuf = frames[pastIdx].CompositePixels(canvasWidth, canvasHeight, layers);
                var tinted = new Color32[pBuf.Length];
                byte aByte = (byte)Mathf.Clamp(Mathf.RoundToInt(onionAlpha * 255f), 10, 255);
                for (int i = 0; i < pBuf.Length; i++)
                {
                    if (pBuf[i].a > 10)
                        tinted[i] = new Color32(60, 140, 255, aByte);
                    else
                        tinted[i] = new Color32(0, 0, 0, 0);
                }
                onionPrevTexture.SetPixels32(tinted);
                onionPrevTexture.Apply();
            }

            // Future frames (Red tint)
            int futureIdx = (currentFrameIndex + 1) % frames.Count;
            if (futureIdx != currentFrameIndex)
            {
                if (onionNextTexture == null || onionNextTexture.width != canvasWidth || onionNextTexture.height != canvasHeight)
                {
                    if (onionNextTexture != null) DestroyImmediate(onionNextTexture);
                    onionNextTexture = new Texture2D(canvasWidth, canvasHeight, TextureFormat.RGBA32, false)
                    {
                        filterMode = FilterMode.Point
                    };
                }
                var nBuf = frames[futureIdx].CompositePixels(canvasWidth, canvasHeight, layers);
                var tinted = new Color32[nBuf.Length];
                byte aByte = (byte)Mathf.Clamp(Mathf.RoundToInt(onionAlpha * 255f), 10, 255);
                for (int i = 0; i < nBuf.Length; i++)
                {
                    if (nBuf[i].a > 10)
                        tinted[i] = new Color32(255, 75, 60, aByte);
                    else
                        tinted[i] = new Color32(0, 0, 0, 0);
                }
                onionNextTexture.SetPixels32(tinted);
                onionNextTexture.Apply();
            }
        }

        private void CenterCanvas()
        {
            if (canvasScreenRect.width <= 0) return;
            float availableWidth = canvasScreenRect.width;
            float availableHeight = canvasScreenRect.height;

            float fitZoomX = (availableWidth * 0.85f) / canvasWidth;
            float fitZoomY = (availableHeight * 0.85f) / canvasHeight;
            zoomLevel = Mathf.Max(1f, Mathf.Min(fitZoomX, fitZoomY));

            float renderedW = canvasWidth * zoomLevel;
            float renderedH = canvasHeight * zoomLevel;

            panOffset = new Vector2(
                (availableWidth - renderedW) * 0.5f,
                (availableHeight - renderedH) * 0.5f
            );
        }

        #region Undo / Redo
        private void RecordUndo()
        {
            var target = CurrentActiveCelPixels;
            if (target == null) return;
            var copy = new Color32[target.Length];
            Array.Copy(target, copy, target.Length);
            undoStack.Push(copy);
            if (undoStack.Count > MaxUndoSteps)
            {
                var list = new List<Color32[]>(undoStack);
                list.RemoveAt(0);
                undoStack.Clear();
                for (int i = list.Count - 1; i >= 0; i--) undoStack.Push(list[i]);
            }
            redoStack.Clear();
        }

        private void PerformUndo()
        {
            if (undoStack.Count == 0) return;
            var target = CurrentActiveCelPixels;
            var currentCopy = new Color32[target.Length];
            Array.Copy(target, currentCopy, target.Length);
            redoStack.Push(currentCopy);

            var popped = undoStack.Pop();
            Array.Copy(popped, target, popped.Length);
            curveStep = 0;
            RebuildPreviewTexture();
            Repaint();
        }

        private void PerformRedo()
        {
            if (redoStack.Count == 0) return;
            var target = CurrentActiveCelPixels;
            var currentCopy = new Color32[target.Length];
            Array.Copy(target, currentCopy, target.Length);
            undoStack.Push(currentCopy);

            var popped = redoStack.Pop();
            Array.Copy(popped, target, popped.Length);
            curveStep = 0;
            RebuildPreviewTexture();
            Repaint();
        }
        #endregion

        #region Aseprite Multi-Layer Operations
        private void AddNewLayer(string customName = null)
        {
            string newLayerName = customName ?? $"Layer {layers.Count + 1}";
            layers.Add(new PixelLayer(newLayerName));
            int newLayerIdx = layers.Count - 1;

            // Add new cel to each frame for this layer
            foreach (var f in frames)
            {
                f.cels.Add(new LayerCel(canvasWidth, canvasHeight));
            }

            currentLayerIndex = newLayerIdx;
            undoStack.Clear();
            redoStack.Clear();
            RebuildPreviewTexture();
            Repaint();
        }

        private void DuplicateCurrentLayer()
        {
            if (layers.Count == 0) return;
            int srcIdx = currentLayerIndex;
            string newName = $"{layers[srcIdx].name} (Copy)";
            var newLayer = new PixelLayer(newName)
            {
                isVisible = layers[srcIdx].isVisible,
                isLocked = layers[srcIdx].isLocked,
                opacity = layers[srcIdx].opacity
            };
            layers.Insert(srcIdx + 1, newLayer);

            foreach (var f in frames)
            {
                var srcCel = f.cels[srcIdx];
                var dupCel = new LayerCel(canvasWidth, canvasHeight, srcCel.pixels);
                f.cels.Insert(srcIdx + 1, dupCel);
            }

            currentLayerIndex = srcIdx + 1;
            undoStack.Clear();
            redoStack.Clear();
            RebuildPreviewTexture();
            Repaint();
        }

        private void DeleteCurrentLayer()
        {
            if (layers.Count <= 1)
            {
                EditorUtility.DisplayDialog(Tr("Xóa Layer", "Delete Layer"),
                    Tr("Không thể xóa layer duy nhất còn lại!", "Cannot delete the only remaining layer!"), "OK");
                return;
            }

            int delIdx = currentLayerIndex;
            layers.RemoveAt(delIdx);

            foreach (var f in frames)
            {
                f.cels[delIdx].Dispose();
                f.cels.RemoveAt(delIdx);
            }

            if (currentLayerIndex >= layers.Count) currentLayerIndex = layers.Count - 1;
            undoStack.Clear();
            redoStack.Clear();
            RebuildPreviewTexture();
            Repaint();
        }

        private void MoveLayer(int delta)
        {
            int targetIdx = currentLayerIndex + delta;
            if (targetIdx < 0 || targetIdx >= layers.Count) return;

            var l = layers[currentLayerIndex];
            layers.RemoveAt(currentLayerIndex);
            layers.Insert(targetIdx, l);

            foreach (var f in frames)
            {
                var c = f.cels[currentLayerIndex];
                f.cels.RemoveAt(currentLayerIndex);
                f.cels.Insert(targetIdx, c);
            }

            currentLayerIndex = targetIdx;
            RebuildPreviewTexture();
            Repaint();
        }

        private void MergeLayerDown(int topLayerIdx)
        {
            if (topLayerIdx <= 0 || topLayerIdx >= layers.Count) return;
            int botLayerIdx = topLayerIdx - 1;

            foreach (var f in frames)
            {
                var topCel = f.cels[topLayerIdx];
                var botCel = f.cels[botLayerIdx];

                for (int i = 0; i < botCel.pixels.Length; i++)
                {
                    Color32 top = topCel.pixels[i];
                    if (top.a == 0) continue;

                    Color32 bot = botCel.pixels[i];
                    float topA = top.a / 255f;
                    float botA = bot.a / 255f;
                    float outA = topA + botA * (1f - topA);

                    if (outA > 0f)
                    {
                        float r = (top.r * topA + bot.r * botA * (1f - topA)) / outA;
                        float g = (top.g * topA + bot.g * botA * (1f - topA)) / outA;
                        float b = (top.b * topA + bot.b * botA * (1f - topA)) / outA;

                        botCel.pixels[i] = new Color32((byte)Mathf.Clamp(Mathf.RoundToInt(r), 0, 255),
                                                       (byte)Mathf.Clamp(Mathf.RoundToInt(g), 0, 255),
                                                       (byte)Mathf.Clamp(Mathf.RoundToInt(b), 0, 255),
                                                       (byte)Mathf.Clamp(Mathf.RoundToInt(outA * 255f), 0, 255));
                    }
                }
                botCel.UpdateThumbnail(canvasWidth, canvasHeight);

                topCel.Dispose();
                f.cels.RemoveAt(topLayerIdx);
            }

            layers.RemoveAt(topLayerIdx);
            currentLayerIndex = botLayerIdx;
            undoStack.Clear();
            redoStack.Clear();
            RebuildPreviewTexture();
            Repaint();
        }
        #endregion

        #region Aseprite Animation Frame Management
        private void AddNewEmptyFrame()
        {
            var newF = new AnimationFrame(layers.Count, canvasWidth, canvasHeight, frames[currentFrameIndex].durationMs);
            frames.Insert(currentFrameIndex + 1, newF);
            currentFrameIndex++;
            playbackFrame = currentFrameIndex;
            undoStack.Clear();
            redoStack.Clear();
            RebuildPreviewTexture();
            Repaint();
        }

        private void DuplicateFrame()
        {
            var newF = new AnimationFrame(layers.Count, canvasWidth, canvasHeight, frames[currentFrameIndex].durationMs);
            var srcFrame = frames[currentFrameIndex];

            for (int l = 0; l < layers.Count; l++)
            {
                if (l < srcFrame.cels.Count)
                {
                    Array.Copy(srcFrame.cels[l].pixels, newF.cels[l].pixels, newF.cels[l].pixels.Length);
                    newF.cels[l].UpdateThumbnail(canvasWidth, canvasHeight);
                }
            }

            frames.Insert(currentFrameIndex + 1, newF);
            currentFrameIndex++;
            playbackFrame = currentFrameIndex;
            undoStack.Clear();
            redoStack.Clear();
            RebuildPreviewTexture();
            Repaint();
        }

        private void DeleteCurrentFrame()
        {
            if (frames.Count <= 1)
            {
                EditorUtility.DisplayDialog(Tr("Xóa Frame", "Delete Frame"),
                    Tr("Không thể xóa frame duy nhất còn lại!", "Cannot delete the only remaining frame!"), "OK");
                return;
            }

            frames[currentFrameIndex].Dispose();
            frames.RemoveAt(currentFrameIndex);
            if (currentFrameIndex >= frames.Count) currentFrameIndex = frames.Count - 1;
            playbackFrame = currentFrameIndex;

            undoStack.Clear();
            redoStack.Clear();
            RebuildPreviewTexture();
            Repaint();
        }

        private void MoveFrame(int delta)
        {
            int targetIdx = currentFrameIndex + delta;
            if (targetIdx < 0 || targetIdx >= frames.Count) return;

            var f = frames[currentFrameIndex];
            frames.RemoveAt(currentFrameIndex);
            frames.Insert(targetIdx, f);
            currentFrameIndex = targetIdx;
            playbackFrame = targetIdx;
            RebuildPreviewTexture();
            Repaint();
        }

        private void SwitchToFrame(int idx)
        {
            if (idx < 0 || idx >= frames.Count || idx == currentFrameIndex) return;
            currentFrameIndex = idx;
            playbackFrame = idx;
            curveStep = 0;
            undoStack.Clear();
            redoStack.Clear();
            RebuildPreviewTexture();
            Repaint();
        }

        private void ReverseAllFrames()
        {
            if (frames.Count <= 1) return;
            frames.Reverse();
            currentFrameIndex = 0;
            playbackFrame = 0;
            RebuildPreviewTexture();
            Repaint();
        }
        #endregion

        #region Localization Helper
        private string Tr(string keyVi, string keyEn)
        {
            return currentLang == EditorLanguage.Vietnamese ? keyVi : keyEn;
        }

        private GUIContent GetToolContent(PixelTool tool)
        {
            Texture icon = null;
            string text = "";
            string tooltip = "";

            switch (tool)
            {
                case PixelTool.Selection:
                    icon = EditorGUIUtility.IconContent("RectTool On").image ?? EditorGUIUtility.IconContent("RectTool").image;
                    text = Tr("🔲 Vùng chọn (S)", "🔲 Selection (S)");
                    tooltip = Tr("Kéo để tạo vùng chọn hình chữ nhật.\n- Di chuyển: Kéo bên trong vùng chọn\n- Xoay 360°: Kéo nút xoay phía trên\n- Chốt (Commit): Enter\n- Bỏ chọn: Ctrl+D hoặc Esc\n- Chọn tất cả: Ctrl+A\nPhím tắt: S",
                                 "Drag to create rectangular selection.\n- Move: Drag inside selection\n- Rotate 360°: Drag rotation handle\n- Commit: Enter\n- Deselect: Ctrl+D or Esc\n- Select All: Ctrl+A\nShortcut: S");
                    break;
                case PixelTool.Pen:
                    icon = EditorGUIUtility.IconContent("d_EditCollider").image ?? EditorGUIUtility.IconContent("CustomTool").image;
                    text = Tr("✏️ Bút vẽ (P)", "✏️ Pencil (P)");
                    tooltip = Tr("Bút vẽ pixel tự do (Pencil Tool)\nPhím tắt: P\n- Chuột trái: Màu chính\n- Chuột phải: Màu phụ",
                                 "Freehand pixel pen tool.\nShortcut: P\n- Left Click: Primary Color\n- Right Click: Secondary Color");
                    break;
                case PixelTool.MirrorPen:
                    icon = EditorGUIUtility.IconContent("d_Mirror").image ?? EditorGUIUtility.IconContent("AvatarPivot").image;
                    text = Tr("🪞 Bút đối xứng (M)", "🪞 Symmetry Pen (M)");
                    tooltip = Tr("Vẽ đối xứng qua trục dọc hoặc ngang theo thời gian thực.\nPhím tắt: M",
                                 "Draw symmetry over vertical/horizontal axis.\nShortcut: M");
                    break;
                case PixelTool.Eraser:
                    icon = EditorGUIUtility.IconContent("Grid.EraserTool").image ?? EditorGUIUtility.IconContent("d_TreeEditor.Trash").image;
                    text = Tr("🧹 Cục tẩy (E)", "🧹 Eraser (E)");
                    tooltip = Tr("Tẩy pixel về trong suốt (Alpha = 0).\nPhím tắt: E",
                                 "Erase pixels to transparent.\nShortcut: E");
                    break;
                case PixelTool.PaintBucket:
                    icon = EditorGUIUtility.IconContent("Grid.PaintTool").image ?? EditorGUIUtility.IconContent("d_Color").image;
                    text = Tr("🪣 Đổ màu (B)", "🪣 Paint Bucket (B)");
                    tooltip = Tr("Đổ màu toàn bộ vùng cùng màu liền kề (Flood Fill).\nPhím tắt: B",
                                 "Fill contiguous area of same color.\nShortcut: B");
                    break;
                case PixelTool.ColorPicker:
                    icon = EditorGUIUtility.IconContent("EyeDropper.Large").image ?? EditorGUIUtility.IconContent("d_EyeDropper.Large").image;
                    text = Tr("🎯 Lấy màu (I)", "🎯 Eyedropper (I)");
                    tooltip = Tr("Hút màu từ pixel trên canvas.\nPhím tắt: I\n- Chuột trái: Gán màu chính\n- Chuột phải: Gán màu phụ",
                                 "Pick color from canvas pixel.\nShortcut: I");
                    break;
                case PixelTool.Line:
                    icon = EditorGUIUtility.IconContent("d_Slider Joint2D Icon").image ?? EditorGUIUtility.IconContent("animationvisibilitytoggleon").image;
                    text = Tr("📏 Đường thẳng (L)", "📏 Line Tool (L)");
                    tooltip = Tr("Vẽ đường thẳng pixel chính xác (Bresenham).\nPhím tắt: L",
                                 "Draw a straight pixel line.\nShortcut: L");
                    break;
                case PixelTool.Curve:
                    icon = EditorGUIUtility.IconContent("Animation.AddKeyframe").image ?? EditorGUIUtility.IconContent("d_Curve").image;
                    text = Tr("〰️ Đường cong MS (K)", "〰️ Curve Tool (K)");
                    tooltip = Tr("Vẽ nét cong Bezier 3 bước giống MS Paint:\n1. Kéo đường thẳng nối 2 đầu\n2. Nhấp/kéo chuột lần 1 để uốn cong vòng 1\n3. Nhấp/kéo chuột lần 2 để uốn lượn vòng 2 (chốt nét)\nPhím tắt: K",
                                 "MS Paint 3-step Bezier Curve:\n1. Drag baseline\n2. Click/drag to bend arch 1\n3. Click/drag to bend arch 2 & finalize\nShortcut: K");
                    break;
                case PixelTool.Rectangle:
                    icon = EditorGUIUtility.IconContent("RectTool").image ?? EditorGUIUtility.IconContent("d_RectTool").image;
                    text = Tr("🔲 Hình chữ nhật (R)", "🔲 Rectangle (R)");
                    tooltip = Tr("Vẽ khung hình chữ nhật rỗng viền pixel.\nPhím tắt: R",
                                 "Draw an outlined rectangle.\nShortcut: R");
                    break;
                case PixelTool.FilledRectangle:
                    icon = EditorGUIUtility.IconContent("RectTool").image ?? EditorGUIUtility.IconContent("d_RectTool").image;
                    text = Tr("⬛ Chữ nhật đặc", "⬛ Filled Rect");
                    tooltip = Tr("Vẽ hình chữ nhật tô đặc kín màu pixel.", "Draw a solid filled rectangle.");
                    break;
                case PixelTool.Circle:
                    icon = EditorGUIUtility.IconContent("d_WheelJoint2D Icon").image ?? EditorGUIUtility.IconContent("d_SphereCollider Icon").image;
                    text = Tr("⭕ Hình tròn (C)", "⭕ Circle (C)");
                    tooltip = Tr("Vẽ viền đường tròn pixel.\nPhím tắt: C", "Draw an outlined pixel circle.\nShortcut: C");
                    break;
                case PixelTool.FilledCircle:
                    icon = EditorGUIUtility.IconContent("d_WheelJoint2D Icon").image ?? EditorGUIUtility.IconContent("d_SphereCollider Icon").image;
                    text = Tr("🟢 Hình tròn đặc", "🟢 Filled Circle");
                    tooltip = Tr("Vẽ hình tròn tô đặc kín màu pixel.", "Draw a solid filled circle.");
                    break;
                case PixelTool.LightenDarken:
                    icon = EditorGUIUtility.IconContent("d_Light").image ?? EditorGUIUtility.IconContent("Light").image;
                    text = Tr("💡 Tăng/Giảm sáng (U)", "💡 Lighten/Dark (U)");
                    tooltip = Tr("Tăng hoặc giảm độ sáng của pixel (Dodge/Burn).\nPhím tắt: U\n- Chuột trái: Làm sáng\n- Chuột phải / Giữ Shift: Làm tối",
                                 "Adjust pixel brightness (Dodge/Burn).\nShortcut: U");
                    break;
            }

            return new GUIContent(text, icon, tooltip);
        }
        #endregion

        #region GUI Layout
        private void OnGUI()
        {
            InitStyles();
            HandleKeyboardShortcuts();
            HandleSplitterResizing();

            EditorGUILayout.BeginVertical();
            DrawTopToolbar();

            EditorGUILayout.BeginHorizontal();

            // 1. Left Tool Panel (Resizable)
            DrawLeftToolPanel();

            // Splitter 1 (Left)
            DrawSplitter(ref isResizingLeft, true);

            // 2. Center Column: Canvas (Top) + Aseprite Timeline & Layers (Bottom)
            DrawCenterColumn();

            // Splitter 2 (Right)
            DrawSplitter(ref isResizingRight, false);

            // 3. Right Panel (Preview & Transforms)
            DrawRightPanel();

            EditorGUILayout.EndHorizontal();

            DrawBottomStatusBar();
            EditorGUILayout.EndVertical();
        }

        private void DrawSplitter(ref bool isResizing, bool isLeft)
        {
            Rect splitterRect = GUILayoutUtility.GetRect(SplitterWidth, Screen.height, GUILayout.Width(SplitterWidth), GUILayout.ExpandHeight(true));
            EditorGUI.DrawRect(splitterRect, new Color(0.12f, 0.12f, 0.12f, 0.8f));
            EditorGUIUtility.AddCursorRect(splitterRect, MouseCursor.ResizeHorizontal);

            Event e = Event.current;
            if (e.type == EventType.MouseDown && splitterRect.Contains(e.mousePosition) && e.button == 0)
            {
                isResizing = true;
                e.Use();
            }
        }

        private void DrawTimelineSplitter()
        {
            Rect splitterRect = GUILayoutUtility.GetRect(Screen.width, 5f, GUILayout.ExpandWidth(true), GUILayout.Height(5f));
            EditorGUI.DrawRect(splitterRect, new Color(0.1f, 0.11f, 0.13f, 1f));
            EditorGUIUtility.AddCursorRect(splitterRect, MouseCursor.ResizeVertical);

            Event e = Event.current;
            if (e.type == EventType.MouseDown && splitterRect.Contains(e.mousePosition) && e.button == 0)
            {
                isResizingTimeline = true;
                e.Use();
            }
        }

        private void HandleSplitterResizing()
        {
            Event e = Event.current;
            if (e.type == EventType.MouseUp)
            {
                if (isResizingLeft || isResizingRight || isResizingTimeline)
                {
                    isResizingLeft = false;
                    isResizingRight = false;
                    isResizingTimeline = false;
                    Repaint();
                }
            }
            else if (e.type == EventType.MouseDrag)
            {
                if (isResizingLeft)
                {
                    leftPanelWidth = Mathf.Clamp(e.mousePosition.x, MinPanelWidth, MaxPanelWidth);
                    Repaint();
                }
                else if (isResizingRight)
                {
                    float rightDist = position.width - e.mousePosition.x;
                    rightPanelWidth = Mathf.Clamp(rightDist, MinPanelWidth, MaxPanelWidth);
                    Repaint();
                }
                else if (isResizingTimeline)
                {
                    float bottomDist = position.height - e.mousePosition.y - 25f;
                    timelineHeight = Mathf.Clamp(bottomDist, 120f, 450f);
                    Repaint();
                }
            }
        }

        private void DrawTopToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.ExpandWidth(true));

            // Language Toggle Dropdown
            var langIcon = EditorGUIUtility.IconContent("d_Localization").image ?? EditorGUIUtility.IconContent("d_FilterByType").image;
            GUIContent langContent = new GUIContent($" 🌐 {(currentLang == EditorLanguage.Vietnamese ? "VN" : "EN")}", langIcon, Tr("Chuyển đổi ngôn ngữ Tiếng Việt / English", "Switch language Vietnamese / English"));
            if (GUILayout.Button(langContent, EditorStyles.toolbarDropDown, GUILayout.Width(68)))
            {
                GenericMenu langMenu = new GenericMenu();
                langMenu.AddItem(new GUIContent("🇻🇳 Tiếng Việt"), currentLang == EditorLanguage.Vietnamese, () => { currentLang = EditorLanguage.Vietnamese; EditorPrefs.SetInt(PREF_LANG_KEY, (int)currentLang); });
                langMenu.AddItem(new GUIContent("🇬🇧 English"), currentLang == EditorLanguage.English, () => { currentLang = EditorLanguage.English; EditorPrefs.SetInt(PREF_LANG_KEY, (int)currentLang); });
                langMenu.ShowAsContext();
            }

            GUILayout.Space(6);

            // New Canvas
            GUIContent newBtnContent = new GUIContent(Tr("Tạo mới", "New Canvas"), EditorGUIUtility.IconContent("CreateAddNew").image, Tr("Tạo canvas mới với kích thước bên cạnh", "Create a new blank canvas with specified size"));
            if (GUILayout.Button(newBtnContent, EditorStyles.toolbarButton, GUILayout.Width(78)))
            {
                if (EditorUtility.DisplayDialog(Tr("Tạo Canvas Mới", "New Canvas"),
                    Tr($"Tạo canvas mới kích thước {newWidthInput}x{newHeightInput}?", $"Create new {newWidthInput}x{newHeightInput} canvas?"),
                    Tr("Tạo", "Create"), Tr("Hủy", "Cancel")))
                {
                    CreateNewCanvas(newWidthInput, newHeightInput);
                    targetAsset = null;
                    targetAssetPath = "";
                }
            }

            newWidthInput = EditorGUILayout.IntField(newWidthInput, EditorStyles.toolbarTextField, GUILayout.Width(35));
            GUILayout.Label("x", EditorStyles.miniLabel, GUILayout.Width(10));
            newHeightInput = EditorGUILayout.IntField(newHeightInput, EditorStyles.toolbarTextField, GUILayout.Width(35));

            GUILayout.Space(6);
            if (GUILayout.Button("16", EditorStyles.toolbarButton, GUILayout.Width(26))) { newWidthInput = 16; newHeightInput = 16; CreateNewCanvas(16, 16); }
            if (GUILayout.Button("32", EditorStyles.toolbarButton, GUILayout.Width(26))) { newWidthInput = 32; newHeightInput = 32; CreateNewCanvas(32, 32); }
            if (GUILayout.Button("64", EditorStyles.toolbarButton, GUILayout.Width(26))) { newWidthInput = 64; newHeightInput = 64; CreateNewCanvas(64, 64); }

            GUILayout.Space(8);
            EditorGUI.BeginDisabledGroup(undoStack.Count == 0);
            GUIContent undoContent = new GUIContent($"Undo ({undoStack.Count})", EditorGUIUtility.IconContent("d_Undo").image, Tr("Hoàn tác (Ctrl+Z)", "Undo (Ctrl+Z)"));
            if (GUILayout.Button(undoContent, EditorStyles.toolbarButton, GUILayout.Width(72))) PerformUndo();
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(redoStack.Count == 0);
            GUIContent redoContent = new GUIContent($"Redo ({redoStack.Count})", EditorGUIUtility.IconContent("d_Redo").image, Tr("Làm lại (Ctrl+Y)", "Redo (Ctrl+Y)"));
            if (GUILayout.Button(redoContent, EditorStyles.toolbarButton, GUILayout.Width(72))) PerformRedo();
            EditorGUI.EndDisabledGroup();

            GUILayout.Space(6);
            showGrid = GUILayout.Toggle(showGrid, Tr("Lưới", "Grid"), EditorStyles.toolbarButton, GUILayout.Width(42));

            // Timeline Toggle
            GUI.backgroundColor = showTimeline ? new Color(0.35f, 0.85f, 1f) : Color.white;
            GUIContent animToggleContent = new GUIContent(Tr($"🎞️ Timeline ({frames.Count}f, {layers.Count}L)", $"🎞️ Timeline ({frames.Count}f, {layers.Count}L)"), Tr("Bật/Tắt thanh Aseprite Timeline", "Toggle Aseprite Timeline"));
            if (GUILayout.Button(animToggleContent, EditorStyles.toolbarButton, GUILayout.Width(130)))
            {
                showTimeline = !showTimeline;
            }
            GUI.backgroundColor = Color.white;

            GUILayout.FlexibleSpace();

            // Texture asset binding & Load/Save
            GUILayout.Label(Tr("Asset:", "Asset:"), EditorStyles.miniLabel, GUILayout.Width(38));
            var prevAsset = targetAsset;
            targetAsset = (Texture2D)EditorGUILayout.ObjectField(targetAsset, typeof(Texture2D), false, GUILayout.Width(100));
            if (targetAsset != prevAsset && targetAsset != null)
            {
                LoadFromTexture(targetAsset);
            }

            GUIContent loadContent = new GUIContent(Tr("Tải Asset Đang Chọn", "Load Selection"), EditorGUIUtility.IconContent("d_Project").image, Tr("Đọc ảnh từ Texture/Sprite đang được chọn trong Project", "Load image from currently selected Texture/Sprite"));
            if (GUILayout.Button(loadContent, EditorStyles.toolbarButton, GUILayout.Width(125)))
            {
                if (Selection.activeObject is Texture2D tex)
                {
                    targetAsset = tex;
                    LoadFromTexture(tex);
                }
                else if (Selection.activeObject is Sprite sp)
                {
                    targetAsset = sp.texture;
                    LoadFromTexture(sp.texture);
                }
                else
                {
                    EditorUtility.DisplayDialog(Tr("Pixel Editor", "Pixel Editor"),
                        Tr("Vui lòng chọn một Texture2D hoặc Sprite trong cửa sổ Project.", "Please select a Texture2D or Sprite in the Project window."), "OK");
                }
            }

            GUI.backgroundColor = new Color(0.55f, 1f, 0.55f);
            GUIContent saveOverContent = new GUIContent(Tr("Lưu Đè", "Save Overwrite"), EditorGUIUtility.IconContent("d_SaveAs").image, Tr("Lưu đè file Texture gốc (Ctrl+S)", "Overwrite current texture file directly (Ctrl+S)"));
            if (GUILayout.Button(saveOverContent, EditorStyles.toolbarButton, GUILayout.Width(80)))
            {
                SaveOverwrite();
            }
            GUI.backgroundColor = Color.white;

            GUIContent saveAsContent = new GUIContent(Tr("Lưu Mới...", "Save As..."), EditorGUIUtility.IconContent("d_Prefab Icon").image, Tr("Lưu thành file ảnh PNG mới vào Project", "Save as a new PNG asset file into Project"));
            if (GUILayout.Button(saveAsContent, EditorStyles.toolbarButton, GUILayout.Width(75)))
            {
                SaveAsNewAsset();
            }

            GUIContent exportSheetContent = new GUIContent(Tr("Xuất Sheet", "Export Sheet"), EditorGUIUtility.IconContent("d_TextureImporter Icon").image, Tr("Xuất toàn bộ các animation frames thành Sprite Sheet PNG", "Export all animation frames into a single Sprite Sheet PNG"));
            if (GUILayout.Button(exportSheetContent, EditorStyles.toolbarButton, GUILayout.Width(95)))
            {
                ExportSpriteSheet();
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawLeftToolPanel()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(leftPanelWidth), GUILayout.ExpandHeight(true));
            leftScroll = EditorGUILayout.BeginScrollView(leftScroll);

            GUILayout.Label(Tr("CÔNG CỤ (TOOLS)", "TOOLS"), headerStyle);
            EditorGUILayout.Space(2);

            DrawToolButton(PixelTool.Selection);
            EditorGUILayout.Space(2);
            DrawToolButton(PixelTool.Pen);
            DrawToolButton(PixelTool.MirrorPen);
            DrawToolButton(PixelTool.Eraser);
            DrawToolButton(PixelTool.PaintBucket);
            DrawToolButton(PixelTool.ColorPicker);
            DrawToolButton(PixelTool.Line);
            DrawToolButton(PixelTool.Curve);
            DrawToolButton(PixelTool.Rectangle);
            DrawToolButton(PixelTool.FilledRectangle);
            DrawToolButton(PixelTool.Circle);
            DrawToolButton(PixelTool.FilledCircle);
            DrawToolButton(PixelTool.LightenDarken);

            EditorGUILayout.Space(6);
            GUILayout.Label(Tr("KÍCH THƯỚC CỌ", "BRUSH SIZE"), headerStyle);
            brushSize = EditorGUILayout.IntSlider(brushSize, 1, 16);

            if (currentTool == PixelTool.Selection)
            {
                EditorGUILayout.Space(4);
                GUILayout.Label(Tr("VÙNG CHỌN (SELECTION)", "SELECTION"), headerStyle);

                // Clipboard operations
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(!hasSelection);
                if (GUILayout.Button(new GUIContent(Tr("📋 Copy", "📋 Copy"), "Ctrl+C"), EditorStyles.miniButton))
                {
                    CopySelection();
                }
                if (GUILayout.Button(new GUIContent(Tr("✂️ Cắt", "✂️ Cut"), "Ctrl+X"), EditorStyles.miniButton))
                {
                    CutSelection();
                }
                EditorGUI.EndDisabledGroup();

                EditorGUI.BeginDisabledGroup(clipboardPixels == null || clipboardPixels.Length == 0);
                if (GUILayout.Button(new GUIContent(Tr("📥 Dán", "📥 Paste"), "Ctrl+V"), EditorStyles.miniButton))
                {
                    PasteSelection();
                }
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();

                EditorGUI.BeginDisabledGroup(!hasSelection);
                if (GUILayout.Button(new GUIContent(Tr("📑 Nhân bản (Ctrl+J)", "📑 Duplicate (Ctrl+J)"), Tr("Tạo bản sao ngay lập tức", "Duplicate selection immediately")), EditorStyles.miniButton))
                {
                    DuplicateSelection();
                }
                EditorGUI.EndDisabledGroup();

                EditorGUILayout.Space(2);

                if (hasSelection && isFloatingSelection)
                {
                    EditorGUILayout.HelpBox(Tr(
                        $"Vùng chọn đang nổi: {selectionBox.width}x{selectionBox.height}px\nGóc xoay: {selectionRotation:F1}° | Dịch: ({floatingOffset.x:F0}, {floatingOffset.y:F0})",
                        $"Floating Selection: {selectionBox.width}x{selectionBox.height}px\nRotation: {selectionRotation:F1}° | Offset: ({floatingOffset.x:F0}, {floatingOffset.y:F0})"), MessageType.Info);

                    GUILayout.Label(Tr("Góc xoay 360°", "Rotation 360°"), EditorStyles.miniBoldLabel);
                    float prevRot = selectionRotation;
                    selectionRotation = EditorGUILayout.Slider(selectionRotation, 0f, 360f);
                    if (Mathf.Abs(prevRot - selectionRotation) > 0.01f) RebuildPreviewTexture();

                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("-90°", EditorStyles.miniButton)) { RotateSelectionBy(-90f); }
                    if (GUILayout.Button("+90°", EditorStyles.miniButton)) { RotateSelectionBy(90f); }
                    if (GUILayout.Button("+45°", EditorStyles.miniButton)) { RotateSelectionBy(45f); }
                    if (GUILayout.Button("180°", EditorStyles.miniButton)) { RotateSelectionBy(180f); }
                    if (GUILayout.Button("0°", EditorStyles.miniButton)) { selectionRotation = 0f; RebuildPreviewTexture(); Repaint(); }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space(2);
                    GUILayout.Label(Tr("Dịch chuyển Pixel", "Nudge Position"), EditorStyles.miniBoldLabel);
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("←", EditorStyles.miniButton)) { ShiftFloatingSelection(-1, 0); }
                    if (GUILayout.Button("→", EditorStyles.miniButton)) { ShiftFloatingSelection(1, 0); }
                    if (GUILayout.Button("↑", EditorStyles.miniButton)) { ShiftFloatingSelection(0, 1); }
                    if (GUILayout.Button("↓", EditorStyles.miniButton)) { ShiftFloatingSelection(0, -1); }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space(4);
                    GUI.backgroundColor = new Color(0.4f, 1f, 0.5f);
                    if (GUILayout.Button(Tr("✅ Chốt (Enter)", "✅ Commit (Enter)"), EditorStyles.miniButton))
                    {
                        CommitFloatingSelection();
                    }
                    GUI.backgroundColor = Color.white;

                    GUI.backgroundColor = new Color(1f, 0.5f, 0.5f);
                    if (GUILayout.Button(Tr("❌ Hủy (Esc)", "❌ Cancel (Esc)"), EditorStyles.miniButton))
                    {
                        CancelSelection();
                    }
                    GUI.backgroundColor = Color.white;

                    if (GUILayout.Button(Tr("🗑 Xóa vùng chọn (Delete)", "🗑 Delete Selection (Del)"), EditorStyles.miniButton))
                    {
                        DeleteSelectionContent();
                    }
                }
                else if (hasSelection && !isFloatingSelection)
                {
                    EditorGUILayout.HelpBox(Tr(
                        $"Đã chọn: {selectionBox.width}x{selectionBox.height}px\n- Kéo núm xanh lá phía trên để xoay\n- Kéo tâm tròn giữa hoặc giữ Ctrl để di chuyển",
                        $"Selected: {selectionBox.width}x{selectionBox.height}px\n- Drag green top knob to rotate\n- Drag center handle or hold Ctrl to move"), MessageType.Info);

                    if (GUILayout.Button(Tr("✋ Nâng pixel để Di chuyển/Xoay", "✋ Lift Pixels to Move/Rotate"), EditorStyles.miniButton))
                    {
                        LiftSelectionPixels();
                    }

                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button(Tr("❌ Bỏ chọn (Ctrl+D)", "❌ Deselect (Ctrl+D)"), EditorStyles.miniButton))
                    {
                        ClearSelectionState();
                        Repaint();
                    }
                    if (GUILayout.Button(Tr("🗑 Xóa (Del)", "🗑 Del"), EditorStyles.miniButton, GUILayout.Width(60)))
                    {
                        DeleteSelectionContent();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    EditorGUILayout.HelpBox(Tr(
                        "Kéo chuột trên canvas để tạo vùng chọn.\nNhấp 1 điểm để bỏ chọn.",
                        "Drag on canvas to select area.\nSingle click to deselect."), MessageType.Info);
                }

                EditorGUILayout.Space(2);
                if (GUILayout.Button(Tr("Ctrl+A: Chọn tất cả", "Ctrl+A: Select All"), EditorStyles.miniButton))
                {
                    SelectAllPixels();
                }
            }

            if (currentTool == PixelTool.MirrorPen)
            {
                EditorGUILayout.Space(4);
                GUILayout.Label(Tr("Trục đối xứng", "Symmetry Axis"), EditorStyles.miniBoldLabel);
                mirrorHorizontal = EditorGUILayout.Toggle(Tr("Ngang (Horiz)", "Horizontal"), mirrorHorizontal);
                mirrorVertical = EditorGUILayout.Toggle(Tr("Dọc (Vert)", "Vertical"), mirrorVertical);
            }

            if (currentTool == PixelTool.Curve)
            {
                EditorGUILayout.Space(4);
                string stepDesc = curveStep == 0
                    ? Tr("Bước 1: Kéo đường thẳng nối 2 đầu", "Step 1: Drag baseline")
                    : (curveStep == 1
                        ? Tr("Bước 2: Kéo uốn cong vòng 1", "Step 2: Drag 1st bend arch")
                        : Tr("Bước 3: Kéo uốn cong vòng 2 (chốt)", "Step 3: Drag 2nd bend arch"));
                EditorGUILayout.HelpBox($"〰️ {stepDesc}\n({Tr("Nhấn Esc hoặc chuột phải để hủy", "Press Esc or Right-click to cancel")})", MessageType.Info);
            }

            if (currentTool == PixelTool.LightenDarken)
            {
                EditorGUILayout.Space(4);
                GUILayout.Label(Tr("Độ sáng/tối", "Adjust Step"), EditorStyles.miniBoldLabel);
                lightenDarkenAmount = EditorGUILayout.Slider(lightenDarkenAmount, 0.02f, 0.5f);
                EditorGUILayout.HelpBox(Tr("Chuột trái: Tăng sáng\nChuột phải / Shift: Giảm sáng", "Left Click: Lighten\nRight Click / Shift: Darken"), MessageType.None);
            }

            EditorGUILayout.Space(8);
            GUILayout.Label(Tr("MÀU SẮC (COLORS)", "COLORS"), headerStyle);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(Tr("Chính:", "Pri:"), GUILayout.Width(45));
            primaryColor = EditorGUILayout.ColorField(GUIContent.none, primaryColor, false, true, false, GUILayout.Height(22));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(Tr("Phụ:", "Sec:"), GUILayout.Width(45));
            secondaryColor = EditorGUILayout.ColorField(GUIContent.none, secondaryColor, false, true, false, GUILayout.Height(22));
            EditorGUILayout.EndHorizontal();

            GUIContent swapContent = new GUIContent(Tr("Hoán Đổi Màu (X)", "Swap Colors (X)"), Tr("Đổi chỗ màu chính và màu phụ (Phím X)", "Swap primary and secondary color (Key X)"));
            if (GUILayout.Button(swapContent, EditorStyles.miniButton))
            {
                SwapColors();
            }

            EditorGUILayout.Space(6);
            GUILayout.Label(Tr("BẢNG MÀU MẪU", "COLOR PALETTE"), headerStyle);

            DrawUniformPaletteSwatches();

            EditorGUILayout.Space(4);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(Tr("+ Thêm màu", "+ Add Color"), EditorStyles.miniButton, GUILayout.ExpandWidth(true)))
            {
                if (!customPalette.Contains(primaryColor)) customPalette.Add(primaryColor);
            }
            if (GUILayout.Button(Tr("Đặt lại", "Reset"), EditorStyles.miniButton, GUILayout.Width(45)))
            {
                ResetDefaultPalette();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(2);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(Tr("🎨 Lấy màu Sprite (+)", "🎨 Get Sprite Colors (+)"), EditorStyles.miniButton, GUILayout.ExpandWidth(true)))
            {
                ExtractPaletteFromCanvas(false);
            }
            if (GUILayout.Button(Tr("🔄 Thay thế", "🔄 Replace"), EditorStyles.miniButton, GUILayout.Width(75)))
            {
                ExtractPaletteFromCanvas(true);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void DrawToolButton(PixelTool tool)
        {
            bool isSelected = currentTool == tool;
            var style = isSelected ? activeToolButtonStyle : toolButtonStyle;
            var content = GetToolContent(tool);

            if (GUILayout.Button(content, style))
            {
                if (currentTool == PixelTool.Selection && tool != PixelTool.Selection && hasSelection && isFloatingSelection)
                {
                    CommitFloatingSelection();
                }
                currentTool = tool;
                curveStep = 0;
            }
        }

        private void DrawUniformPaletteSwatches()
        {
            const float swatchSize = 22f;
            const float spacing = 4f;

            float availableWidth = Mathf.Max(leftPanelWidth - 28f, swatchSize);
            int cols = Mathf.Max(1, Mathf.FloorToInt((availableWidth + spacing) / (swatchSize + spacing)));
            int count = customPalette.Count;
            int rows = Mathf.CeilToInt((float)count / cols);

            for (int r = 0; r < rows; r++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int c = 0; c < cols; c++)
                {
                    int index = r * cols + c;
                    if (index < count)
                    {
                        var col = customPalette[index];
                        Rect rRect = GUILayoutUtility.GetRect(swatchSize, swatchSize, GUILayout.Width(swatchSize), GUILayout.Height(swatchSize));
                        EditorGUI.DrawRect(rRect, col);

                        Handles.DrawSolidRectangleWithOutline(rRect, Color.clear, new Color(0.15f, 0.15f, 0.15f, 0.9f));

                        if (ColorsMatch(col, primaryColor))
                        {
                            Handles.DrawSolidRectangleWithOutline(rRect, Color.clear, Color.yellow);
                        }

                        if (Event.current.type == EventType.MouseDown && rRect.Contains(Event.current.mousePosition))
                        {
                            if (Event.current.button == 0)
                            {
                                primaryColor = col;
                            }
                            else if (Event.current.button == 1)
                            {
                                GenericMenu swatchMenu = new GenericMenu();
                                swatchMenu.AddItem(new GUIContent(Tr("Chọn làm màu phụ", "Set as Secondary Color")), false, () => { secondaryColor = col; Repaint(); });
                                swatchMenu.AddItem(new GUIContent(Tr("Xóa màu này khỏi bảng", "Remove from Palette")), false, () => { customPalette.RemoveAt(index); Repaint(); });
                                swatchMenu.ShowAsContext();
                            }
                            Event.current.Use();
                            Repaint();
                        }
                    }
                    else
                    {
                        GUILayout.Space(swatchSize);
                    }
                    GUILayout.Space(spacing);
                }
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(spacing);
            }
        }

        private bool ColorsMatch(Color a, Color b)
        {
            return Mathf.Abs(a.r - b.r) < 0.01f && Mathf.Abs(a.g - b.g) < 0.01f && Mathf.Abs(a.b - b.b) < 0.01f && Mathf.Abs(a.a - b.a) < 0.01f;
        }

        private void ResetDefaultPalette()
        {
            customPalette = new List<Color>()
            {
                Color.black, Color.white, new Color(0.2f, 0.2f, 0.2f), new Color(0.5f, 0.5f, 0.5f),
                new Color(0.85f, 0.85f, 0.85f), new Color(0.85f, 0.15f, 0.15f), new Color(1f, 0.45f, 0.45f),
                new Color(1f, 0.6f, 0f), new Color(1f, 0.92f, 0.05f), new Color(0.2f, 0.8f, 0.2f),
                new Color(0.08f, 0.48f, 0.15f), new Color(0.1f, 0.8f, 1f), new Color(0.15f, 0.4f, 0.95f),
                new Color(0.55f, 0.15f, 0.85f), new Color(0.95f, 0.35f, 0.75f), new Color(0.55f, 0.3f, 0.1f)
            };
        }

        private void DrawCenterColumn()
        {
            float totalCenterHeight = position.height - 22f /* top toolbar */ - 22f /* bottom status bar */;
            float currentTimelineH = showTimeline ? Mathf.Clamp(timelineHeight, 130f, totalCenterHeight - 120f) : 0f;
            float canvasAllocatedH = totalCenterHeight - currentTimelineH - (showTimeline ? 5f : 0f);

            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.Height(totalCenterHeight));

            // 1. Center Canvas with calculated height
            Rect canvasArea = GUILayoutUtility.GetRect(50, canvasAllocatedH, GUILayout.ExpandWidth(true), GUILayout.Height(canvasAllocatedH));
            canvasScreenRect = canvasArea;

            EditorGUI.DrawRect(canvasArea, new Color(0.12f, 0.12f, 0.14f, 1f));

            if (Event.current.type == EventType.Repaint)
            {
                RenderCanvasContent(canvasArea);
            }

            HandleCanvasInput(canvasArea);

            // 2. Timeline with guaranteed height
            if (showTimeline)
            {
                DrawTimelineSplitter();
                DrawAsepriteTimeline(currentTimelineH);
            }

            EditorGUILayout.EndVertical();
        }

        private void RenderCanvasContent(Rect canvasArea)
        {
            GUI.BeginClip(canvasArea);

            float w = canvasWidth * zoomLevel;
            float h = canvasHeight * zoomLevel;
            Rect drawRect = new Rect(panOffset.x, panOffset.y, w, h);

            // 1. Checkerboard background
            if (checkerTexture != null)
            {
                float checkerTileSize = 16f;
                Rect uvRect = new Rect(0, 0, w / checkerTileSize, h / checkerTileSize);
                GUI.DrawTextureWithTexCoords(drawRect, checkerTexture, uvRect, false);
            }

            // 2. Aseprite-Style Onion Skinning
            if (onionSkinning && frames.Count > 1 && !isPlaying)
            {
                if (onionPrevTexture != null) GUI.DrawTexture(drawRect, onionPrevTexture, ScaleMode.StretchToFill, true);
                if (onionNextTexture != null) GUI.DrawTexture(drawRect, onionNextTexture, ScaleMode.StretchToFill, true);
            }

            // 3. Current Texture Buffer
            Texture2D activeTex = (isPlaying && frames.Count > 1 && playbackFrame >= 0 && playbackFrame < frames.Count)
                ? frames[playbackFrame].compositeThumbnail
                : previewTexture;

            if (activeTex != null)
            {
                GUI.DrawTexture(drawRect, activeTex, ScaleMode.StretchToFill, true);
            }

            // 4. Grid lines
            if (showGrid && zoomLevel >= 4f)
            {
                Handles.color = new Color(0.5f, 0.5f, 0.5f, Mathf.Clamp01((zoomLevel - 4f) / 10f) * 0.35f);
                for (int x = 0; x <= canvasWidth; x++)
                {
                    float px = panOffset.x + x * zoomLevel;
                    Handles.DrawLine(new Vector3(px, panOffset.y, 0), new Vector3(px, panOffset.y + h, 0));
                }
                for (int y = 0; y <= canvasHeight; y++)
                {
                    float py = panOffset.y + y * zoomLevel;
                    Handles.DrawLine(new Vector3(panOffset.x, py, 0), new Vector3(panOffset.x + w, py, 0));
                }
            }

            // 5. Border around canvas
            Handles.color = new Color(0.85f, 0.85f, 0.85f, 0.9f);
            Handles.DrawPolyLine(
                new Vector3(panOffset.x, panOffset.y, 0),
                new Vector3(panOffset.x + w, panOffset.y, 0),
                new Vector3(panOffset.x + w, panOffset.y + h, 0),
                new Vector3(panOffset.x, panOffset.y + h, 0),
                new Vector3(panOffset.x, panOffset.y, 0)
            );

            // 6. Symmetry Axis Guide
            if (currentTool == PixelTool.MirrorPen)
            {
                Handles.color = new Color(0.2f, 0.85f, 1f, 0.7f);
                if (mirrorHorizontal)
                {
                    float midX = panOffset.x + (canvasWidth * 0.5f) * zoomLevel;
                    Handles.DrawLine(new Vector3(midX, panOffset.y, 0), new Vector3(midX, panOffset.y + h, 0));
                }
                if (mirrorVertical)
                {
                    float midY = panOffset.y + (canvasHeight * 0.5f) * zoomLevel;
                    Handles.DrawLine(new Vector3(panOffset.x, midY, 0), new Vector3(panOffset.x + w, midY, 0));
                }
            }

            // 7. MS Paint Curve Control Points Guide
            if (currentTool == PixelTool.Curve && curveStep > 0)
            {
                Handles.color = new Color(0.1f, 1f, 0.5f, 0.8f);
                Vector2 s0 = PixelToScreen(curveP0);
                Vector2 s3 = PixelToScreen(curveP3);
                Handles.DrawWireDisc(s0, Vector3.forward, 4f);
                Handles.DrawWireDisc(s3, Vector3.forward, 4f);

                if (curveStep >= 1)
                {
                    Vector2 s1 = PixelToScreen(curveP1);
                    Handles.color = Color.magenta;
                    Handles.DrawWireDisc(s1, Vector3.forward, 5f);
                    Handles.DrawLine(s0, s1);
                }
                if (curveStep >= 2)
                {
                    Vector2 s2 = PixelToScreen(curveP2);
                    Handles.color = Color.cyan;
                    Handles.DrawWireDisc(s2, Vector3.forward, 5f);
                    Handles.DrawLine(s3, s2);
                }
            }

            // 8. Selection Gizmo (Marching Ants + Rotation Handle)
            if (hasSelection && currentTool == PixelTool.Selection && selectionDragMode != SelectionDragMode.CreatingBox && selectionBox.width > 0 && selectionBox.height > 0)
            {
                float selCenterPX = selectionBox.x + floatingOffset.x + selectionBox.width * 0.5f;
                float selCenterPY = selectionBox.y + floatingOffset.y + selectionBox.height * 0.5f;

                // Calculate screen-space corners of the (possibly rotated) selection box
                float radians = selectionRotation * Mathf.Deg2Rad;
                float cosA = Mathf.Cos(radians);
                float sinA = Mathf.Sin(radians);

                float halfW = selectionBox.width * 0.5f;
                float halfH = selectionBox.height * 0.5f;

                // 4 corners in pixel space relative to center, then rotate
                Vector2[] localCorners = new Vector2[]
                {
                    new Vector2(-halfW, -halfH), // 0: Bottom-Left
                    new Vector2(halfW, -halfH),  // 1: Bottom-Right
                    new Vector2(halfW, halfH),   // 2: Top-Right (top on screen)
                    new Vector2(-halfW, halfH)   // 3: Top-Left (top on screen)
                };

                Vector3[] screenCorners = new Vector3[4];
                for (int i = 0; i < 4; i++)
                {
                    float rx = cosA * localCorners[i].x - sinA * localCorners[i].y;
                    float ry = sinA * localCorners[i].x + cosA * localCorners[i].y;

                    float px = selCenterPX + rx;
                    float py = selCenterPY + ry;

                    screenCorners[i] = new Vector3(
                        panOffset.x + px * zoomLevel,
                        panOffset.y + (canvasHeight - py) * zoomLevel,
                        0f
                    );
                }

                // Transparent highlight fill (when unrotated)
                if (Mathf.Abs(selectionRotation) < 0.01f)
                {
                    float minSX = Mathf.Min(screenCorners[0].x, screenCorners[1].x);
                    float minSY = Mathf.Min(screenCorners[2].y, screenCorners[3].y);
                    float boxW = Mathf.Abs(screenCorners[1].x - screenCorners[0].x);
                    float boxH = Mathf.Abs(screenCorners[0].y - screenCorners[3].y);
                    Handles.DrawSolidRectangleWithOutline(new Rect(minSX, minSY, boxW, boxH), new Color(0.2f, 0.6f, 1f, 0.15f), Color.clear);
                }

                // Draw dashed marching ants outline
                Handles.color = Color.white;
                Handles.DrawDottedLine(screenCorners[0], screenCorners[1], 4f);
                Handles.DrawDottedLine(screenCorners[1], screenCorners[2], 4f);
                Handles.DrawDottedLine(screenCorners[2], screenCorners[3], 4f);
                Handles.DrawDottedLine(screenCorners[3], screenCorners[0], 4f);

                // Inner black outline for contrast
                Handles.color = new Color(0f, 0f, 0f, 0.7f);
                Handles.DrawDottedLine(screenCorners[0] + new Vector3(1, 1, 0), screenCorners[1] + new Vector3(1, 1, 0), 4f);
                Handles.DrawDottedLine(screenCorners[1] + new Vector3(1, 1, 0), screenCorners[2] + new Vector3(1, 1, 0), 4f);
                Handles.DrawDottedLine(screenCorners[2] + new Vector3(1, 1, 0), screenCorners[3] + new Vector3(1, 1, 0), 4f);
                Handles.DrawDottedLine(screenCorners[3] + new Vector3(1, 1, 0), screenCorners[0] + new Vector3(1, 1, 0), 4f);

                // Corner handles
                Handles.color = Color.cyan;
                for (int i = 0; i < 4; i++)
                {
                    Handles.DrawSolidDisc(screenCorners[i], Vector3.forward, 4f);
                }

                // Center Move Handle
                Vector2 screenCenter = new Vector2(
                    panOffset.x + selCenterPX * zoomLevel,
                    panOffset.y + (canvasHeight - selCenterPY) * zoomLevel
                );
                Handles.color = new Color(0.2f, 0.7f, 1f, 0.9f);
                Handles.DrawSolidDisc(screenCenter, Vector3.forward, 5f);
                Handles.color = Color.white;
                Handles.DrawWireDisc(screenCenter, Vector3.forward, 5f);

                // Rotation Handle (knob above the TOP-CENTER of the selection box)
                // Top edge on screen is between screenCorners[3] and screenCorners[2]
                Vector2 topMid = ((Vector2)screenCorners[2] + (Vector2)screenCorners[3]) * 0.5f;
                Vector2 handleDir = (topMid - screenCenter).normalized;
                if (handleDir.sqrMagnitude < 0.001f) handleDir = new Vector2(0, -1);
                Vector2 rotHandle = topMid + handleDir * 24f;

                Handles.color = new Color(0.3f, 1f, 0.3f, 0.9f);
                Handles.DrawLine(topMid, (Vector3)rotHandle);
                Handles.DrawSolidDisc(rotHandle, Vector3.forward, 6f);
                Handles.color = new Color(0f, 0.5f, 0f, 1f);
                Handles.DrawWireDisc(rotHandle, Vector3.forward, 6f);
            }

            // 9. Selection drag box preview (while creating)
            if (currentTool == PixelTool.Selection && selectionDragMode == SelectionDragMode.CreatingBox && isDrawing)
            {
                int x0 = Mathf.Clamp(Mathf.Min(strokeStartPixel.x, currentHoverPixel.x), 0, canvasWidth - 1);
                int y0 = Mathf.Clamp(Mathf.Min(strokeStartPixel.y, currentHoverPixel.y), 0, canvasHeight - 1);
                int x1 = Mathf.Clamp(Mathf.Max(strokeStartPixel.x, currentHoverPixel.x), 0, canvasWidth - 1);
                int y1 = Mathf.Clamp(Mathf.Max(strokeStartPixel.y, currentHoverPixel.y), 0, canvasHeight - 1);
                int w1 = x1 - x0 + 1;
                int h1 = y1 - y0 + 1;

                float sx = panOffset.x + x0 * zoomLevel;
                float sy = panOffset.y + (canvasHeight - (y0 + h)) * zoomLevel;
                float sw = w1 * zoomLevel;
                float sh = h1 * zoomLevel;

                Handles.DrawSolidRectangleWithOutline(new Rect(sx, sy, sw, sh), new Color(0.3f, 0.6f, 1f, 0.2f), Color.clear);

                Handles.color = Color.white;
                Handles.DrawDottedLine(new Vector3(sx, sy, 0), new Vector3(sx + sw, sy, 0), 3f);
                Handles.DrawDottedLine(new Vector3(sx + sw, sy, 0), new Vector3(sx + sw, sy + sh, 0), 3f);
                Handles.DrawDottedLine(new Vector3(sx + sw, sy + sh, 0), new Vector3(sx, sy + sh, 0), 3f);
                Handles.DrawDottedLine(new Vector3(sx, sy + sh, 0), new Vector3(sx, sy, 0), 3f);
            }

            // 10. Hover Brush cursor outline
            if (!isPlaying && currentTool != PixelTool.Selection &&
                currentHoverPixel.x >= 0 && currentHoverPixel.x < canvasWidth &&
                currentHoverPixel.y >= 0 && currentHoverPixel.y < canvasHeight)
            {
                int bHalf = brushSize / 2;
                int minX = Mathf.Max(0, currentHoverPixel.x - bHalf);
                int maxX = Mathf.Min(canvasWidth - 1, minX + brushSize - 1);
                int minY = Mathf.Max(0, currentHoverPixel.y - bHalf);
                int maxY = Mathf.Min(canvasHeight - 1, minY + brushSize - 1);

                float cursorX = panOffset.x + minX * zoomLevel;
                float cursorY = panOffset.y + (canvasHeight - 1 - maxY) * zoomLevel;
                float cursorW = (maxX - minX + 1) * zoomLevel;
                float cursorH = (maxY - minY + 1) * zoomLevel;

                Rect cursorRect = new Rect(cursorX, cursorY, cursorW, cursorH);
                Handles.DrawSolidRectangleWithOutline(cursorRect, Color.clear, Color.yellow);
            }

            GUI.EndClip();
        }

        private Vector2 PixelToScreen(Vector2Int p)
        {
            return new Vector2(
                panOffset.x + (p.x + 0.5f) * zoomLevel,
                panOffset.y + (canvasHeight - 1 - p.y + 0.5f) * zoomLevel
            );
        }

        #region Aseprite Multi-Layer & Frame Timeline Panel
        private void DrawAsepriteTimeline(float height)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Height(height), GUILayout.MinHeight(120));

            // 1. Aseprite Control Bar
            DrawAsepriteControlBar();

            // 2. Aseprite Grid Track (Layers Column on Left, Frames Matrix on Right)
            DrawAsepriteMultiLayerTrack();

            EditorGUILayout.EndVertical();
        }

        private void DrawAsepriteControlBar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.Height(24));

            // Play / Stop Button
            GUI.backgroundColor = isPlaying ? new Color(1f, 0.4f, 0.4f) : new Color(0.4f, 0.9f, 0.5f);
            string playIconName = isPlaying ? "d_PauseButton" : "d_PlayButton";
            GUIContent playBtnContent = new GUIContent(isPlaying ? Tr("⏹ Dừng", "⏹ Stop") : Tr("▶ Phát", "▶ Play"),
                EditorGUIUtility.IconContent(playIconName).image, Tr("Phát/dừng animation (Phím Space)", "Play/stop animation (Space)"));
            if (GUILayout.Button(playBtnContent, EditorStyles.toolbarButton, GUILayout.Width(75)))
            {
                isPlaying = !isPlaying;
                lastAnimTick = EditorApplication.timeSinceStartup;
            }
            GUI.backgroundColor = Color.white;

            // Navigation Track Buttons
            if (GUILayout.Button(new GUIContent("⏮", Tr("Đến Frame đầu tiên (Home)", "Go to First Frame (Home)")), EditorStyles.toolbarButton, GUILayout.Width(24))) SwitchToFrame(0);
            if (GUILayout.Button(new GUIContent("◀", Tr("Frame trước (,)", "Previous Frame (,)")), EditorStyles.toolbarButton, GUILayout.Width(24))) SwitchToFrame(currentFrameIndex - 1);
            if (GUILayout.Button(new GUIContent("▶", Tr("Frame kế tiếp (.)", "Next Frame (.)")), EditorStyles.toolbarButton, GUILayout.Width(24))) SwitchToFrame(currentFrameIndex + 1);
            if (GUILayout.Button(new GUIContent("⏭", Tr("Đến Frame cuối cùng (End)", "Go to Last Frame (End)")), EditorStyles.toolbarButton, GUILayout.Width(24))) SwitchToFrame(frames.Count - 1);

            // Loop Toggle
            isLooping = GUILayout.Toggle(isLooping, new GUIContent("🔁", Tr("Lặp lại Animation", "Loop Animation")), EditorStyles.toolbarButton, GUILayout.Width(28));

            GUILayout.Space(6);

            // Onion Skinning Toggle
            GUI.backgroundColor = onionSkinning ? new Color(0.4f, 0.8f, 1f) : Color.white;
            onionSkinning = GUILayout.Toggle(onionSkinning, new GUIContent(Tr("🧅 Onion Skin", "🧅 Onion Skin"), Tr("Bật/tắt hiển thị bóng mờ Frame trước/sau (Phím O)", "Toggle Onion Skinning (Key O)")), EditorStyles.toolbarButton, GUILayout.Width(95));
            GUI.backgroundColor = Color.white;
            if (onionSkinning) UpdateOnionSkinTextures();

            GUILayout.Space(6);

            // FPS Settings
            GUILayout.Label(Tr("FPS:", "FPS:"), EditorStyles.miniLabel, GUILayout.Width(28));
            globalFps = EditorGUILayout.Slider(globalFps, 1f, 30f, GUILayout.Width(80));
            GUILayout.Label($"{Mathf.RoundToInt(globalFps)}", EditorStyles.miniBoldLabel, GUILayout.Width(18));

            GUILayout.FlexibleSpace();

            // Layer Management Buttons
            GUIContent addLayerContent = new GUIContent(Tr("+ Layer (Shift+N)", "+ Layer (Shift+N)"), EditorGUIUtility.IconContent("d_Toolbar Plus").image, Tr("Thêm một layer mới", "Add new layer"));
            if (GUILayout.Button(addLayerContent, EditorStyles.toolbarButton, GUILayout.Width(115)))
            {
                AddNewLayer();
            }

            // Frame Management Buttons
            GUIContent addFrameContent = new GUIContent(Tr("+ Frame (Alt+N)", "+ Frame (Alt+N)"), EditorGUIUtility.IconContent("CreateAddNew").image, Tr("Thêm frame mới trống (Alt+N)", "Add empty frame (Alt+N)"));
            if (GUILayout.Button(addFrameContent, EditorStyles.toolbarButton, GUILayout.Width(110)))
            {
                AddNewEmptyFrame();
            }

            GUIContent dupFrameContent = new GUIContent(Tr("📋 (Alt+D)", "📋 (Alt+D)"), EditorGUIUtility.IconContent("d_TreeEditor.Duplicate").image, Tr("Nhân bản frame đang chọn (Alt+D)", "Duplicate current frame (Alt+D)"));
            if (GUILayout.Button(dupFrameContent, EditorStyles.toolbarButton, GUILayout.Width(80)))
            {
                DuplicateFrame();
            }

            GUI.backgroundColor = new Color(1f, 0.6f, 0.6f);
            GUIContent delFrameContent = new GUIContent(Tr("🗑️ (Del)", "🗑️ (Del)"), EditorGUIUtility.IconContent("d_TreeEditor.Trash").image, Tr("Xóa frame đang chọn", "Delete current frame"));
            if (GUILayout.Button(delFrameContent, EditorStyles.toolbarButton, GUILayout.Width(65)))
            {
                DeleteCurrentFrame();
            }
            GUI.backgroundColor = Color.white;

            EditorGUILayout.EndHorizontal();
        }

        private void DrawAsepriteMultiLayerTrack()
        {
            const float layerColWidth = 145f;
            const float celWidth = 46f;
            const float headerHeight = 22f;
            const float rowHeight = 30f;

            timelineScroll = EditorGUILayout.BeginScrollView(timelineScroll, GUILayout.ExpandHeight(true));
            EditorGUILayout.BeginHorizontal();

            // ================= 1. LEFT COLUMN: LAYERS LIST =================
            EditorGUILayout.BeginVertical(GUILayout.Width(layerColWidth));

            // Layer Title Box
            Rect layerHeaderRect = GUILayoutUtility.GetRect(layerColWidth, headerHeight);
            EditorGUI.DrawRect(layerHeaderRect, new Color(0.16f, 0.17f, 0.2f, 1f));
            Handles.DrawSolidRectangleWithOutline(layerHeaderRect, Color.clear, new Color(0.1f, 0.1f, 0.12f, 1f));

            Rect lTitleRect = new Rect(layerHeaderRect.x + 4, layerHeaderRect.y + 2, 80, 18);
            GUI.Label(lTitleRect, Tr("LỚP (LAYERS)", "LAYERS"), EditorStyles.miniBoldLabel);

            // Quick Add Layer button inside header
            Rect addLBtnRect = new Rect(layerHeaderRect.x + layerColWidth - 24, layerHeaderRect.y + 2, 20, 18);
            if (GUI.Button(addLBtnRect, "+", EditorStyles.miniButton))
            {
                AddNewLayer();
            }

            // Draw Layers from Top Layer (index N-1) to Bottom Layer (index 0) - Aseprite / Photoshop style
            for (int l = layers.Count - 1; l >= 0; l--)
            {
                int layerIdx = l;
                bool isLayerSelected = (currentLayerIndex == layerIdx);
                var layerObj = layers[layerIdx];

                Rect layerRowRect = GUILayoutUtility.GetRect(layerColWidth, rowHeight);
                Color rowBg = isLayerSelected ? new Color(0.25f, 0.32f, 0.45f, 1f) : new Color(0.18f, 0.2f, 0.24f, 1f);
                EditorGUI.DrawRect(layerRowRect, rowBg);
                Handles.DrawSolidRectangleWithOutline(layerRowRect, Color.clear, new Color(0.1f, 0.1f, 0.12f, 1f));

                // Visibility Eye toggle
                Rect eyeRect = new Rect(layerRowRect.x + 3, layerRowRect.y + 5, 20, 20);
                string eyeIcon = layerObj.isVisible ? "👁️" : "⎯";
                if (GUI.Button(eyeRect, new GUIContent(eyeIcon, Tr("Bật/tắt hiển thị layer này", "Toggle visibility")), EditorStyles.label))
                {
                    layerObj.isVisible = !layerObj.isVisible;
                    RebuildPreviewTexture();
                }

                // Lock toggle
                Rect lockRect = new Rect(layerRowRect.x + 23, layerRowRect.y + 5, 20, 20);
                string lockIcon = layerObj.isLocked ? "🔒" : "🔓";
                if (GUI.Button(lockRect, new GUIContent(lockIcon, Tr("Khóa/mở khóa chỉnh sửa layer", "Lock/unlock layer")), EditorStyles.label))
                {
                    layerObj.isLocked = !layerObj.isLocked;
                }

                // Layer Name (Click to select)
                Rect nameRect = new Rect(layerRowRect.x + 44, layerRowRect.y + 6, layerColWidth - 48, 20);
                GUI.Label(nameRect, layerObj.name, isLayerSelected ? EditorStyles.boldLabel : EditorStyles.label);

                // Right click on Layer opens Context Menu
                if (Event.current.type == EventType.MouseDown && layerRowRect.Contains(Event.current.mousePosition))
                {
                    if (Event.current.button == 0)
                    {
                        currentLayerIndex = layerIdx;
                        undoStack.Clear();
                        redoStack.Clear();
                        Repaint();
                    }
                    else if (Event.current.button == 1)
                    {
                        ShowLayerContextMenu(layerIdx);
                        Event.current.Use();
                    }
                }
            }

            EditorGUILayout.EndVertical();

            // ================= 2. RIGHT COLUMNS: FRAME NUMBERS & CELS MATRIX =================
            for (int f = 0; f < frames.Count; f++)
            {
                int frameIdx = f;
                bool isFrameSelected = (currentFrameIndex == frameIdx);
                bool isPlaybackActive = (isPlaying && playbackFrame == frameIdx);
                var curFrame = frames[frameIdx];

                EditorGUILayout.BeginVertical(GUILayout.Width(celWidth));

                // --- 1. Frame Header Cell ---
                Rect fHeaderRect = GUILayoutUtility.GetRect(celWidth, headerHeight);
                Color headerBg = isPlaybackActive
                    ? new Color(0.9f, 0.3f, 0.3f, 1f)
                    : (isFrameSelected ? new Color(0.85f, 0.55f, 0.2f, 1f) : new Color(0.22f, 0.24f, 0.28f, 1f));

                EditorGUI.DrawRect(fHeaderRect, headerBg);
                Handles.DrawSolidRectangleWithOutline(fHeaderRect, Color.clear, new Color(0.1f, 0.1f, 0.12f, 1f));

                GUIStyle numStyle = (isFrameSelected || isPlaybackActive) ? asepriteActiveHeaderStyle : asepriteHeaderStyle;
                GUI.Label(fHeaderRect, $"{frameIdx + 1}", numStyle);

                // Click Header to Switch Frame
                if (Event.current.type == EventType.MouseDown && fHeaderRect.Contains(Event.current.mousePosition))
                {
                    if (Event.current.button == 0)
                    {
                        SwitchToFrame(frameIdx);
                        Event.current.Use();
                    }
                    else if (Event.current.button == 1)
                    {
                        ShowFrameContextMenu(frameIdx);
                        Event.current.Use();
                    }
                }

                // --- 2. Cels for each Layer (Top to Bottom) ---
                for (int l = layers.Count - 1; l >= 0; l--)
                {
                    int layerIdx = l;
                    bool isCelActive = (isFrameSelected && currentLayerIndex == layerIdx);

                    while (curFrame.cels.Count <= layerIdx)
                    {
                        curFrame.cels.Add(new LayerCel(canvasWidth, canvasHeight));
                    }
                    var cel = curFrame.cels[layerIdx];

                    Rect celRect = GUILayoutUtility.GetRect(celWidth, rowHeight);
                    Color celBg = isCelActive ? new Color(0.3f, 0.38f, 0.52f, 1f) : new Color(0.16f, 0.18f, 0.22f, 1f);
                    EditorGUI.DrawRect(celRect, celBg);

                    // Cel thumbnail
                    if (cel.thumbnail != null)
                    {
                        Rect thumbRect = new Rect(celRect.x + 3, celRect.y + 2, celWidth - 6, rowHeight - 4);
                        if (checkerTexture != null) GUI.DrawTexture(thumbRect, checkerTexture, ScaleMode.StretchToFill);
                        GUI.DrawTexture(thumbRect, cel.thumbnail, ScaleMode.ScaleToFit, true);
                    }

                    // Content Dot indicator
                    bool hasContent = cel.HasContent();
                    Rect dotRect = new Rect(celRect.x + (celWidth - 6) * 0.5f, celRect.y + rowHeight - 8, 6, 6);
                    if (hasContent)
                    {
                        EditorGUI.DrawRect(dotRect, new Color(1f, 1f, 1f, 0.9f));
                    }
                    else
                    {
                        Handles.DrawSolidRectangleWithOutline(dotRect, Color.clear, new Color(0.5f, 0.5f, 0.5f, 0.5f));
                    }

                    // Cel Outline
                    Color borderCol = isCelActive ? Color.yellow : new Color(0.1f, 0.1f, 0.12f, 1f);
                    Handles.DrawSolidRectangleWithOutline(celRect, Color.clear, borderCol);

                    // Click Cel to Select BOTH Frame and Layer!
                    if (Event.current.type == EventType.MouseDown && celRect.Contains(Event.current.mousePosition))
                    {
                        if (Event.current.button == 0)
                        {
                            currentFrameIndex = frameIdx;
                            currentLayerIndex = layerIdx;
                            playbackFrame = frameIdx;
                            undoStack.Clear();
                            redoStack.Clear();
                            RebuildPreviewTexture();
                            Event.current.Use();
                            Repaint();
                        }
                        else if (Event.current.button == 1)
                        {
                            ShowCelContextMenu(frameIdx, layerIdx);
                            Event.current.Use();
                        }
                    }
                }

                EditorGUILayout.EndVertical();
            }

            // Quick '+' Button to add new frame at end
            EditorGUILayout.BeginVertical(GUILayout.Width(28));
            Rect addBtnRect = GUILayoutUtility.GetRect(28, headerHeight);
            if (GUI.Button(addBtnRect, "+", EditorStyles.miniButton))
            {
                AddNewEmptyFrame();
            }
            EditorGUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
        }

        private void ShowLayerContextMenu(int layerIdx)
        {
            var l = layers[layerIdx];
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent(Tr($"Layer: {l.name}", $"Layer: {l.name}")), false, null);
            menu.AddSeparator("");
            menu.AddItem(new GUIContent(Tr("➕ Thêm Layer mới (Shift+N)", "➕ Add New Layer (Shift+N)")), false, () => AddNewLayer());
            menu.AddItem(new GUIContent(Tr("📋 Nhân bản Layer này", "📋 Duplicate Layer")), false, DuplicateCurrentLayer);
            menu.AddItem(new GUIContent(Tr("✏️ Đổi tên Layer...", "✏️ Rename Layer...")), false, () => PromptRenameLayer(layerIdx));
            menu.AddSeparator("");
            menu.AddItem(new GUIContent(Tr("▲ Di chuyển Layer lên trên", "▲ Move Layer Up")), layerIdx < layers.Count - 1, () => { currentLayerIndex = layerIdx; MoveLayer(1); });
            menu.AddItem(new GUIContent(Tr("▼ Di chuyển Layer xuống dưới", "▼ Move Layer Down")), layerIdx > 0, () => { currentLayerIndex = layerIdx; MoveLayer(-1); });
            menu.AddItem(new GUIContent(Tr("🔗 Gộp xuống Layer dưới (Merge Down)", "🔗 Merge Down")), layerIdx > 0, () => MergeLayerDown(layerIdx));
            menu.AddSeparator("");
            menu.AddItem(new GUIContent(Tr("🗑️ Xóa Layer này", "🗑️ Delete Layer")), layers.Count > 1, DeleteCurrentLayer);
            menu.ShowAsContext();
        }

        private void PromptRenameLayer(int layerIdx)
        {
            var l = layers[layerIdx];
            string res = EditorUtility.DisplayDialogComplex(Tr("Đổi Tên Layer", "Rename Layer"),
                Tr($"Nhập tên mới cho {l.name}:", $"New name for {l.name}:"),
                "Body", "Cancel", "Head") == 0 ? "Body" : "Head";
            l.name = res;
            Repaint();
        }

        private void ShowFrameContextMenu(int frameIdx)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent(Tr($"Frame #{frameIdx + 1}", $"Frame #{frameIdx + 1}")), false, null);
            menu.AddSeparator("");
            menu.AddItem(new GUIContent(Tr("📋 Nhân bản Frame này (Alt+D)", "📋 Duplicate Frame (Alt+D)")), false, () => { currentFrameIndex = frameIdx; DuplicateFrame(); });
            menu.AddItem(new GUIContent(Tr("➕ Thêm Frame trống mới (Alt+N)", "➕ New Empty Frame (Alt+N)")), false, () => { currentFrameIndex = frameIdx; AddNewEmptyFrame(); });
            menu.AddSeparator("");
            menu.AddItem(new GUIContent(Tr("◀ Di chuyển Frame sang trái", "◀ Move Frame Left")), frameIdx > 0, () => { currentFrameIndex = frameIdx; MoveFrame(-1); });
            menu.AddItem(new GUIContent(Tr("Di chuyển Frame sang phải ▶", "Move Frame Right ▶")), frameIdx < frames.Count - 1, () => { currentFrameIndex = frameIdx; MoveFrame(1); });
            menu.AddItem(new GUIContent(Tr("🔄 Đảo ngược thứ tự toàn bộ Frames", "🔄 Reverse All Frames")), frames.Count > 1, ReverseAllFrames);
            menu.AddSeparator("");
            menu.AddItem(new GUIContent(Tr("🗑️ Xóa Frame này (Del)", "🗑️ Delete Frame (Del)")), frames.Count > 1, () => { currentFrameIndex = frameIdx; DeleteCurrentFrame(); });
            menu.ShowAsContext();
        }

        private void ShowCelContextMenu(int frameIdx, int layerIdx)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent(Tr($"Cel [{layers[layerIdx].name}, Frame #{frameIdx + 1}]", $"Cel [{layers[layerIdx].name}, Frame #{frameIdx + 1}]")), false, null);
            menu.AddSeparator("");
            menu.AddItem(new GUIContent(Tr("🧹 Xóa sạch Cel này (Trong suốt)", "🧹 Clear Cel (Transparent)")), false, () =>
            {
                var cel = frames[frameIdx].cels[layerIdx];
                for (int i = 0; i < cel.pixels.Length; i++) cel.pixels[i] = new Color32(0, 0, 0, 0);
                RebuildPreviewTexture();
                Repaint();
            });
            menu.ShowAsContext();
        }
        #endregion

        private void DrawRightPanel()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(rightPanelWidth), GUILayout.ExpandHeight(true));
            rightScroll = EditorGUILayout.BeginScrollView(rightScroll);

            GUILayout.Label(Tr("XEM TRƯỚC (PREVIEW)", "PREVIEW (1:1 & Scaled)"), headerStyle);
            EditorGUILayout.Space(2);

            Texture2D displayTex = previewTexture;
            if (isPlaying && frames.Count > 1 && playbackFrame >= 0 && playbackFrame < frames.Count)
            {
                displayTex = frames[playbackFrame].compositeThumbnail;
            }

            if (displayTex != null)
            {
                // 1:1 Preview
                Rect r1 = GUILayoutUtility.GetRect(canvasWidth, canvasHeight, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
                GUI.DrawTexture(r1, displayTex, ScaleMode.ScaleToFit, true);
                GUILayout.Label(Tr($"Gốc: {canvasWidth}x{canvasHeight}px", $"Original: {canvasWidth}x{canvasHeight}px"), EditorStyles.miniLabel);

                EditorGUILayout.Space(6);

                // Scaled Preview
                float previewBoxSize = Mathf.Min(rightPanelWidth - 25f, 160f);
                Rect rScaled = GUILayoutUtility.GetRect(previewBoxSize, previewBoxSize);
                EditorGUI.DrawRect(rScaled, new Color(0.1f, 0.1f, 0.1f, 1f));
                if (checkerTexture != null) GUI.DrawTexture(rScaled, checkerTexture, ScaleMode.StretchToFill);
                GUI.DrawTexture(rScaled, displayTex, ScaleMode.ScaleToFit, true);
            }

            EditorGUILayout.Space(10);
            GUILayout.Label(Tr("LAYER ĐANG CHỌN", "ACTIVE LAYER"), headerStyle);

            if (currentLayerIndex >= 0 && currentLayerIndex < layers.Count)
            {
                var curL = layers[currentLayerIndex];
                curL.name = EditorGUILayout.TextField(curL.name);

                EditorGUILayout.BeginHorizontal();
                curL.isVisible = EditorGUILayout.ToggleLeft("Hiện (Eye)", curL.isVisible, GUILayout.Width(85));
                curL.isLocked = EditorGUILayout.ToggleLeft("Khóa (Lock)", curL.isLocked, GUILayout.Width(85));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Opacity:", GUILayout.Width(50));
                float prevOp = curL.opacity;
                curL.opacity = EditorGUILayout.Slider(curL.opacity, 0f, 1f);
                if (Mathf.Abs(prevOp - curL.opacity) > 0.01f) RebuildPreviewTexture();
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space(8);
            GUILayout.Label(Tr("BIẾN ĐỔI (TRANSFORM)", "TRANSFORM"), headerStyle);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(Tr("Lật Ngang", "Flip X"), EditorStyles.miniButton)) FlipHorizontal();
            if (GUILayout.Button(Tr("Lật Dọc", "Flip Y"), EditorStyles.miniButton)) FlipVertical();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(Tr("Xoay 90° CW", "Rot 90° CW"), EditorStyles.miniButton)) Rotate90CW();
            if (GUILayout.Button(Tr("Xoay 90° CCW", "Rot 90° CCW"), EditorStyles.miniButton)) Rotate90CCW();
            EditorGUILayout.EndHorizontal();

            if (hasSelection && isFloatingSelection)
            {
                EditorGUILayout.Space(2);
                GUILayout.Label(Tr($"Vùng chọn đang nổi ({selectionRotation:F0}°)", $"Floating Selection ({selectionRotation:F0}°)"), EditorStyles.miniBoldLabel);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("-90°", EditorStyles.miniButton)) RotateSelectionBy(-90f);
                if (GUILayout.Button("+90°", EditorStyles.miniButton)) RotateSelectionBy(90f);
                if (GUILayout.Button(Tr("Chốt", "Commit"), EditorStyles.miniButton)) CommitFloatingSelection();
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space(4);
            GUILayout.Label(Tr("Dịch chuyển Pixel (Layer này)", "Shift Pixels (Layer)"), EditorStyles.miniBoldLabel);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("←", EditorStyles.miniButton)) ShiftCanvas(-1, 0);
            if (GUILayout.Button("→", EditorStyles.miniButton)) ShiftCanvas(1, 0);
            if (GUILayout.Button("↑", EditorStyles.miniButton)) ShiftCanvas(0, 1);
            if (GUILayout.Button("↓", EditorStyles.miniButton)) ShiftCanvas(0, -1);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(8);
            GUILayout.Label(Tr("THAO TÁC KHÁC", "CANVAS ACTIONS"), headerStyle);
            if (GUILayout.Button(Tr("Xóa Cel này (Trong suốt)", "Clear Cel (Transparent)"), EditorStyles.miniButton))
            {
                RecordUndo();
                var buf = CurrentActiveCelPixels;
                for (int i = 0; i < buf.Length; i++) buf[i] = new Color32(0, 0, 0, 0);
                RebuildPreviewTexture();
            }

            if (GUILayout.Button(Tr("Đảo màu Cel (Invert)", "Invert Cel Colors"), EditorStyles.miniButton))
            {
                InvertColors();
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void DrawBottomStatusBar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.Height(22));

            string coordsText = (currentHoverPixel.x >= 0 && currentHoverPixel.x < canvasWidth && currentHoverPixel.y >= 0 && currentHoverPixel.y < canvasHeight)
                ? $"X: {currentHoverPixel.x}, Y: {currentHoverPixel.y}"
                : "X: --, Y: --";

            GUILayout.Label(coordsText, infoLabelStyle, GUILayout.Width(100));
            GUILayout.Label(Tr($"Kích thước: {canvasWidth}x{canvasHeight}px", $"Size: {canvasWidth}x{canvasHeight}px"), infoLabelStyle, GUILayout.Width(120));
            GUILayout.Label(Tr($"Frame: {currentFrameIndex + 1}/{frames.Count}", $"Frame: {currentFrameIndex + 1}/{frames.Count}"), infoLabelStyle, GUILayout.Width(90));

            string layerInfo = (currentLayerIndex >= 0 && currentLayerIndex < layers.Count) ? layers[currentLayerIndex].name : "--";
            GUILayout.Label(Tr($"Layer: {layerInfo}", $"Layer: {layerInfo}"), infoLabelStyle, GUILayout.Width(110));

            if (!string.IsNullOrEmpty(targetAssetPath))
            {
                GUILayout.Label($"{Tr("Tệp:", "File:")} {Path.GetFileName(targetAssetPath)}", infoLabelStyle);
            }

            GUILayout.FlexibleSpace();
            GUILayout.Label(Tr("Phím tắt: [Shift+N] Layer mới [Alt+N] Frame mới [Alt+D] Nhân bản [Del] Xóa [Space] Phát",
                               "Keys: [Shift+N] New Layer [Alt+N] New Frame [Alt+D] Duplicate [Del] Delete [Space] Play"), EditorStyles.miniLabel);

            EditorGUILayout.EndHorizontal();
        }
        #endregion

        #region Input Handling
        private void HandleKeyboardShortcuts()
        {
            var e = Event.current;
            if (e.type == EventType.KeyDown)
            {
                // --- Escape: Cancel selection first, then cancel curve ---
                if (e.keyCode == KeyCode.Escape)
                {
                    if (hasSelection)
                    {
                        CancelSelection();
                        e.Use();
                        Repaint();
                        return;
                    }
                    if (curveStep > 0)
                    {
                        curveStep = 0;
                        if (undoStack.Count > 0)
                        {
                            var b = undoStack.Pop();
                            Array.Copy(b, CurrentActiveCelPixels, b.Length);
                            RebuildPreviewTexture();
                        }
                        e.Use();
                        Repaint();
                        return;
                    }
                }

                // --- Enter / Return: Commit floating selection ---
                if (e.keyCode == KeyCode.Return || e.keyCode == KeyCode.KeypadEnter)
                {
                    if (hasSelection && isFloatingSelection)
                    {
                        CommitFloatingSelection();
                        e.Use();
                        Repaint();
                        return;
                    }
                }

                // Shift + N -> New Layer
                if (e.shift && e.keyCode == KeyCode.N)
                {
                    AddNewLayer();
                    e.Use();
                    return;
                }

                // Alt Shortcuts
                if (e.alt)
                {
                    if (e.keyCode == KeyCode.N)
                    {
                        AddNewEmptyFrame();
                        e.Use();
                        return;
                    }
                    if (e.keyCode == KeyCode.D)
                    {
                        DuplicateFrame();
                        e.Use();
                        return;
                    }
                }

                // --- Delete: delete selection content if active, else delete frame ---
                if (e.keyCode == KeyCode.Delete)
                {
                    if (hasSelection && currentTool == PixelTool.Selection)
                    {
                        DeleteSelectionContent();
                        e.Use();
                        return;
                    }
                    DeleteCurrentFrame();
                    e.Use();
                    return;
                }

                if (e.control || e.command)
                {
                    if (e.keyCode == KeyCode.Z)
                    {
                        if (e.shift) PerformRedo();
                        else PerformUndo();
                        e.Use();
                    }
                    else if (e.keyCode == KeyCode.Y)
                    {
                        PerformRedo();
                        e.Use();
                    }
                    else if (e.keyCode == KeyCode.S)
                    {
                        SaveOverwrite();
                        e.Use();
                    }
                    else if (e.keyCode == KeyCode.A)
                    {
                        // Ctrl+A: Select All
                        SelectAllPixels();
                        e.Use();
                        Repaint();
                    }
                    else if (e.keyCode == KeyCode.D)
                    {
                        // Ctrl+D: Deselect / Commit
                        if (hasSelection)
                        {
                            if (isFloatingSelection) CommitFloatingSelection();
                            else ClearSelectionState();
                            e.Use();
                            Repaint();
                        }
                    }
                    else if (e.keyCode == KeyCode.C)
                    {
                        CopySelection();
                        e.Use();
                    }
                    else if (e.keyCode == KeyCode.X)
                    {
                        CutSelection();
                        e.Use();
                    }
                    else if (e.keyCode == KeyCode.V)
                    {
                        PasteSelection();
                        e.Use();
                    }
                    else if (e.keyCode == KeyCode.J)
                    {
                        DuplicateSelection();
                        e.Use();
                    }
                }
                else
                {
                    switch (e.keyCode)
                    {
                        case KeyCode.Space:
                            isPlaying = !isPlaying;
                            lastAnimTick = EditorApplication.timeSinceStartup;
                            e.Use();
                            Repaint();
                            break;
                        case KeyCode.Comma:
                        case KeyCode.LeftBracket:
                            SwitchToFrame(currentFrameIndex - 1);
                            e.Use();
                            break;
                        case KeyCode.Period:
                        case KeyCode.RightBracket:
                            SwitchToFrame(currentFrameIndex + 1);
                            e.Use();
                            break;
                        case KeyCode.Home:
                            SwitchToFrame(0);
                            e.Use();
                            break;
                        case KeyCode.End:
                            SwitchToFrame(frames.Count - 1);
                            e.Use();
                            break;
                        case KeyCode.O:
                            onionSkinning = !onionSkinning;
                            if (onionSkinning) UpdateOnionSkinTextures();
                            e.Use();
                            Repaint();
                            break;
                        case KeyCode.S: currentTool = PixelTool.Selection; curveStep = 0; Repaint(); break;
                        case KeyCode.P: currentTool = PixelTool.Pen; curveStep = 0; Repaint(); break;
                        case KeyCode.M: currentTool = PixelTool.MirrorPen; curveStep = 0; Repaint(); break;
                        case KeyCode.E: currentTool = PixelTool.Eraser; curveStep = 0; Repaint(); break;
                        case KeyCode.B: currentTool = PixelTool.PaintBucket; curveStep = 0; Repaint(); break;
                        case KeyCode.I: currentTool = PixelTool.ColorPicker; curveStep = 0; Repaint(); break;
                        case KeyCode.L: currentTool = PixelTool.Line; curveStep = 0; Repaint(); break;
                        case KeyCode.K: currentTool = PixelTool.Curve; curveStep = 0; Repaint(); break;
                        case KeyCode.R: currentTool = PixelTool.Rectangle; curveStep = 0; Repaint(); break;
                        case KeyCode.C: currentTool = PixelTool.Circle; curveStep = 0; Repaint(); break;
                        case KeyCode.U: currentTool = PixelTool.LightenDarken; curveStep = 0; Repaint(); break;
                        case KeyCode.X: SwapColors(); Repaint(); break;
                    }
                }
            }
        }

        private void SwapColors()
        {
            var tmp = primaryColor;
            primaryColor = secondaryColor;
            secondaryColor = tmp;
        }

        private void HandleCanvasInput(Rect canvasArea)
        {
            Event e = Event.current;
            Vector2 localMouse = e.mousePosition - canvasArea.position;

            currentHoverPixel = ScreenToPixelCoords(localMouse);

            // Zooming
            if (e.type == EventType.ScrollWheel && canvasArea.Contains(e.mousePosition))
            {
                float zoomDelta = -e.delta.y * (zoomLevel > 10 ? 2f : 1f);
                float oldZoom = zoomLevel;
                zoomLevel = Mathf.Clamp(zoomLevel + zoomDelta, 1f, 64f);

                Vector2 mouseCanvasPos = localMouse - panOffset;
                panOffset -= mouseCanvasPos * (zoomLevel / oldZoom - 1f);

                e.Use();
                Repaint();
                return;
            }

            // Panning
            if (e.type == EventType.MouseDown && (e.button == 2 || (e.button == 0 && e.alt)))
            {
                isPanning = true;
                lastMousePanPos = e.mousePosition;
                e.Use();
                return;
            }

            if (e.type == EventType.MouseDrag && isPanning)
            {
                panOffset += e.mousePosition - lastMousePanPos;
                lastMousePanPos = e.mousePosition;
                e.Use();
                Repaint();
                return;
            }

            if (e.type == EventType.MouseUp && isPanning)
            {
                isPanning = false;
                e.Use();
                return;
            }

            // Check if active layer is locked or hidden
            if (currentLayerIndex >= 0 && currentLayerIndex < layers.Count)
            {
                if (layers[currentLayerIndex].isLocked || !layers[currentLayerIndex].isVisible)
                {
                    return; // Prevent drawing on locked/hidden layer
                }
            }

            // ======== Selection Tool Input ========
            if (currentTool == PixelTool.Selection)
            {
                HandleSelectionToolInput(e, canvasArea, localMouse);
                if (e.type == EventType.MouseMove) Repaint();
                return;
            }

            // Drawing Tools
            if (canvasArea.Contains(e.mousePosition))
            {
                if (currentTool == PixelTool.Curve)
                {
                    HandleCurveToolInput(e);
                    return;
                }

                if (e.type == EventType.MouseDown && (e.button == 0 || e.button == 1))
                {
                    Color drawCol = (e.button == 0) ? primaryColor : secondaryColor;
                    if (currentTool == PixelTool.Eraser) drawCol = new Color(0, 0, 0, 0);

                    if (currentTool == PixelTool.ColorPicker)
                    {
                        PickColorAt(currentHoverPixel, e.button == 0);
                        e.Use();
                        return;
                    }

                    if (currentTool == PixelTool.PaintBucket)
                    {
                        RecordUndo();
                        FloodFill(currentHoverPixel.x, currentHoverPixel.y, (Color32)drawCol);
                        RebuildPreviewTexture();
                        e.Use();
                        return;
                    }

                    RecordUndo();
                    isDrawing = true;
                    strokeStartPixel = currentHoverPixel;
                    lastDrawnPixel = currentHoverPixel;

                    if (IsDirectDrawTool(currentTool))
                    {
                        ApplyToolAt(currentHoverPixel, drawCol, e.button == 1 || e.shift);
                    }
                    else
                    {
                        CreateShapePreview(strokeStartPixel, currentHoverPixel, drawCol);
                    }

                    RebuildPreviewTexture();
                    e.Use();
                }
                else if (e.type == EventType.MouseDrag && isDrawing)
                {
                    Color drawCol = (e.button == 0) ? primaryColor : secondaryColor;
                    if (currentTool == PixelTool.Eraser) drawCol = new Color(0, 0, 0, 0);

                    if (IsDirectDrawTool(currentTool))
                    {
                        DrawLineContinuous(lastDrawnPixel, currentHoverPixel, drawCol, e.button == 1 || e.shift);
                        lastDrawnPixel = currentHoverPixel;
                        RebuildPreviewTexture();
                    }
                    else
                    {
                        CreateShapePreview(strokeStartPixel, currentHoverPixel, drawCol);
                    }

                    e.Use();
                    Repaint();
                }
                else if (e.type == EventType.MouseUp && isDrawing)
                {
                    Color drawCol = (e.button == 0) ? primaryColor : secondaryColor;
                    if (currentTool == PixelTool.Eraser) drawCol = new Color(0, 0, 0, 0);

                    if (!IsDirectDrawTool(currentTool))
                    {
                        CommitShape(strokeStartPixel, currentHoverPixel, drawCol);
                    }

                    isDrawing = false;
                    RebuildPreviewTexture();
                    e.Use();
                    Repaint();
                }
            }
            else
            {
                if (e.type == EventType.MouseUp && isDrawing)
                {
                    isDrawing = false;
                    RebuildPreviewTexture();
                }
            }

            if (e.type == EventType.MouseMove)
            {
                Repaint();
            }
        }

        private void HandleSelectionToolInput(Event e, Rect canvasArea, Vector2 localMouse)
        {
            // --- Right Click Context Menu ---
            if (e.type == EventType.MouseDown && e.button == 1 && canvasArea.Contains(e.mousePosition))
            {
                GenericMenu selMenu = new GenericMenu();
                if (hasSelection)
                {
                    selMenu.AddItem(new GUIContent(Tr("📋 Sao chép (Ctrl+C)", "📋 Copy (Ctrl+C)")), false, () => CopySelection());
                    selMenu.AddItem(new GUIContent(Tr("✂️ Cắt (Ctrl+X)", "✂️ Cut (Ctrl+X)")), false, () => CutSelection());
                    selMenu.AddItem(new GUIContent(Tr("📑 Nhân bản (Duplicate)", "📑 Duplicate")), false, () => DuplicateSelection());
                    selMenu.AddSeparator("");
                    if (isFloatingSelection)
                    {
                        selMenu.AddItem(new GUIContent(Tr("✅ Chốt vùng chọn (Enter)", "✅ Commit (Enter)")), false, () => CommitFloatingSelection());
                        selMenu.AddItem(new GUIContent(Tr("❌ Hủy thay đổi (Esc)", "❌ Cancel (Esc)")), false, () => CancelSelection());
                    }
                    else
                    {
                        selMenu.AddItem(new GUIContent(Tr("✋ Nâng pixel để di chuyển", "✋ Lift Pixels")), false, () => LiftSelectionPixels());
                        selMenu.AddItem(new GUIContent(Tr("❌ Bỏ chọn (Ctrl+D)", "❌ Deselect (Ctrl+D)")), false, () => { ClearSelectionState(); Repaint(); });
                    }
                    selMenu.AddItem(new GUIContent(Tr("🗑 Xóa nội dung (Del)", "🗑 Delete Content (Del)")), false, () => DeleteSelectionContent());
                }
                else
                {
                    selMenu.AddDisabledItem(new GUIContent(Tr("📋 Sao chép (Ctrl+C)", "📋 Copy (Ctrl+C)")));
                    selMenu.AddDisabledItem(new GUIContent(Tr("✂️ Cắt (Ctrl+X)", "✂️ Cut (Ctrl+X)")));
                }

                if (clipboardPixels != null && clipboardWidth > 0 && clipboardHeight > 0)
                {
                    selMenu.AddItem(new GUIContent(Tr("📥 Dán (Ctrl+V)", "📥 Paste (Ctrl+V)")), false, () => PasteSelection());
                }
                else
                {
                    selMenu.AddDisabledItem(new GUIContent(Tr("📥 Dán (Ctrl+V)", "📥 Paste (Ctrl+V)")));
                }

                selMenu.AddItem(new GUIContent(Tr("🔲 Chọn tất cả (Ctrl+A)", "🔲 Select All (Ctrl+A)")), false, () => SelectAllPixels());

                selMenu.ShowAsContext();
                e.Use();
                return;
            }

            // --- MouseDown ---
            if (e.type == EventType.MouseDown && e.button == 0 && canvasArea.Contains(e.mousePosition))
            {
                if (hasSelection && selectionBox.width > 0 && selectionBox.height > 0)
                {
                    float selCenterPX = selectionBox.x + floatingOffset.x + selectionBox.width * 0.5f;
                    float selCenterPY = selectionBox.y + floatingOffset.y + selectionBox.height * 0.5f;

                    float radians = selectionRotation * Mathf.Deg2Rad;
                    float cosA = Mathf.Cos(radians);
                    float sinA = Mathf.Sin(radians);
                    float halfW = selectionBox.width * 0.5f;
                    float halfH = selectionBox.height * 0.5f;

                    // 4 corners in pixel space
                    Vector2 c2Local = new Vector2(halfW, halfH);   // Top-Right
                    Vector2 c3Local = new Vector2(-halfW, halfH);  // Top-Left

                    Vector2 c2Screen = new Vector2(
                        panOffset.x + (selCenterPX + cosA * c2Local.x - sinA * c2Local.y) * zoomLevel,
                        panOffset.y + (canvasHeight - (selCenterPY + sinA * c2Local.x + cosA * c2Local.y)) * zoomLevel
                    );
                    Vector2 c3Screen = new Vector2(
                        panOffset.x + (selCenterPX + cosA * c3Local.x - sinA * c3Local.y) * zoomLevel,
                        panOffset.y + (canvasHeight - (selCenterPY + sinA * c3Local.x + cosA * c3Local.y)) * zoomLevel
                    );

                    Vector2 screenCenter = new Vector2(
                        panOffset.x + selCenterPX * zoomLevel,
                        panOffset.y + (canvasHeight - selCenterPY) * zoomLevel
                    );

                    // Rotation handle is 24px above the top-mid of the selection box
                    Vector2 topMidScreen = (c2Screen + c3Screen) * 0.5f;
                    Vector2 handleDir = (topMidScreen - screenCenter).normalized;
                    if (handleDir.sqrMagnitude < 0.001f) handleDir = new Vector2(0, -1);
                    Vector2 rotHandleScreen = topMidScreen + handleDir * 24f;

                    // 1. Check Rotation Handle click
                    if (Vector2.Distance(localMouse, rotHandleScreen) <= 16f)
                    {
                        if (!isFloatingSelection) LiftSelectionPixels();
                        selectionDragMode = SelectionDragMode.RotatingSelection;
                        selDragStartAngle = Mathf.Atan2(localMouse.y - screenCenter.y, localMouse.x - screenCenter.x) * Mathf.Rad2Deg;
                        selDragStartRotation = selectionRotation;
                        isDrawing = true;
                        e.Use();
                        return;
                    }

                    // 2. Check Center Move Handle click
                    if (Vector2.Distance(localMouse, screenCenter) <= 14f)
                    {
                        if (!isFloatingSelection) LiftSelectionPixels();
                        selectionDragMode = SelectionDragMode.MovingSelection;
                        selDragStartPixel = currentHoverPixel;
                        selDragStartOffset = floatingOffset;
                        isDrawing = true;
                        e.Use();
                        return;
                    }

                    // 3. If already floating, clicking anywhere inside moves it
                    if (isFloatingSelection && IsPointInsideSelection(currentHoverPixel))
                    {
                        selectionDragMode = SelectionDragMode.MovingSelection;
                        selDragStartPixel = currentHoverPixel;
                        selDragStartOffset = floatingOffset;
                        isDrawing = true;
                        e.Use();
                        return;
                    }

                    // 4. If holding Ctrl or Alt and clicking inside un-floated selection -> lift and move
                    if (!isFloatingSelection && (e.control || e.alt || e.command) && IsPointInSelectionBox(currentHoverPixel))
                    {
                        LiftSelectionPixels();
                        selectionDragMode = SelectionDragMode.MovingSelection;
                        selDragStartPixel = currentHoverPixel;
                        selDragStartOffset = floatingOffset;
                        isDrawing = true;
                        e.Use();
                        return;
                    }

                    // If floating and clicked outside -> commit current floating selection
                    if (isFloatingSelection)
                    {
                        CommitFloatingSelection();
                    }
                }

                // If not dragging a handle, start creating a brand new selection box!
                ClearSelectionState();
                selectionDragMode = SelectionDragMode.CreatingBox;
                strokeStartPixel = new Vector2Int(
                    Mathf.Clamp(currentHoverPixel.x, 0, canvasWidth - 1),
                    Mathf.Clamp(currentHoverPixel.y, 0, canvasHeight - 1)
                );
                isDrawing = true;
                e.Use();
                Repaint();
                return;
            }

            // --- MouseDrag ---
            if (e.type == EventType.MouseDrag && isDrawing)
            {
                if (selectionDragMode == SelectionDragMode.CreatingBox)
                {
                    e.Use();
                    Repaint();
                }
                else if (selectionDragMode == SelectionDragMode.MovingSelection && isFloatingSelection)
                {
                    Vector2Int delta = currentHoverPixel - selDragStartPixel;
                    floatingOffset = selDragStartOffset + new Vector2(delta.x, delta.y);
                    RebuildPreviewTexture();
                    e.Use();
                    Repaint();
                }
                else if (selectionDragMode == SelectionDragMode.RotatingSelection && isFloatingSelection)
                {
                    float selCenterPX = selectionBox.x + floatingOffset.x + selectionBox.width * 0.5f;
                    float selCenterPY = selectionBox.y + floatingOffset.y + selectionBox.height * 0.5f;
                    Vector2 centerScreen = new Vector2(
                        panOffset.x + selCenterPX * zoomLevel,
                        panOffset.y + (canvasHeight - selCenterPY) * zoomLevel
                    );

                    float currentAngle = Mathf.Atan2(localMouse.y - centerScreen.y, localMouse.x - centerScreen.x) * Mathf.Rad2Deg;
                    float angleDelta = currentAngle - selDragStartAngle;
                    selectionRotation = (selDragStartRotation + angleDelta) % 360f;
                    if (selectionRotation < 0f) selectionRotation += 360f;

                    RebuildPreviewTexture();
                    e.Use();
                    Repaint();
                }
                return;
            }

            // --- MouseUp ---
            if (e.type == EventType.MouseUp && isDrawing)
            {
                if (selectionDragMode == SelectionDragMode.CreatingBox)
                {
                    int clampedCurX = Mathf.Clamp(currentHoverPixel.x, 0, canvasWidth - 1);
                    int clampedCurY = Mathf.Clamp(currentHoverPixel.y, 0, canvasHeight - 1);

                    int x0 = Mathf.Min(strokeStartPixel.x, clampedCurX);
                    int y0 = Mathf.Min(strokeStartPixel.y, clampedCurY);
                    int x1 = Mathf.Max(strokeStartPixel.x, clampedCurX);
                    int y1 = Mathf.Max(strokeStartPixel.y, clampedCurY);

                    int selW = x1 - x0 + 1;
                    int selH = y1 - y0 + 1;

                    // If user merely clicked a single pixel without dragging, deselect
                    if (strokeStartPixel.x == clampedCurX && strokeStartPixel.y == clampedCurY)
                    {
                        ClearSelectionState();
                    }
                    else if (selW > 0 && selH > 0)
                    {
                        selectionBox = new RectInt(x0, y0, selW, selH);
                        hasSelection = true;
                        isFloatingSelection = false;
                        floatingOffset = Vector2.zero;
                        selectionRotation = 0f;
                        floatingPixels = null;
                    }
                }

                selectionDragMode = SelectionDragMode.None;
                isDrawing = false;
                e.Use();
                Repaint();
            }
        }

        private void HandleCurveToolInput(Event e)
        {
            Color drawCol = (e.button == 0) ? primaryColor : secondaryColor;

            if (e.type == EventType.MouseDown && e.button == 1)
            {
                if (curveStep > 0)
                {
                    curveStep = 0;
                    if (undoStack.Count > 0)
                    {
                        var b = undoStack.Pop();
                        Array.Copy(b, CurrentActiveCelPixels, b.Length);
                        RebuildPreviewTexture();
                    }
                    e.Use();
                    Repaint();
                    return;
                }
            }

            if (e.type == EventType.MouseDown && e.button == 0)
            {
                if (curveStep == 0)
                {
                    RecordUndo();
                    curveP0 = currentHoverPixel;
                    curveP3 = currentHoverPixel;
                    curveP1 = curveP0;
                    curveP2 = curveP3;
                    isDrawing = true;
                }
                else if (curveStep == 1)
                {
                    curveP1 = currentHoverPixel;
                    curveP2 = currentHoverPixel;
                    isDrawing = true;
                }
                else if (curveStep == 2)
                {
                    curveP2 = currentHoverPixel;
                    isDrawing = true;
                }
                e.Use();
            }
            else if (e.type == EventType.MouseDrag && isDrawing)
            {
                if (curveStep == 0)
                {
                    curveP3 = currentHoverPixel;
                    curveP1 = curveP0;
                    curveP2 = curveP3;
                }
                else if (curveStep == 1)
                {
                    curveP1 = currentHoverPixel;
                    curveP2 = currentHoverPixel;
                }
                else if (curveStep == 2)
                {
                    curveP2 = currentHoverPixel;
                }

                if (undoStack.Count > 0)
                {
                    var baseBuf = undoStack.Peek();
                    Array.Copy(baseBuf, CurrentActiveCelPixels, CurrentActiveCelPixels.Length);
                }

                DrawCubicBezier(curveP0, curveP1, curveP2, curveP3, (Color32)drawCol);
                RebuildPreviewTexture();
                e.Use();
                Repaint();
            }
            else if (e.type == EventType.MouseUp && isDrawing)
            {
                isDrawing = false;
                if (curveStep == 0)
                {
                    curveP3 = currentHoverPixel;
                    curveP1 = curveP0;
                    curveP2 = curveP3;
                    curveStep = 1;
                }
                else if (curveStep == 1)
                {
                    curveP1 = currentHoverPixel;
                    curveStep = 2;
                }
                else if (curveStep == 2)
                {
                    curveP2 = currentHoverPixel;
                    if (undoStack.Count > 0)
                    {
                        var baseBuf = undoStack.Peek();
                        Array.Copy(baseBuf, CurrentActiveCelPixels, CurrentActiveCelPixels.Length);
                    }
                    DrawCubicBezier(curveP0, curveP1, curveP2, curveP3, (Color32)drawCol);
                    curveStep = 0;
                }

                RebuildPreviewTexture();
                e.Use();
                Repaint();
            }
        }

        private bool IsDirectDrawTool(PixelTool tool)
        {
            return tool == PixelTool.Pen || tool == PixelTool.MirrorPen || tool == PixelTool.Eraser || tool == PixelTool.LightenDarken;
        }

        private Vector2Int ScreenToPixelCoords(Vector2 localMouse)
        {
            float normX = (localMouse.x - panOffset.x) / zoomLevel;
            float normY = (localMouse.y - panOffset.y) / zoomLevel;

            int px = Mathf.FloorToInt(normX);
            int py = canvasHeight - 1 - Mathf.FloorToInt(normY);

            return new Vector2Int(px, py);
        }
        #endregion

        #region Pixel Operations & Tools
        private void SetPixelRaw(int x, int y, Color32 color)
        {
            if (x < 0 || x >= canvasWidth || y < 0 || y >= canvasHeight) return;
            CurrentActiveCelPixels[y * canvasWidth + x] = color;
        }

        private Color32 GetPixelRaw(int x, int y)
        {
            if (x < 0 || x >= canvasWidth || y < 0 || y >= canvasHeight) return new Color32(0, 0, 0, 0);
            return CurrentActiveCelPixels[y * canvasWidth + x];
        }

        private void ApplyBrush(int cx, int cy, Color32 color)
        {
            int bHalf = brushSize / 2;
            for (int ox = -bHalf; ox < brushSize - bHalf; ox++)
            {
                for (int oy = -bHalf; oy < brushSize - bHalf; oy++)
                {
                    SetPixelRaw(cx + ox, cy + oy, color);
                }
            }
        }

        private void ApplyToolAt(Vector2Int p, Color drawCol, bool alternateMode)
        {
            if (currentTool == PixelTool.LightenDarken)
            {
                ApplyLightenDarken(p.x, p.y, alternateMode);
                return;
            }

            Color32 c = (Color32)drawCol;
            ApplyBrush(p.x, p.y, c);

            if (currentTool == PixelTool.MirrorPen)
            {
                if (mirrorHorizontal)
                {
                    int mx = canvasWidth - 1 - p.x;
                    ApplyBrush(mx, p.y, c);
                }
                if (mirrorVertical)
                {
                    int my = canvasHeight - 1 - p.y;
                    ApplyBrush(p.x, my, c);
                }
                if (mirrorHorizontal && mirrorVertical)
                {
                    int mx = canvasWidth - 1 - p.x;
                    int my = canvasHeight - 1 - p.y;
                    ApplyBrush(mx, my, c);
                }
            }
        }

        private void ApplyLightenDarken(int cx, int cy, bool darken)
        {
            int bHalf = brushSize / 2;
            for (int ox = -bHalf; ox < brushSize - bHalf; ox++)
            {
                for (int oy = -bHalf; oy < brushSize - bHalf; oy++)
                {
                    int px = cx + ox;
                    int py = cy + oy;
                    if (px < 0 || px >= canvasWidth || py < 0 || py >= canvasHeight) continue;

                    Color32 orig = GetPixelRaw(px, py);
                    if (orig.a == 0) continue;

                    float factor = darken ? (1f - lightenDarkenAmount) : (1f + lightenDarkenAmount);
                    byte r = (byte)Mathf.Clamp(Mathf.RoundToInt(orig.r * factor), 0, 255);
                    byte g = (byte)Mathf.Clamp(Mathf.RoundToInt(orig.g * factor), 0, 255);
                    byte b = (byte)Mathf.Clamp(Mathf.RoundToInt(orig.b * factor), 0, 255);

                    SetPixelRaw(px, py, new Color32(r, g, b, orig.a));
                }
            }
        }

        private void PickColorAt(Vector2Int p, bool setPrimary)
        {
            if (p.x < 0 || p.x >= canvasWidth || p.y < 0 || p.y >= canvasHeight) return;
            // Pick from composite texture
            Color32[] comp = frames[currentFrameIndex].CompositePixels(canvasWidth, canvasHeight, layers);
            Color32 c = comp[p.y * canvasWidth + p.x];
            if (setPrimary) primaryColor = c;
            else secondaryColor = c;
            Repaint();
        }

        private void DrawLineContinuous(Vector2Int from, Vector2Int to, Color drawCol, bool alternateMode)
        {
            int x0 = from.x;
            int y0 = from.y;
            int x1 = to.x;
            int y1 = to.y;

            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                ApplyToolAt(new Vector2Int(x0, y0), drawCol, alternateMode);
                if (x0 == x1 && y0 == y1) break;
                int e2 = 2 * err;
                if (e2 > -dy) { err -= dy; x0 += sx; }
                if (e2 < dx) { err += dx; y0 += sy; }
            }
        }

        private void FloodFill(int startX, int startY, Color32 newColor)
        {
            if (startX < 0 || startX >= canvasWidth || startY < 0 || startY >= canvasHeight) return;

            Color32 targetColor = GetPixelRaw(startX, startY);
            if (ColorsEqual(targetColor, newColor)) return;

            var queue = new Queue<Vector2Int>();
            var visited = new bool[canvasWidth * canvasHeight];

            queue.Enqueue(new Vector2Int(startX, startY));
            visited[startY * canvasWidth + startX] = true;

            while (queue.Count > 0)
            {
                var pt = queue.Dequeue();
                SetPixelRaw(pt.x, pt.y, newColor);

                CheckAndEnqueue(pt.x + 1, pt.y, targetColor, visited, queue);
                CheckAndEnqueue(pt.x - 1, pt.y, targetColor, visited, queue);
                CheckAndEnqueue(pt.x, pt.y + 1, targetColor, visited, queue);
                CheckAndEnqueue(pt.x, pt.y - 1, targetColor, visited, queue);
            }
        }

        private void CheckAndEnqueue(int x, int y, Color32 targetColor, bool[] visited, Queue<Vector2Int> queue)
        {
            if (x < 0 || x >= canvasWidth || y < 0 || y >= canvasHeight) return;
            int index = y * canvasWidth + x;
            if (visited[index]) return;

            if (ColorsEqual(GetPixelRaw(x, y), targetColor))
            {
                visited[index] = true;
                queue.Enqueue(new Vector2Int(x, y));
            }
        }

        private bool ColorsEqual(Color32 c1, Color32 c2)
        {
            return c1.r == c2.r && c1.g == c2.g && c1.b == c2.b && c1.a == c2.a;
        }

        private void CreateShapePreview(Vector2Int start, Vector2Int end, Color drawCol)
        {
            if (undoStack.Count > 0)
            {
                var baseBuf = undoStack.Peek();
                Array.Copy(baseBuf, CurrentActiveCelPixels, CurrentActiveCelPixels.Length);
            }

            CommitShape(start, end, drawCol);
            RebuildPreviewTexture();
        }

        private void CommitShape(Vector2Int start, Vector2Int end, Color drawCol)
        {
            Color32 c = (Color32)drawCol;
            switch (currentTool)
            {
                case PixelTool.Line:
                    DrawBresenhamLine(start.x, start.y, end.x, end.y, c);
                    break;
                case PixelTool.Rectangle:
                    DrawRectangle(start.x, start.y, end.x, end.y, c, false);
                    break;
                case PixelTool.FilledRectangle:
                    DrawRectangle(start.x, start.y, end.x, end.y, c, true);
                    break;
                case PixelTool.Circle:
                    DrawCircle(start.x, start.y, end.x, end.y, c, false);
                    break;
                case PixelTool.FilledCircle:
                    DrawCircle(start.x, start.y, end.x, end.y, c, true);
                    break;
            }
        }

        private void DrawBresenhamLine(int x0, int y0, int x1, int y1, Color32 col)
        {
            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                ApplyBrush(x0, y0, col);
                if (x0 == x1 && y0 == y1) break;
                int e2 = 2 * err;
                if (e2 > -dy) { err -= dy; x0 += sx; }
                if (e2 < dx) { err += dx; y0 += sy; }
            }
        }

        private void DrawCubicBezier(Vector2Int p0, Vector2Int p1, Vector2Int p2, Vector2Int p3, Color32 col)
        {
            float approxDist = Vector2.Distance(p0, p1) + Vector2.Distance(p1, p2) + Vector2.Distance(p2, p3);
            int steps = Mathf.Max(20, Mathf.CeilToInt(approxDist * 2.5f));

            Vector2Int prev = p0;
            for (int i = 0; i <= steps; i++)
            {
                float t = (float)i / steps;
                float u = 1f - t;
                float tt = t * t;
                float uu = u * u;
                float uuu = uu * u;
                float ttt = tt * t;

                Vector2 pt = uuu * (Vector2)p0 + 3f * uu * t * (Vector2)p1 + 3f * u * tt * (Vector2)p2 + ttt * (Vector2)p3;
                Vector2Int curr = new Vector2Int(Mathf.RoundToInt(pt.x), Mathf.RoundToInt(pt.y));

                DrawBresenhamLine(prev.x, prev.y, curr.x, curr.y, col);
                prev = curr;
            }
        }

        private void DrawRectangle(int x0, int y0, int x1, int y1, Color32 col, bool filled)
        {
            int minX = Mathf.Min(x0, x1);
            int maxX = Mathf.Max(x0, x1);
            int minY = Mathf.Min(y0, y1);
            int maxY = Mathf.Max(y0, y1);

            if (filled)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    for (int x = minX; x <= maxX; x++)
                    {
                        ApplyBrush(x, y, col);
                    }
                }
            }
            else
            {
                for (int x = minX; x <= maxX; x++) { ApplyBrush(x, minY, col); ApplyBrush(x, maxY, col); }
                for (int y = minY; y <= maxY; y++) { ApplyBrush(minX, y, col); ApplyBrush(maxX, y, col); }
            }
        }

        private void DrawCircle(int x0, int y0, int x1, int y1, Color32 col, bool filled)
        {
            int cx = (x0 + x1) / 2;
            int cy = (y0 + y1) / 2;
            int rx = Math.Abs(x1 - x0) / 2;
            int ry = Math.Abs(y1 - y0) / 2;

            if (rx <= 0 && ry <= 0) { ApplyBrush(cx, cy, col); return; }

            int radius = Mathf.Max(rx, ry);

            if (filled)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    for (int x = -radius; x <= radius; x++)
                    {
                        if (x * x + y * y <= radius * radius)
                        {
                            ApplyBrush(cx + x, cy + y, col);
                        }
                    }
                }
            }
            else
            {
                int x = radius;
                int y = 0;
                int err = 0;

                while (x >= y)
                {
                    ApplyBrush(cx + x, cy + y, col);
                    ApplyBrush(cx + y, cy + x, col);
                    ApplyBrush(cx - y, cy + x, col);
                    ApplyBrush(cx - x, cy + y, col);
                    ApplyBrush(cx - x, cy - y, col);
                    ApplyBrush(cx - y, cy - x, col);
                    ApplyBrush(cx + y, cy - x, col);
                    ApplyBrush(cx + x, cy - y, col);

                    if (err <= 0) { y += 1; err += 2 * y + 1; }
                    if (err > 0) { x -= 1; err -= 2 * x + 1; }
                }
            }
        }
        #endregion

        #region Selection & Transform System

        private bool IsPointInSelectionBox(Vector2Int p)
        {
            return p.x >= selectionBox.x && p.x < selectionBox.x + selectionBox.width &&
                   p.y >= selectionBox.y && p.y < selectionBox.y + selectionBox.height;
        }

        private bool IsPointInsideSelection(Vector2Int p)
        {
            if (!hasSelection || selectionBox.width <= 0 || selectionBox.height <= 0) return false;

            // Transform point into selection-local space (accounting for offset and rotation)
            float selCenterPX = selectionBox.x + floatingOffset.x + selectionBox.width * 0.5f;
            float selCenterPY = selectionBox.y + floatingOffset.y + selectionBox.height * 0.5f;

            float dx = p.x - selCenterPX + 0.5f;
            float dy = p.y - selCenterPY + 0.5f;

            float radians = -selectionRotation * Mathf.Deg2Rad;
            float cosA = Mathf.Cos(radians);
            float sinA = Mathf.Sin(radians);

            float localX = cosA * dx + sinA * dy;
            float localY = -sinA * dx + cosA * dy;

            float halfW = selectionBox.width * 0.5f;
            float halfH = selectionBox.height * 0.5f;

            return localX >= -halfW && localX <= halfW && localY >= -halfH && localY <= halfH;
        }

        private void LiftSelectionPixels()
        {
            if (!hasSelection || selectionBox.width <= 0 || selectionBox.height <= 0) return;

            RecordUndo();

            int selW = selectionBox.width;
            int selH = selectionBox.height;
            floatingPixels = new Color32[selW * selH];
            var celPixels = CurrentActiveCelPixels;

            // Copy pixels from the active cel into the floating buffer and clear original
            for (int ly = 0; ly < selH; ly++)
            {
                for (int lx = 0; lx < selW; lx++)
                {
                    int cx = selectionBox.x + lx;
                    int cy = selectionBox.y + ly;

                    if (cx >= 0 && cx < canvasWidth && cy >= 0 && cy < canvasHeight)
                    {
                        int celIdx = cy * canvasWidth + cx;
                        floatingPixels[ly * selW + lx] = celPixels[celIdx];
                        celPixels[celIdx] = new Color32(0, 0, 0, 0); // Clear original
                    }
                    else
                    {
                        floatingPixels[ly * selW + lx] = new Color32(0, 0, 0, 0);
                    }
                }
            }

            isFloatingSelection = true;
            floatingOffset = Vector2.zero;
            selectionRotation = 0f;
            RebuildPreviewTexture();
        }

        private Color32[] CompositeFloatingSelection(Color32[] basePixels)
        {
            Color32[] result = new Color32[basePixels.Length];
            Array.Copy(basePixels, result, basePixels.Length);

            if (floatingPixels == null || selectionBox.width <= 0 || selectionBox.height <= 0) return result;

            int selW = selectionBox.width;
            int selH = selectionBox.height;

            // Center of the floating selection in canvas pixel space
            float pivotX = selectionBox.x + floatingOffset.x + selW * 0.5f;
            float pivotY = selectionBox.y + floatingOffset.y + selH * 0.5f;

            // Inverse rotation angle (to map from destination back to source)
            float radians = -selectionRotation * Mathf.Deg2Rad;
            float cosA = Mathf.Cos(radians);
            float sinA = Mathf.Sin(radians);

            // Calculate bounding circle to limit the scan area
            int radius = Mathf.CeilToInt(Mathf.Sqrt(selW * selW + selH * selH) * 0.5f) + 2;
            int scanMinX = Mathf.Max(0, Mathf.FloorToInt(pivotX - radius));
            int scanMaxX = Mathf.Min(canvasWidth - 1, Mathf.CeilToInt(pivotX + radius));
            int scanMinY = Mathf.Max(0, Mathf.FloorToInt(pivotY - radius));
            int scanMaxY = Mathf.Min(canvasHeight - 1, Mathf.CeilToInt(pivotY + radius));

            float halfW = selW * 0.5f;
            float halfH = selH * 0.5f;

            for (int y = scanMinY; y <= scanMaxY; y++)
            {
                for (int x = scanMinX; x <= scanMaxX; x++)
                {
                    // Transform destination pixel to selection-local coords via inverse rotation
                    float dx = x - pivotX + 0.5f;
                    float dy = y - pivotY + 0.5f;

                    float srcX = cosA * dx + sinA * dy + halfW;
                    float srcY = -sinA * dx + cosA * dy + halfH;

                    int sx = Mathf.FloorToInt(srcX);
                    int sy = Mathf.FloorToInt(srcY);

                    if (sx >= 0 && sx < selW && sy >= 0 && sy < selH)
                    {
                        Color32 srcCol = floatingPixels[sy * selW + sx];
                        if (srcCol.a > 0)
                        {
                            int dstIdx = y * canvasWidth + x;
                            Color32 dstCol = result[dstIdx];

                            // Alpha blend source over destination
                            float srcA = srcCol.a / 255f;
                            float dstA = dstCol.a / 255f;
                            float outA = srcA + dstA * (1f - srcA);

                            if (outA > 0f)
                            {
                                float r = (srcCol.r * srcA + dstCol.r * dstA * (1f - srcA)) / outA;
                                float g = (srcCol.g * srcA + dstCol.g * dstA * (1f - srcA)) / outA;
                                float b = (srcCol.b * srcA + dstCol.b * dstA * (1f - srcA)) / outA;
                                result[dstIdx] = new Color32(
                                    (byte)Mathf.Clamp(Mathf.RoundToInt(r), 0, 255),
                                    (byte)Mathf.Clamp(Mathf.RoundToInt(g), 0, 255),
                                    (byte)Mathf.Clamp(Mathf.RoundToInt(b), 0, 255),
                                    (byte)Mathf.Clamp(Mathf.RoundToInt(outA * 255f), 0, 255)
                                );
                            }
                        }
                    }
                }
            }

            return result;
        }

        private void CommitFloatingSelection()
        {
            if (!hasSelection || !isFloatingSelection || floatingPixels == null) 
            {
                ClearSelectionState();
                return;
            }

            // Stamp the rotated floating pixels onto the active cel using nearest-neighbor sampling
            var celPixels = CurrentActiveCelPixels;
            int selW = selectionBox.width;
            int selH = selectionBox.height;

            float pivotX = selectionBox.x + floatingOffset.x + selW * 0.5f;
            float pivotY = selectionBox.y + floatingOffset.y + selH * 0.5f;

            float radians = -selectionRotation * Mathf.Deg2Rad;
            float cosA = Mathf.Cos(radians);
            float sinA = Mathf.Sin(radians);

            int radius = Mathf.CeilToInt(Mathf.Sqrt(selW * selW + selH * selH) * 0.5f) + 2;
            int scanMinX = Mathf.Max(0, Mathf.FloorToInt(pivotX - radius));
            int scanMaxX = Mathf.Min(canvasWidth - 1, Mathf.CeilToInt(pivotX + radius));
            int scanMinY = Mathf.Max(0, Mathf.FloorToInt(pivotY - radius));
            int scanMaxY = Mathf.Min(canvasHeight - 1, Mathf.CeilToInt(pivotY + radius));

            float halfW = selW * 0.5f;
            float halfH = selH * 0.5f;

            for (int y = scanMinY; y <= scanMaxY; y++)
            {
                for (int x = scanMinX; x <= scanMaxX; x++)
                {
                    float dx = x - pivotX + 0.5f;
                    float dy = y - pivotY + 0.5f;

                    float srcX = cosA * dx + sinA * dy + halfW;
                    float srcY = -sinA * dx + cosA * dy + halfH;

                    int sx = Mathf.FloorToInt(srcX);
                    int sy = Mathf.FloorToInt(srcY);

                    if (sx >= 0 && sx < selW && sy >= 0 && sy < selH)
                    {
                        Color32 srcCol = floatingPixels[sy * selW + sx];
                        if (srcCol.a > 0)
                        {
                            int dstIdx = y * canvasWidth + x;
                            Color32 dstCol = celPixels[dstIdx];

                            float srcA = srcCol.a / 255f;
                            float dstA = dstCol.a / 255f;
                            float outA = srcA + dstA * (1f - srcA);

                            if (outA > 0f)
                            {
                                float r = (srcCol.r * srcA + dstCol.r * dstA * (1f - srcA)) / outA;
                                float g = (srcCol.g * srcA + dstCol.g * dstA * (1f - srcA)) / outA;
                                float b = (srcCol.b * srcA + dstCol.b * dstA * (1f - srcA)) / outA;
                                celPixels[dstIdx] = new Color32(
                                    (byte)Mathf.Clamp(Mathf.RoundToInt(r), 0, 255),
                                    (byte)Mathf.Clamp(Mathf.RoundToInt(g), 0, 255),
                                    (byte)Mathf.Clamp(Mathf.RoundToInt(b), 0, 255),
                                    (byte)Mathf.Clamp(Mathf.RoundToInt(outA * 255f), 0, 255)
                                );
                            }
                        }
                    }
                }
            }

            ClearSelectionState();
            RebuildPreviewTexture();
            Repaint();
        }

        private void CancelSelection()
        {
            if (hasSelection && isFloatingSelection && floatingPixels != null)
            {
                // Restore the floating pixels back to their original position (no rotation)
                var celPixels = CurrentActiveCelPixels;
                int selW = selectionBox.width;
                int selH = selectionBox.height;

                for (int ly = 0; ly < selH; ly++)
                {
                    for (int lx = 0; lx < selW; lx++)
                    {
                        int cx = selectionBox.x + lx;
                        int cy = selectionBox.y + ly;

                        if (cx >= 0 && cx < canvasWidth && cy >= 0 && cy < canvasHeight)
                        {
                            Color32 srcCol = floatingPixels[ly * selW + lx];
                            if (srcCol.a > 0)
                            {
                                celPixels[cy * canvasWidth + cx] = srcCol;
                            }
                        }
                    }
                }
            }

            ClearSelectionState();
            RebuildPreviewTexture();
            Repaint();
        }

        private void ClearSelectionState()
        {
            hasSelection = false;
            isFloatingSelection = false;
            floatingPixels = null;
            floatingOffset = Vector2.zero;
            selectionRotation = 0f;
            selectionDragMode = SelectionDragMode.None;
            selectionBox = new RectInt(0, 0, 0, 0);
        }

        private void DeleteSelectionContent()
        {
            if (!hasSelection) return;

            if (isFloatingSelection)
            {
                // Just discard the floating pixels (don't stamp them back)
                ClearSelectionState();
                RebuildPreviewTexture();
                Repaint();
                return;
            }

            // Delete pixels within the selection box on the active cel
            RecordUndo();
            var celPixels = CurrentActiveCelPixels;
            int selW = selectionBox.width;
            int selH = selectionBox.height;

            for (int ly = 0; ly < selH; ly++)
            {
                for (int lx = 0; lx < selW; lx++)
                {
                    int cx = selectionBox.x + lx;
                    int cy = selectionBox.y + ly;

                    if (cx >= 0 && cx < canvasWidth && cy >= 0 && cy < canvasHeight)
                    {
                        celPixels[cy * canvasWidth + cx] = new Color32(0, 0, 0, 0);
                    }
                }
            }

            ClearSelectionState();
            RebuildPreviewTexture();
            Repaint();
        }

        private void SelectAllPixels()
        {
            // If there's an active floating selection, commit it first
            if (hasSelection && isFloatingSelection)
            {
                CommitFloatingSelection();
            }

            currentTool = PixelTool.Selection;
            selectionBox = new RectInt(0, 0, canvasWidth, canvasHeight);
            hasSelection = true;
            isFloatingSelection = false;
            floatingPixels = null;
            floatingOffset = Vector2.zero;
            selectionRotation = 0f;
            selectionDragMode = SelectionDragMode.None;
            Repaint();
        }

        private void CopySelection()
        {
            if (!hasSelection || selectionBox.width <= 0 || selectionBox.height <= 0)
            {
                ShowNotification(new GUIContent(Tr("Chưa chọn vùng để copy!", "No selection to copy!")));
                return;
            }

            clipboardWidth = selectionBox.width;
            clipboardHeight = selectionBox.height;
            clipboardPixels = new Color32[clipboardWidth * clipboardHeight];

            if (isFloatingSelection && floatingPixels != null)
            {
                Array.Copy(floatingPixels, clipboardPixels, floatingPixels.Length);
            }
            else
            {
                var celPixels = CurrentActiveCelPixels;
                for (int ly = 0; ly < clipboardHeight; ly++)
                {
                    for (int lx = 0; lx < clipboardWidth; lx++)
                    {
                        int cx = selectionBox.x + lx;
                        int cy = selectionBox.y + ly;
                        if (cx >= 0 && cx < canvasWidth && cy >= 0 && cy < canvasHeight)
                        {
                            clipboardPixels[ly * clipboardWidth + lx] = celPixels[cy * canvasWidth + cx];
                        }
                        else
                        {
                            clipboardPixels[ly * clipboardWidth + lx] = new Color32(0, 0, 0, 0);
                        }
                    }
                }
            }

            ShowNotification(new GUIContent(Tr($"Đã sao chép {clipboardWidth}x{clipboardHeight}px (Ctrl+C)", $"Copied {clipboardWidth}x{clipboardHeight}px (Ctrl+C)")));
        }

        private void CutSelection()
        {
            if (!hasSelection || selectionBox.width <= 0 || selectionBox.height <= 0) return;
            CopySelection();
            RecordUndo();
            DeleteSelectionContent();
            ShowNotification(new GUIContent(Tr("Đã cắt vùng chọn (Ctrl+X)", "Cut selection (Ctrl+X)")));
        }

        private void PasteSelection()
        {
            if (clipboardPixels == null || clipboardWidth <= 0 || clipboardHeight <= 0)
            {
                ShowNotification(new GUIContent(Tr("Clipboard trống!", "Clipboard is empty!")));
                return;
            }

            if (hasSelection && isFloatingSelection)
            {
                CommitFloatingSelection();
            }

            RecordUndo();
            currentTool = PixelTool.Selection;

            // Target position: if there's an existing selection box, paste slightly offset or center on canvas
            int pasteX = hasSelection ? selectionBox.x : Mathf.Clamp((canvasWidth - clipboardWidth) / 2, 0, canvasWidth - 1);
            int pasteY = hasSelection ? selectionBox.y : Mathf.Clamp((canvasHeight - clipboardHeight) / 2, 0, canvasHeight - 1);

            if (hasSelection && !isFloatingSelection && pasteX == selectionBox.x && pasteY == selectionBox.y)
            {
                pasteX = Mathf.Clamp(pasteX + 2, 0, Mathf.Max(0, canvasWidth - clipboardWidth));
                pasteY = Mathf.Clamp(pasteY - 2, 0, Mathf.Max(0, canvasHeight - clipboardHeight));
            }

            selectionBox = new RectInt(pasteX, pasteY, clipboardWidth, clipboardHeight);
            floatingPixels = new Color32[clipboardWidth * clipboardHeight];
            Array.Copy(clipboardPixels, floatingPixels, clipboardPixels.Length);

            isFloatingSelection = true;
            hasSelection = true;
            floatingOffset = Vector2.zero;
            selectionRotation = 0f;
            selectionDragMode = SelectionDragMode.None;

            RebuildPreviewTexture();
            Repaint();
            ShowNotification(new GUIContent(Tr($"Đã dán {clipboardWidth}x{clipboardHeight}px (Ctrl+V)", $"Pasted {clipboardWidth}x{clipboardHeight}px (Ctrl+V)")));
        }

        private void DuplicateSelection()
        {
            if (!hasSelection || selectionBox.width <= 0 || selectionBox.height <= 0) return;
            CopySelection();
            PasteSelection();
        }

        private void RotateSelectionBy(float degrees)
        {
            if (!hasSelection) return;
            if (!isFloatingSelection) LiftSelectionPixels();
            selectionRotation = (selectionRotation + degrees) % 360f;
            if (selectionRotation < 0f) selectionRotation += 360f;
            RebuildPreviewTexture();
            Repaint();
        }

        private void ShiftFloatingSelection(int dx, int dy)
        {
            if (!hasSelection) return;
            if (!isFloatingSelection) LiftSelectionPixels();
            floatingOffset += new Vector2(dx, dy);
            RebuildPreviewTexture();
            Repaint();
        }

        private void ExtractPaletteFromCanvas(bool replaceExisting)
        {
            var uniqueColors = new List<Color>();
            var activePixels = CurrentActiveCelPixels;
            if (activePixels == null || activePixels.Length == 0) return;

            int addedCount = 0;
            for (int i = 0; i < activePixels.Length; i++)
            {
                Color32 c32 = activePixels[i];
                if (c32.a < 15) continue; // Skip transparent pixels

                Color c = c32;
                bool exists = false;
                for (int u = 0; u < uniqueColors.Count; u++)
                {
                    if (ColorsMatch(uniqueColors[u], c))
                    {
                        exists = true;
                        break;
                    }
                }
                if (!exists)
                {
                    uniqueColors.Add(c);
                }
            }

            if (uniqueColors.Count == 0)
            {
                ShowNotification(new GUIContent(Tr("Không tìm thấy màu nào trong sprite!", "No colors found in sprite!")));
                return;
            }

            if (replaceExisting)
            {
                customPalette.Clear();
                customPalette.AddRange(uniqueColors);
                addedCount = uniqueColors.Count;
            }
            else
            {
                for (int u = 0; u < uniqueColors.Count; u++)
                {
                    bool inPalette = false;
                    for (int p = 0; p < customPalette.Count; p++)
                    {
                        if (ColorsMatch(customPalette[p], uniqueColors[u]))
                        {
                            inPalette = true;
                            break;
                        }
                    }
                    if (!inPalette)
                    {
                        customPalette.Add(uniqueColors[u]);
                        addedCount++;
                    }
                }
            }

            ShowNotification(new GUIContent(Tr($"Đã trích xuất {addedCount} màu từ Sprite!", $"Extracted {addedCount} colors from Sprite!")));
            Repaint();
        }

        #endregion

        #region Transformations
        private void FlipHorizontal()
        {
            RecordUndo();
            var buf = CurrentActiveCelPixels;
            var newBuf = new Color32[buf.Length];
            for (int y = 0; y < canvasHeight; y++)
            {
                for (int x = 0; x < canvasWidth; x++)
                {
                    newBuf[y * canvasWidth + x] = buf[y * canvasWidth + (canvasWidth - 1 - x)];
                }
            }
            Array.Copy(newBuf, buf, buf.Length);
            RebuildPreviewTexture();
        }

        private void FlipVertical()
        {
            RecordUndo();
            var buf = CurrentActiveCelPixels;
            var newBuf = new Color32[buf.Length];
            for (int y = 0; y < canvasHeight; y++)
            {
                for (int x = 0; x < canvasWidth; x++)
                {
                    newBuf[y * canvasWidth + x] = buf[(canvasHeight - 1 - y) * canvasWidth + x];
                }
            }
            Array.Copy(newBuf, buf, buf.Length);
            RebuildPreviewTexture();
        }

        private void Rotate90CW()
        {
            RecordUndo();
            int newW = canvasHeight;
            int newH = canvasWidth;

            foreach (var f in frames)
            {
                foreach (var cel in f.cels)
                {
                    var oldBuf = cel.pixels;
                    var newBuf = new Color32[newW * newH];
                    for (int y = 0; y < canvasHeight; y++)
                    {
                        for (int x = 0; x < canvasWidth; x++)
                        {
                            int nx = y;
                            int ny = newH - 1 - x;
                            newBuf[ny * newW + nx] = oldBuf[y * canvasWidth + x];
                        }
                    }
                    cel.pixels = newBuf;
                    cel.UpdateThumbnail(newW, newH);
                }
            }

            canvasWidth = newW;
            canvasHeight = newH;
            newWidthInput = newW;
            newHeightInput = newH;
            RebuildPreviewTexture();
        }

        private void Rotate90CCW()
        {
            RecordUndo();
            int newW = canvasHeight;
            int newH = canvasWidth;

            foreach (var f in frames)
            {
                foreach (var cel in f.cels)
                {
                    var oldBuf = cel.pixels;
                    var newBuf = new Color32[newW * newH];
                    for (int y = 0; y < canvasHeight; y++)
                    {
                        for (int x = 0; x < canvasWidth; x++)
                        {
                            int nx = newW - 1 - y;
                            int ny = x;
                            newBuf[ny * newW + nx] = oldBuf[y * canvasWidth + x];
                        }
                    }
                    cel.pixels = newBuf;
                    cel.UpdateThumbnail(newW, newH);
                }
            }

            canvasWidth = newW;
            canvasHeight = newH;
            newWidthInput = newW;
            newHeightInput = newH;
            RebuildPreviewTexture();
        }

        private void ShiftCanvas(int dx, int dy)
        {
            RecordUndo();
            var buf = CurrentActiveCelPixels;
            var newBuf = new Color32[buf.Length];
            for (int y = 0; y < canvasHeight; y++)
            {
                for (int x = 0; x < canvasWidth; x++)
                {
                    int sx = (x - dx + canvasWidth) % canvasWidth;
                    int sy = (y - dy + canvasHeight) % canvasHeight;
                    newBuf[y * canvasWidth + x] = buf[sy * canvasWidth + sx];
                }
            }
            Array.Copy(newBuf, buf, buf.Length);
            RebuildPreviewTexture();
        }

        private void InvertColors()
        {
            RecordUndo();
            var buf = CurrentActiveCelPixels;
            for (int i = 0; i < buf.Length; i++)
            {
                var c = buf[i];
                if (c.a > 0)
                {
                    buf[i] = new Color32((byte)(255 - c.r), (byte)(255 - c.g), (byte)(255 - c.b), c.a);
                }
            }
            RebuildPreviewTexture();
        }
        #endregion

        #region File Load, Save & Export Sprite Sheet
        public void LoadFromTexture(Texture2D sourceTex)
        {
            if (sourceTex == null) return;

            string assetPath = AssetDatabase.GetAssetPath(sourceTex);
            targetAssetPath = assetPath;
            targetAsset = sourceTex;

            RenderTexture rt = RenderTexture.GetTemporary(sourceTex.width, sourceTex.height, 0, RenderTextureFormat.ARGB32);
            Graphics.Blit(sourceTex, rt);

            RenderTexture prevRt = RenderTexture.active;
            RenderTexture.active = rt;

            Texture2D readableCopy = new Texture2D(sourceTex.width, sourceTex.height, TextureFormat.RGBA32, false);
            readableCopy.ReadPixels(new Rect(0, 0, sourceTex.width, sourceTex.height), 0, 0);
            readableCopy.Apply();

            RenderTexture.active = prevRt;
            RenderTexture.ReleaseTemporary(rt);

            canvasWidth = readableCopy.width;
            canvasHeight = readableCopy.height;
            newWidthInput = canvasWidth;
            newHeightInput = canvasHeight;

            foreach (var f in frames) f.Dispose();
            frames.Clear();
            layers.Clear();

            layers.Add(new PixelLayer("Layer 1"));
            currentLayerIndex = 0;

            var f1 = new AnimationFrame(1, canvasWidth, canvasHeight);
            Array.Copy(readableCopy.GetPixels32(), f1.cels[0].pixels, f1.cels[0].pixels.Length);
            f1.cels[0].UpdateThumbnail(canvasWidth, canvasHeight);
            frames.Add(f1);

            currentFrameIndex = 0;
            playbackFrame = 0;
            curveStep = 0;

            DestroyImmediate(readableCopy);

            undoStack.Clear();
            redoStack.Clear();

            RebuildPreviewTexture();
            CenterCanvas();
            ExtractPaletteFromCanvas(false);
            Repaint();
        }

        public void SaveOverwrite()
        {
            if (string.IsNullOrEmpty(targetAssetPath) || !File.Exists(targetAssetPath))
            {
                SaveAsNewAsset();
                return;
            }

            try
            {
                byte[] pngData = previewTexture.EncodeToPNG();
                File.WriteAllBytes(targetAssetPath, pngData);
                AssetDatabase.ImportAsset(targetAssetPath, ImportAssetOptions.ForceUpdate);

                TextureImporter importer = AssetImporter.GetAtPath(targetAssetPath) as TextureImporter;
                if (importer != null)
                {
                    bool changed = false;
                    if (importer.filterMode != FilterMode.Point) { importer.filterMode = FilterMode.Point; changed = true; }
                    if (importer.textureCompression != TextureImporterCompression.Uncompressed) { importer.textureCompression = TextureImporterCompression.Uncompressed; changed = true; }
                    if (!importer.isReadable) { importer.isReadable = true; changed = true; }
                    if (changed) importer.SaveAndReimport();
                }

                EditorUtility.DisplayDialog(Tr("Lưu Thành Công", "Save Success"),
                    Tr($"Đã lưu thay đổi vào:\n{targetAssetPath}", $"Saved changes to:\n{targetAssetPath}"), "OK");
            }
            catch (Exception ex)
            {
                EditorUtility.DisplayDialog(Tr("Lỗi Khi Lưu", "Save Error"),
                    Tr($"Không thể lưu file: {ex.Message}", $"Failed to save: {ex.Message}"), "OK");
            }
        }

        public void SaveAsNewAsset()
        {
            string defaultDir = "Assets";
            if (!string.IsNullOrEmpty(targetAssetPath))
            {
                defaultDir = Path.GetDirectoryName(targetAssetPath);
            }

            string defaultName = string.IsNullOrEmpty(targetAssetPath) ? "NewPixelArt.png" : Path.GetFileNameWithoutExtension(targetAssetPath) + "_edited.png";
            string path = EditorUtility.SaveFilePanelInProject(
                Tr("Lưu Ảnh Pixel Art PNG", "Save Pixel Art As PNG"),
                defaultName,
                "png",
                Tr("Nhập tên tệp PNG để lưu:", "Please enter a file name to save the texture to:"),
                defaultDir);

            if (string.IsNullOrEmpty(path)) return;

            try
            {
                byte[] pngData = previewTexture.EncodeToPNG();
                File.WriteAllBytes(path, pngData);
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
                if (importer != null)
                {
                    importer.textureType = TextureImporterType.Sprite;
                    importer.spritePixelsPerUnit = Mathf.Max(canvasWidth, canvasHeight);
                    importer.filterMode = FilterMode.Point;
                    importer.textureCompression = TextureImporterCompression.Uncompressed;
                    importer.isReadable = true;
                    importer.SaveAndReimport();
                }

                targetAssetPath = path;
                targetAsset = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                EditorUtility.DisplayDialog(Tr("Lưu Thành Công", "Save Success"),
                    Tr($"Đã tạo file pixel art mới tại:\n{path}", $"Created new pixel art asset at:\n{path}"), "OK");
            }
            catch (Exception ex)
            {
                EditorUtility.DisplayDialog(Tr("Lỗi Khi Lưu", "Save Error"),
                    Tr($"Không thể lưu file: {ex.Message}", $"Failed to save new file: {ex.Message}"), "OK");
            }
        }

        public void ExportSpriteSheet()
        {
            if (frames.Count == 0) return;

            string defaultDir = "Assets";
            if (!string.IsNullOrEmpty(targetAssetPath))
            {
                defaultDir = Path.GetDirectoryName(targetAssetPath);
            }

            string defaultName = string.IsNullOrEmpty(targetAssetPath) ? "SpriteSheet.png" : Path.GetFileNameWithoutExtension(targetAssetPath) + "_sheet.png";
            string path = EditorUtility.SaveFilePanelInProject(
                Tr("Xuất Sprite Sheet PNG", "Export Sprite Sheet PNG"),
                defaultName,
                "png",
                Tr("Nhập tên file Sprite Sheet:", "Enter Sprite Sheet file name:"),
                defaultDir);

            if (string.IsNullOrEmpty(path)) return;

            try
            {
                int totalFrames = frames.Count;
                int sheetWidth = canvasWidth * totalFrames;
                int sheetHeight = canvasHeight;

                Texture2D sheetTex = new Texture2D(sheetWidth, sheetHeight, TextureFormat.RGBA32, false);
                Color32[] sheetPixels = new Color32[sheetWidth * sheetHeight];

                for (int f = 0; f < totalFrames; f++)
                {
                    var fPixels = frames[f].CompositePixels(canvasWidth, canvasHeight, layers);
                    int xOffset = f * canvasWidth;

                    for (int y = 0; y < canvasHeight; y++)
                    {
                        for (int x = 0; x < canvasWidth; x++)
                        {
                            int sheetIdx = y * sheetWidth + (xOffset + x);
                            int frameIdx = y * canvasWidth + x;
                            sheetPixels[sheetIdx] = fPixels[frameIdx];
                        }
                    }
                }

                sheetTex.SetPixels32(sheetPixels);
                sheetTex.Apply();

                byte[] pngData = sheetTex.EncodeToPNG();
                DestroyImmediate(sheetTex);
                File.WriteAllBytes(path, pngData);
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
                if (importer != null)
                {
                    importer.textureType = TextureImporterType.Sprite;
                    importer.spriteImportMode = SpriteImportMode.Multiple;
                    importer.spritePixelsPerUnit = Mathf.Max(canvasWidth, canvasHeight);
                    importer.filterMode = FilterMode.Point;
                    importer.textureCompression = TextureImporterCompression.Uncompressed;
                    importer.isReadable = true;

                    var sheetMeta = new List<SpriteMetaData>();
                    for (int f = 0; f < totalFrames; f++)
                    {
                        var meta = new SpriteMetaData
                        {
                            name = $"{Path.GetFileNameWithoutExtension(path)}_{f}",
                            rect = new Rect(f * canvasWidth, 0, canvasWidth, canvasHeight),
                            alignment = (int)SpriteAlignment.Center,
                            pivot = new Vector2(0.5f, 0.5f)
                        };
                        sheetMeta.Add(meta);
                    }
                    importer.spritesheet = sheetMeta.ToArray();
                    importer.SaveAndReimport();
                }

                EditorUtility.DisplayDialog(Tr("Xuất Sprite Sheet Thành Công", "Export Sheet Success"),
                    Tr($"Đã xuất Sprite Sheet ({totalFrames} frames) tại:\n{path}\n(Tự động gộp các layer và cắt Sprite Multiple sẵn sàng cho Animator!)",
                       $"Exported Sprite Sheet ({totalFrames} frames) to:\n{path}"), "OK");
            }
            catch (Exception ex)
            {
                EditorUtility.DisplayDialog(Tr("Lỗi Xuất Sheet", "Export Error"),
                    Tr($"Không thể xuất Sprite Sheet: {ex.Message}", $"Failed to export sheet: {ex.Message}"), "OK");
            }
        }
        #endregion
    }
}

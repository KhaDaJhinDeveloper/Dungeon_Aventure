using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class AutoSaveEditor : EditorWindow
{
    private static bool autoSaveEnabled = false;
    private static float autoSaveInterval = 10f; // time mặc định
    private static DateTime lastSaveTime;
    private static bool saveScenes = true;
    private static bool saveAssets = true;
    private static bool showNotifications = true;
    private static bool showGUIAutosave = false;

    // Keys để lưu preferences
    private const string PREF_AUTO_SAVE_ENABLED = "AutoSave_Enabled";
    private const string PREF_AUTO_SAVE_INTERVAL = "AutoSave_Interval";
    private const string PREF_SAVE_SCENES = "AutoSave_SaveScenes";
    private const string PREF_SAVE_ASSETS = "AutoSave_SaveAssets";
    private const string PREF_SHOW_NOTIFICATIONS = "AutoSave_ShowNotifications";

    [MenuItem("Tools/Auto Save Settings")]
    public static void ShowWindow()
    {
        GetWindow<AutoSaveEditor>("Auto Save");
    }

    private void OnEnable()
    {
        // Load preferences
        LoadPreferences();

        // Khởi tạo thời gian lần cuối lưu
        lastSaveTime = DateTime.Now;

        // Đăng ký callback cho update
        EditorApplication.update += OnEditorUpdate;
    }

    private void OnDisable()
    {
        // Hủy đăng ký callback
        EditorApplication.update -= OnEditorUpdate;

        // Lưu preferences
        SavePreferences();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space(10);

        // Title
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 16;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.MiddleCenter;

        EditorGUILayout.LabelField("Unity Auto Save Tool", titleStyle);

        EditorGUILayout.Space(10);

        // Đường kẻ ngang
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.Space(5);

        // Main enable/disable toggle
        EditorGUI.BeginChangeCheck();
        autoSaveEnabled = EditorGUILayout.Toggle("Enable Auto Save", autoSaveEnabled);
        showGUIAutosave = EditorGUILayout.Toggle("Show notification AutoSave", showGUIAutosave);
        if (EditorGUI.EndChangeCheck())
        {
            if (autoSaveEnabled)
            {
                lastSaveTime = DateTime.Now;
                Debug.Log("Auto Save: Enabled");
            }
            else
            {
                Debug.Log("Auto Save: Disabled");
            }
        }

        EditorGUILayout.Space(10);

        // Settings group
        EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);

        EditorGUI.BeginDisabledGroup(!autoSaveEnabled);

        // Interval slider
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Save Interval:", GUILayout.Width(100));
        autoSaveInterval = EditorGUILayout.Slider(autoSaveInterval, 60f, 1800f); // 1 phút đến 30 phút
        EditorGUILayout.LabelField($"{(int)(autoSaveInterval / 60)} min {(int)(autoSaveInterval % 60)} sec", GUILayout.Width(80));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(5);

        // What to save options
        EditorGUILayout.LabelField("What to save:", EditorStyles.boldLabel);
        saveScenes = EditorGUILayout.Toggle("Save Scenes", saveScenes);
        saveAssets = EditorGUILayout.Toggle("Save Assets", saveAssets);

        EditorGUILayout.Space(5);

        // Notification option
        showNotifications = EditorGUILayout.Toggle("Show Notifications", showNotifications);

        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space(10);

        // Status group
        EditorGUILayout.LabelField("Status", EditorStyles.boldLabel);

        if (autoSaveEnabled)
        {
            float timeSinceLastSave = (float)(DateTime.Now - lastSaveTime).TotalSeconds;
            float timeUntilNextSave = autoSaveInterval - timeSinceLastSave;

            if (timeUntilNextSave > 0)
            {
                EditorGUILayout.LabelField($"Next save in: {(int)(timeUntilNextSave / 60)}:{(int)(timeUntilNextSave % 60):00}");
            }
            else
            {
                EditorGUILayout.LabelField("Saving...");
            }

            EditorGUILayout.LabelField($"Last save: {lastSaveTime:HH:mm:ss}");
        }
        else
        {
            EditorGUILayout.LabelField("Auto Save is disabled");
        }

        EditorGUILayout.Space(10);

        // Manual save button
        if (GUILayout.Button("Save Now", GUILayout.Height(30)))
        {
            PerformSave();
        }

        EditorGUILayout.Space(5);

        // Info box
        EditorGUILayout.HelpBox("This tool automatically saves your scenes and assets at the specified interval. " +
                               "Make sure to test your project regularly and keep backups!", MessageType.Info);
    }

    private static void OnEditorUpdate()
    {
        if (!autoSaveEnabled) return;

        // Kiểm tra nếu đã đến thời gian lưu
        if ((DateTime.Now - lastSaveTime).TotalSeconds >= autoSaveInterval)
        {
            PerformSave();
        }
    }

    private static void PerformSave()
    {
        try
        {
            bool savedSomething = false;

            if (saveScenes)
            {
                // Lưu scene hiện tại
                if (EditorSceneManager.SaveOpenScenes())
                {
                    savedSomething = true;
                    Debug.Log("Auto Save: Scenes saved successfully");
                }
            }

            if (saveAssets)
            {
                // Lưu assets
                AssetDatabase.SaveAssets();
                savedSomething = true;
                Debug.Log("Auto Save: Assets saved successfully");
            }

            if (savedSomething)
            {
                lastSaveTime = DateTime.Now;

                if (showNotifications && showGUIAutosave)
                {
                    EditorUtility.DisplayDialog("Auto Save",
                        $"Project saved automatically at {lastSaveTime:HH:mm:ss}", "OK");
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Auto Save Error: {e.Message}");
        }
    }

    private void LoadPreferences()
    {
        autoSaveEnabled = EditorPrefs.GetBool(PREF_AUTO_SAVE_ENABLED, false);
        autoSaveInterval = EditorPrefs.GetFloat(PREF_AUTO_SAVE_INTERVAL, 300f);
        saveScenes = EditorPrefs.GetBool(PREF_SAVE_SCENES, true);
        saveAssets = EditorPrefs.GetBool(PREF_SAVE_ASSETS, true);
        showNotifications = EditorPrefs.GetBool(PREF_SHOW_NOTIFICATIONS, true);
    }

    private void SavePreferences()
    {
        EditorPrefs.SetBool(PREF_AUTO_SAVE_ENABLED, autoSaveEnabled);
        EditorPrefs.SetFloat(PREF_AUTO_SAVE_INTERVAL, autoSaveInterval);
        EditorPrefs.SetBool(PREF_SAVE_SCENES, saveScenes);
        EditorPrefs.SetBool(PREF_SAVE_ASSETS, saveAssets);
        EditorPrefs.SetBool(PREF_SHOW_NOTIFICATIONS, showNotifications);
    }
}

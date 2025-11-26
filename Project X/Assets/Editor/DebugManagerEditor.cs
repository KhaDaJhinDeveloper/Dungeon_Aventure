using UnityEngine;
using UnityEditor;

public class DebugManagerEditor : EditorWindow
{
    private bool enableLogs = true;
    private bool enableWarnings = true;
    private bool enableErrors = true;
    
    [MenuItem("Tools/Debug Manager")]
    public static void ShowWindow()
    {
        GetWindow<DebugManagerEditor>("Debug Manager");
    }
    
    private void OnEnable()
    {
        LoadSettings();
    }
    
    private void LoadSettings()
    {
        enableLogs = PlayerPrefs.GetInt("Debug_EnableLogs", 1) == 1;
        enableWarnings = PlayerPrefs.GetInt("Debug_EnableWarnings", 1) == 1;
        enableErrors = PlayerPrefs.GetInt("Debug_EnableErrors", 1) == 1;
    }
    
    private void OnGUI()
    {
        GUILayout.Label("Debug Log Settings", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        EditorGUILayout.BeginVertical("box");
        
        enableLogs = EditorGUILayout.Toggle("Enable Logs", enableLogs);
        enableWarnings = EditorGUILayout.Toggle("Enable Warnings", enableWarnings);
        enableErrors = EditorGUILayout.Toggle("Enable Errors", enableErrors);
        
        EditorGUILayout.Space();
        
        EditorGUILayout.BeginHorizontal();
        
        if (GUILayout.Button("Apply Settings"))
        {
            ApplySettings();
        }
        
        if (GUILayout.Button("Disable All"))
        {
            enableLogs = false;
            enableWarnings = false;
            enableErrors = false;
            ApplySettings();
        }
        
        if (GUILayout.Button("Enable All"))
        {
            enableLogs = true;
            enableWarnings = true;
            enableErrors = true;
            ApplySettings();
        }
        
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space();
        
        EditorGUILayout.HelpBox(
            "Settings are saved to PlayerPrefs and will persist across sessions.\n" +
            "Changes apply immediately to DebugLogger class.",
            MessageType.Info
        );
        
        EditorGUILayout.EndVertical();
        
        // Show current runtime status
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Current Runtime Status:", EditorStyles.boldLabel);
        EditorGUILayout.LabelField($"Logs: {(DebugLogger.EnableLogs ? "Enabled" : "Disabled")}");
        EditorGUILayout.LabelField($"Warnings: {(DebugLogger.EnableWarnings ? "Enabled" : "Disabled")}");
        EditorGUILayout.LabelField($"Errors: {(DebugLogger.EnableErrors ? "Enabled" : "Disabled")}");
    }
    
    private void ApplySettings()
    {
        DebugLogger.EnableLogs = enableLogs;
        DebugLogger.EnableWarnings = enableWarnings;
        DebugLogger.EnableErrors = enableErrors;
        
        EditorUtility.DisplayDialog("Debug Manager", "Settings applied successfully!", "OK");
    }
}

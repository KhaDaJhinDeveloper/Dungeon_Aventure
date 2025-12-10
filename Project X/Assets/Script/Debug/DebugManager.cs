using UnityEngine;

public class DebugManager : Singleton<DebugManager>
{
//    [Header("Debug Settings")]
//    [SerializeField] private bool enableLogsInEditor = true;
//    [SerializeField] private bool enableLogsInBuild = false;
//    [SerializeField] private bool enableWarningsInEditor = true;
//    [SerializeField] private bool enableWarningsInBuild = false;
//    [SerializeField] private bool enableErrorsInEditor = true;
//    [SerializeField] private bool enableErrorsInBuild = true; // Errors nên luôn bật

//    protected override void Awake()
//    {
//        base.Awake();
//        ApplySettings();
//    }

//    private void ApplySettings()
//    {
//#if UNITY_EDITOR
//        DebugLogger.EnableLogs = enableLogsInEditor;
//        DebugLogger.EnableWarnings = enableWarningsInEditor;
//        DebugLogger.EnableErrors = enableErrorsInEditor;
//#else
//        DebugLogger.EnableLogs = enableLogsInBuild;
//        DebugLogger.EnableWarnings = enableWarningsInBuild;
//        DebugLogger.EnableErrors = enableErrorsInBuild;
//#endif
//    }

//    // Method để thay đổi settings runtime
//    public void SetLogsEnabled(bool enabled)
//    {
//        DebugLogger.EnableLogs = enabled;
//    }

//    public void SetWarningsEnabled(bool enabled)
//    {
//        DebugLogger.EnableWarnings = enabled;
//    }

//    public void SetErrorsEnabled(bool enabled)
//    {
//        DebugLogger.EnableErrors = enabled;
//    }

//    public void ToggleAllLogs()
//    {
//        bool current = DebugLogger.EnableLogs;
//        DebugLogger.EnableLogs = !current;
//        DebugLogger.EnableWarnings = !current;
//        DebugLogger.EnableErrors = !current;
//    }
}
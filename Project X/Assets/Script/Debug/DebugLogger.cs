using UnityEngine;
using System.Diagnostics;

/// <summary>
/// Static class để thay thế Debug.Log với khả năng bật/tắt
/// Đảm bảo không có debug trong build bằng Conditional attribute
/// </summary>
public static class DebugLogger
{
    private static bool _enableLogs = true;
    private static bool _enableWarnings = true;
    private static bool _enableErrors = true;

    // Settings được load từ PlayerPrefs hoặc ScriptableObject
    public static bool EnableLogs
    {
        get => _enableLogs;
        set
        {
            _enableLogs = value;
            PlayerPrefs.SetInt("Debug_EnableLogs", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public static bool EnableWarnings
    {
        get => _enableWarnings;
        set
        {
            _enableWarnings = value;
            PlayerPrefs.SetInt("Debug_EnableWarnings", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public static bool EnableErrors
    {
        get => _enableErrors;
        set
        {
            _enableErrors = value;
            PlayerPrefs.SetInt("Debug_EnableErrors", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    static DebugLogger()
    {
        // Load settings từ PlayerPrefs
        _enableLogs = PlayerPrefs.GetInt("Debug_EnableLogs", 1) == 1;
        _enableWarnings = PlayerPrefs.GetInt("Debug_EnableWarnings", 1) == 1;
        _enableErrors = PlayerPrefs.GetInt("Debug_EnableErrors", 1) == 1;
    }

    // Sử dụng Conditional attribute + #if UNITY_EDITOR để đảm bảo code không được compile vào build
    // Conditional: Loại bỏ method calls trong build
    // #if UNITY_EDITOR: Đảm bảo code bên trong chỉ compile trong Editor
    // Dùng UnityEngine.Debug để tránh conflict với System.Diagnostics.Debug
    [Conditional("UNITY_EDITOR")]
    public static void Log(object message)
    {
#if UNITY_EDITOR
        if (_enableLogs)
            UnityEngine.Debug.Log(message);
#endif
    }

    [Conditional("UNITY_EDITOR")]
    public static void Log(object message, Object context)
    {
#if UNITY_EDITOR
        if (_enableLogs)
            UnityEngine.Debug.Log(message, context);
#endif
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogFormat(string format, params object[] args)
    {
#if UNITY_EDITOR
        if (_enableLogs)
            UnityEngine.Debug.LogFormat(format, args);
#endif
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogWarning(object message)
    {
#if UNITY_EDITOR
        if (_enableWarnings)
            UnityEngine.Debug.LogWarning(message);
#endif
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogWarning(object message, Object context)
    {
#if UNITY_EDITOR
        if (_enableWarnings)
            UnityEngine.Debug.LogWarning(message, context);
#endif
    }

    // Errors: Có thể giữ lại trong build để debug production issues
    // Nếu muốn loại bỏ hoàn toàn, uncomment [Conditional("UNITY_EDITOR")] bên dưới
    // [Conditional("UNITY_EDITOR")]
    public static void LogError(object message)
    {
#if UNITY_EDITOR
        if (_enableErrors)
            UnityEngine.Debug.LogError(message);
#else
        // Trong build, có thể log vào file hoặc bỏ qua hoàn toàn
        // Code này sẽ không được compile vào build nếu dùng Conditional
#endif
    }

    // [Conditional("UNITY_EDITOR")]
    public static void LogError(object message, Object context)
    {
#if UNITY_EDITOR
        if (_enableErrors)
            UnityEngine.Debug.LogError(message, context);
#endif
    }

    // Method để tắt tất cả
    public static void DisableAll()
    {
        EnableLogs = false;
        EnableWarnings = false;
        EnableErrors = false;
    }

    // Method để bật tất cả
    public static void EnableAll()
    {
        EnableLogs = true;
        EnableWarnings = true;
        EnableErrors = true;
    }
}
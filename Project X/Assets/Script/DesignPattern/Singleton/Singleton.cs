using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T>: MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static bool _isQuitting;

    public static bool HasInstance => _instance != null;

    public static bool TryGetInstance(out T instance)
    {
        if (_instance == null && !_isQuitting)
            _instance = FindFirstObjectByType<T>();

        instance = _instance;
        return instance != null;
    }

    public static T Instance
    {
        get
        {
            if (_isQuitting) return null;

            if (_instance == null)
            {
                _instance = FindFirstObjectByType<T>();
                if (_instance == null && Application.isPlaying)
                {
                    GameObject _newobj = new GameObject("Auto Generated" + typeof(T).Name);
                    _instance = _newobj.AddComponent<T>();
                    DontDestroyOnLoad(_newobj);
                }
            }
            return _instance;
        }
    }
    protected virtual void Awake()
    {
        _isQuitting = false;

        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this) Destroy(gameObject);
    }

    protected virtual void OnApplicationQuit()
    {
        _isQuitting = true;
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }
}

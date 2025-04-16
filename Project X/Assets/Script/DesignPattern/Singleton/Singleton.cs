using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T>: MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<T>();
                if(_instance == null)
                {
                    GameObject _newobj = new GameObject("Auto Generated" + typeof(T));
                    _instance = _newobj.AddComponent<T>();
                    DontDestroyOnLoad(_newobj);
                }
            }  
            return _instance;
        }
    }
    protected virtual void Awake()
    {
        if(_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(this);
        }
        else if(_instance == this) Destroy(this);
    }
}

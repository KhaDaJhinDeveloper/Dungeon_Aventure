using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneExtensions
{ 
    public static string GetCurrentSceneName()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        return SceneManager.GetActiveScene().name;
    }
}

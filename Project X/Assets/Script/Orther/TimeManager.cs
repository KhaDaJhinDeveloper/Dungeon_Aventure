using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static void TimePause()
    {
        Time.timeScale = 0f;
    }    
    public static void TimeResume()
    {
        Time.timeScale = 1f;
    }    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSetUp : MonoBehaviour
{
    private Canvas canvas;
    private void Start()
    {
        this.canvas = GetComponent<Canvas>();
        if (this.canvas !=null )
        {   
            this.canvas.renderMode = RenderMode.ScreenSpaceCamera;
            this.canvas.worldCamera = Camera.main;
        }            
    }
}

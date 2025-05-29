using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ScaleShadow : MonoBehaviour
{
    private CircleCollider2D cl;
    private Light2D Light2D;
    private void Start()
    {
        this.cl = GetComponent<CircleCollider2D>();
        this.Light2D = GetComponent<Light2D>();
        this.cl.radius = Light2D.pointLightOuterRadius - Light2D.pointLightOuterRadius * 0.15f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ScaleShadow : MonoBehaviour
{
    [HideInInspector] public float radiusOriginal;
    [HideInInspector] public CircleCollider2D cl;
    [HideInInspector] public Light2D Light2D;
    private void Start()
    {
        this.cl = GetComponent<CircleCollider2D>();
        this.Light2D = GetComponent<Light2D>();
        this.radiusOriginal = Light2D.pointLightOuterRadius;
        this.cl.radius = this.radiusOriginal;
    }
}

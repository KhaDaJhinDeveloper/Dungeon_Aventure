using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerStatusManager : MonoBehaviour
{
    private PlayerStats playerStats;
    private ScaleShadow scaleShadow;
    private int speedOriginal;
    void Start()
    {
        this.playerStats = GameObject.FindWithTag(TagManager.TAG_PLAYER).GetComponent<PlayerStats>();
        this.scaleShadow = GameObject.FindWithTag(TagManager.TAG_PLAYER).GetComponentInChildren<ScaleShadow>();
        IndexDefault();
    }
    public void StatusNormal()
    {
        this.playerStats.Speed = this.speedOriginal;
        if (this.scaleShadow.Light2D.pointLightOuterRadius < this.scaleShadow.radiusOriginal)
        {
            this.scaleShadow.Light2D.pointLightOuterRadius += 3 * Time.deltaTime;
            if (this.scaleShadow.Light2D.pointLightOuterRadius >= this.scaleShadow.radiusOriginal)
            {
                this.scaleShadow.Light2D.pointLightOuterRadius = this.scaleShadow.radiusOriginal;
            }
            this.scaleShadow.cl.radius = this.scaleShadow.Light2D.pointLightOuterRadius;
        }
    }    
    public void StatusHungry()
    {
        this.playerStats.Speed = 4;
        if (this.scaleShadow.Light2D.pointLightOuterRadius < this.scaleShadow.radiusOriginal)
        {
            this.scaleShadow.Light2D.pointLightOuterRadius += 3 * Time.deltaTime;
            if (this.scaleShadow.Light2D.pointLightOuterRadius >= this.scaleShadow.radiusOriginal )
            {
                this.scaleShadow.Light2D.pointLightOuterRadius = this.scaleShadow.radiusOriginal;
            }
            this.scaleShadow.cl.radius = this.scaleShadow.Light2D.pointLightOuterRadius;
        }
    }
    public void StatusThirsty()
    {
        this.playerStats.Speed = 4;
        if (this.scaleShadow.Light2D.pointLightOuterRadius >= this.scaleShadow.radiusOriginal * 0.3f)
        {
            this.scaleShadow.Light2D.pointLightOuterRadius -= 3 * Time.deltaTime;
            if (this.scaleShadow.Light2D.pointLightOuterRadius <= this.scaleShadow.radiusOriginal * 0.3f)
            {
                this.scaleShadow.Light2D.pointLightOuterRadius = this.scaleShadow.radiusOriginal * 0.3f;
            }
            this.scaleShadow.cl.radius = this.scaleShadow.Light2D.pointLightOuterRadius;
        }
    }    
    void IndexDefault()
    {
        this.speedOriginal = this.playerStats.Speed;
    }    
}

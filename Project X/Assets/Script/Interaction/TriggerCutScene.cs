using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerCutScene : BaseInteraction
{
    private PlayableDirector cutscene;
    private SceneBossState bossState;
    private Collider2D colli;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.cutscene = GameObject.FindObjectOfType<PlayableDirector>();
        this.bossState = GameObject.FindObjectOfType<SceneBossState>();
        this.colli = GetComponent<Collider2D>();
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
            StartCoroutine(CutScene());          
    }
    IEnumerator CutScene()
    {
        this.colli.enabled = false;
        float time = (float)this.cutscene.duration + 0.2f;
        this.cutscene.Play();
        yield return new WaitForSeconds(time);
        this.bossState.StartBossFight();
    }    
}

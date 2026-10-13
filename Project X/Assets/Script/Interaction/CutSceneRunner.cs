using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class CutSceneRunner : BaseInteraction
{
    #region Input
    private PlayableDirector cutscene;
    private SceneBossState bossState;
    private GameObject player;
    private GameObject boss;
    [SerializeField] private KeyPool keyBoss;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform bossTransform;
    private Collider2D colli;
    public string CutSceneID;
    #endregion
    protected override void Start()
    {
        base.Start();
        if(GameSceneStateManager.Instance.HasCutSceneID(this.CutSceneID))
        {
            GameStateSceneName data = GameSceneStateManager.Instance.GetData();
            this.canInteract = data.bossState.canInteract;
            this.bossState.LoadState(data.bossState.isEnd);
            if (!this.canInteract)
                DisableObject();
        }           
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.cutscene = GetComponent<PlayableDirector>();
        this.bossState = GetComponent<SceneBossState>();
        this.colli = GetComponent<Collider2D>();
        this.player = GameObject.FindGameObjectWithTag(TagManager.TAG_PLAYER);
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(TagManager.TAG_PLAYER) && this.canInteract)
            StartCoroutine(CutScene());          
    }
    IEnumerator CutScene()
    {
        this.colli.enabled = false;
        float time = (float)this.cutscene.duration + 0.2f;
        this.cutscene.Play();
        yield return new WaitForSeconds(time);
        this.canInteract = false;
        this.bossState.StartBossFight();
    }
    public void StartCutScene()
    {
        this.player.SetActive(false);
    }
    public void EndCutScene()
    {
        if (this.boss == null)
            this.boss = ObjectPooling.Instance?.GetPool(this.keyBoss);
        SetTransform(this.boss.transform, this.bossTransform);
        this.player.SetActive(true);
        SetTransform(this.player.transform, this.playerTransform);
    }
    void SetTransform(Transform origin, Transform target)
    {
        origin.position = target.position;
    }
    public override void DisableObject()
    {
        base.DisableObject();
        this.canInteract = false;
        this.colli.enabled =false;
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ladder : BaseInteraction
{
    [SerializeField] private string targetObjectName;
    private Transform player;
    private bool canTeleport = false;
    private Transform target;
    private bool istele = false;
    protected override void Start()
    {
        base.Start();
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.player = GameObject.FindGameObjectWithTag(TagManager.TAG_PLAYER).transform;
        this.target = GameObject.Find(targetObjectName).transform;
    }
    protected override void Update()
    {
        if (this.canTeleport == true)
            if (Input.GetKeyDown(KeyCode.E))
                TeleportPlayer();
    }
    private void TeleportPlayer()
    {
        if (this.target != null)
            StartCoroutine(Tele(1f));
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
            EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_ShowButtonTrigger);
            this.canTeleport = true;
        }    
    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
            EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_ShowButtonTrigger);
            this.canTeleport = false;
        }
    }
    IEnumerator Tele(float duration)
    {
        if (!this.istele)
        {
            this.istele = true;
            yield return new WaitForSeconds(duration);
            this.player.position = this.target.transform.position;
            this.istele = false;
        }
    }
}

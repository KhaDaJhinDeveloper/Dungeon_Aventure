using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : BaseInteraction
{
    public string nameScene;
    private bool isLoad = false;
    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            if(this.isLoad)
                StartCoroutine(LoadLevel());
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
            EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_ShowButtonTrigger);
            this.isLoad = true;
        }    
    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
            EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_ShowButtonTrigger);
            this.isLoad= false;
        }
    }
    IEnumerator LoadLevel()
    {    
        this.isLoad = false;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(nameScene);
    }    
}

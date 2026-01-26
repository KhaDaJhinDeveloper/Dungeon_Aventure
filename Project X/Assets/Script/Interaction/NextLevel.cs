using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : BaseInteraction
{
    public NameScene nameScene;
    private bool isLoad = false;
    [SerializeField] private bool allowloadData =true;
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
            EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_HiddenButtonTrigger);
            this.isLoad= false;
        }
    }
    IEnumerator LoadLevel()
    {
        string namecurrentscene = SceneManager.GetActiveScene().name;
        if( namecurrentscene != "LevelTutorial")
        {
            GameSaveManager.Instance.SaveDataWhenPlay();
            this.isLoad = false;
        }
        yield return new WaitForSeconds(1f);
        SceneManager.sceneLoaded += OnSceneLoadedForPlayLoad;
        SceneManager.LoadScene(this.nameScene.ToString());
    }
    private void OnSceneLoadedForPlayLoad(Scene scene, LoadSceneMode mode)
    {
        if (this.allowloadData)
            GameSaveManager.Instance.LoadDataWhenPlay();
        else
            GameSaveManager.Instance.DeleteAllDataLocal();
        SceneManager.sceneLoaded -= OnSceneLoadedForPlayLoad;
    }
}

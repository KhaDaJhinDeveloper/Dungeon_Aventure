using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TestDOTWEEN : MonoBehaviour
{
    //public GameObject panelTestRight;
    //public GameObject panelTestDown;
    //Vector3 startpos;
    //Vector3 startposdown;
    public GameObject cubeObject;
    public CanvasGroup gr;
    public float moveDistance;
    public float time;
    void Start()
    {
        //startpos = panelTestRight.transform.position;
        //startposdown = panelTestDown.transform.position;
    }
    void Update()
    {
        //PanelRight();
        //PanelDown();
        if (Input.GetKeyDown(KeyCode.X))
        {
            Test();
        }
    }
    //void PanelRight()
    //{
    //    if (Input.GetKeyDown(KeyCode.X))
    //    {
    //        panelTestRight.SetActive(true);
    //        panelTestRight.transform.DOKill();
    //        panelTestRight.transform.position = startpos;
    //        panelTestRight.transform.DOMoveX(panelTestRight.transform.position.x - moveDistance, 0.5f).SetDelay(0.1f).SetUpdate(true);
    //        TimeManager.TimePause();
    //    }
    //    if (Input.GetKeyDown(KeyCode.Z))
    //    {
    //        panelTestRight.transform.DOMoveX(panelTestRight.transform.position.x + moveDistance, 0.5f).SetUpdate(true).OnComplete(() => { panelTestRight.SetActive(false); TimeManager.TimeResume(); });
    //    }
    //}
    //void PanelDown()
    //{
    //    if (Input.GetKeyDown(KeyCode.X))
    //    {
    //        panelTestDown.SetActive(true);
    //        panelTestDown.transform.DOKill();
    //        panelTestDown.transform.position = startposdown;
    //        panelTestDown.transform.DOMoveY(panelTestDown.transform.position.y + moveDistance, 0.5f).SetDelay(0.1f).SetUpdate(true);
    //        TimeManager.TimePause();
    //    }
    //    if (Input.GetKeyDown(KeyCode.Z))
    //    {
    //        panelTestDown.transform.DOMoveY(panelTestDown.transform.position.y - moveDistance, 0.5f).SetUpdate(true).OnComplete(() => { panelTestDown.SetActive(false); TimeManager.TimeResume(); });
    //    }
    //}
    void Test()
    {
        cubeObject.transform.DOKill();
        //cubeObject.transform.DOMoveX(moveDistance,time);
        //cubeObject.transform.DORotate(new Vector3(0, 180, 0), 0.5f);
        //cubeObject.transform.DOScale(1.5f, 0.5f).SetLoops(-1, LoopType.Yoyo); 
        //cubeObject.transform.DOLocalMove(new Vector3(0, 0, 0),time).SetLoops(-1, LoopType.Restart);
        cubeObject.transform.DOLocalMove(new Vector3(0, 0, 0), time).SetEase(Ease.OutBounce).SetUpdate(true);
        gr.DOFade(0.5f,time);
    }
    private void OnDestroy()
    {
        cubeObject.transform.DOKill();
    }
}

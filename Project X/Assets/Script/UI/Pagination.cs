using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Pagination : MonoBehaviour
{
    [SerializeField] List<Button> listButton = new List<Button>();
    [SerializeField] GameObject[] page;
    void Start()
    {
        for (int i = 0; i< listButton.Count; i++)
        {
            int index = i;
            listButton[i].onClick.AddListener(() => ShowPage(index));
        }    
        ShowPage(0);
    }
    void ShowPage(int index)
    {
        foreach (var indexpage in page)
        {
            indexpage.SetActive(false);
        }
        this.page[index].SetActive(true);
    }    
}

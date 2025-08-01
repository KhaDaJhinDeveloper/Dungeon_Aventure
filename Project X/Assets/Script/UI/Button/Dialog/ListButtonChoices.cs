using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListButtonChoices : MonoBehaviour
{
    public Button[] arrayButtonChoices;
    private void Start()
    {
        HideButton();
    }
    public void HideButton()
    {
        for (int i = 0; i < this.arrayButtonChoices.Length; i++)
        {
            this.arrayButtonChoices[i].gameObject.SetActive(false);
        }    
    }  
    public void ActiveButton(int length)
    {
        for (int i = 0; i < length; i++)
        {
            this.arrayButtonChoices[i].gameObject.SetActive(true);
        }
    }   
}

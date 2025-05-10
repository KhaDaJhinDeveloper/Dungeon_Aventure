using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHideContrroll : MonoBehaviour
{
    [SerializeField] GameObject heaalUI;
    private bool isHide;
    private void FixedUpdate()
    {
         this.heaalUI.SetActive(this.isHide);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Light"))
        {
            this.isHide = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Light"))
        {
            isHide = false;
        }
    }
}

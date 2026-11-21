using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTriggerTest : MonoBehaviour
{
    [SerializeField]ItemDetailsShop itemShop;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
            EventManager.Instance.TriggerEvent(NameEvent.Event_ShopSystem_GetItemDetails, this.itemShop);
            EventManager.Instance.TriggerEvent(NameEvent.Event_OpenShopUI);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
            EventManager.Instance?.TriggerEvent(NameEvent.Event_CloseShopUI);
        }
    }
}

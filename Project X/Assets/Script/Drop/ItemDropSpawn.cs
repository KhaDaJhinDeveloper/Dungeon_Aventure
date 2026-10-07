using Unity.VisualScripting;
using UnityEngine;

public class ItemDropSpawn : MonoBehaviour
{
    public static ItemDropSpawn itemDropSpawn_Instance;
    public ItemDropRate[] itemDropRates;
    public KeyPool[] weaponNormal;
    public KeyPool[] weaponLegend;
    void Awake()
    {
        itemDropSpawn_Instance = this;
    }
    public void DropItem(Vector3 posDrop, int coinCout)
    {
        int index = Random.Range(1, 5);
        if(posDrop == null) return;
        for(int i = 0; i < coinCout; i ++)
        {
            GameObject coinDrop = ObjectPooling.Instance.GetPool(KeyPool.KEY_ITEM_GOLD);
            coinDrop.transform.position = RandomPosDrop(posDrop);
        }
        for(int j = 0; j < index; j++)
        {
            GameObject manaDrop = ObjectPooling.Instance.GetPool(KeyPool.KEY_INTERACT_MANADROP);
            manaDrop.transform.position = RandomPosDrop(posDrop);
        }
        foreach(ItemDropRate item in this.itemDropRates)
        {
            if(Random.value < item.dropRate)
            {
                GameObject itemDrop = ObjectPooling.Instance.GetPool(item.itemId);
                if(itemDrop != null ) 
                    itemDrop.transform.position = RandomPosDrop(posDrop);
            }    
        }    
    }
    public void DropWeapon(Vector3 posDrop)
    {
        float rate = Random.Range(0, 1f);
        if(rate <= 0.05f)
        {
            int index1 = (Random.Range(0, this.weaponLegend.Length));
            GameObject itemDrop1 = ObjectPooling.Instance.GetPool(this.weaponLegend[index1]);
            if (itemDrop1 != null)
                itemDrop1.transform.position = RandomPosDrop(posDrop);
            return;
        }
        int index2 = (Random.Range(0, this.weaponNormal.Length));
        GameObject itemDrop2 = ObjectPooling.Instance.GetPool(this.weaponNormal[index2]);
        if (itemDrop2 != null)
            itemDrop2.transform.position = RandomPosDrop(posDrop);
    }
    Vector3 RandomPosDrop(Vector3 pos)
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector3 dropPosition = pos + (Vector3)randomDirection * Random.Range(0.5f, 1f);
        return dropPosition;
    }
    /*public void EditRateDropItem(float rate)
    {
        foreach (ItemDropRate item in this.itemDropRates)
        {
            item.dropRate += rate;
        }
    }*/
}

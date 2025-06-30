using Unity.VisualScripting;
using UnityEngine;

public class ItemDropSpawn : MonoBehaviour
{
    public static ItemDropSpawn itemDropSpawn_Instance;
    public ItemDropRate[] itemDropRates;
    public GameObject[] weaponDropRate;
    void Awake()
    {
        itemDropSpawn_Instance = this;
    }
    public void DropItem(Vector3 posDrop, int coinCout)
    {
        if(posDrop == null) return;
        for(int i = 0; i < coinCout; i ++)
        {
            GameObject coinDrop = ObjectPooling.ObjectPooling_Instance.GetPool("GoldItem");
            coinDrop.transform.position = RandomPosDrop(posDrop);
        }  
        foreach(ItemDropRate item in this.itemDropRates)
        {
            if(Random.value < item.dropRate)
            {
                string name = KeyClean.CleanKey(item.item.name);
                GameObject itemDrop = ObjectPooling.ObjectPooling_Instance.GetPool(name);
                if(itemDrop != null ) 
                    itemDrop.transform.position = RandomPosDrop(posDrop);
            }    
        }    
    }
    public void DropWeapon(Vector3 posDrop)
    {
        int index = (Random.Range(0, this.weaponDropRate.Length));
        GameObject itemDrop = ObjectPooling.ObjectPooling_Instance.GetPool(KeyClean.CleanKey(this.weaponDropRate[index].name));
        if (itemDrop != null)
            itemDrop.transform.position = RandomPosDrop(posDrop);
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

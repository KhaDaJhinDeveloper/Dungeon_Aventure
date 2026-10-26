using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class ItemDropSpawn : MonoBehaviour
{
    public static ItemDropSpawn itemDropSpawn_Instance;
    public ItemDropRate[] itemDropRates;
    public KeyPool[] weaponNormal;
    public KeyPool[] weaponLegend;
    #region EffectDropInput
    [SerializeField] private LayerMask layerWall;
    [SerializeField] private float minDropDistance;
    [SerializeField] private float maxDropDistance;
    [SerializeField] private float wallOffset;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpDuration;

    #endregion
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
            AnimateDrop(coinDrop, posDrop);
        }
        for(int j = 0; j < index; j++)
        {
            GameObject manaDrop = ObjectPooling.Instance.GetPool(KeyPool.KEY_INTERACT_MANADROP);
            AnimateDrop(manaDrop, posDrop);
        }
        foreach(ItemDropRate item in this.itemDropRates)
        {
            if(Random.value < item.dropRate)
            {
                GameObject itemDrop = ObjectPooling.Instance.GetPool(item.itemId);
                if(itemDrop != null )
                    AnimateDrop(itemDrop, posDrop);
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
                AnimateDrop(itemDrop1, posDrop);
            return;
        }
        int index2 = (Random.Range(0, this.weaponNormal.Length));
        GameObject itemDrop2 = ObjectPooling.Instance.GetPool(this.weaponNormal[index2]);
        if (itemDrop2 != null)
            AnimateDrop(itemDrop2, posDrop);
    }
    /*public void EditRateDropItem(float rate)
    {
        foreach (ItemDropRate item in this.itemDropRates)
        {
            item.dropRate += rate;
        }
    }*/
    public void AnimateDrop(GameObject item, Vector3 pos)
    {
        item.transform.position = pos;
        Vector3 targetPos = GetSafePosDrop(pos);
        if (item.TryGetComponent<Rigidbody2D>(out var rb))
            rb.velocity = Vector2.zero;
        Collider2D col = item.GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        item.transform.DOKill();
        item.transform.DOJump(targetPos, this.jumpForce, 2, this.jumpDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                if (col != null) col.enabled = true;
            });
    }
    Vector3 GetSafePosDrop(Vector3 pos)
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float desiredDistance = Random.Range(this.minDropDistance, this.maxDropDistance);
        RaycastHit2D hit = Physics2D.Raycast(pos, randomDirection, desiredDistance, this.layerWall);
        if(hit.collider != null)
        {
            float safeDistance = Mathf.Max(0.15f, hit.distance - this.wallOffset);
            return pos + (Vector3)(randomDirection * safeDistance); 
        }
        return pos + (Vector3)(randomDirection * desiredDistance);
    }
}

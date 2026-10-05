using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crevice : MonoBehaviour
{
    [SerializeField] private Animator ani;
    [SerializeField] private float duration;
    private void OnEnable()
    {
        ani.SetBool("active", true);
        StartCoroutine(Hide(duration));
    }
    IEnumerator Hide(float duration)
    {
        yield return new WaitForSeconds(duration);
        ani.SetTrigger("hide");
        yield return new WaitForSeconds(0.5f);
        ObjectPooling.ObjectPooling_Instance.ReturnToPool(KeyPool.KEY_VFX_CREVICE, this.gameObject);
    }
}

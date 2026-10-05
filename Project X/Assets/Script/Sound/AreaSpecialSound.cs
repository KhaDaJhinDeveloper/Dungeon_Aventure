using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSpecialSound : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
            StartCoroutine(SoundManager.Instance?.SmoothMusicBGTransition(SoundManager.Instance.bg_Play));
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
            StartCoroutine(SoundManager.Instance?.SmoothMusicBGTransition(SoundManager.Instance.bg_MainMenu));
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}

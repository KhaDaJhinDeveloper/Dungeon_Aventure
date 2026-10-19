using UnityEngine;

public class AreaSpecialSound : BaseInteraction
{
    [SerializeField] SoundID soundID;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
            SoundManager.Instance?.SmoothMusicBGTransition(this.soundID);
    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
            SoundManager.Instance?.SmoothMusicBGTransition(SoundManager.Instance.soundReserve);
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}

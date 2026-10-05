using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundManager : Singleton<SoundManager>
{
    #region input
    [SerializeField] private AudioSource audioBG;
    [SerializeField] private AudioSource audioSFX;
    public float volumeBG = 1;
    public float volumeSFX = 1;
    private AudioSource currentMusicBG;
    private AudioSource[] arrayAudio;
    private AudioSource[] arrayAudioBG;
    private AudioSource[] arrayAudioSFX;
    #endregion
    [Header("SoundAsset")]
    //BG
    public AudioClip bg_MainMenu;
    public AudioClip bg_Play;
    //SFX
    public AudioClip sfx_PlantingBomb;
    public AudioClip sfx_Explode;
    public AudioClip sfx_PickBuff;
    public AudioClip sfx_PlayerDie;
    public AudioClip sfx_LoadLevel;
    public AudioClip sfx_PassLevel;
    public AudioClip sfx_PressButton;
    public AudioClip sfx_EnemyDeath;
    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        ///SettingSaveLoad.Instance.LoadData();
        CreateSoundPool();
        //PlayMusicBG(SoundManager.Instance.bg_MainMenu);
    }
    private void CreateSoundPool()
    {
        ObjectPooling.Instance.CreatePool(KeyPool.KEY_SOUND_MUSICBACKGROUND, this.audioBG.gameObject, 3, this.transform);
        ObjectPooling.Instance.CreatePool(KeyPool.KEY_SOUND_MUSICSFX, this.audioSFX.gameObject, 8, this.transform);
        this.arrayAudio = GameObject.FindObjectsByType<AudioSource>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        this.arrayAudioBG = this.arrayAudio.Where(s => s.CompareTag(TagManager.TAG_BGSOUND)).ToArray();
        this.arrayAudioSFX = this.arrayAudio.Where(s => s.CompareTag(TagManager.TAG_SFXSOUND)).ToArray();
    }
    #region MusicBG
    public void PlayMusicBG(AudioClip clipBG)
    {
        StopPlayMusicBG();
        AudioSource audioBG = ObjectPooling.Instance.GetPool(KeyPool.KEY_SOUND_MUSICBACKGROUND, this.transform).GetComponent<AudioSource>();
        this.currentMusicBG = audioBG;
        this.currentMusicBG.clip = clipBG;
        this.currentMusicBG.volume = this.volumeBG;
        this.currentMusicBG.Play();
    }
    public void StopPlayMusicBG()
    {
        if (this.currentMusicBG != null)
        {
            this.currentMusicBG.Stop();
            ObjectPooling.Instance.ReturnToPool(KeyPool.KEY_SOUND_MUSICBACKGROUND, this.currentMusicBG.gameObject);
        }
        this.currentMusicBG = null;
    }
    public IEnumerator SmoothMusicBGTransition(AudioClip newMusicBG)
    {
        float timer = 0f;
        if (this.currentMusicBG != null)
        {
            AudioSource oldAudio = this.currentMusicBG;
            float oldVolume = oldAudio.volume;
            AudioSource newAudioBG = ObjectPooling.Instance.GetPool(KeyPool.KEY_SOUND_MUSICBACKGROUND, this.transform).GetComponent<AudioSource>();
            newAudioBG.clip = newMusicBG;
            newAudioBG.Play();
            newAudioBG.volume = 0f;
            while(timer < 1.5f)
            {
                timer += Time.deltaTime;
                float progess = timer / 1.5f;
                oldAudio.volume = Mathf.Lerp(oldVolume, 0f,progess);
                newAudioBG.volume = Mathf.Lerp(0f, this.volumeBG, progess);
                yield return null;
            }
            if (oldAudio.volume == 0)
            {
                oldAudio.Stop();
                ReturnBG(oldAudio);            
            }
            this.currentMusicBG = newAudioBG;
        }
        else
            PlayMusicBG(newMusicBG);
    }
    public void SetVolumeBG(float value)
    {
        value = Mathf.Clamp01(value);
        foreach (AudioSource audiBG in this.arrayAudioBG)
            audiBG.volume = value;
    }

    void ReturnBG(AudioSource objBG)
    {
        ObjectPooling.Instance.ReturnToPool(KeyPool.KEY_SOUND_MUSICBACKGROUND, objBG.gameObject);
    }
    #endregion
    #region MusicSFX
    public void PlayMusicSFX(AudioClip clipSFX)
    {
        AudioSource audioSFX = ObjectPooling.Instance.GetPool(KeyPool.KEY_SOUND_MUSICSFX, this.transform).GetComponent<AudioSource>();
        audioSFX.clip = clipSFX;
        audioSFX.volume = this.volumeSFX;
        audioSFX.Play();
        StartCoroutine(ReturnSFX(audioSFX, clipSFX.length));
    }
    IEnumerator ReturnSFX(AudioSource objSfx, float time)
    {
        yield return new WaitForSeconds(time);
        ObjectPooling.Instance.ReturnToPool(KeyPool.KEY_SOUND_MUSICSFX, objSfx.gameObject);
    }
    public void SetVolumeSFX(float value)
    {
        value = Mathf.Clamp01(value);
        foreach (AudioSource audiSFX in this.arrayAudioSFX)
            audiSFX.volume = value;
    }
    #endregion
    protected override void OnDestroy()
    {
        StopAllCoroutines();
    }
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class SoundManager : Singleton<SoundManager>
{
    #region Input
    #region SoundAsset
    [SerializeField] private SoundDatabaseSO soundDatabase;
    private Dictionary<SoundID, AudioClip> soundDictionary;
    private Dictionary<NameScene, SoundID> mapSoundDictionary;
    [SerializeField] private AudioSource audioBG;
    [SerializeField] private AudioSource audioSFX;
    #endregion
    public float volumeBG = 1;
    public float volumeSFX = 1;
    #region RefAudioSource
    private AudioSource currentMusicBG;
    private AudioSource[] arrayAudio;
    private AudioSource[] arrayAudioBG;
    private AudioSource[] arrayAudioSFX;
    #endregion
    private Tween fadeTween;
    public SoundID soundReserve {  get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        ///SettingSaveLoad.Instance.LoadData();
        CreateSoundPool();
        Initialize();
        PlayMusicBG(SoundID.BG_MainMenu);
    }
    private void CreateSoundPool()
    {
        ObjectPooling.Instance.CreatePool(KeyPool.KEY_SOUND_MUSICBACKGROUND, this.audioBG.gameObject, 3, this.transform);
        ObjectPooling.Instance.CreatePool(KeyPool.KEY_SOUND_MUSICSFX, this.audioSFX.gameObject, 8, this.transform);
        this.arrayAudio = GameObject.FindObjectsByType<AudioSource>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        this.arrayAudioBG = this.arrayAudio.Where(s => s.CompareTag(TagManager.TAG_BGSOUND)).ToArray();
        this.arrayAudioSFX = this.arrayAudio.Where(s => s.CompareTag(TagManager.TAG_SFXSOUND)).ToArray();
    }
    public void Initialize()
    {
        if (this.soundDatabase == null) return;
        this.soundDictionary = new();
        foreach (var sound in this.soundDatabase.sounds)
        {
            if (sound.soundID == SoundID.None || sound.clip == null) continue;
            if (!this.soundDictionary.ContainsKey(sound.soundID))
            {
                this.soundDictionary.Add(sound.soundID, sound.clip);
            }
        }
        this.mapSoundDictionary = new();
        if (this.soundDatabase.mapSounds.Count <= 0) return;
        foreach (var mapSound in this.soundDatabase.mapSounds)
        {
            if(!this.mapSoundDictionary.ContainsKey(mapSound.sceneID))
                this.mapSoundDictionary.Add(mapSound.sceneID, mapSound.soundID);
        }
    }
    #region MusicBG
    public void PlaySceneMusicBG(NameScene nameScene)
    {
        if (this.mapSoundDictionary.TryGetValue(nameScene, out SoundID soundID))
        {
            PlayMusicBG(soundID);
        }
    }
    public void PlayMusicBG(SoundID soundID)
    {
        if (soundID == SoundID.None) return;
        StopPlayMusicBG();
        if (this.soundDictionary.TryGetValue(soundID, out AudioClip clipBG))
        {
            this.fadeTween?.Kill();
            AudioSource audioBG = ObjectPooling.Instance.GetPool(KeyPool.KEY_SOUND_MUSICBACKGROUND, this.transform).GetComponent<AudioSource>();
            this.currentMusicBG = audioBG;
            this.currentMusicBG.clip = clipBG;
            this.currentMusicBG.Play();
            this.fadeTween = this.currentMusicBG.DOFade(SoundManager.Instance.volumeBG, 0.05f);
            this.soundReserve = soundID;
        }
    }
    public void StopPlayMusicBG()
    {
        if (this.currentMusicBG != null)
        {
            this.fadeTween = this.currentMusicBG.DOFade(0f, 1f).OnComplete(() => 
            {   
                this.currentMusicBG.Stop();
               
            });
            ObjectPooling.Instance.ReturnToPool(KeyPool.KEY_SOUND_MUSICBACKGROUND, this.currentMusicBG.gameObject);
        }
        this.currentMusicBG = null;
    }
    public void SmoothMusicBGTransition(SoundID newSoundIDBG)
    {
        if(this.soundDictionary.TryGetValue(newSoundIDBG, out AudioClip newClipBG))
        {
            if (this.currentMusicBG != null)
            {
                AudioSource oldAudio = this.currentMusicBG;
                float oldVolume = oldAudio.volume;
                AudioSource newAudioBG = ObjectPooling.Instance.GetPool(KeyPool.KEY_SOUND_MUSICBACKGROUND, this.transform).GetComponent<AudioSource>();
                newAudioBG.clip = newClipBG;
                newAudioBG.Play();
                newAudioBG.volume = 0f;
                this.fadeTween = newAudioBG.DOFade(SoundManager.Instance.volumeBG, 1f);
                this.fadeTween = oldAudio.DOFade(0, 1f).OnComplete(()=> 
                {
                    oldAudio.Stop();
                    ReturnBG(oldAudio);
                });

                this.currentMusicBG = newAudioBG;
            }
            else
                PlayMusicBG(newSoundIDBG);
        }
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
    public void PlayMusicSFX(SoundID soundID, Transform pos = null, float blend = 1)
    {
        if (this.soundDictionary.TryGetValue(soundID, out AudioClip clipSFX))
        {
            AudioSource audioSFX = ObjectPooling.Instance.GetPool(KeyPool.KEY_SOUND_MUSICSFX, this.transform).GetComponent<AudioSource>();
            if (pos != null)
                audioSFX.transform.position = new Vector3(pos.position.x, pos.position.y, 0);
            else blend = 0;
            audioSFX.spatialBlend = blend;
            audioSFX.clip = clipSFX;
            audioSFX.volume = this.volumeSFX;
            audioSFX.Play();
            StartCoroutine(ReturnSFX(audioSFX, clipSFX.length));
        }
    }
    IEnumerator ReturnSFX(AudioSource objSfx, float time)
    {
        yield return new WaitForSecondsRealtime(time);
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

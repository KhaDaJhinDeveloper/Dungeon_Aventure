using UnityEngine;
[System.Serializable]
public struct SoundData
{
    public SoundID soundID;
    public AudioClip clip;
}
[System.Serializable]
public struct MapSoundData
{
    public NameScene sceneID;
    public SoundID soundID;
}
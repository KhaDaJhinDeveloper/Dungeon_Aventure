using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SoundDatabase", menuName = "Audio/SoundDatabase")]
public class SoundDatabaseSO : ScriptableObject
{
    public List<SoundData> sounds;
    public List<MapSoundData> mapSounds;
}

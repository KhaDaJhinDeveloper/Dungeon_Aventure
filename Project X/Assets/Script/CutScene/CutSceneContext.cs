using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class CutSceneContext
{
    public PlayableDirector director;
    public MonoBehaviour runner;
    public Transform bossPos;
    public Transform playerPos;
    public Camera camera;
    public Dictionary<string, Object> binDings = new();
}
[System.Serializable]
public struct TrackConnect
{
    public string nameTrack;
    public string connectKey;
}
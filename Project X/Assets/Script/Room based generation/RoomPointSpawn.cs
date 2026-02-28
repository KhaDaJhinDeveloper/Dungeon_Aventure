using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnPoint
{
    public Transform point;
    [HideInInspector] public bool isUsed;
}


[System.Serializable]
public class SpawnBoxPoint
{
    public Transform point;
    [HideInInspector] public bool isUsed;
}

[System.Serializable]
public class SpawnnBonFirePoint
    {
    public Transform point;
    [HideInInspector] public bool isUsed;
}
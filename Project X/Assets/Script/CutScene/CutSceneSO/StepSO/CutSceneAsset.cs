
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "CutScene/CutScene Asset")]
public class CutSceneAsset : ScriptableObject
{
    public List<CutSceneStep> stepsList;
}

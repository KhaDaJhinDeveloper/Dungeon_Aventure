
using System.Collections;
using UnityEngine;
public abstract class CutSceneStep : ScriptableObject
{
    public abstract IEnumerator CutSceneExcute(CutSceneContext context);
    public abstract void Complete(CutSceneContext context);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[CreateAssetMenu(menuName = "CutScene/Step/TimeLine")]
public class TimeLineStep : CutSceneStep
{
    public PlayableAsset playableAsset;
    public List<TrackConnect> trackConnects = new();
    void SetUp(CutSceneContext csContext )
    {
        var director = csContext.director;
        director.playableAsset = playableAsset;
        director.extrapolationMode = DirectorWrapMode.Hold;
        director.time = 0;
        foreach(var output in playableAsset.outputs)
        {
            foreach(var b in trackConnects)
            {
                if (output.streamName == b.nameTrack && csContext.binDings.TryGetValue(b.connectKey, out Object target))
                    director.SetGenericBinding(output.sourceObject, target);
            }
        }
    }
    public override IEnumerator CutSceneExcute(CutSceneContext context)
    {
        SetUp(context);
        context.director.Play();
        var director = context.director;
        yield return new WaitUntil(() => director.time >= director.time - 0.001f);
    }
    public override void Complete(CutSceneContext context)
    {
        var director = context.director;
        if(director.playableAsset != this.playableAsset) SetUp(context);
        director.time = 0;
        director.Evaluate();
        director.Pause();
    }
}


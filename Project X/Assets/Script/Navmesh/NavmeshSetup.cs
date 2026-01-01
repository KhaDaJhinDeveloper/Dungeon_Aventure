using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavmeshSetup : MonoBehaviour
{
    [SerializeField]private NavMeshSurface NavMeshSurface;
    private void Start()
    {
        StartCoroutine(Build());
    }
    IEnumerator Build()
    {
        yield return null;
        NavMeshSurface.BuildNavMesh();
    }
}

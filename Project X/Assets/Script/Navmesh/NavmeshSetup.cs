using NavMeshPlus.Components;
using UnityEngine;

public class NavmeshSetup : MonoBehaviour
{
    [SerializeField]private NavMeshSurface NavMeshSurface;
    private void OnEnable()
    {
        EventManager.Instance?.Subscribe(NameEvent.Event_NavMeshSetUp, Build);
    }
    public void Build()
    {
        NavMeshSurface.BuildNavMesh();
    }
    private void OnDestroy()
    {
        EventManager.Instance?.Unsubscribe(NameEvent.Event_NavMeshSetUp, Build);
    }
}

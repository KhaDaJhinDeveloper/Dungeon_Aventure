using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraFollower : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private void Start()
    {
        this.virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.TAG_PLAYER))
            this.virtualCamera.Priority = 10;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.TAG_PLAYER))
            this.virtualCamera.Priority = 1;
    }
}

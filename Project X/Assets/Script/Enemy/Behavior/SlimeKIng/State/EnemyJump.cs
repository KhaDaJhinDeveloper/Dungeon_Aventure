using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJump : MonoBehaviour
{
    public float jumpUpDuration = 0.5f; // Thời gian bay lên
    public float hoverDuration = 0.1f; // Thời gian đứng yên trên không
    public float fallDownDuration = 0.3f; // Thời gian rơi xuống
    public float jumpHeight = 3f; // Độ cao nhảy

    private bool isJumping = false;
    Vector3 target;
    Transform player;
    private void Start()
    {
        player = GameObject.FindWithTag(TagManager.TAG_PLAYER).transform;
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.K))
        {
            target = player.position;
            JumpToPlayer(target);
        }
    }
    public void JumpToPlayer(Vector3 targetPosition)
    {
        if (!isJumping)
        {
            StartCoroutine(JumpCoroutine(targetPosition));
        }
    }

    private IEnumerator JumpCoroutine(Vector3 targetPosition)
    {
        isJumping = true;

        Vector3 startPosition = transform.position;
        // Vị trí trên đầu player
        Vector3 aboveTargetPosition = new Vector3(targetPosition.x, targetPosition.y + jumpHeight, targetPosition.z);

        // Phase 1: Bay lên tới vị trí trên đầu player
        float elapsedTime = 0f;
        while (elapsedTime < jumpUpDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / jumpUpDuration;

            // Có thể dùng EaseOut để bay lên mượt hơn
            float smoothT = 1f - Mathf.Pow(1f - t, 2f);
            transform.position = Vector3.Lerp(startPosition, aboveTargetPosition, smoothT);

            yield return null;
        }

        transform.position = aboveTargetPosition;

        // Phase 2: Đứng yên trên không 0.1 giây
        yield return new WaitForSeconds(hoverDuration);

        // Phase 3: Rơi thẳng xuống vị trí player
        elapsedTime = 0f;
        while (elapsedTime < fallDownDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fallDownDuration;

            // Có thể dùng EaseIn để rơi nhanh dần
            float smoothT = Mathf.Pow(t, 2f);
            transform.position = Vector3.Lerp(aboveTargetPosition, targetPosition, smoothT);

            yield return null;
        }

        // Đảm bảo vị trí cuối chính xác
        transform.position = targetPosition;
        isJumping = false;

        // Có thể thêm hiệu ứng khi chạm đất ở đây
        OnLanded();
    }

    private void OnLanded()
    {
        // Thêm hiệu ứng chạm đất: rung camera, particle effect, v.v.
        Debug.Log("Enemy landed!");
    }
}

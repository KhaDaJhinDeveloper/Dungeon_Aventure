using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePathfinding : MonoBehaviour
{
    [Header("Settings")]
    public LayerMask obstacleLayer;
    public float gridSize = 1f;
    public float moveSpeed = 3f;

    [Header("Physics Settings")]
    public float acceleration = 10f;
    public float stoppingDistance = 0.2f;
    public bool useVelocity = true; // true: dùng velocity, false: dùng MovePosition

    private Transform target;
    private List<Vector2> currentPath;
    private int currentPathIndex;
    private Rigidbody2D rb;

    void Start()
    {
        // Lấy Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            DebugLogger.LogError("Cần có Rigidbody2D component để di chuyển bằng vật lý!");
            return;
        }

        // Thiết lập Rigidbody2D cho pathfinding
        rb.gravityScale = 0f; // Tắt gravity cho 2D top-down
        rb.drag = 5f; // Thêm drag để dừng lại mượt mà

        // Tìm player làm target
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (target == null || rb == null) return;

        // Tính path mới mỗi 0.5 giây để tối ưu performance
        if (Time.time % 0.5f < Time.deltaTime)
        {
            FindPath(transform.position, target.position);
        }
    }

    void FixedUpdate()
    {
        // Di chuyển trong FixedUpdate cho vật lý ổn định
        if (rb != null)
        {
            MoveAlongPath();
        }
    }

    void FindPath(Vector2 start, Vector2 goal)
    {
        // Đơn giản hóa: chỉ kiểm tra đường thẳng có bị cản không
        if (!IsPathBlocked(start, goal))
        {
            // Đường thẳng không bị cản
            currentPath = new List<Vector2> { goal };
            currentPathIndex = 0;
        }
        else
        {
            // Nếu bị cản, tìm đường vòng đơn giản
            currentPath = FindSimpleDetour(start, goal);
            currentPathIndex = 0;
        }
    }

    bool IsPathBlocked(Vector2 start, Vector2 end)
    {
        Vector2 direction = (end - start).normalized;
        float distance = Vector2.Distance(start, end);

        RaycastHit2D hit = Physics2D.Raycast(start, direction, distance, obstacleLayer);
        return hit.collider != null;
    }

    List<Vector2> FindSimpleDetour(Vector2 start, Vector2 goal)
    {
        List<Vector2> path = new List<Vector2>();

        // Thử các hướng vòng đơn giản
        Vector2[] detourDirections = {
            Vector2.up, Vector2.down, Vector2.left, Vector2.right,
            new Vector2(1, 1), new Vector2(-1, 1), new Vector2(1, -1), new Vector2(-1, -1)
        };

        foreach (Vector2 dir in detourDirections)
        {
            Vector2 detourPoint = start + dir * gridSize * 2;

            if (!IsPathBlocked(start, detourPoint) && !IsPathBlocked(detourPoint, goal))
            {
                path.Add(detourPoint);
                path.Add(goal);
                return path;
            }
        }

        // Nếu không tìm được đường vòng, di chuyển về phía target
        path.Add(goal);
        return path;
    }

    void MoveAlongPath()
    {
        if (currentPath == null || currentPath.Count == 0)
        {
            // Dừng lại khi không có path
            if (useVelocity)
                rb.velocity = Vector2.zero;
            return;
        }

        if (currentPathIndex >= currentPath.Count)
        {
            currentPath = null;
            if (useVelocity)
                rb.velocity = Vector2.zero;
            return;
        }

        Vector2 targetPos = currentPath[currentPathIndex];
        Vector2 currentPos = rb.position; // Dùng rb.position thay vì transform.position

        Vector2 direction = (targetPos - currentPos).normalized;
        float distanceToTarget = Vector2.Distance(currentPos, targetPos);

        if (useVelocity)
        {
            // CÁCH 1: Sử dụng velocity
            MoveWithVelocity(direction, distanceToTarget);
        }
        else
        {
            // CÁCH 2: Sử dụng MovePosition
            MoveWithMovePosition(currentPos, direction);
        }

        // Kiểm tra đã đến điểm chưa
        if (distanceToTarget < stoppingDistance)
        {
            currentPathIndex++;
        }
    }

    void MoveWithVelocity(Vector2 direction, float distanceToTarget)
    {
        // Tính toán velocity với acceleration và deceleration
        float targetSpeed = moveSpeed;

        // Giảm tốc khi gần đến target
        if (distanceToTarget < stoppingDistance * 3f)
        {
            targetSpeed = moveSpeed * (distanceToTarget / (stoppingDistance * 3f));
        }

        Vector2 targetVelocity = direction * targetSpeed;

        // Lerp velocity để có chuyển động mượt mà
        rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity, acceleration * Time.fixedDeltaTime);
    }

    void MoveWithMovePosition(Vector2 currentPos, Vector2 direction)
    {
        // Tính vị trí mới
        Vector2 newPosition = currentPos + direction * moveSpeed * Time.fixedDeltaTime;

        // KIỂM TRA VA CHẠM TRƯỚC KHI DI CHUYỂN
        if (CanMoveTo(newPosition))
        {
            rb.MovePosition(newPosition);
        }
        else
        {
            // Nếu không thể di chuyển thẳng, tìm path mới
            FindPath(transform.position, target.position);
        }
    }

    bool CanMoveTo(Vector2 targetPosition)
    {
        Vector2 currentPos = rb.position;
        Vector2 direction = (targetPosition - currentPos).normalized;
        float distance = Vector2.Distance(currentPos, targetPosition);

        // Kiểm tra va chạm bằng raycast
        RaycastHit2D hit = Physics2D.Raycast(currentPos, direction, distance, obstacleLayer);
        return hit.collider == null;
    }

    // Phương thức để thay đổi target từ script khác
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    // Phương thức để dừng di chuyển
    public void StopMovement()
    {
        currentPath = null;
        if (rb != null && useVelocity)
        {
            rb.velocity = Vector2.zero;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Vẽ path để debug
        if (currentPath != null && currentPath.Count > 0)
        {
            Gizmos.color = Color.red;
            Vector3 previousPos = transform.position;

            foreach (Vector2 point in currentPath)
            {
                Gizmos.DrawLine(previousPos, point);
                Gizmos.DrawWireSphere(point, 0.2f);
                previousPos = point;
            }

            // Vẽ điểm hiện tại đang di chuyển đến
            if (currentPathIndex < currentPath.Count)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(currentPath[currentPathIndex], 0.3f);
            }
        }

        // Vẽ velocity hiện tại
        if (rb != null && Application.isPlaying)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, rb.velocity);
        }
    }
}

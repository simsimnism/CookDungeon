using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // 플레이어 이동 속도
    private Rigidbody2D rb;       // Rigidbody2D 참조
    private Vector2 movement;     // 이동 방향

    void Start()
    {
        // Rigidbody2D 컴포넌트 가져오기
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 입력 처리 (상하좌우)
        movement.x = Input.GetAxisRaw("Horizontal");  // 왼쪽(-1), 오른쪽(1)
        movement.y = Input.GetAxisRaw("Vertical");    // 아래(-1), 위(1)
    }

    void FixedUpdate()
    {
        // Rigidbody2D를 사용하여 물리 기반 이동 처리
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}

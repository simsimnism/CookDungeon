using UnityEngine;

public class AnimeController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private float speed;
    private Animator animator;
    private SpriteRenderer rend;
    Rigidbody2D rbody;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        speed = Managers.Player.moveSpeed;
        rend.flipX = true;
    }

    void Start()
    {
        PlayerAnime(); // 초기 애니메이션 설정
    }

    void Update()
    {
        PlayerAnime(); // 매 프레임마다 애니메이션 갱신
    }

    private void PlayerAnime()
    {
        if (!Managers.GM.IsMoving)
            return;

        // WASD 입력 값으로 이동 방향 계산
        float moveVertical = 0;
        float moveHorizontal = 0;

        if (Input.GetKey(KeyCode.W)) moveVertical = 1f;
        if (Input.GetKey(KeyCode.S)) moveVertical = -1f;
        if (Input.GetKey(KeyCode.A)) moveHorizontal = -1f;
        if (Input.GetKey(KeyCode.D)) moveHorizontal = 1f;

        // 이동 방향 계산
        Vector2 direction = new Vector2(moveHorizontal, moveVertical).normalized;

        // 대쉬 상태가 아닐 때 걷기 애니메이션 실행
        if (direction != Vector2.zero && !Managers.Player.isDashing)
        {
            PlayWalkAnimation(direction);
        }
        else if (direction == Vector2.zero)
        {
            // 정지 상태일 때 Stand 애니메이션
            ResetAllTriggers();
            animator.SetTrigger("Stand");
        }

        // 대쉬 시작
        if (Input.GetKeyDown(KeyCode.LeftShift) && direction != Vector2.zero)
        {
            // 대쉬 애니메이션 트리거
            PlayDashAnimation(direction);
        }
    }

    // 걷기 애니메이션 실행 함수
    private void PlayWalkAnimation(Vector2 direction)
    {
        ResetAllTriggers(); // 이전 트리거 초기화

        if (direction.x < 0) // 왼쪽
        {
            animator.SetTrigger("WalkLeft"); // 왼쪽으로 걷는 애니메이션 트리거
        }
        else if (direction.x > 0) // 오른쪽
        {
            animator.SetTrigger("WalkLeft"); // 오른쪽 애니메이션이 없으므로 왼쪽 애니메이션 재사용
            rend.flipX = false; // 오른쪽으로 반전
        }
        else if (direction.y > 0) // 위쪽
        {
            animator.SetTrigger("WalkUp");
        }
        else if (direction.y < 0) // 아래쪽
        {
            animator.SetTrigger("WalkDown");
        }
    }

    // 대쉬 애니메이션 실행 함수
    private void PlayDashAnimation(Vector2 direction)
    {
        ResetAllTriggers(); // 이전 트리거 초기화

        if (direction.x < 0) // 왼쪽 대쉬
        {
            animator.SetTrigger("LeftDash");
        }
        else if (direction.x > 0) // 오른쪽 대쉬
        {
            animator.SetTrigger("LeftDash"); // 오른쪽 애니메이션이 없으므로 왼쪽 애니메이션 재사용
            rend.flipX = false; // 오른쪽을 바라보도록 반전
        }
        else if (direction.y > 0) // 위쪽 대쉬
        {
            animator.SetTrigger("UpDash");
        }
        else if (direction.y < 0) // 아래쪽 대쉬
        {
            animator.SetTrigger("DownDash");
        }
    }

    // 트리거를 초기화하는 함수
    private void ResetAllTriggers()
    {
        animator.ResetTrigger("WalkLeft");
        animator.ResetTrigger("WalkUp");
        animator.ResetTrigger("WalkDown");
        animator.ResetTrigger("Stand");
        animator.ResetTrigger("LeftDash");
        animator.ResetTrigger("UpDash");
        animator.ResetTrigger("DownDash");
        // 필요에 따라 더 추가
    }

    // 플레이어의 공격 상태 애니메이션
    void AttackAnime()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ResetAllTriggers(); // 공격 시 다른 트리거 초기화
            animator.SetTrigger("Attack1");
        }
    }
}

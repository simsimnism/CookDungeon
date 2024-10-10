using System.Collections;
using UnityEngine;

public class AnimeController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Animator animator;
    private SpriteRenderer rend;
    private Rigidbody2D rbody;

    public bool StartAttack;
    public PlayerAttack pa;

    private bool isComboAttack;
    private bool isAttacking;  // 공격 중인지 확인하는 플래그
    private float comboTimer;
    private const float comboTimeLimit = 1.0f; // 콤보 입력 시간 제한

    public Transform playerTransform; // 플레이어의 Transform
    public Vector2 worldMousePosition; // 월드 좌표에서의 마우스 위치 (외부에서 설정 가능)

    // 애니메이션 이벤트 또는 상태 완료 감지
    private int currentAttackIndex;  // 현재 공격 상태를 추적

    void Awake()
    {
        animator = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        // 플레이어의 Transform 자동 할당
        if (playerTransform == null && player != null)
        {
            playerTransform = player.transform;
        }

        currentAttackIndex = 0;
    }

    void Start()
    {

    }

    void Update()
    {
        if (isAttacking)
            return;
        PlayerAnime();   // 플레이어 애니메이션 갱신

    }

    void LateUpdate()
    {
        // 마우스의 z 값을 플레이어와 동일한 z 값으로 설정 (필요 시 수정)
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(Camera.main.transform.position.z); // 카메라와 월드 좌표 간의 깊이 설정

        // 마우스 좌표를 월드 좌표로 변환
        worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        PlayerAnime();   // 플레이어 애니메이션 갱신
    }

    // 플레이어 애니메이션 처리
    private void PlayerAnime()
    {
        if (!Managers.GM.IsMoving || isAttacking) // 공격 중일 때는 이동 애니메이션 처리 안 함
            return;

        HandleMovement();   // 이동 애니메이션 처리
        HandleDash();       // 대쉬 애니메이션 처리
    }

    // 이동 애니메이션 처리 함수
    private void HandleMovement()
    {
        float moveVertical = 0;
        float moveHorizontal = 0;

        if (Input.GetKey(KeyCode.W)) moveVertical = 1f;
        if (Input.GetKey(KeyCode.S)) moveVertical = -1f;
        if (Input.GetKey(KeyCode.A)) moveHorizontal = -1f;
        if (Input.GetKey(KeyCode.D)) moveHorizontal = -1f;

        Vector2 direction = new Vector2(moveHorizontal, moveVertical).normalized;

        if (direction != Vector2.zero && !Managers.Player.isDashing)
        {
            PlayWalkAnimation(direction);
        }
        else if (direction == Vector2.zero)
        {
            ResetAllTriggers();
            animator.SetTrigger("Stand");
        }
    }

    // 대쉬 애니메이션 처리 함수
    private void HandleDash()
    {
        float moveVertical = 0;
        float moveHorizontal = 0;

        if (Input.GetKey(KeyCode.W)) moveVertical = 1f;
        if (Input.GetKey(KeyCode.S)) moveVertical = -1f;
        if (Input.GetKey(KeyCode.A)) moveHorizontal = -1f;
        if (Input.GetKey(KeyCode.D)) moveHorizontal = -1f;

        Vector2 direction = new Vector2(moveHorizontal, moveVertical).normalized;

        if (Input.GetKeyDown(KeyCode.LeftShift) && direction != Vector2.zero)
        {
            PlayDashAnimation(direction);
        }
    }

    // 걷기 애니메이션 실행 함수
    private void PlayWalkAnimation(Vector2 direction)
    {
        ResetAllTriggers();

        if (direction.x < 0) // 왼쪽으로 이동
        {
            animator.SetTrigger("WalkLeft");
        }
        else if (direction.x > 0) // 오른쪽으로 이동
        {
            rend.flipX = false; // 오른쪽을 바라보게
            animator.SetTrigger("WalkLeft");
        }
        else if (direction.y > 0) // 위쪽으로 이동
        {
            animator.SetTrigger("WalkUp");
        }
        else if (direction.y < 0) // 아래쪽으로 이동
        {
            animator.SetTrigger("WalkDown");
        }
    }

    // 대쉬 애니메이션 실행 함수
    private void PlayDashAnimation(Vector2 direction)
    {
        ResetAllTriggers();

        if (direction.x < 0) // 왼쪽 대쉬
        {
            animator.SetTrigger("LeftDash");
        }
        else if (direction.x > 0) // 오른쪽 대쉬
        {
            rend.flipX = false;
            animator.SetTrigger("LeftDash");
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

    // 애니메이션 트리거 초기화 함수
    private void ResetAllTriggers()
    {
        animator.ResetTrigger("WalkLeft");
        animator.ResetTrigger("WalkUp");
        animator.ResetTrigger("WalkDown");
        animator.ResetTrigger("Stand");
        animator.ResetTrigger("LeftDash");
        animator.ResetTrigger("UpDash");
        animator.ResetTrigger("DownDash");
        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
    }
}

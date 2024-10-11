using System.Collections;
using UnityEngine;

public class AnimeController : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rbody;
    public Transform playerTransform; // 플레이어의 Transform

    private Vector2 attackDirection;
    private int comboStep = 0;
    private float lastAttackTime = 0f;
    private float comboDelay = 1.0f; // 콤보 유효 시간
    private Vector3 mousePosition;
    private bool isAttacking = false; // 공격 중 상태 플래그 추가



    void Awake()
    {
        animator = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 플레이어의 Transform 자동 할당
    }

    void Start()
    {
        
    }

    void Update()
    {
        PlayerAnime();   // 플레이어 애니메이션 갱신
        if(Input.GetMouseButtonDown(0)) 
        {
            HandleAttack();
        }
        if(Time.time -lastAttackTime >comboDelay)
        {
            comboStep = 0;
        }

    }

    void LateUpdate()
    {
        // 마우스의 z 값을 플레이어와 동일한 z 값으로 설정 (필요 시 수정)
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(Camera.main.transform.position.z); // 카메라와 월드 좌표 간의 깊이 설정

    }

    // 플레이어 애니메이션 처리
    private void PlayerAnime()
    {

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
            animator.SetTrigger("WalkRight");
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
            animator.SetTrigger("RightDash");
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


    void HandleAttack()
    {
        isAttacking = true; // 공격 시작 상태로 변경

        // 마우스 좌표를 스크린에서 가져오고 z축 값을 0으로 고정
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane; // Z값을 카메라의 가까운 클리핑 평면으로 설정

        // 스크린 좌표를 월드 좌표로 변환
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // 캐릭터 위치에서 마우스 위치로 향하는 방향 벡터 계산
        Vector2 direction = (worldPosition - (Vector2)transform.position).normalized;


        // 애니메이션 트리거
        if (comboStep == 0)
        {
            animator.SetTrigger("Attack1");
        }
        else if (comboStep == 1)
        {
            animator.SetTrigger("Attack2");
        }

        comboStep = (comboStep + 1) % 2; // 콤보 단계 증가
        lastAttackTime = Time.time; // 마지막 공격 시간 기록
    }

    public void OnAttackAnimationEnd() // 공격 애니메이션 종료 후 호출
    {
        isAttacking = false; // 공격 상태를 리셋
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

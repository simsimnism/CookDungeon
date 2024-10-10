using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;


// 애니매이션 관련 코드 사실상 공격시 플레이어를 따라오는 카메라 계산을 제외하면 끝
public class AnimeController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Animator animator;
    private SpriteRenderer rend;
    private Rigidbody2D rbody;
    CameraController camera;

    public bool StartAttack;
    public PlayerAttack pa;

    private bool isComboAttack;
    private bool isAttacking;  // 공격 중인지 확인하는 플래그
    private float comboTimer;
    private const float comboTimeLimit = 1.0f; // 콤보 입력 시간 제한

    // 애니메이션 이벤트 또는 상태 완료 감지
    private int currentAttackIndex;  // 현재 공격 상태를 추적

    void Awake()
    {
      
        animator = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();

        currentAttackIndex = 0;
    }

    void Start()
    {
        camera = FindAnyObjectByType<CameraController>();
        PlayerAnime(); // 초기 애니메이션 설정
        ComboAttack(); // 초기 콤보 설정
    }

    void Update()
    {
        PlayerAnime();   // 플레이어 애니메이션 갱신
        ComboAttack();   // 콤보 공격 갱신
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

    // 콤보 공격 처리
    private void ComboAttack()
    {
        ComboAttackLogic(); // 콤보 공격 로직 처리
    }

    // 콤보 공격 로직
    private void ComboAttackLogic()
    {
        Vector2 direction = GetAttackDirection(); // 공격 방향 계산

        if (Input.GetMouseButtonDown(0) && !isAttacking) // 공격 중이 아닐 때만 공격 시작
        {
            if (!isComboAttack)
            {
                isAttacking = true; // 공격 상태로 전환
                AttackTransform1(direction); // 첫 번째 공격
                isComboAttack = true; // 콤보 상태로 전환
                comboTimer = 0; // 타이머 초기화
            }
            else if (comboTimer <= comboTimeLimit)
            {
                AttackTransform2(direction); // 두 번째 공격
                isComboAttack = false; // 콤보 종료
            }
        }

        // 콤보 타이머 관리
        if (isComboAttack)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer > comboTimeLimit)
            {
                isComboAttack = false; // 콤보 시간 초과 시 종료
            }
        }
    }

    // 공격 방향을 계산하는 함수
    // 공격 방향을 계산하는 함수
    Vector2 GetAttackDirection()
    {
        // 카메라의 위치를 가져옵니다.
        Vector3 cameraPosition = camera.GetCameraPosition();

        // 마우스 좌표를 가져옵니다.
        Vector3 mousePos = Input.mousePosition; // 스크린 좌표에서 마우스 위치

        // 카메라의 Z축 값에 따라 마우스의 Z축을 설정
        mousePos.z = cameraPosition.z; // 카메라의 Z축을 사용

        // 마우스의 월드 좌표를 계산
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);

        // 캐릭터의 2D 좌표 (Z축은 무시)
        Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);

        // 마우스와 캐릭터 간의 방향을 계산
        Vector2 direction = (worldMousePos - (Vector3)playerPos).normalized;

        // 각도를 계산
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 공격 방향 계산
        if (angle >= 45 && angle < 135)
        {
            // 상단 공격
            return new Vector2(0, 1).normalized; // 위쪽
        }
        else if (angle >= -135 && angle < -45)
        {
            // 하단 공격
            return new Vector2(0, -1).normalized; // 아래쪽
        }
        else
        {
            // 좌우 공격
            return direction.x >= 0 ? new Vector2(1, 0).normalized : new Vector2(-1, 0).normalized; // 오른쪽 또는 왼쪽
        }
    }

    // 첫 번째 공격 애니메이션
    private void AttackTransform1(Vector2 direction)
    {
        // 방향에 따른 트리거 설정
        if (direction.x > 0 && direction.y > 0) // 우상단 공격
        {
            rend.flipX = false;
            animator.SetTrigger("Attack1");
        }
        else if (direction.x < 0 && direction.y > 0) // 좌상단 공격
        {
 
            animator.SetTrigger("Attack1");
        }
        else if (direction.x > 0 && direction.y < 0) // 우하단 공격
        {
            rend.flipX = false;
            animator.SetTrigger("Attack1");
        }
        else if (direction.x < 0 && direction.y < 0) // 좌하단 공격
        {

            animator.SetTrigger("Attack1");
        }
        else if (direction.x > 0) // 오른쪽 공격
        {
            rend.flipX = false;
            animator.SetTrigger("Attack1");
        }
        else if (direction.x < 0) // 왼쪽 공격
        {

            animator.SetTrigger("Attack1");
        }

        // 공격이 끝났을 때 자동으로 트리거 리셋
        StartCoroutine(EndAttackState());
    }

    // 두 번째 공격 애니메이션
    private void AttackTransform2(Vector2 direction)
    {
        // 방향에 따른 트리거 설정
        if (direction.x > 0 && direction.y > 0) // 우상단 공격
        {
            rend.flipX = false;
            animator.SetTrigger("Attack2");
        }
        else if (direction.x < 0 && direction.y > 0) // 좌상단 공격
        {

            animator.SetTrigger("Attack2");
        }
        else if (direction.x > 0 && direction.y < 0) // 우하단 공격
        {
            rend.flipX = false;
            animator.SetTrigger("Attack2");
        }
        else if (direction.x < 0 && direction.y < 0) // 좌하단 공격
        {

            animator.SetTrigger("Attack2");
        }
        else if (direction.x > 0) // 오른쪽 공격
        {
            rend.flipX = false;
            animator.SetTrigger("Attack2");
        }
        else if (direction.x < 0) // 왼쪽 공격
        {
            animator.SetTrigger("Attack2");
        }

        // 공격이 끝났을 때 자동으로 트리거 리셋
        StartCoroutine(EndAttackState());
    }

    // 공격 상태 리셋 함수
    private IEnumerator EndAttackState()
    {
        yield return new WaitForSeconds(0.5f); // 애니메이션이 끝난 후 일정 시간 기다림
        isAttacking = false; // 공격 상태 해제
        ResetAllTriggers();  // 트리거 초기화
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

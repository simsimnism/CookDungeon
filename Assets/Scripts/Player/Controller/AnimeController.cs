using System.Collections;
using UnityEngine;

public class AnimeController : MonoBehaviour
{
    //파티션을 관리하는 코드
    public GameObject RightAttackEffect;
    public GameObject LeftAttackEffect;
    public GameObject RightComboEffect;
    public GameObject LeftComboEffect;

    private Animator animator;
    private Rigidbody2D rbody;
    public Transform playerTransform;

    private Vector2 attackDirection;
    private int comboStep = 0;
    private float lastAttackTime = 0f;
    private float comboDelay = 0.5f; // 콤보 유효 시간
    private float resetMouseDelay = 0.3f; // 마우스 좌표 리셋 시간
    private Vector2 lastMousePosition; // 마지막 마우스 좌표 저장
    private bool isAttacking = false;  // 공격 상태 체크

    void Awake()
    {

        animator = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        if (!isAttacking)
        {
            PlayerAnime(); // 플레이어 애니메이션 갱신
        }

        if (Input.GetMouseButtonDown(0))
        {
            // 매 프레임마다 회전값을 (0, 0, 0)으로 고정
            transform.rotation = Quaternion.Euler(0, 0, 0);
            HandleAttack();
        }

        if (Time.time - lastAttackTime > comboDelay)
        {
            comboStep = 0; // 콤보 상태 초기화
        }
    }

    // 플레이어 애니메이션 처리
    private void PlayerAnime()
    {
        HandleMovement();   // 이동 애니메이션 처리
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

        if (direction != Vector2.zero && !isAttacking) // 공격 중일 때는 이동 애니메이션 중단
        {
            PlayWalkAnimation(direction);
        }
        else
        {
            ResetAllTriggers();
            animator.SetTrigger("Stand");  // 이동하지 않으면 스탠드 애니메이션 실행
        }
    }

    // 걷기 애니메이션 실행 함수
    private void PlayWalkAnimation(Vector2 direction)
    {
        ResetAllTriggers();

        if (direction.x < 0) // 왼쪽 이동
        {
            animator.SetTrigger("WalkLeft");
        }
        else if (direction.x > 0) // 오른쪽 이동
        {
            animator.SetTrigger("WalkRight");
        }
        else if (direction.y > 0) // 위쪽 이동
        {
            animator.SetTrigger("WalkUp");
        }
        else if (direction.y < 0) // 아래쪽 이동
        {
            animator.SetTrigger("WalkDown");
        }
    }

    // 공격 처리 함수
    void HandleAttack()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -10f; // Z 값을 -10으로 고정
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // 플레이어와 마우스 위치 사이의 거리 계산
        float distanceToMouse = Vector2.Distance(worldPosition, playerTransform.position);

        if (distanceToMouse > 0.1f) // 거리가 매우 짧지 않으면 공격 수행
        {
            lastMousePosition = worldPosition; // 마지막 마우스 좌표 저장
            isAttacking = true;  // 공격 상태 설정
            DetermineAttackDirection(worldPosition);
        }

        // 콤보 상태 업데이트
        comboStep = (comboStep + 1) % 2; // 0과 1 사이에서 반복
        lastAttackTime = Time.time;

        // 0.3초 후에 마우스 좌표 리셋
        Invoke("ResetMousePosition", resetMouseDelay);
    }

    // 공격 방향 결정 함수 (플레이어 방향과 무관하게 좌표 기반)
    private void DetermineAttackDirection(Vector2 worldPosition)
    {
        attackDirection = (worldPosition - (Vector2)playerTransform.position).normalized;

        ResetAllTriggers(); // 공격 전에 모든 트리거를 초기화

        // 공격 방향 결정 (마우스 클릭 위치를 기준으로)
        if (attackDirection.x > 0) // 오른쪽 공격
        {
            

            if (comboStep == 0)
            {
                RightAttackEffect.SetActive(false);
                RightAttackEffect.SetActive(true);
                animator.SetTrigger("RightAttack");
            }
            else
            {
                RightComboEffect.SetActive(false);
                RightComboEffect.SetActive(true);
                animator.SetTrigger("RightComboAttack");
            }
        }
        else if (attackDirection.x < 0) // 왼쪽 공격
        {


            if (comboStep == 0)
            {
                LeftAttackEffect.SetActive(false);
                LeftAttackEffect.SetActive(true);
                animator.SetTrigger("LeftAttack");
            }
            else
            {
                LeftComboEffect.SetActive(false);
                LeftComboEffect.SetActive(true);
                animator.SetTrigger("LeftComboAttack");
            }
        }

        // 공격이 끝난 후 상태 초기화
        Invoke("EndAttack", 0.3f);  // 공격이 끝난 후 0.3초 뒤에 공격 상태 해제
    }

    // 공격 상태 종료 함수
    private void EndAttack()
    {
        isAttacking = false;  // 공격 상태 해제
    }

    // 마우스 좌표 리셋 함수
    private void ResetMousePosition()
    {
        lastMousePosition = Vector2.zero; // 마우스 좌표를 0으로 리셋
    }

    // 애니메이션 트리거 초기화 함수
    private void ResetAllTriggers()
    {
        // 모든 트리거를 초기화하여 상태 전환을 원활하게 함
        animator.ResetTrigger("WalkLeft");
        animator.ResetTrigger("WalkRight");
        animator.ResetTrigger("WalkUp");
        animator.ResetTrigger("WalkDown");
        animator.ResetTrigger("Stand");
        animator.ResetTrigger("LeftAttack");
        animator.ResetTrigger("LeftComboAttack");
        animator.ResetTrigger("RightAttack");
        animator.ResetTrigger("RightComboAttack");
    }
}

using UnityEngine;

// 에그슬라임 애니매이션
public class MonsterAnime2 : MonoBehaviour
{
    private Animator animator;
    private Vector3 previousPosition;  // 이전 프레임의 위치를 저장할 변수
    private string currentTrigger = "";  // 현재 활성화된 트리거 저장
    public Transform player;
    public MonsterAI Ma;
    public float attackRange = 5f;  // 공격 범위 설정
    private bool isAttacking = false;  // 현재 공격 중인지 확인

    void Awake()
    {
        Ma = GetComponent<MonsterAI>();
    }

    void Start()
    {
        // 플레이어를 찾는 함수를 호출
        FindPlayer();

        animator = GetComponent<Animator>();

        // 기본 상태를 Stand로 설정 (처음에는 대기 상태)
        SetAnimationTrigger("Stand");

        // 시작할 때 이전 위치를 현재 위치로 초기화
        previousPosition = transform.position;

        // 처음에 회전 값을 (0, 0, 0)으로 설정합니다.
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void Update()
    {
        // 매 프레임마다 플레이어가 있는지 확인하고 없으면 다시 찾기 시도
        if (player == null)
        {
            FindPlayer();
        }

        // 매 프레임마다 회전 값을 (0, 0, 0)으로 고정합니다.
        transform.rotation = Quaternion.Euler(0, 0, 0);

        // 현재 몬스터 위치 가져오기
        Vector3 currentPosition = transform.position;

        // 플레이어와 몬스터 간의 거리 계산
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // 공격 범위에 들어왔을 때 공격 애니메이션 실행
        if (distanceToPlayer <= attackRange && !isAttacking)
        {
            PlayAttackAnimation(currentPosition, previousPosition);  // 공격 애니메이션 실행
            isAttacking = true;  // 공격 중 상태로 변경
        }
        else if (distanceToPlayer > attackRange && isAttacking)
        {
            isAttacking = false;  // 공격 상태 해제
        }

        // 공격 중이 아닐 때만 이동 애니메이션 관리
        if (!isAttacking)
        {
            // 이동이 없는 경우 스탠딩 상태 유지
            if (currentPosition == previousPosition)
            {
                SetAnimationTrigger("Stand");
            }
            else
            {
                // 이동 애니메이션 실행
                PlayWalkAnimation(currentPosition, previousPosition);
            }
        }

        // 현재 위치를 이전 위치로 업데이트
        previousPosition = currentPosition;
    }

    // 플레이어를 찾는 함수
    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    // 공격 상태일 때 실행되는 애니메이션 설정 함수
    private void PlayAttackAnimation(Vector3 currentPosition, Vector3 previousPosition)
    {
        // 수평 이동이 더 큰 경우
        if (Mathf.Abs(currentPosition.x - previousPosition.x) > Mathf.Abs(currentPosition.y - previousPosition.y))
        {
            if (currentPosition.x < previousPosition.x) // 왼쪽 방향
            {
                SetAnimationTrigger("AttackLeft");
            }
            else if (currentPosition.x > previousPosition.x) // 오른쪽 방향
            {
                SetAnimationTrigger("AttackRight");
            }
        }
        // 수직 이동이 더 큰 경우
        else
        {
            if (currentPosition.y > previousPosition.y) // 위쪽 방향
            {
                SetAnimationTrigger("AttackUp");
            }
            else if (currentPosition.y < previousPosition.y) // 아래쪽 방향
            {
                SetAnimationTrigger("AttackDown");
            }
        }
    }

    // 이전 위치와 현재 위치를 비교하여 이동 애니메이션 설정
    private void PlayWalkAnimation(Vector3 currentPosition, Vector3 previousPosition)
    {
        // 수평 이동이 더 큰 경우
        if (Mathf.Abs(currentPosition.x - previousPosition.x) > Mathf.Abs(currentPosition.y - previousPosition.y))
        {
            if (currentPosition.x < previousPosition.x) // 왼쪽 이동
            {
                SetAnimationTrigger("WalkLeft");
            }
            else if (currentPosition.x > previousPosition.x) // 오른쪽 이동
            {
                SetAnimationTrigger("WalkRight");
            }
        }
        // 수직 이동이 더 큰 경우
        else
        {
            if (currentPosition.y > previousPosition.y) // 위쪽 이동
            {
                SetAnimationTrigger("WalkUp");
            }
            else if (currentPosition.y < previousPosition.y) // 아래쪽 이동
            {
                SetAnimationTrigger("WalkDown");
            }
        }
    }

    // 애니메이션 트리거를 설정하고 기존 트리거 리셋
    private void SetAnimationTrigger(string triggerName)
    {
        // 중복 설정 방지: 현재 트리거와 동일하면 무시
        if (currentTrigger == triggerName) return;

        // 트리거가 바뀔 때만 모든 트리거 초기화
        ResetAllTriggers();

        // 해당하는 트리거 활성화
        animator.SetTrigger(triggerName);

        // 현재 활성화된 트리거 저장
        currentTrigger = triggerName;
    }

    // 모든 애니메이션 트리거 초기화 함수
    private void ResetAllTriggers()
    {
        animator.ResetTrigger("WalkUp");
        animator.ResetTrigger("WalkDown");
        animator.ResetTrigger("WalkLeft");
        animator.ResetTrigger("WalkRight");
        animator.ResetTrigger("Stand");
        animator.ResetTrigger("AttackUp");
        animator.ResetTrigger("AttackDown");
        animator.ResetTrigger("AttackLeft");
        animator.ResetTrigger("AttackRight");
    }
}

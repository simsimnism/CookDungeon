using UnityEngine;

public class MonsterAnime : MonoBehaviour
{
    private Animator animator;
    private Vector3 previousPosition;  // 이전 프레임의 위치를 저장할 변수
    private string currentTrigger = "";  // 현재 활성화된 트리거 저장

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        // 기본 상태를 Stand로 설정 (처음에는 대기 상태)
        SetAnimationTrigger("Stand");

        // 시작할 때 이전 위치를 현재 위치로 초기화
        previousPosition = transform.position;

        // 매 프레임마다 회전 값을 (0, 0, 0)으로 고정합니다.
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // 매 프레임마다 회전 값을 (0, 0, 0)으로 고정합니다.
        transform.rotation = Quaternion.Euler(0, 0, 0);

        // 현재 몬스터 위치 가져오기
        Vector3 currentPosition = transform.position;

        // 이동이 있는지 확인
        if (currentPosition == previousPosition)
        {
            SetAnimationTrigger("Stand");
        }
        else
        {
            // 이전 위치와 현재 위치 비교하여 애니메이션 설정
            PlayWalkAnimation(currentPosition, previousPosition);
        }

        // 현재 위치를 이전 위치로 업데이트
        previousPosition = currentPosition;
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

    // 이전 위치와 현재 위치를 비교하여 애니메이션 설정
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

    // 모든 애니메이션 트리거 초기화 함수
    private void ResetAllTriggers()
    {
        animator.ResetTrigger("WalkUp");
        animator.ResetTrigger("WalkDown");
        animator.ResetTrigger("WalkLeft");
        animator.ResetTrigger("WalkRight");
        animator.ResetTrigger("Stand");
    }
}

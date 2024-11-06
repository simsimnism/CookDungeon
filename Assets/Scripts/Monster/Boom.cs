using UnityEngine;

public class Boom : MonoBehaviour
{
    private Animator animator; // Animator 컴포넌트
    private MonsterAI3 monsterAI; // 기존 몬스터 AI 스크립트
    private bool isAttacking = false; // 공격 중인지 여부 확인

    void Start()
    {
        // Animator와 MonsterAI3 컴포넌트 연결
        animator = GetComponent<Animator>();
        monsterAI = GetComponent<MonsterAI3>();
    }

    void Update()
    {
        // 공격 상태에 따른 애니메이션 트리거 설정
        if (monsterAI.isAttacking && !isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("Attack");  // 공격 애니메이션 실행
        }
        else if (!monsterAI.isAttacking && isAttacking)
        {
            isAttacking = false;
            animator.ResetTrigger("Attack");  // 공격 끝났을 때 초기화
            animator.SetBool("IsIdle", true);  // Idle 상태로 전환
        }
    }
}

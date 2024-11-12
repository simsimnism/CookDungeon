using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour
{
    private float attackRange = 4f;           // 공격 범위 (원의 반지름)
    private float attackDelay = 1.5f;           // 공격 딜레이 (초 단위)
    private int damage = 10;                  // 공격 데미지
    private float lastAttackTime = 0f; // 마지막 공격 시점

    private List<Transform> enemiesInRange = new List<Transform>();
    private float attackTimer = 0f;

    // 파티클 시스템을 연결하기 위한 변수
    [SerializeField] private ParticleSystem attackParticle;
    [SerializeField] private float playDuration = 0.05f; // 재생할 구간 시간

    private void Start()
    {
        // 시작 시 파티클 비활성화
        if (attackParticle != null)
        {
            attackParticle.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // 현재 시간과 마지막 공격 시간을 비교하여 일정한 간격을 유지
        if (Time.time >= lastAttackTime + attackDelay)
        {
            DetectEnemiesInRange(); // 범위 내 적 감지
            AttackFirstEnemy(); // 범위 내 첫 번째 적 공격
            lastAttackTime = Time.time; // 마지막 공격 시간 갱신
        }
    }

    private void DetectEnemiesInRange()
    {
        enemiesInRange.Clear(); // 이전 적 목록 초기화
        // 범위 내의 적을 감지
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Monsters"))
            {
                enemiesInRange.Add(collider.transform);
            }
        }
    }

    private void AttackFirstEnemy()
    {
        if (enemiesInRange.Count > 0) // 범위 내에 적이 있는지 확인
        {
            Transform enemy = enemiesInRange[0]; // 첫 번째 적 선택

            if (enemy != null)
            {
                // 파티클을 적 위치에서 즉시 재생
                PlayAttackParticle(enemy.position);

                // 몬스터에게 데미지를 가하는 함수 호출
                MonsterAI monsterAI = enemy.GetComponent<MonsterAI>();
                if (monsterAI != null)
                {
                    monsterAI.TakeDamage(damage, (enemy.position - transform.position).normalized);

                    // 데미지 후 생존 여부 확인
                    if (monsterAI.health <= 0) // health가 0 이하인지 확인
                    {
                        enemiesInRange.RemoveAt(0); // 사망한 몬스터를 리스트에서 제거
                    }
                }
            }
        }
    }

    private void PlayAttackParticle(Vector3 position)
    {
        if (attackParticle != null)
        {
            // 파티클 위치를 몬스터 위치로 설정하고 활성화
            attackParticle.transform.position = position;
            attackParticle.transform.rotation = Quaternion.identity; // 방향을 초기화하여 고정
            attackParticle.gameObject.SetActive(true);

            // 특정 시간만큼 미리 진행된 상태로 시작
            attackParticle.Simulate(1f, true, true);
            attackParticle.Play();

            // 파티클 재생 시간 후 비활성화 처리
            StartCoroutine(DeactivateParticleAfterPlay());
        }
    }

    private IEnumerator DeactivateParticleAfterPlay()
    {
        // playDuration에 설정된 시간만큼 대기
        yield return new WaitForSeconds(playDuration);
        attackParticle.gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        // 공격 범위 시각화를 위한 Gizmos
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

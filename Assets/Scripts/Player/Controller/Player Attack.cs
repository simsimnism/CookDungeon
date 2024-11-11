using UnityEngine;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour
{
    private float attackRange = 3f;           // 공격 범위 (원의 반지름)
    private float attackDelay = 2f;           // 공격 딜레이 (초 단위)
    public int damage = 10;                  // 공격 데미지

    private List<Transform> enemiesInRange = new List<Transform>();
    private float attackTimer = 0f;

    private void Update()
    {
        attackTimer += Time.deltaTime;

        // 공격 가능 여부 확인
        if (attackTimer >= attackDelay)
        {
            DetectEnemiesInRange(); // 범위 내 적 감지
            AttackEnemies(); // 모든 적 공격
            attackTimer = 0f; // 타이머 초기화
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

    private void AttackEnemies()
    {
        for (int i = enemiesInRange.Count - 1; i >= 0; i--) // 역순으로 루프
        {
            Transform enemy = enemiesInRange[i];

            if (enemy == null)
            {
                enemiesInRange.RemoveAt(i); // 적이 이미 제거된 경우 리스트에서 제거
                continue;
            }

            // 몬스터에게 데미지를 가하는 함수 호출
            MonsterAI monsterAI = enemy.GetComponent<MonsterAI>();
            if (monsterAI != null)
            {
                monsterAI.TakeDamage(damage, (enemy.position - transform.position).normalized);

                // 데미지 후 생존 여부 확인
                if (enemy.GetComponent<MonsterAI>().health <= 0) // health가 0 이하인지 확인
                {
                    enemiesInRange.RemoveAt(i); // 사망한 몬스터를 리스트에서 제거
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // 공격 범위 시각화를 위한 Gizmos
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

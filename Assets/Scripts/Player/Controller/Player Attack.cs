using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 5f;           // 공격 범위 (원의 반지름)
    public float attackDelay = 1f;           // 공격 딜레이 (초 단위)
    public int damage = 10;                  // 공격 데미지

    private List<Transform> enemiesInRange = new List<Transform>();
    private float attackTimer = 0f;

    private void Update()
    {
        attackTimer += Time.deltaTime;

        // 공격 가능 여부 확인
        if (attackTimer >= attackDelay && enemiesInRange.Count > 0)
        {
            AttackClosestEnemy();
            attackTimer = 0f; // 타이머 초기화
        }
    }

    private void AttackClosestEnemy()
    {
        Transform closestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Transform enemy in enemiesInRange)
        {
            if (enemy == null) continue; // 적이 이미 제거된 경우 건너뜁니다.

            float distanceToEnemy = Vector2.Distance(transform.position, enemy.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            // 몬스터에게 데미지를 가하는 함수 호출
            MonsterAI monsterAI = closestEnemy.GetComponent<MonsterAI>();
            if (monsterAI != null)
            {
                monsterAI.TakeDamage(damage, (closestEnemy.position - transform.position).normalized);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // "Monster" 태그로 범위 내의 적을 감지
        if (other.CompareTag("Monsters"))
        {
            enemiesInRange.Add(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 범위 밖으로 나간 적을 리스트에서 제거
        if (other.CompareTag("Monsters"))
        {
            enemiesInRange.Remove(other.transform);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // 공격 범위 시각화를 위한 Gizmos
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

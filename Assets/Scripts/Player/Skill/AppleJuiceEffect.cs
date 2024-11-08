using UnityEngine;

public class AppleJuiceEffect : MonoBehaviour
{
    private float radius;
    private float damagePerSecond;
    private float duration;
    private float timer = 0f;

    // 초기화 메서드 - 사과주스 효과의 속성을 설정
    public void Initialize(float radius, float damagePerSecond, float duration)
    {
        this.radius = radius;
        this.damagePerSecond = damagePerSecond;
        this.duration = duration;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // 지속 시간이 지나면 사과주스 효과 제거
        if (timer >= duration)
        {
            Destroy(gameObject);
        }

        // 범위 내 적에게 지속 데미지
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Monster"))
            {
                MonsterAI monsterAI = enemy.GetComponent<MonsterAI>();
                if (monsterAI != null)
                {
                    monsterAI.TakeDamage((int)(damagePerSecond * Time.deltaTime), (enemy.transform.position - transform.position).normalized);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // 사과주스 범위를 시각화하기 위한 Gizmos
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

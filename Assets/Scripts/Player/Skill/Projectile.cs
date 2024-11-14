using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    private float damage = 10f;
    private Transform target;

    public void Initialize(Transform targetTransform)
    {
        target = targetTransform;
        StartCoroutine(DestroyAfterTime(3f)); // 3초 후 자동 삭제
    }

    void Update()
    {
        if (target != null)
        {
            // 타겟 방향으로 이동
            Vector2 direction = (target.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monsters"))
        {
            // 몬스터에 맞았을 때 피해를 주고 삭제
            MonsterAI monsterAI = collision.GetComponent<MonsterAI>();
            monsterAI?.TakeDamage((int)damage, (collision.transform.position - transform.position).normalized);
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}

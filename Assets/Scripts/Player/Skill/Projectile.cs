using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    private float damage;

    public void SetDamage(float damageValue)
    {
        damage = damageValue;
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            Vector3 hitDirection = (collision.transform.position - transform.position).normalized;
            collision.GetComponent<MonsterAI>().TakeDamage((int)damage, hitDirection); // hitDirection을 추가하여 호출
            Destroy(gameObject); // 적에게 충돌 시 발사체 파괴
        }
    }
}

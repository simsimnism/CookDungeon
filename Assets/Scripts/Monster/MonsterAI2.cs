using UnityEngine;
using System.Collections;

//오랜지 복어
public class MonsterAI2 : MonoBehaviour
{
    public MonsterDataSO monsterDataSO;  // ScriptableObject로 데이터를 저장
    [SerializeField] private GameObject monsters;
    public int health;
    public int attack;

    public float range;
    public float speed;
    public GameObject bubblePrefab;
    public Transform player;

    private bool isFixedPosition = false;
    Rigidbody2D rb;

    // 피격 시 정해진 거리만큼 뒤로 밀려남
    public float knockbackDistance = 0.1f;  // 정해진 넉백 거리

    void Start()
    {
        Orenge();
    }

    void Update()
    {
        Orenge();
    }

    // 모든 몬스터가 플레이어의 공격을 받아 데미지를 입는 로직
    public void TakeDamage(int damage, Vector3 hitDirection)
    {
        health = 10;
        // 체력 감소
        health -= damage;
        Debug.Log($"{gameObject.name} 가 {damage} 의 데미지를, remaining health: {health}");

        // 체력이 0 이하로 떨어지면 몬스터 사망
        if (health <= 0)
        {
            Die();
            return;
        }

    }
    // 몬스터가 죽을 때 처리
    void Die()
    {
        Debug.Log($"{gameObject.name} has died!");
        Destroy(gameObject);  // 몬스터 오브젝트 제거
    }

    // 오렌지 복어
    void Orenge()
    {
        if (isFixedPosition == true)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= range)
            {
                FireBubble();
            }
        }
    }

    // 거품 발사 로직 (bullet 스크립트를 사용하는 발사체)
    void FireBubble()
    {
        Debug.Log("Orenge is firing a bubble!");
        if (bubblePrefab != null)
        {
            GameObject bubble = Instantiate(bubblePrefab, transform.position, Quaternion.identity);
            Bullet bulletScript = bubble.GetComponent<Bullet>();
            if (bulletScript != null && player != null)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                bulletScript.SetDirection(direction);
            }
        }
        else
        {
            Debug.LogWarning("Bubble prefab is not assigned!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        attack = 1;
        // 플레이어와 충돌 시
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(attack);  // 플레이어에게 몬스터의 공격력만큼 데미지 입힘
            }
        }
    }
   
}

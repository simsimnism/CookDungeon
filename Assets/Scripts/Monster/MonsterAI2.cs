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
        // "(Clone)"을 제거하고 이름을 가져옴
        string monsterName = gameObject.name.Replace("(Clone)", "").Trim();

        // 이름을 기준으로 몬스터 데이터를 검색
        MonsterDataSO data = Managers.Data.GetMonsterDataByName(monsterName);
        if (data != null)
        {
            AssignData(data);  // 데이터를 AI에 할당
        }
        else
        {
            Debug.LogError($"Monster 이름을 파싱할 수 없습니다: {monsterName}");
        }
        MonsterCollisionIgnore();
    }

    // 데이터를 할당하는 메서드
    void AssignData(MonsterDataSO data)
    {
        monsterDataSO = data;

        health = monsterDataSO.health;
        attack = monsterDataSO.attack;
        range = monsterDataSO.range;
        speed = monsterDataSO.speed;

        Debug.Log($"몬스터 데이터 적용됨: {monsterDataSO.monsterName} (ID: {monsterDataSO.id})");
    }

    void Update()
    {
        Orenge();
    }

    void MonsterCollisionIgnore()
    {
        // "Monster" 태그를 가진 모든 오브젝트를 찾습니다.
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monsters");

        // 각 몬스터 오브젝트들의 Collider를 가져와 서로 충돌을 무시하도록 설정합니다.
        for (int i = 0; i < monsters.Length; i++)
        {
            for (int j = i + 1; j < monsters.Length; j++)
            {
                Collider col1 = monsters[i].GetComponent<Collider>();
                Collider col2 = monsters[j].GetComponent<Collider>();

                if (col1 != null && col2 != null)
                {
                    // 두 Collider 간의 충돌을 무시합니다.
                    Physics.IgnoreCollision(col1, col2);
                }
            }
        }
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

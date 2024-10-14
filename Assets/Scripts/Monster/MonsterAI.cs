using UnityEngine;
using System.Collections;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MonsterAI : MonoBehaviour
{
    public MonsterDataSO monsterDataSO;  // ScriptableObject로 데이터를 저장

    private int health;
    private int attack;
    private float range;
    private float speed;
    private int id;  // 몬스터의 ID
    public Transform player;

    //넉백 관련 로직
    public float knockbackForce = 2f;  // AddForce에 사용할 힘
    public float maxKnockbackDistance = 2f;  // 최대 이동 거리
    public float knockbackDistance = 2f;  // 정해진 넉백 거리
    private Vector3 knockbackStartPos;  // 넉백이 시작된 위치
    Rigidbody2D rb;

    //랜덤이동 관련로직
    private Vector2 randomDirection; // 랜덤 이동 방향
    public float changeDirectionTime = 3f; //랜덤방향 이동 변경주기
    private float timer = 0; // 랜덤이동 타이머
    private enum MonsterState { Idle, Chasing }; // 상태 관리
    private MonsterState currentState = MonsterState.Idle;

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
        player = GameObject.FindWithTag("Player").transform;
        MonsterMovement();
        // z축 좌표 고정
        Vector3 fixedPosition = transform.position;
        fixedPosition.z = 0;  // 원하는 z값으로 고정, 예: 0
        transform.position = fixedPosition;
    }

    // 몬스터가 데미지를 입는 메서드 (예시)
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }


    void MonsterMovement()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < range)
        {
            currentState = MonsterState.Chasing;
        }
        else
        {
            currentState = MonsterState.Idle;
        }

        if (currentState == MonsterState.Chasing)
        {
            ChasePlayer();
        }
        else if (currentState == MonsterState.Idle)
        {
            RandomMovement();
        }
    }

    //플레이어를 추적하는 로직
    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    void RandomMovement()
    {
        timer += Time.deltaTime;
        if (timer > changeDirectionTime)
        {
            randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            timer = 0f;
        }

        transform.position += (Vector3)randomDirection * speed * Time.deltaTime;
    }

    // 모든 몬스터가 플레이어의 공격을 받아 데미지를 입는 로직
    public void TakeDamage(int damage, Vector3 hitDirection)
    {
        // 체력 감소
        health -= damage;
        Debug.Log($"{gameObject.name} 가 {damage} 의 데미지를, remaining health: {health}");

        // 체력이 0 이하로 떨어지면 몬스터 사망
        if (health <= 0)
        {
            Die();
            return;
        }

        // 피격 시 밀려나는 효과 (노크백)
        Knockback(hitDirection);
    }

    // 넉백 처리 (코루틴으로 거리 체크)
    public void Knockback(Vector3 hitDirection)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // 넉백 시작 위치 저장
            knockbackStartPos = transform.position;

            // 피격 방향으로 AddForce 적용
            Vector2 knockbackDirection = hitDirection.normalized * knockbackForce;
            rb.AddForce(knockbackDirection, ForceMode2D.Impulse);

            // 넉백 중 거리 체크를 위한 코루틴 실행
            StartCoroutine(CheckKnockbackDistance(rb));
        }
    }

    // 넉백 거리 체크 코루틴
    private IEnumerator CheckKnockbackDistance(Rigidbody2D rb)
    {
        while (true)
        {
            float distanceMoved = Vector3.Distance(knockbackStartPos, transform.position);

            if (distanceMoved >= maxKnockbackDistance)
            {
                // 최대 이동 거리를 넘으면 넉백 중단
                StopKnockback(rb);
                yield break;  // 코루틴 종료
            }

            yield return null;  // 다음 프레임까지 대기
        }
    }

    // 넉백 중단 처리
    void StopKnockback(Rigidbody2D rb)
    {
        // Rigidbody2D의 속도를 0으로 설정하여 멈춤
        rb.velocity = Vector2.zero;
    }

    // 몬스터가 죽을 때 처리
    void Die()
    {
        Debug.Log($"{gameObject.name} has died!");
        Destroy(gameObject);  // 몬스터 오브젝트 제거
    }

    //몬스터의 충돌처리
    void OnTriggerEnter2D(Collider2D other)
    {
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
    
    //몬스터끼리 충돌하지 않도록 하는 로직
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
}

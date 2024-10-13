using UnityEngine;
using System.Collections;
using static UnityEditor.Experimental.GraphView.GraphView;

//에그 슬라임, 포테비, 만드래플
public class MonsterAI : MonoBehaviour
{
    [SerializeField] private MonsterDataSO monsterData;
    public int health;
    public int attack;
    public int attackrange;
    public float range;
    public float speed;
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
    private float timer =0; // 랜덤이동 타이머

    private enum MonsterState { Idle, Chasing }; // 상태 관리
    private MonsterState currentState = MonsterState.Idle;

    void Start()
    {
        // 스크립터블 오브젝트의 값을 할당
        if (monsterData != null)
        {
            health = monsterData.health;
            attack = monsterData.attack;
            range = monsterData.range;
            speed = monsterData.speed;
        }

        randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    void Update()
    {
        player = GameObject.FindWithTag("Player").transform; // 플레이어 오브젝트를 태그로 찾아 Transform 할당
        MonsterMovement();
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
    
    //-----------------------------------------------------------------//

    //몬스터의 전체적인 움직임을 관리하는 코드
    void MonsterMovement()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // 플레이어를 감지하면 추적 상태로 전환, 감지 범위 밖에 있으면 랜덤 이동 상태
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
            // 플레이어 추적 로직
            ChasePlayer();
        }
        else if (currentState == MonsterState.Idle)
        {
            // 랜덤 이동 로직
            RandomMovement();
        }

    }

    //몬스터가 플레이어를 추적하는 로직
    void ChasePlayer()
    {
        // 플레이어를 향해 이동
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    void RandomMovement()
    {
        // 랜덤 이동 로직
        timer += Time.deltaTime;
        if (timer > changeDirectionTime)
        {
            // 방향을 랜덤하게 변경
            randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            timer = 0f; // 타이머 리셋
        }
        // 랜덤 방향으로 이동
        transform.position += (Vector3)randomDirection *speed * Time.deltaTime;
    }
}

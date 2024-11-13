using UnityEngine;
using System.Collections;

// 베추구리
public class MonsterAI3 : MonoBehaviour
{
    public MonsterDataSO monsterDataSO;  // ScriptableObject로 데이터를 저장

    private int health;
    private int attack;
    private int attackRange;
    private float range;
    private float speed;
    private int id;  // 몬스터의 ID
    public Transform player;

    // 넉백 관련 로직
    public float knockbackForce = 2f;  // AddForce에 사용할 힘
    public float maxKnockbackDistance = 2f;  // 최대 이동 거리
    private Vector3 knockbackStartPos;  // 넉백이 시작된 위치
    Rigidbody2D rb;

    // 랜덤 이동 관련 로직
    private Vector2 randomDirection; // 랜덤 이동 방향
    public float changeDirectionTime = 3f; // 랜덤 방향 이동 변경 주기
    private float timer = 0; // 랜덤 이동 타이머
    private enum MonsterState { Idle, Chasing }; // 상태 관리

    public bool isAttacking = false;  // 플레이어 공격 중인지 여부를 추적

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
        attackRange = monsterDataSO.attackRange;
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
        Managers.Sound.PlaySFX(Define.SFX.Hit1, -1);
        health -= damage;
        if (health <= 0)
        {
            MonsterDestroyed();
        }
    }

    void MonsterMovement()
    {
        if (isAttacking) return;  // 공격 중일 때 다른 행동을 하지 않음

        // 플레이어와 몬스터 사이 거리 계산
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        attackRange = 3;
        // 플레이어가 공격 범위 안에 들어오면 공격 실행
        if (distanceToPlayer <= attackRange)
        {
            StartCoroutine(StopAndAttackPlayer());  // 즉시 공격
            return;  // 공격을 시작하면 추격 중단
        }

        // 공격 범위 안에 있지 않으면 플레이어 추격
        if (distanceToPlayer < range)
        {
            ChasePlayer();  // 추격
        }
        else
        {

            RandomMovement();  // 플레이어가 없을 때는 랜덤 이동
        }
    }

    // 1초 후 플레이어와 자신에게 데미지를 주는 코루틴
    IEnumerator StopAndAttackPlayer()
    {
        if (isAttacking) yield break;  // 이미 공격 중이면 중복 실행 방지

        isAttacking = true;  // 공격 중으로 상태 전환
        Debug.Log("플레이어를 감지했습니다. 1초 후에 공격합니다.");

        // 1초 대기
        yield return new WaitForSeconds(1f);

        // 플레이어와 자신에게 데미지 주기
        Managers.Sound.PlaySFX(Define.SFX.Bomb1);
        AttackPlayerAndSelf();

        // 공격이 끝나면 다시 행동 가능
        isAttacking = false;
    }

    // 플레이어에게만 범위 내에서 데미지를 주는 메서드
    void AttackPlayerAndSelf()
    {
        // 자신의 위치를 기준으로 원형 범위 내의 콜라이더 탐색
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (var hit in hits)
        {
            // 탐지된 객체가 플레이어라면 데미지
            if (hit.CompareTag("Player"))
            {
                Player playerComponent = hit.GetComponent<Player>();
                if (playerComponent != null)
                {
                    playerComponent.TakeDamage(attack);
                    Debug.Log($"플레이어 {playerComponent.name}에게 {attack} 데미지를 주었습니다.");
                }
            }
        }

        // 자신에게도 데미지
        TakeDamage(attack);
        Debug.Log($"자신 {gameObject.name}에게 {attack} 데미지를 주었습니다.");
    }

    // 플레이어를 추적하는 로직
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

    //몬스터가 사망할 때 아이템을 떨어트리는 로직(그냥 사망 로직과 통합)
    private void MonsterDestroyed()
    {
        DestroyEvent destroyedEvent = GetComponent<DestroyEvent>();
        destroyedEvent.CallDestroyedEvent(false, 0);

        // 5% 확률로 프리팹 드랍
        float dropChance = Random.Range(0f, 1f);
        if (dropChance <= 0.5f)  // 5% 확률 체크
        {
            string prefabPath = null;

            // 몬스터의 ID에 따른 드랍 프리팹 경로 설정
            switch (id)
            {
                case 1: // 예: ID가 1인 몬스터
                    prefabPath = "Food/Apple";  // 사과 프리팹 경로
                    break;
                case 2:
                    prefabPath = "Food/Mushroom";  // 빵 프리팹 경로
                    break;
                case 3: // 예: ID가 3인 몬스터
                    prefabPath = "Food/Egg";  // 감자 프리팹 경로
                    break;
                case 4: // 예: ID가 3인 몬스터
                    prefabPath = "Food/Cabbage";  // 감자 프리팹 경로
                    break;
                case 5: // 예: ID가 3인 몬스터
                    prefabPath = "Food/Orange";  // 감자 프리팹 경로
                    break;
                case 6: // 예: ID가 3인 몬스터
                    prefabPath = "Food/Potato";  // 감자 프리팹 경로
                    break;
                default:
                    prefabPath = "Food/DefaultFood";  // 기본 드랍 아이템 경로
                    break;
            }

            // Managers.Resource를 통해 프리팹 소환
            if (prefabPath != null)
            {
                GameObject dropPrefab = Managers.Resource.Instantiate(prefabPath, null);  // 부모 객체 지정 없이 인스턴스화
                if (dropPrefab != null)
                {
                    dropPrefab.transform.position = transform.position;  // 드랍 위치 설정
                    Debug.Log($"{prefabPath} 프리팹이 성공적으로 드랍되었습니다.");
                }
                else
                {
                    Debug.LogWarning("프리팹 드랍 실패: 경로를 확인하세요.");
                }
            }
        }
    }

    // 몬스터의 충돌 처리
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

    // 몬스터끼리 충돌하지 않도록 하는 로직
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

    // 공격 범위를 시각적으로 확인하기 위한 Gizmo
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

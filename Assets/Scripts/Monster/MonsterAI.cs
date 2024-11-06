using UnityEngine;
using System.Collections;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(DestroyEvent))]
[RequireComponent(typeof(MonsterDestroy))]

public class MonsterAI : MonoBehaviour
{
    public MonsterDataSO monsterDataSO;  // ScriptableObject로 데이터를 저장

    public int health;
    private int attack;
    private float range;

    private float attackRange;

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

    // �����͸� �Ҵ��ϴ� �޼���
    void AssignData(MonsterDataSO data)
    {
        monsterDataSO = data;

        health = monsterDataSO.health;
        attack = monsterDataSO.attack;

        attackRange = monsterDataSO.attackRange;
        id = monsterDataSO.id;  
        range = monsterDataSO.range;
        speed = monsterDataSO.speed;

        Debug.Log($"몬스터 데이터 적용됨: {monsterDataSO.monsterName} (ID: {monsterDataSO.id})");
    }

    void Update()
    {
        player = GameObject.FindWithTag("Player").transform;
        MonsterMovement();
        // z�� ��ǥ ����
        Vector3 fixedPosition = transform.position;
        fixedPosition.z = 0;  // ���ϴ� z������ ����, ��: 0
        transform.position = fixedPosition;
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

    //�÷��̾ �����ϴ� ����
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

    // ��� ���Ͱ� �÷��̾��� ������ �޾� �������� �Դ� ����
    public void TakeDamage(int damage, Vector3 hitDirection)
    {
        // ü�� ����
        health -= damage;
        Debug.Log($"{gameObject.name} 가 {damage} 의 데미지를, remaining health: {health}");

        // ü���� 0 ���Ϸ� �������� ���� ���
        if (health <= 0)
        {
            MonsterDestroyed();
            return;
        }

        // �ǰ� �� �з����� ȿ�� (��ũ��)
        Knockback(hitDirection);
    }

    // �˹� ó�� (�ڷ�ƾ���� �Ÿ� üũ)
    public void Knockback(Vector3 hitDirection)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // �˹� ���� ��ġ ����
            knockbackStartPos = transform.position;

            // �ǰ� �������� AddForce ����
            Vector2 knockbackDirection = hitDirection.normalized * knockbackForce;
            rb.AddForce(knockbackDirection, ForceMode2D.Impulse);

            // �˹� �� �Ÿ� üũ�� ���� �ڷ�ƾ ����
            StartCoroutine(CheckKnockbackDistance(rb));
        }
    }

    // �˹� �Ÿ� üũ �ڷ�ƾ
    private IEnumerator CheckKnockbackDistance(Rigidbody2D rb)
    {
        while (true)
        {
            float distanceMoved = Vector3.Distance(knockbackStartPos, transform.position);

            if (distanceMoved >= maxKnockbackDistance)
            {
                // �ִ� �̵� �Ÿ��� ������ �˹� �ߴ�
                StopKnockback(rb);
                yield break;  // �ڷ�ƾ ����
            }

            yield return null;  // ���� �����ӱ��� ���
        }
    }

    // �˹� �ߴ� ó��
    void StopKnockback(Rigidbody2D rb)
    {
        // Rigidbody2D�� �ӵ��� 0���� �����Ͽ� ����
        rb.velocity = Vector2.zero;
    }

    private void MonsterDestroyed()
    {
        DestroyEvent destroyedEvent = GetComponent<DestroyEvent>();
        destroyedEvent.CallDestroyedEvent(false, 0);

        // 5% 확률로 프리팹 드랍
        float dropChance = Random.Range(0f, 1f);
        if (dropChance <= 0.05f)  // 5% 확률 체크
        {
            string prefabPath = null;

            // 몬스터의 ID에 따른 드랍 프리팹 경로 설정
            switch (id)
            {
                case 1: // 예: ID가 1인 몬스터
                    prefabPath = "Food/101";  // 사과 프리팹 경로
                    break;
                case 3: 
                    prefabPath = "Food/102";  // 빵 프리팹 경로
                    break;
                case 6: // 예: ID가 3인 몬스터
                    prefabPath = "Food/106";  // 치즈 프리팹 경로
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



    //������ �浹ó��
    void OnTriggerEnter2D(Collider2D other)
    {
        // �÷��̾�� �浹 ��
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(attack);  // �÷��̾�� ������ ���ݷ¸�ŭ ������ ����
            }
        }
    }

    //���ͳ��� �浹���� �ʵ��� �ϴ� ����
    void MonsterCollisionIgnore()
    {
        // "Monster" �±׸� ���� ��� ������Ʈ�� ã���ϴ�.
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monsters");

        // �� ���� ������Ʈ���� Collider�� ������ ���� �浹�� �����ϵ��� �����մϴ�.
        for (int i = 0; i < monsters.Length; i++)
        {
            for (int j = i + 1; j < monsters.Length; j++)
            {
                Collider col1 = monsters[i].GetComponent<Collider>();
                Collider col2 = monsters[j].GetComponent<Collider>();

                if (col1 != null && col2 != null)
                {
                    // �� Collider ���� �浹�� �����մϴ�.
                    Physics.IgnoreCollision(col1, col2);
                }
            }
        }
    }
}

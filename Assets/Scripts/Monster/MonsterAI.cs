using UnityEngine;
using System.Collections;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(DestroyEvent))]
[RequireComponent(typeof(MonsterDestroy))]

public class MonsterAI : MonoBehaviour
{
    public MonsterDataSO monsterDataSO;  // ScriptableObject�� �����͸� ����

    private int health;
    private int attack;
    private float range;

    private float attackRange;

    private float speed;
    private int id;  // ������ ID
    public Transform player;

    //�˹� ���� ����
    public float knockbackForce = 2f;  // AddForce�� ����� ��
    public float maxKnockbackDistance = 2f;  // �ִ� �̵� �Ÿ�
    public float knockbackDistance = 2f;  // ������ �˹� �Ÿ�
    private Vector3 knockbackStartPos;  // �˹��� ���۵� ��ġ
    Rigidbody2D rb;

    //�����̵� ���÷���
    private Vector2 randomDirection; // ���� �̵� ����
    public float changeDirectionTime = 3f; //�������� �̵� �����ֱ�
    private float timer = 0; // �����̵� Ÿ�̸�
    private enum MonsterState { Idle, Chasing }; // ���� ����
    private MonsterState currentState = MonsterState.Idle;

    void Start()
    {
        // "(Clone)"�� �����ϰ� �̸��� ������
        string monsterName = gameObject.name.Replace("(Clone)", "").Trim();

        // �̸��� �������� ���� �����͸� �˻�
        MonsterDataSO data = Managers.Data.GetMonsterDataByName(monsterName);
        if (data != null)
        {
            AssignData(data);  // �����͸� AI�� �Ҵ�
        }
        else
        {
            Debug.LogError($"Monster �̸��� �Ľ��� �� �����ϴ�: {monsterName}");
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

        range = monsterDataSO.range;
        speed = monsterDataSO.speed;

        Debug.Log($"���� ������ �����: {monsterDataSO.monsterName} (ID: {monsterDataSO.id})");
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
        Debug.Log($"{gameObject.name} �� {damage} �� ��������, remaining health: {health}");

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

    // ���Ͱ� ���� �� �����ϴ� �Լ�
    private void MonsterDestroyed()
    {
        DestroyEvent destroyedEvent = GetComponent<DestroyEvent>();
        destroyedEvent.CallDestroyedEvent(false, 0);
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

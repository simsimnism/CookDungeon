using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public float moveSpeed;   // 몬스터의 이동 속도
    public float attackRange; // 몬스터의 공격 범위
    public float AttackDamage;
    public int monsterID;     // 몬스터의 ID
    private Transform player; // 플레이어의 위치를 추적하기 위한 참조

    private MonsterManager monsterManager;
    private MonsterManager.MonsterData monsterData;

    private float currentHealth; // 현재 체력

    void Awake() 
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        monsterManager = Managers.Monster;  // MonsterManager 오브젝트를 찾음
        MonsterDataLoad();
    }

    private void Start()
    {

    }

    void MonsterDataLoad()
    {
        // 몬스터 데이터를 로드 (ID에 따른 몬스터 로드)
        monsterData = monsterManager.GetMonsterDataByID(monsterID);

        if (monsterData != null)
        {
            moveSpeed = monsterData.speed;
            attackRange = monsterData.range;
            currentHealth = monsterData.health; // 몬스터 체력을 데이터에서 가져옴
            Debug.Log(monsterData.name + " 몬스터 로드됨. 체력: " + currentHealth);
        }
        else
        {
            Debug.LogError("몬스터 데이터를 찾을 수 없습니다.");
        }

    }

    public void Init(MonsterManager.MonsterData data)
    {
        monsterData = data;

        // 적 데이터를 기반으로 설정
        moveSpeed = monsterData.speed;
        attackRange = monsterData.range;
        currentHealth = monsterData.health;

        Debug.Log(monsterData.name + "가 스폰되었습니다. 체력: " + currentHealth);
    }

    private void Update()
    {
        MoveTowardsPlayer();
    }

    // 플레이어를 향해 이동
    void MoveTowardsPlayer()
    {
        if (player != null && monsterData != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // 공격 범위 안에 있지 않으면 플레이어를 향해 이동
            if (distanceToPlayer > attackRange)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            }
            else
            {
                // 공격 로직 또는 행동 추가
                AttackPlayer();
            }
        }
    }

    // 플레이어를 공격하는 함수 (간단한 예시)
    void AttackPlayer()
    {
        Debug.Log(monsterData.name + "이(가) 플레이어를 공격합니다!");
        // 실제 공격 로직 구현 (예: 데미지를 주는 등)
    }

    // 대미지를 받는 함수
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(monsterData.name + "이(가) " + damage + " 대미지를 받았습니다. 현재 체력: " + currentHealth);

        // 체력이 0 이하일 경우 몬스터가 죽음
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // 몬스터가 죽는 로직
    void Die()
    {
        Debug.Log(monsterData.name + " 몬스터가 죽었습니다!");
        // 몬스터 제거 로직 (예: 파괴 또는 비활성화)
        Destroy(gameObject); // 몬스터 오브젝트 삭제
    }

}

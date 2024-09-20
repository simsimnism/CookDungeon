using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

// 적 클래스
public class Enemy : MonoBehaviour
{
    // 적의 이동 속도를 나타냄
    public float speed;

    // 적의 현재 체력을 나타냄
    public float health;

    // 적의 최대 체력을 나타냄
    public float maxHealth;

    // 애니메이터 컨트롤러 배열
    public RuntimeAnimatorController[] animCon;

    // 타겟 (플레이어)
    public Rigidbody2D target;

    // 적이 살아 있는지 여부를 나타냄
    bool isLive;

    // Rigidbody2D 컴포넌트
    Rigidbody2D rigid;

    // Animator 컴포넌트
    Animator anim;

    // SpriteRenderer 컴포넌트
    SpriteRenderer spriter;

    // 몬스터의 ID
    private int enemyId;

    // 몬스터의 공격 범위
    private float attackRange;

    // 공격 쿨타임 (초)
    public float attackCooldown = 1f;

    // 마지막 공격 시간
    private float lastAttackTime = 0f;

    void Awake()
    {
        // 컴포넌트를 가져오기
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        // 적이 살아 있지 않으면 리턴
        if (!isLive)
            return;

        // 플레이어를 향해 이동
        Vector2 dirVec = target.position - rigid.position;
        float distance = dirVec.magnitude;

        if (distance > attackRange)
        {
            // 공격 범위 밖에 있으면 이동
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
            rigid.velocity = Vector2.zero; // 이동 후 속도 초기화
        }
        else
        {
            // 공격 범위 내에 있으면 공격
            EnemyAttack();
        }
    }

    void LateUpdate()
    {
        // 적이 살아 있지 않으면 리턴
        if (!isLive)
            return;

        // 플레이어의 위치에 따라 스프라이트의 방향을 변경
        spriter.flipX = target.position.x < rigid.position.x;
    }

    private void OnEnable()
    {
        // 적이 활성화될 때 초기화
        target = GameManager.Instance.Player.GetComponent<Rigidbody2D>();
        isLive = true;
        health = maxHealth;
    }

    // 적 데이터를 받아 초기화하는 메서드
    public void Init(EnemyData data)
    {
        // id에 따른 애니메이터 컨트롤러 설정
        anim.runtimeAnimatorController = animCon[data.spriteType % animCon.Length];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
        enemyId = data.id;
        attackRange = data.attackRange;
    }

    // 공격 시도 메서드
    private void EnemyAttack()
    {
        // 쿨타임 확인
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        // 쿨타임 갱신
        lastAttackTime = Time.time;

        // 몬스터의 ID에 따라 공격 방식 결정 >>> 순서 반드시 맞출것
        if (enemyId == 1) // 예: ID가 1인 경우 [머시론]
        {
            anim.SetTrigger("atk");
            Debug.Log("공격!");
            // 추가적인 공격 로직을 여기에 작성할 수 있습니다.
        }
        else if (enemyId == 2) // 예: ID가 2인 경우 [배추구리]
        {
            anim.SetTrigger("atk");
            Debug.Log("공격!2");
            // 공격 후 몬스터를 사라지게 함
            Dead();
        }
        // 추가적는 여기에
    }

    // 무기와 충돌했을 때 호출되는 메서드
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 무기가 아닌 경우 리턴
        if (!collision.CompareTag("Weapon"))
            return;

        // 무기의 데미지를 받아 체력을 감소시킴
        health -= collision.GetComponent<Weapon>().damage;

        // 체력이 남아 있는 경우
        if (health > 0)
        {
            // 생략 가능 (추가 로직을 넣을 수 있음)
        }
        else
        {
            // 체력이 0 이하가 된 경우 죽음 처리
            Dead();
        }
    }

    // 적을 비활성화하는 메서드
    void Dead()
    {
        isLive = false; // 적을 비활성화
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEditor;

public class PlayerAttack : MonoBehaviour
{

    //플레이어 공격 관련 변수 PM에서 관리함
    [SerializeField] private GameObject player;
    [SerializeField] private int meleeAttackDamage;
    [SerializeField] private float comboResetTime;
    [SerializeField] private float attackRadius;
    [SerializeField] private float attackAngle;
    private int comboStep = 0;// 현재 콤보 단계
    private bool isAttacking;  // 공격 중인지 여부
    private float lastAttackTime;// 마지막 공격 시간
    private bool canChainCombo; // 콤보 연결 가능 여부
    private bool isShowingAttackRange; //공격 범위를 가시화

    //이건 삭제하면 안됨 다른 코드에서도 사용함
    private Vector2 attackDirection;  // 현재 공격 방향
    private Player playerS;

    void Awake()
    {
        meleeAttackDamage = Managers.Player.meleeAttackDamage;
        comboResetTime = Managers.Player.comboResetTime;
        attackRadius = Managers.Player.attackRadius;
        attackAngle = Managers.Player.attackAngle;
        comboStep = Managers.Player.comboStep;
        isAttacking = Managers.Player.isAttacking;
        lastAttackTime = Managers.Player.lastAttackTime;
        canChainCombo = Managers.Player.canChainCombo;
        isShowingAttackRange = Managers.Player.isShowingAttackRange;
        playerS = player.GetComponent<Player>();
    }

    void Start()
    {
        Managers.Input.KeyAction -= Attack;
        Managers.Input.KeyAction += Attack;

    }

    void Update()
    {
    }

    //공격범위 시각화 폐기 가능
    void HideAttackRange() 
    {
        isShowingAttackRange = false;
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 첫 번째 공격 시작
            if (!isAttacking)
            {
                Debug.Log("1공격");
                StartAttack();
                
            }
            // 콤보 연결
            else if (canChainCombo)
            {
                Debug.Log("2공격");
                ChainComboAttack();
            }
       
        }
    }

    void StartAttack()
    {

        // 첫 번째 콤보 공격 실행
        comboStep = 1;
        isAttacking = true;
        lastAttackTime = Time.time;
        canChainCombo = true;

        // 마우스 위치에 따라 4방위 공격 방향 결정
        attackDirection = GetAttackDirection();

        // 공격 시각화 활성화
        isShowingAttackRange = true;

        // 공격 시 움직임을 제한
        Managers.Player.DisableMovement();

        ExecuteAttack(attackDirection);
        
        // 0.5초 후에 공격 범위 시각화를 비활성화 (유니티에서는 Invoke 사용 가능)
        Invoke("HideAttackRange", 0.5f); // 공격 범위 시각화 비활성화

        Invoke("ResetCombo",  1f);
    }

    void ChainComboAttack()
    {

        // 기존 콤보 리셋 타이머 취소
        CancelInvoke("ResetCombo");

        // 두 번째 콤보 공격 실행
        comboStep++;
        lastAttackTime = Time.time;
        canChainCombo = true;

        // 마우스 위치에 따라 4방위 공격 방향 결정
        attackDirection = GetAttackDirection();
        // 공격 시각화 활성화
        isShowingAttackRange = true;

        ExecuteAttack(attackDirection);

        //0.5초 후에 공격 범위 시각화를 비활성화
        Invoke("HideAttackRange", 0.5f);

        // 2단계 콤보 이후에는 콤보를 리셋
        if (comboStep >= 2)  // 2단계 콤보
        {
            ResetCombo();
        }
    }

    // 4방위 공격 방향 계산 함수
    Vector2 GetAttackDirection()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = transform.position;
        Vector2 direction = (mousePos - playerPos).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //공격방향 계산 함수
        if (angle >= 45 && angle < 135)
        {
            // 상단 공격
            if (direction.x >= 0)
            {
                // 우상단
                return new Vector2(1, 1).normalized;
            }
            else
            {
                // 좌상단
                return new Vector2(-1, 1).normalized;
            }
        }
        else if (angle >= -135 && angle < -45)
        {
            // 하단 공격
            if (direction.x >= 0)
            {
                // 우하단
                return new Vector2(1, -1).normalized;
            }
            else
            {
                // 좌하단
                return new Vector2(-1, -1).normalized;
            }
        }

        return direction; // 기본적으로 마우스 방향을 반환
    }

    // 실제 공격 함수
    void ExecuteAttack(Vector2 attackDirection)
    {
        // 공격 범위 내의 적들을 찾음
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Monsters"))
            {
                Vector2 targetDir = (collider.transform.position - transform.position).normalized;
                float angle = Vector2.Angle(attackDirection, targetDir);

                if (angle <= attackAngle / 2)
                {
                    // 적에게 콤보 단계에 따른 데미지를 가함
                    MonsterAI enemy = collider.GetComponent<MonsterAI>();
                    if (enemy != null)
                    {
                        // 콤보 단계에 따른 총 데미지를 계산
                        int totalDamage = meleeAttackDamage * comboStep;

                        // 피격 방향(공격받은 방향) 계산
                        Vector3 hitDirection = (enemy.transform.position - transform.position).normalized;

                        // 몬스터에게 데미지를 주고 피격 방향 전달
                        //enemy.TakeDamage(totalDamage, hitDirection);

                        Debug.Log($"콤보 {comboStep}로 {enemy.name}에게 {totalDamage} 데미지를 입혔습니다.");
                    }
                }
            }
        }
    }


    void ResetCombo()
    {
        comboStep = 0;            // 콤보 단계 리셋
        isAttacking = false;      // 공격 상태 초기화
        canChainCombo = false;    // 콤보 연결 불가능 상태

        Managers.Player.EnableMovement();

        Debug.Log("콤보가 리셋되었습니다!");
    }

    // 공격 범위 시각화: 공격이 발생할 때만 부채꼴을 표시합니다.
    void OnDrawGizmos()
    {
        if (isShowingAttackRange)
        {
            Handles.color = new Color(1f, 0f, 0f, 0.2f);  // 빨간색, 투명도 20%

            // 부채꼴의 시작 각도와 끝 각도를 공격 방향에 맞게 설정
            Vector3 forward = new Vector3(attackDirection.x, attackDirection.y, 0);
            float halfAngle = attackAngle / 2;

            // Handles를 이용하여 부채꼴을 그림
            Handles.DrawSolidArc(
                transform.position,         // 부채꼴의 중심
                Vector3.forward,            // 부채꼴을 그릴 평면 (Z축 기준)
                Quaternion.Euler(0, 0, -halfAngle) * forward,  // 부채꼴의 시작 방향
                attackAngle,                // 부채꼴의 전체 각도
                attackRadius                // 부채꼴의 반경
            );

            // Gizmos를 사용하여 범위 경계선을 그릴 수도 있음
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }
    }
}



using Cook.InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine.UIElements;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float meleeAttackDamage = 10f; // 기본 근접 공격 데미지
    [SerializeField] private float comboResetTime = 1f; // 콤보를 초기화하는 시간
    [SerializeField] private float attackRadius = 3f;
    [SerializeField] private float attackAngle = 160f;
    private int comboStep = 0;
    private bool isAttacking = false;
    private float lastAttackTime;
    private bool canChainCombo = false; // 다음 콤보로 연결 가능한지 확인

    void Start()
    {

    }

    void Update()
    {
        Managers.Input.KeyAction -= Attack;
        Managers.Input.KeyAction += Attack;
  
    }

    void Attack()
    {
        if(Input.GetMouseButtonDown(0))
        { 
            StartAttack();
            OnDrawGizmosSelected();
        }
    }
    
    void StartAttack()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // 2D이므로 z 좌표는 0으로 설정

        // 플레이어 위치
        Vector3 playerPos = transform.position;

        // 마우스 방향으로 공격 벡터 계산
        Vector3 attackDirection = (mousePos - playerPos).normalized;

        // 실제 공격 함수 실행 (방향을 전달)
        meleeAttack(attackDirection);
    }

    //콤보 공격 관련 실행코드
    void meleeAttack(Vector3 attackDirection)
    {
        if (isAttacking && !canChainCombo)
            return; // 공격 중이고 다음 콤보로 연결되지 않으면 아무것도 하지 않음

        // 첫 번째 공격 시작 시
        if (comboStep == 0)
        {
            isAttacking = true;
            Debug.Log("첫 번째 공격!");

            // 첫 번째 공격 실행 로직
            ExecuteAttack(attackDirection);

            comboStep++;
            lastAttackTime = Time.time;
            canChainCombo = true; // 콤보 연결 가능 상태로 설정

        }
        // 두 번째 공격으로 연결
        else if (comboStep == 1 && Time.time - lastAttackTime <= comboResetTime)
        {
            Debug.Log("두 번째 공격!");

            // 두 번째 공격 실행 로직
            ExecuteAttack(attackDirection);

            comboStep = 0; // 콤보 초기화
            isAttacking = false;
            canChainCombo = false; // 콤보 끝, 연결 불가능 상태로 변경
        }
        else 
        { 
            ResetCombo();
        }
    }

    //공격의 범위 등 실제 공격 효과를 관리
    void ExecuteAttack(Vector3 attackDirection)
    {
        // 실제 공격 효과 (애니메이션, 데미지 처리 등) 적용
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRadius);

        foreach (Collider2D collider in colliders)
        {
            //enemy 태그일 경우
            if (collider.CompareTag("enemy"))
            {

                //targetDir: 적과의 플레이어 사이의 방향 벡터
                Vector3 targetDir= (collider.transform.position - transform.position).normalized;
                
                //플레이어의 공격 방향과 적 사이의 각도 계산
                float angle = Vector3.Angle(attackDirection, targetDir); 

                if (angle <= attackAngle / 2)
                {
                    // 적에게 데미지 적용
                    MonsterMovement enemy = collider.GetComponent<MonsterMovement>();
                    if (enemy != null)
                    {
                        int totalDamage = (int)meleeAttackDamage * (comboStep + 1);
                        enemy.TakeDamage(totalDamage);//적에게 데미지 적용

                    }
                }
            }
            
            
        }

        // 콤보가 종료되면 다시 초기화 시간까지 대기
        Invoke("ResetCombo", comboResetTime);
    }

    void ResetCombo()
    {
        if (Time.time - lastAttackTime > comboResetTime)
        {
            comboStep = 0; // 시간이 지나면 콤보 초기화
            isAttacking = false;
            canChainCombo = false; // 더 이상 콤보 연결 불가능
        }
    }

    // 기즈모로 공격 범위를 시각적으로 표시
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        // 부채꼴 공격 범위의 시작과 끝 지점을 계산
        Vector3 rightBoundary = Quaternion.Euler(0, 0, attackAngle / 2) * Vector3.right;
        Vector3 leftBoundary = Quaternion.Euler(0, 0, -attackAngle / 2) * Vector3.right;

        // 공격 범위 반경을 사용해 부채꼴을 그리기
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * attackRadius);
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * attackRadius);
    }
}


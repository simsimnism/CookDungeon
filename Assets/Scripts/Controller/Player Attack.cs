using Cook.InventorySystem;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float meleeAttackDamage = 10f; // 기본 근접 공격 데미지
    [SerializeField] private float comboResetTime = 1f; // 콤보를 초기화하는 시간
    private int comboStep = 0;
    private bool isAttacking = false;
    private float lastAttackTime;
    private bool canChainCombo = false; // 다음 콤보로 연결 가능한지 확인

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartAttack();
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
        PerformAttack(attackDirection);
    }

    void PerformAttack(Vector3 attackDirection)
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
    }

    void ExecuteAttack(Vector3 attackDirection)
    {
        // 실제 공격 효과 (애니메이션, 데미지 처리 등) 적용
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(2f, 1f), 0f);
        foreach (Collider2D collider in colliders)
        {
            /*
            if (collider.CompareTag("enemy"))
            {


                // 적에게 데미지 적용
                MonsterMovement enemy = collider.GetComponent<MonsterMovement>();
                if (enemy != null)
                {
                    /*int totalDamage = meleeAttackDamage * (comboStep + 1); 
                    /*MonsterMovement.TakeDamage(totalDamage); 
                    
                }
            }
            */
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
}


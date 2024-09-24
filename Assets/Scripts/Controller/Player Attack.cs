using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Weapon weapon; // 플레이어가 사용할 무기
    public Transform attackPoint; // 공격이 발생할 위치
    public float comboResetTime = 1.0f; // 콤보가 초기화되는 시간
    private int comboStep = 0; // 현재 콤보 단계
    private float lastClickTime = 0; // 마지막 공격 시간

    void Update()
    {
        // 공격 입력 처리
        if (Input.GetButtonDown("Fire1"))
        {
            HandleComboAttack();
        }

        // 콤보가 일정 시간 초과시 초기화
        if (Time.time - lastClickTime > comboResetTime)
        {
            ResetCombo();
        }
    }

    // 콤보 공격 처리
    void HandleComboAttack()
    {
        lastClickTime = Time.time; // 마지막 클릭 시간을 업데이트

        // 콤보 단계에 따른 공격
        if (comboStep == 0)
        {
            comboStep = 1;
            PerformAttack(1); // 1타 콤보 애니메이션
        }
        else if (comboStep == 1)
        {
            comboStep = 2;
            PerformAttack(2); // 2타 콤보 애니메이션
        }
        else if (comboStep == 2)
        {
            comboStep = 3;
            PerformAttack(3); // 3타 콤보 애니메이션
        }
    }

    // 공격 처리 함수
    void PerformAttack(int comboStep)
    {
        if (weapon != null)
        {
            // 공격 애니메이션 실행 (콤보 단계에 따른 트리거)
            weapon.SetComboAttack(comboStep);

            // 공격 방향 설정 (마우스 방향으로)
            Vector2 direction = GetMouseDirection();
            RotatePlayerTowards(direction); // 플레이어 회전
            weapon.DealDamage(); // 데미지 처리
        }
    }

    // 플레이어가 마우스 방향을 향하도록 회전
    void RotatePlayerTowards(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    // 마우스 방향 가져오기
    Vector2 GetMouseDirection()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        return direction.normalized; // 방향 벡터를 정규화하여 반환
    }

    // 콤보 초기화
    void ResetCombo()
    {
        comboStep = 0;
    }

    // 무기를 변경하는 함수
    public void ChangeWeapon(string newWeaponName)
    {
        if (weapon != null)
        {
            weapon.LoadWeaponData(newWeaponName); // 새로운 무기 로드
        }
    }
}

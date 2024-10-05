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

    public Animator animator; // 애니메이터를 제어하기 위한 Animator 컴포넌트
    public float weaponHideTime = 5.0f; // 무기가 비가시화 되는 시간
    private bool weaponVisible = true; // 무기 가시 상태

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

        // 일정 시간 공격이 없을 경우 무기 비가시화
        if (Time.time - lastClickTime > weaponHideTime && weaponVisible)
        {
            HideWeapon();
        }
    }

    // 콤보 공격 처리
    void HandleComboAttack()
    {
        lastClickTime = Time.time; // 마지막 클릭 시간을 업데이트

        // 무기가 비가시화 상태라면 공격을 하기 전에 무기를 가시화
        if (!weaponVisible)
        {
            ShowWeapon();
        }

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

    // 공격 처리 함수 (애니메이션 트리거 포함)
    void PerformAttack(int comboStep)
    {
        if (weapon != null)
        {
            // 콤보 단계에 따른 애니메이션 트리거 설정
            SetComboAnimation(comboStep);

            // 공격 방향 설정 (마우스 방향으로)
            Vector2 direction = GetMouseDirection();
            RotatePlayerTowards(direction); // 플레이어 회전

            // 무기 공격 처리 (데미지 계산)
            weapon.DealDamage();
        }
    }

    // 콤보 단계에 따른 애니메이션 트리거를 설정하는 함수
    void SetComboAnimation(int comboStep)
    {
        string trigger = ""; // 애니메이션 트리거를 저장할 변수

        // 콤보 단계에 따른 애니메이션 트리거 설정
        switch (comboStep)
        {
            case 1:
                trigger = "Combo1"; // 1타 콤보 애니메이션 트리거
                break;
            case 2:
                trigger = "Combo2"; // 2타 콤보 애니메이션 트리거
                break;
            case 3:
                trigger = "Combo3"; // 3타 콤보 애니메이션 트리거
                break;
        }

        // 애니메이션이 연속 실행되도록 트리거 설정
        if (!string.IsNullOrEmpty(trigger))
        {
            animator.SetTrigger(trigger); // 새로운 애니메이션 트리거 실행
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

    // 무기를 가시화하는 함수
    void ShowWeapon()
    {
        if (weapon != null)
        {
            weapon.SetVisible(true); // 무기를 가시화
            weaponVisible = true;
        }
    }

    // 무기를 비가시화하는 함수
    void HideWeapon()
    {
        if (weapon != null)
        {
            weapon.SetVisible(false); // 무기를 비가시화
            weaponVisible = false;
        }
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

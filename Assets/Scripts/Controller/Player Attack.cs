using Cook.InventorySystem;
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
        if (Input.GetButtonDown("Fire1"))
        {
            HandleComboAttack();
        }

        if (Time.time - lastClickTime > comboResetTime)
        {
            ResetCombo();
        }

        if (Time.time - lastClickTime > weaponHideTime && weaponVisible)
        {
            HideWeapon();
        }
    }

    // 콤보 공격 처리
    void HandleComboAttack()
    {
        lastClickTime = Time.time;

        if (!weaponVisible)
        {
            ShowWeapon();
        }

        if (comboStep == 0)
        {
            comboStep = 1;
            PerformAttack(1);
        }
        else if (comboStep == 1)
        {
            comboStep = 2;
            PerformAttack(2);
        }
        else if (comboStep == 2)
        {
            comboStep = 3;
            PerformAttack(3);
        }
    }

    // 공격 처리
    void PerformAttack(int comboStep)
    {
        if (weapon != null)
        {
            weapon.PerformComboAttack(comboStep);

            Vector2 direction = GetMouseDirection();
            RotatePlayerTowards(direction);

            weapon.DealDamage();
        }
    }

    // 콤보 초기화
    void ResetCombo()
    {
        comboStep = 0;
    }

    // 무기를 가시화
    void ShowWeapon()
    {
        if (weapon != null)
        {
            weapon.SetVisible(true);
            weaponVisible = true;
        }
    }

    // 무기를 비가시화
    void HideWeapon()
    {
        if (weapon != null)
        {
            weapon.SetVisible(false);
            weaponVisible = false;
        }
    }

    // 마우스 방향을 구하고 플레이어 회전
    void RotatePlayerTowards(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    Vector2 GetMouseDirection()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        return direction.normalized;
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

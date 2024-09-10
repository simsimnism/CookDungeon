using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Animator animator; // 무기 애니메이터
    private WeaponData currentWeaponData;

    public float attackDamage;
    public float attackRange;

    // 무기 데이터를 로드하는 함수 (public으로 설정)
    public void LoadWeaponData(string weaponName)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("weapons");

        if (jsonFile != null)
        {
            WeaponDataList weaponDataList = JsonUtility.FromJson<WeaponDataList>(jsonFile.text);

            // weaponName에 해당하는 무기 데이터 찾기
            foreach (WeaponData weaponData in weaponDataList.weapons)
            {
                if (weaponData.name == weaponName)
                {
                    currentWeaponData = weaponData;
                    ApplyWeaponData();
                    break;
                }
            }
        }
        else
        {
            Debug.LogError("JSON 파일을 찾을 수 없습니다.");
        }
    }

    // 무기 데이터를 기반으로 애니메이션 설정
    private void ApplyWeaponData()
    {
        if (currentWeaponData != null)
        {
            // 공격력 및 공격 범위 설정
            this.attackDamage = currentWeaponData.attackDamage;
            this.attackRange = currentWeaponData.attackRange;
        }
    }

    // 콤보 애니메이션 트리거 설정
    public void SetComboAttack(int comboStep)
    {
        if (animator != null && currentWeaponData != null)
        {
            string trigger = "";

            switch (comboStep)
            {
                case 1:
                    trigger = currentWeaponData.combo1Trigger; // 1타 콤보
                    break;
                case 2:
                    trigger = currentWeaponData.combo2Trigger; // 2타 콤보
                    break;
                case 3:
                    trigger = currentWeaponData.combo3Trigger; // 3타 콤보
                    break;
            }

            if (!string.IsNullOrEmpty(trigger))
            {
                animator.SetTrigger(trigger);
            }
        }
    }

    // 데미지 처리 함수
    public void DealDamage()
    {
        // 공격 범위 내의 적들을 찾는 로직
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange);

        // 적들에게 데미지를 주는 로직
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<Monsters>().TakeDamage(attackDamage);
            }
        }
    }

    // 디버그용 공격 범위 시각화
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

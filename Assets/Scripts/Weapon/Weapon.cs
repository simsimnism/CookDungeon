using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// 무기 기능을 담당하는 클래스
public class Weapon : MonoBehaviour
{
    public Animator animator; // 무기 애니메이터를 제어하기 위한 Animator 컴포넌트
    private WeaponData currentWeaponData; // 현재 장착된 무기의 데이터를 저장하는 변수

    public float attackDamage; // 무기의 공격력
    public float attackRange; // 무기의 공격 범위

    // 무기 데이터를 로드하는 함수 (public으로 설정)
    public void LoadWeaponData(string weaponName)
    {
        // "Resources/weapons" 경로에 있는 JSON 파일을 로드
        TextAsset jsonFile = Resources.Load<TextAsset>("weapons");

        // JSON 파일이 존재하는지 확인
        if (jsonFile != null)
        {
            // JSON 파일을 WeaponDataList 타입으로 변환
            WeaponDataList weaponDataList = JsonUtility.FromJson<WeaponDataList>(jsonFile.text);

            // weaponName과 일치하는 무기 데이터를 검색
            foreach (WeaponData weaponData in weaponDataList.weapons)
            {
                if (weaponData.name == weaponName) // 이름이 일치하는 경우
                {
                    currentWeaponData = weaponData; // 해당 무기 데이터를 현재 무기 데이터로 설정
                    ApplyWeaponData(); // 무기 데이터 적용
                    break;
                }
            }
        }
        else
        {
            Debug.LogError("JSON 파일을 찾을 수 없습니다."); // 파일이 없을 경우 에러 메시지 출력
        }
    }

    // 현재 무기 데이터에 따라 무기의 속성 및 애니메이션 설정
    private void ApplyWeaponData()
    {
        if (currentWeaponData != null) // 무기 데이터가 존재하는지 확인
        {
            // 무기의 공격력 및 공격 범위를 설정
            this.attackDamage = currentWeaponData.attackDamage;
            this.attackRange = currentWeaponData.attackRange;
        }
    }

    // 콤보 공격을 위한 애니메이션 트리거를 설정하는 함수
    public void SetComboAttack(int comboStep)
    {
        // 애니메이터와 무기 데이터가 존재하는지 확인
        if (animator != null && currentWeaponData != null)
        {
            string trigger = ""; // 애니메이션 트리거를 저장할 변수

            // 콤보 스텝에 따라 애니메이션 트리거를 설정
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

            // 설정된 트리거가 있을 경우 애니메이션 트리거 실행
            if (!string.IsNullOrEmpty(trigger))
            {
                animator.SetTrigger(trigger); // 애니메이션 트리거 설정
            }
        }
    }

    // 무기 공격으로 데미지를 주는 함수
    public void DealDamage()
    {
        // 공격 범위 내의 모든 콜라이더를 찾음
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange);

        // 발견된 적들에게 데미지를 적용
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy")) // 'Enemy' 태그를 가진 오브젝트만 처리
            {
                enemy.GetComponent<Monsters>().TakeDamage(attackDamage); // 적에게 데미지를 줌
            }
        }
    }

    // 공격 범위를 디버그하기 위한 시각화
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange); // 공격 범위를 표시하는 원 그리기
    }
}

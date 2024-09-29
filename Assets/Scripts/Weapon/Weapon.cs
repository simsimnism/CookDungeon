using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 무기 기능을 담당하는 클래스 - Item을 상속
public class Weapon : Item
{
    private WeaponData currentWeaponData; // 현재 장착된 무기의 데이터를 저장하는 변수

    public float attackDamage; // 무기의 공격력
    public float attackRange; // 무기의 공격 범위
    private SpriteRenderer spriteRenderer; // 무기의 시각적 표현을 담당하는 SpriteRenderer
    private Transform transform;
    void Awake()
    {
        // SpriteRenderer 컴포넌트 가져오기
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform = GetComponent<Transform>();
    }

    private T GetComponent<T>()
    {
        throw new NotImplementedException();
    }

    // 무기 데이터를 로드하는 함수
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

    // 현재 무기 데이터에 따라 무기의 속성 설정
    private void ApplyWeaponData()
    {
        if (currentWeaponData != null) // 무기 데이터가 존재하는지 확인
        {
            // 무기의 공격력 및 공격 범위를 설정
            this.attackDamage = currentWeaponData.attackDamage;
            this.attackRange = currentWeaponData.attackRange;

            // 상속받은 itemName과 icon 설정
            this.itemName = currentWeaponData.name;
            this.icon = currentWeaponData.weaponSprite;

            // SpriteRenderer가 있으면 아이콘(스프라이트) 적용
            if (spriteRenderer != null && this.icon != null)
            {
                spriteRenderer.sprite = this.icon;
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

    // 무기 가시화 여부 설정
    public void SetVisible(bool isVisible)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = isVisible; // SpriteRenderer 활성/비활성화
        }
    }

    // 아이템을 사용할 때 무기의 공격력을 발휘하는 Use 메서드 재정의
    public override void Use()
    {
        base.Use(); // 상위 클래스의 Use 메서드 호출 (디버그 메시지 출력)
        Debug.Log(itemName + " is being used to attack!"); // 무기 특화된 동작 추가
        DealDamage(); // 무기 공격 실행
    }

    // 공격 범위를 디버그하기 위한 시각화
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange); // 공격 범위를 표시하는 원 그리기
    }
}

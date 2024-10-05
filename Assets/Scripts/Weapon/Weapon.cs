using System.Collections.Generic;
using UnityEngine;

namespace Rito.InventorySystem
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform weaponTransform;
        private WeaponData currentWeaponData;

        public float attackDamage;  // 무기의 공격력
        public float attackRange;   // 무기의 공격 범위

        // 콤보 트리거 필드를 ComboTrigger enum으로 변경
        public WeaponItemData.ComboTrigger combo1Trigger;
        public WeaponItemData.ComboTrigger combo2Trigger;
        public WeaponItemData.ComboTrigger combo3Trigger;
        public string itemName;
        public Sprite icon;

        public void Initialize(SpriteRenderer sr, Animator anim, Transform weaponTransform)
        {
            this.spriteRenderer = sr;
            this.animator = anim;
            this.weaponTransform = weaponTransform;
        }

        // 무기 데이터를 로드하는 함수
        public void LoadWeaponData(string weaponName)
        {
            TextAsset jsonFile = Resources.Load<TextAsset>("weapons");

            if (jsonFile != null)
            {
                WeaponDataList weaponDataList = JsonUtility.FromJson<WeaponDataList>(jsonFile.text);

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

        // 현재 무기 데이터에 따라 무기의 속성 설정
        private void ApplyWeaponData()
        {
            if (currentWeaponData != null)
            {
                this.attackDamage = currentWeaponData.attackDamage;
                this.attackRange = currentWeaponData.attackRange;

                // 콤보 트리거 설정은 ComboTrigger 열거형으로 처리
                this.combo1Trigger = WeaponItemData.ComboTrigger.Combo1;
                this.combo2Trigger = WeaponItemData.ComboTrigger.Combo2;
                this.combo3Trigger = WeaponItemData.ComboTrigger.Combo3;

                this.itemName = currentWeaponData.name;
                this.icon = currentWeaponData.weaponSprite;

                if (spriteRenderer != null && this.icon != null)
                {
                    spriteRenderer.sprite = this.icon;
                }
            }
        }

        // 무기 공격으로 데미지를 주는 함수
        public void DealDamage()
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(weaponTransform.position, attackRange);

            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    enemy.GetComponent<Monsters>().TakeDamage(attackDamage);
                }
            }
        }

        // 콤보 공격 애니메이션 트리거 실행
        public void PerformComboAttack(int comboStep)
        {
            if (animator != null)
            {
                string trigger = "";
                switch (comboStep)
                {
                    case 1:
                        trigger = combo1Trigger.ToString(); // 열거형을 문자열로 변환
                        break;
                    case 2:
                        trigger = combo2Trigger.ToString(); // 열거형을 문자열로 변환
                        break;
                    case 3:
                        trigger = combo3Trigger.ToString(); // 열거형을 문자열로 변환
                        break;
                }

                if (!string.IsNullOrEmpty(trigger))
                {
                    animator.SetTrigger(trigger); // 애니메이션 트리거 설정
                }
            }
        }

        // 무기 가시화 여부 설정
        public void SetVisible(bool isVisible)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = isVisible;
            }
        }
    }
}

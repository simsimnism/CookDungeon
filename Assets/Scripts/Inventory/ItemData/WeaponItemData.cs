using UnityEngine;

namespace Cook.InventorySystem
{
    [CreateAssetMenu(fileName = "Item_Weapon_", menuName = "Inventory System/Item Data/Weapon", order = 1)]
    public class WeaponItemData : EquipmentItemData
    {
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _range = 1.0f;

        // 콤보 트리거를 enum으로 변경
        public enum ComboTrigger
        {
            Combo1,
            Combo2,
            Combo3
        }

        [SerializeField] private ComboTrigger _combo1Trigger = ComboTrigger.Combo1;
        [SerializeField] private ComboTrigger _combo2Trigger = ComboTrigger.Combo2;
        [SerializeField] private ComboTrigger _combo3Trigger = ComboTrigger.Combo3;

        // 무기 속성 데이터들을 반환하는 프로퍼티들
        public int Damage => _damage;
        public float Range => _range;

        // 콤보 트리거 반환 프로퍼티
        public ComboTrigger Combo1Trigger => _combo1Trigger;
        public ComboTrigger Combo2Trigger => _combo2Trigger;
        public ComboTrigger Combo3Trigger => _combo3Trigger;

        public override Item CreateItem()
        {
            return new WeaponItem(this);
        }

        // Weapon에 데이터를 전달하는 함수
        public void ApplyToWeapon(Weapon weapon)
        {
            weapon.attackDamage = _damage;
            weapon.attackRange = _range;

            // ComboTrigger enum 값을 weapon에 할당
            weapon.combo1Trigger = _combo1Trigger;
            weapon.combo2Trigger = _combo2Trigger;
            weapon.combo3Trigger = _combo3Trigger;
        }
    }
}

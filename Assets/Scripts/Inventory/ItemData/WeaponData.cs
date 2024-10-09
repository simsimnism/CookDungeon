using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponData
{
    public string name;          // 무기 이름
    public string type;          // 무기 종류 (ex: Sword, Dagger)
    public float attackDamage;   // 공격력
    public float attackRange;    // 공격 범위
    public ComboTrigger combo1Trigger; // 1타 콤보 애니메이션 트리거
    public ComboTrigger combo2Trigger; // 2타 콤보 애니메이션 트리거
    public ComboTrigger combo3Trigger; // 3타 콤보 애니메이션 트리거
    internal Sprite weaponSprite;
}
public enum ComboTrigger
{
    Combo1,
    Combo2,
    Combo3
}

[System.Serializable]
public class WeaponDataList
{
    public List<WeaponData> weapons;
}

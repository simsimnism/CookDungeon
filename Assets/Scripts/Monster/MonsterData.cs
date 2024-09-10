using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterData
{
    public string name;    // 몬스터 이름
    public int id;         // 몬스터 ID
    public int health;     // 체력
    public int attack;     // 공격력
    public float range;    // 공격 범위
    public float speed;    // 이동 속도
}

[System.Serializable]
public class MonsterList
{
    public MonsterData[] NormalMonsters;   // 일반 몬스터 목록
    public MonsterData[] SpecialMonsters;  // 특수 몬스터 목록
}

[System.Serializable]
public class Monsters
{
    public MonsterList monster;  // 전체 몬스터 목록

    internal void TakeDamage(float attackDamage)
    {
        throw new NotImplementedException();
    }
}


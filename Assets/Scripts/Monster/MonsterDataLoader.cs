using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterInfo
{
    public string name;
    public int id;
    public int health;
    public int attack;
    public int attackrange;
    public float range;
    public float speed;
}

[System.Serializable]
public class MonsterDataLoader : DataManager.ILoader<int, MonsterDataSO>
{
    public List<MonsterInfo> NormalMonsters;

    // 몬스터 데이터를 Dictionary로 변환
    public Dictionary<int, MonsterDataSO> MakeDictionary()
    {
        Dictionary<int, MonsterDataSO> monsterDict = new Dictionary<int, MonsterDataSO>();

        foreach (MonsterInfo info in NormalMonsters)
        {
            // MonsterDataSO 인스턴스를 생성하고 데이터를 채움
            MonsterDataSO monsterData = ScriptableObject.CreateInstance<MonsterDataSO>();
            monsterData.monsterName = info.name;
            monsterData.health = info.health;
            monsterData.attack = info.attack;
            monsterData.range = info.range;
            monsterData.speed = info.speed;

            // 몬스터 프리팹을 Resources에서 로드 (필요에 맞게 변경 가능)
            monsterData.monsterPrefab = Resources.Load<GameObject>($"Prefabs/Monster/{info.name}");

            // 몬스터 id를 key로 Dictionary에 추가
            monsterDict.Add(info.id, monsterData);
        }

        return monsterDict;
    }

    // 이름으로 몬스터 데이터를 찾는 메서드
    public MonsterDataSO GetMonsterDataByName(string name)
    {
        foreach (var monsterInfo in NormalMonsters)
        {
            if (monsterInfo.name == name)
            {
                // MonsterDataSO 인스턴스를 생성하고 데이터를 채움
                MonsterDataSO monsterData = ScriptableObject.CreateInstance<MonsterDataSO>();
                monsterData.monsterName = monsterInfo.name;
                monsterData.health = monsterInfo.health;
                monsterData.attack = monsterInfo.attack;
                monsterData.range = monsterInfo.range;
                monsterData.speed = monsterInfo.speed;

                // 몬스터 프리팹을 Resources에서 로드
                monsterData.monsterPrefab = Resources.Load<GameObject>($"Prefabs/Monster/{monsterInfo.name}");

                return monsterData;
            }
        }

        // 이름으로 찾지 못한 경우 null 반환
        Debug.LogError($"해당 이름의 몬스터를 찾을 수 없습니다: {name}");
        return null;
    }
}

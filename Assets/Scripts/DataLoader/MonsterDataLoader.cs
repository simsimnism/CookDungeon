using System.Collections.Generic;
using UnityEngine;
using System;

public class MonsterDataLoader
{
    // ILoader 인터페이스
    public interface ILoader<Key, Value>
    {
        Dictionary<Key, Value> MakeDictionary();
    }

    // MonsterData 클래스 (몬스터 데이터를 담고 있는 클래스)
    [Serializable]
    public class MonsterData
    {
        public int id;
        public string name;
        public int health;
        public int attack;
        public int attackRange;
        public float range;
        public float speed;
    }

    // MonsterDataList 클래스 (JSON 데이터를 파싱하여 MonsterData를 관리)
    [Serializable]
    public class MonsterDataList : ILoader<int, MonsterData>
    {
        public List<MonsterData> monsters;

        public Dictionary<int, MonsterData> MakeDictionary()
        {
            Dictionary<int, MonsterData> dic = new Dictionary<int, MonsterData>();
            foreach (MonsterData data in monsters)
            {
                dic.Add(data.id, data);
            }
            return dic;
        }
    }

    private Dictionary<int, MonsterDataSO> monsterDataSOList = new Dictionary<int, MonsterDataSO>();

    // JSON 파일에서 몬스터 데이터 로드
    public void LoadMonsterData(string path)
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>(path);
        if (textAsset == null)
        {
            Debug.LogError($"'{path}' 경로에서 파일을 찾을 수 없습니다.");
            return;
        }

        MonsterDataList loader = JsonUtility.FromJson<MonsterDataList>(textAsset.text);
        Dictionary<int, MonsterData> monsterDataDic = loader.MakeDictionary();

        PopulateMonsterSO(monsterDataDic);  // ScriptableObject로 변환
    }

    // MonsterData를 MonsterDataSO로 변환하여 저장
    private void PopulateMonsterSO(Dictionary<int, MonsterData> monsterDataDic)
    {
        foreach (KeyValuePair<int, MonsterData> entry in monsterDataDic)
        {
            MonsterDataSO monsterDataSO = ScriptableObject.CreateInstance<MonsterDataSO>();
            monsterDataSO.id = entry.Value.id;
            monsterDataSO.monsterName = entry.Value.name;
            monsterDataSO.health = entry.Value.health;
            monsterDataSO.attack = entry.Value.attack;
            monsterDataSO.attackRange = entry.Value.attackRange;
            monsterDataSO.range = entry.Value.range;
            monsterDataSO.speed = entry.Value.speed;

            monsterDataSOList.Add(entry.Key, monsterDataSO);  // ScriptableObject 딕셔너리에 추가
        }
    }

    // 이름을 통해 몬스터 데이터 검색
    public MonsterDataSO GetMonsterDataByName(string name)
    {
        foreach (var monsterDataSO in monsterDataSOList.Values)
        {
            if (monsterDataSO.monsterName == name)
            {
                return monsterDataSO;
            }
        }

        Debug.LogError($"'{name}'에 해당하는 몬스터 데이터를 찾을 수 없습니다.");
        return null;
    }

    // ID를 통한 데이터 검색 기능
    public MonsterDataSO GetMonsterDataById(int id)
    {
        if (monsterDataSOList.TryGetValue(id, out MonsterDataSO monsterDataSO))
        {
            return monsterDataSO;
        }

        Debug.LogError($"'{id}'에 해당하는 몬스터 데이터를 찾을 수 없습니다.");
        return null;
    }
}

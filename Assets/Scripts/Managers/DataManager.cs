using System.Collections.Generic;
using UnityEngine;
using System;

public class DataManager 
{
    public interface ILoader<Key, Value>
    {
        Dictionary<Key, Value> MakeDictionary();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        // 경로에서 확장자(.json)를 제외하고 로드
        TextAsset textAsset = Managers.Resource.Load<TextAsset>("Monsters/json/Monsters");
        if (textAsset == null)
        {
            Debug.LogError($"'{path}' 경로에서 파일을 찾을 수 없습니다.");
            return default;
        }

        return JsonUtility.FromJson<Loader>(textAsset.text);
    }



    [Serializable]
    public class MonsterData
    {
        public int id;
        public string name;
        public int health;
        public int attack;
        public float range;
        public float speed;
    }

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

    public Dictionary<int, MonsterDataSO> monsterDataSOList = new Dictionary<int, MonsterDataSO>();

    public void Init()
    {
        // JSON 파일을 로드하여 딕셔너리로 변환
        Dictionary<int, MonsterData> monsterDataDic = LoadJson<MonsterDataList, int, MonsterData>("Data/Monsters").MakeDictionary();

        foreach (KeyValuePair<int, MonsterData> entry in monsterDataDic)
        {
            MonsterDataSO monsterDataSO = ScriptableObject.CreateInstance<MonsterDataSO>();
            monsterDataSO.id = entry.Value.id;
            monsterDataSO.monsterName = entry.Value.name;
            monsterDataSO.health = entry.Value.health;
            monsterDataSO.attack = entry.Value.attack;
            monsterDataSO.range = entry.Value.range;
            monsterDataSO.speed = entry.Value.speed;

            monsterDataSOList.Add(entry.Key, monsterDataSO);  // ScriptableObject로 생성된 데이터를 딕셔너리에 저장
        }
    }


    public MonsterDataSO GetMonsterDataByName(string name)
    {
        // monsterDataSOList를 순회하며 이름을 기반으로 데이터를 찾음
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

}

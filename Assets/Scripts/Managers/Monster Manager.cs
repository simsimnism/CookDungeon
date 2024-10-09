using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager 
{
    public MonsterData[] normalMonsters;   // 일반 몬스터 배열
    public MonsterData[] specialMonsters;  // 특수 몬스터 배열

    private string jsonFilePath = "monsters";  // Resources 폴더 안에 monsters.json 파일

    void Start()
    {
        LoadMonsterData();
    }

    // JSON 파일에서 몬스터 데이터를 불러오는 함수
    void LoadMonsterData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFilePath);

        if (jsonFile != null)
        {
            Monsters monstersList = JsonUtility.FromJson<Monsters>(jsonFile.text);

            // NormalMonsters와 SpecialMonsters 데이터를 저장
            normalMonsters = monstersList.monster.NormalMonsters;
            specialMonsters = monstersList.monster.SpecialMonsters;

            // 데이터를 확인하는 로그 출력 (디버깅용)
            Debug.Log("일반 몬스터 수: " + normalMonsters.Length);
            Debug.Log("특수 몬스터 수: " + specialMonsters.Length);
        }
        else
        {
            Debug.LogError("몬스터 JSON 파일을 찾을 수 없습니다.");
        }
    }

    // 특정 ID의 몬스터 데이터를 가져오는 함수
    public MonsterData GetMonsterDataByID(int id)
    {
        foreach (MonsterData monster in normalMonsters)
        {
            if (monster.id == id)
            {
                return monster;
            }
        }

        foreach (MonsterData monster in specialMonsters)
        {
            if (monster.id == id)
            {
                return monster;
            }
        }

        return null;
    }
}


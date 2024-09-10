using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDataLoader : MonoBehaviour
{
    // 몬스터 데이터를 JSON 파일 또는 다른 소스로부터 로드하는 함수
    public List<MonsterData> LoadMonsterData()
    {
        // 여기에 JSON 또는 파일 로딩 로직이 들어갑니다.
        // 예시로 간단하게 몬스터 데이터를 반환합니다.
        List<MonsterData> monsterDataList = new List<MonsterData>();

        // 임시 데이터 예시
        monsterDataList.Add(new MonsterData { id = 1, name = "Goblin", health = 100, attack = 10, range = 5, speed = 3 });
        monsterDataList.Add(new MonsterData { id = 3, name = "Orc", health = 150, attack = 20, range = 6, speed = 2 });

        return monsterDataList;
    }
}


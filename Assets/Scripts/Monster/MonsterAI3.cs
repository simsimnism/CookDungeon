using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI3 : MonoBehaviour
{

    public MonsterDataSO monsterDataSO;  // ScriptableObject로 데이터를 저장
    private int health;
    private int attack;
    private float range;
    private float speed;
    private int id;  // 몬스터의 ID
    public Transform player;

    void Start()
    {
        // "(Clone)"을 제거하고 이름을 가져옴
        string monsterName = gameObject.name.Replace("(Clone)", "").Trim();

        // 이름을 기준으로 몬스터 데이터를 검색
        MonsterDataSO data = Managers.Data.GetMonsterDataByName(monsterName);
        if (data != null)
        {
            AssignData(data);  // 데이터를 AI에 할당
        }
        else
        {
            Debug.LogError($"Monster 이름을 파싱할 수 없습니다: {monsterName}");
        }
    }
    void AssignData(MonsterDataSO data)
    {
        monsterDataSO = data;

        health = monsterDataSO.health;
        attack = monsterDataSO.attack;
        range = monsterDataSO.range;
        speed = monsterDataSO.speed;

        Debug.Log($"몬스터 데이터 적용됨: {monsterDataSO.monsterName} (ID: {monsterDataSO.id})");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

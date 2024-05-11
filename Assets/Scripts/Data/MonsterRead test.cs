using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRead
{
    // 싱글톤 인스턴스
    private static MonsterRead _instance;
    public static MonsterRead Instance { get { return _instance; } }

    // 몬스터 데이터를 저장할 딕셔너리
    private Dictionary<int, MonsterData> monsterData = new Dictionary<int, MonsterData>();
    private object gameObject;

    // Awake 함수에서 싱글톤 인스턴스 초기화
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void DontDestroyOnLoad(object gameObject)
    {
        throw new NotImplementedException();
    }

    private void Destroy(object gameObject)
    {
        throw new NotImplementedException();
    }

    // CSV 파일에서 데이터를 읽어오는 함수
    public void ReadData()
    {
        List<Dictionary<string, object>> dataList = CSVReader.Read("MonsterData");
        foreach (var dataDict in dataList)
        {
            // 각 행의 데이터 추출
            int id = (int)dataDict["ID"];
            int hp = (int)dataDict["HP"];
            int at = (int)dataDict["AT"];
            int rg = (int)dataDict["RG"];
            int sp = (int)dataDict["SP"];

            // 추출한 데이터를 MonsterData 객체로 생성하여 딕셔너리에 추가
            monsterData.Add(id, new MonsterData(id, hp, at, rg, sp));
        }
    }

    // 지정된 ID의 몬스터 데이터를 가져오는 함수
    public MonsterData GetMonsterData(int id)
    {
        if (monsterData.ContainsKey(id))
        {
            return monsterData[id];
        }
        else
        {
            // 지정된 ID의 데이터가 없는 경우 에러 메시지 출력
            Debug.LogError($"Monster data with ID {id} does not exist.");
            return null;
        }
    }
}

// 몬스터 데이터 클래스
public class MonsterData
{
    // 속성들은 읽기 전용으로 설정되어야 함
    public int ID { get; private set; }
    public int HP { get; private set; }
    public int AT { get; private set; }
    public int RG { get; private set; }
    public int SP { get; private set; }

    // 생성자를 통해 속성 초기화
    public MonsterData(int id, int hp, int at, int rg, int sp)
    {
        ID = id;
        HP = hp;
        AT = at;
        RG = rg;
        SP = sp;
    }
}
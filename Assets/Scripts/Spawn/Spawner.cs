using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Spawner : MonoBehaviour
{
    public Tilemap tilemap;  // 타일맵 참조
    public List<Vector3> spawnPositions;  // 스폰 포인트 리스트
    public MonsterDataLoader monsterDataLoader; // 몬스터 데이터 로더
    public ObjectPoolManager objectPoolManager; // 풀링 매니저 참조

    List<MonsterData> monsterDataList;  // 전체 몬스터 데이터 리스트
    List<MonsterData> filteredMonsterDataList; // 필터링된 몬스터 데이터 리스트

    void Start()
    {
        // 몬스터 데이터를 로드
        monsterDataList = monsterDataLoader.LoadMonsterData();

        // 특정 ID의 몬스터만 필터링 (예: ID가 1번 또는 3번인 몬스터)
        filteredMonsterDataList = monsterDataList.FindAll(data => data.id == 1 || data.id == 3);

        // 타일맵에서 특정 타일의 위치를 스폰 포인트로 추가
        spawnPositions = new List<Vector3>
        {
            tilemap.CellToWorld(new Vector3Int(2, 3, 0)),  // 예시: (2, 3) 타일 위치
            tilemap.CellToWorld(new Vector3Int(5, 1, 0)),  // 예시: (5, 1) 타일 위치
            // 필요에 따라 추가적인 스폰 포인트...
        };
    }

    void Update()
    {
        // 스페이스바 입력을 감지하여 몬스터 소환
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Spawn();  // 몬스터 소환
        }
    }

    void Spawn()
    {
        // 랜덤하게 필터링된 몬스터 데이터를 선택
        MonsterData monsterData = filteredMonsterDataList[Random.Range(0, filteredMonsterDataList.Count)];

        // 랜덤하게 스폰 포인트를 선택하여 위치 설정
        Vector3 spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Count)];

        // 풀에서 몬스터를 스폰 (몬스터의 ID를 태그로 사용하여 가져옴)
        GameObject monster = objectPoolManager.SpawnFromPool(monsterData.id.ToString(), spawnPosition, Quaternion.identity);

        // 몬스터 초기화: MonsterMovement 클래스를 통해 초기화
        if (monster != null)
        {
            MonsterMovement monsterComponent = monster.GetComponent<MonsterMovement>();
            if (monsterComponent != null)
            {
                monsterComponent.Init(monsterData); // 몬스터 데이터로 초기화
            }
        }
    }
}

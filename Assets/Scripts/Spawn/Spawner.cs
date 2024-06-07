using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Spawner : MonoBehaviour
{
    public Tilemap tilemap;  // 타일맵을 참조
    public List<Vector3> spawnPositions;  // 스폰 포인트 리스트
    public EnemyDataLoader enemyDataLoader; // 적 데이터 로더
    public GameObject[] enemyPrefabs; // 프리팹 배열

    List<EnemyData> enemyDataList;
    List<EnemyData> filteredEnemyDataList; // 필터링된 적 데이터 리스트

    void Start()
    {
        // 적 데이터를 로드
        enemyDataList = enemyDataLoader.LoadEnemyData();

        // 특정 ID의 몬스터만 필터링 (예: 1번과 3번)
        filteredEnemyDataList = enemyDataList.FindAll(data => data.id == 1 || data.id == 3);

        // 타일맵에서 특정 타일의 위치를 스폰 포인트로 추가
        spawnPositions = new List<Vector3>
        {
            tilemap.CellToWorld(new Vector3Int(2, 3, 0)),  // 예시: (2, 3) 타일 위치
            tilemap.CellToWorld(new Vector3Int(5, 1, 0)),  // 예시: (5, 1) 타일 위치
            // 추가 스폰 포인트...
        };
    }

    void Update()
    {
        // 스페이스바 입력 체크
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Spawn();  // 몬스터 소환
        }
    }

    void Spawn()
    {
        // 랜덤하게 필터링된 몬스터 데이터를 선택
        EnemyData enemyData = filteredEnemyDataList[Random.Range(0, filteredEnemyDataList.Count)];

        // 몬스터의 id에 해당하는 프리팹을 가져옴
        GameObject enemyPrefab = enemyPrefabs[enemyData.id - 1];

        // 프리팹 인스턴스화
        GameObject enemy = Instantiate(enemyPrefab);

        // 랜덤하게 스폰 포인트를 선택하여 몬스터 위치 설정
        enemy.transform.position = spawnPositions[Random.Range(0, spawnPositions.Count)];

        // 적 초기화
        Enemy enemyComponent = enemy.GetComponent<Enemy>();
        enemyComponent.Init(enemyData);
    }
}
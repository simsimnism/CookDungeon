using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class MonsterSpawner : MonoBehaviour
{
    private int enemiesToSpawn;
    private int currentEnemyCount;
    private int enemiesSpawnedSoFar;
    private int enemyMaxConcurrentSpawnNumber;
    private Room currentRoom;
    private RoomEnemySpawnParameters roomEnemySpawnParameters;

    private void OnEnable()
    {
        // subscribe to room changed event
        EventHandle.OnRoomChange += StaticEventHandler_OnRoomChanged;
    }

    private void OnDisable()
    {
        // unsubscribe from room changed event
        EventHandle.OnRoomChange -= StaticEventHandler_OnRoomChanged;
    }

    // 현재 방을 변경하는 함수
    private void StaticEventHandler_OnRoomChanged(RoomChangeEvent roomChangedEvent)
    {
        enemiesSpawnedSoFar = 0;
        currentEnemyCount = 0;

        currentRoom = roomChangedEvent.room;

        // if the room is a corridor or the entrance then return
        if (currentRoom.roomNodeType.isCorridorEW || currentRoom.roomNodeType.isCorridorNS || currentRoom.roomNodeType.isEntrance)
            return;

        // if the room has already been defeated then return
        if (currentRoom.isClearedOfMonster) return;

        // Get random number of enemies to spawn
        enemiesToSpawn = currentRoom.GetNumberOfSpawnMonsters(Managers.GM.GetCurrentDungeonLevel());

        // Get room enemy spawn parameters
        roomEnemySpawnParameters = currentRoom.GetNumberOfSpawnParameter(Managers.GM.GetCurrentDungeonLevel());

        // If no enemies to spawn return
        if (enemiesToSpawn == 0)
        {
            // Mark the room as cleared
            currentRoom.isClearedOfMonster = true;

            return;
        }

        // Get concurrent number of enemies to spawn
        enemyMaxConcurrentSpawnNumber = GetConcurrentEnemies();

        // 문을 잠구기
        // currentRoom.instantiatedRoom.LockDoors();

        // Spawn enemies
        SpawnEnemies();
    }

    // 몬스터 스폰
    private void SpawnEnemies()
    {
        // 게임 스테이트를 
        if (Managers.GM.gameState == GameState.bossRoom)
        {
            Managers.GM.previousGameState = GameState.bossRoom;
            Managers.GM.gameState = GameState.BossBattle;
        }

        // Set gamestate engaging enemies
        else if (Managers.GM.gameState == GameState.playingLevel)
        {
            Managers.GM.previousGameState = GameState.playingLevel;
            Managers.GM.gameState = GameState.MonsterBattle;
        }

        StartCoroutine(SpawnEnemiesRoutine());
    }

    // 몬스터를 스폰하는 코루틴
    private IEnumerator SpawnEnemiesRoutine()
    {
        Grid grid = currentRoom.instantiatedRoom.grid;

        // 랜덤으로 몬스터를 선택하는 데 사용되는 도우미 클래스의 인스턴스 생성
        MonsterRandomSpawn<MonsterDataSO> randomEnemyHelperClass = new MonsterRandomSpawn<MonsterDataSO>(currentRoom.MonsterByLevelList);

        // 몬스터를 생성할 수 있는지 확인하는 작업
        if (currentRoom.spawnPositionArray.Length > 0)
        {
            // 모든 몬스터가 생성될 때 까지 반복
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                // 현재 몬스터 수가 동시에 나올 수 있는 최대 몬스터 수보다 적어질 때까지 대기 상태
                while (currentEnemyCount >= enemyMaxConcurrentSpawnNumber)
                {
                    yield return null;
                }

                Vector3Int cellPosition = (Vector3Int)currentRoom.spawnPositionArray[Random.Range(0, currentRoom.spawnPositionArray.Length)];

                // 몬스터 생성 (다음번에 생성될 몬스터의 정보를 받음)
                CreateMonster(randomEnemyHelperClass.GetItem(), grid.CellToWorld(cellPosition));

                yield return new WaitForSeconds(GetEnemySpawnInterval());
            }
        }
    }

    /// <summary>
    /// Get a random spawn interval between the minimum and maximum values
    /// </summary>
    private float GetEnemySpawnInterval()
    {
        return (Random.Range(roomEnemySpawnParameters.minSpawnInterval, roomEnemySpawnParameters.maxSpawnInterval));
    }

    /// <summary>
    /// Get a random number of concurrent enemies between the minimum and maximum values
    /// </summary>
    private int GetConcurrentEnemies()
    {
        return (Random.Range(roomEnemySpawnParameters.minConcurrentEnemies, roomEnemySpawnParameters.maxConcurrentEnemies));
    }

    // 지정된 위치에 적을 생성하는 함수
    private void CreateMonster(MonsterDataSO monsterData, Vector3 position)
    {
        // 지금까지 스폰된 몬스터 수를 추적
        enemiesSpawnedSoFar++;

        // 현재 몬스터 카운트에 1 추가 ( 몬스터가 죽을 때 감소 )
        currentEnemyCount++;

        // 현재 던전 레벨을 가져옴.
        DungeonLevelSO dungeonLevel = Managers.GM.GetCurrentDungeonLevel();

        // 몬스터 인스턴스화
        GameObject monster = Instantiate(monsterData.monsterPrefab, position, Quaternion.identity, transform);

        // 몬스터 파괴 이벤트를 구독함
        monster.GetComponent<DestroyEvent>().OnDestroyed += Enemy_OnDestroyed;
    }

    // 몬스터 제거 함수
    private void Enemy_OnDestroyed(DestroyEvent destroyedEvent, DestroyedEventArgs destroyedEventArgs)
    {
        // 이벤트 구독 해제
        destroyedEvent.OnDestroyed -= Enemy_OnDestroyed;

        // 방 안에 있는 현재 몬스터의 수를 줄임
        currentEnemyCount--;
        Debug.Log("현재 남은 적 수 : " + currentEnemyCount);

        if (currentEnemyCount <= 0 && enemiesSpawnedSoFar == enemiesToSpawn)
        {
            currentRoom.isClearedOfMonster = true;

            // 게임 스테이트 변경
            if (Managers.GM.gameState == GameState.MonsterBattle)
            {
                Managers.GM.gameState = GameState.playingLevel;
                Managers.GM.previousGameState = GameState.MonsterBattle;
            }

            else if (Managers.GM.gameState == GameState.BossBattle)
            {
                Managers.GM.gameState = GameState.bossRoom;
                Managers.GM.previousGameState = GameState.BossBattle;
            }

            // 문이 열림 (문 미완)
            // currentRoom.instantiatedRoom.UnlockDoors(Settings.doorUnlockDelay);

            // 방에 몬스터가 없어서 클리어되었다는 이벤트 출력
            EventHandle.CallRoomMonsterClearEvent(currentRoom);
            Debug.Log("방을 클리어 했습니다!");
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // 임시 고치기

public class GameManager
{
    [HideInInspector] public GameState gameState; // 현재 게임 상태
    [HideInInspector] public GameState previousGameState; // 이전 게임 상태

    // 던전 레벨 리스트 설정
    private List<DungeonLevelSO> dungeonLevelList;
    // 초기 던전 레벨 값 (스테이지 번호)
    private int currentDungeonLevelListIndex = 0;

    private bool _isMoving = true;

    //플레이어의 무적 상태를 관리
    private bool _isInvincible = false;

    public bool IsInvincible { get { return _isInvincible; } set { _isInvincible = value; } }

    public bool IsMoving { get { return _isMoving; } set { _isMoving = value; } }

    public void Init()
    {
        GameStart();
        //previousGameState = GameState.gameStarted;
        //gameState = GameState.gameStarted;
    }

    public void HandleGameState()
    {
        switch (gameState)
        {
            case GameState.title:
                break;
            case GameState.gameStarted:
                // 임시 고치기 ( 씬이 로드 되기도 전에 GameStart함수를 실행시켜서 null 레퍼런스가 나와서 방지용 코드 )
                Scene scene = SceneManager.GetActiveScene();
                if (scene.name == "GameScene")
                {
                    GameStart();
                }
                break;
            default:
                break;
        }
    }

    void GameStart()
    {
        // 던전 레벨 리스트 생성
        dungeonLevelList = new List<DungeonLevelSO>();

        // 리소스 폴더에서 던전 레벨 SO 로드
        DungeonLevelSO[] DungeonLevel = Resources.LoadAll<DungeonLevelSO>("ScriptableObjectAssets/Dungeon/Level");

        // 던전 레벨 길이만큼 레벨 리스트 추가
        dungeonLevelList.AddRange(DungeonLevel);

        // 최초 던전 빌드
        // 만약 현재 던전 인덱스가 0보다 크고 던전 레벨 리스트 개수보다 작으면 던전을 실행
        if (currentDungeonLevelListIndex >= 0 && currentDungeonLevelListIndex < dungeonLevelList.Count)
        {
            genDungeon(currentDungeonLevelListIndex);
        }
        else
        {
            Debug.LogError("유효하지 않은 던전 레벨 인덱스: " + currentDungeonLevelListIndex);
        }

        // 캐릭터 생성
        GameObject Player = Managers.Resource.Instantiate("Player/Player");

        // 카메라 세팅
        GameObject Camera = Managers.Resource.Instantiate("Camera/PlayerCamera");

        gameState = GameState.playingLevel; // 게임 상태를 진행 중으로 변경
    }

    // 최종적으로 던전을 생성하는 함수
    void genDungeon(int dungeonLevelListIndex)
    {
        Debug.Log(dungeonLevelListIndex);
        Debug.Log(dungeonLevelList[dungeonLevelListIndex] == null);
        // 던전 인덱스 번호에 따라 던전 리스트에 있는 던전을 생성 
        bool dungeonBuiltSucessfully = DungeonBuilder.Instance.GenerateDungeon(dungeonLevelList[dungeonLevelListIndex]);

        // 던전 빌드가 실패하면
        if (!dungeonBuiltSucessfully)
        {
            Debug.LogError("던전 생성 실패 - 지정된 방과 노드 그래프에서 던전을 만들 수 없습니다.");
        }
    }
}
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

    // 현재 & 이전 방에 대한 정보
    private Room currentRoom;
    private Room previousRoom;
    private Player player;

    // 던전 레벨 리스트 설정
    private List<DungeonLevelSO> dungeonLevelList;
    // 초기 던전 레벨 값 (스테이지 번호)
    public int currentDungeonLevelListIndex = 0;

    private bool _CookAbleTime = false;

    private bool _isMoving = true;

    //플레이어의 무적 상태를 관리
    private bool _isInvincible = false;

    public bool IsInvincible { get { return _isInvincible; } set { _isInvincible = value; } }

    public bool IsMoving { get { return _isMoving; } set { _isMoving = value; } }

    public bool CookAbleTime { get { return _CookAbleTime; } set { _CookAbleTime = value; } }

    public void Init()
    {
        previousGameState = GameState.title;
        gameState = GameState.title;
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
            case GameState.levelCompleted:
                LevelCompleted();
                break;
            case GameState.restartGame:
                RestartGame();
                break;
            default:
                break;
        }
    }

    // 방이 변경되는 이벤트
    private void EventHandle_RoomChangeEvent(RoomChangeEvent roomChangedEventArgs)
    {
        SetCurrentRoom(roomChangedEventArgs.room);
    }

    // 현재 플레이어가 있는 방의 정보를 가져오는 함수
    public Room GetCurrentRoom()
    {
        return currentRoom;
    }

    // 현재 플레이어가 있는 방을 설정하는 함수
    public void SetCurrentRoom(Room room)
    {
        previousRoom = currentRoom;
        currentRoom = room;
    }

    void GameStart()
    {

        // Subscribe to room changed event.        
        EventHandle.OnRoomChange += EventHandle_RoomChangeEvent;

        //게임 로딩 생성
        Managers.Popup.OpenGameLoading();

        gameState = GameState.playingLevel; // 게임 상태를 진행 중으로 변경

        //인트로UI 생성 나중에 매니저에서 게임 리스타트에서 게임 스타트로 바뀌는 함수 만들거임 연결을 전부 끊고 재시작 해야해서 기능 수정 많이 필요함
        //Managers.Popup.OpenIntro();

        // 캐릭터 생성
        GameObject Player = Managers.Resource.Instantiate("Player/Player");
        player = Player.GetComponent<Player>();

        //게임 클리어 함수 생성 후 세팅
        //Managers.Popup.OpenGameClear();

        // 카메라 세팅
        GameObject Camera = Managers.Resource.Instantiate("Camera/PlayerCamera");

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

        EventHandle.CallRoomChangeEvent(currentRoom);

    }

    public Player GetPlayer()
    {
        return player;
    }

    // 최종적으로 던전을 생성하는 함수
    void genDungeon(int dungeonLevelListIndex)
    {
        // 던전 인덱스 번호에 따라 던전 리스트에 있는 던전을 생성 
        bool dungeonBuiltSucessfully = DungeonBuilder.Instance.GenerateDungeon(dungeonLevelList[dungeonLevelListIndex]);

        // 던전 빌드가 실패하면
        if (!dungeonBuiltSucessfully)
        {
            Debug.LogError("던전 생성 실패 - 지정된 방과 노드 그래프에서 던전을 만들 수 없습니다.");
        }

        // Call static event that room has changed.
        EventHandle.CallRoomChangeEvent(currentRoom);

        // 플레이어를 방 중앙에 세팅
        player.gameObject.transform.position = new Vector3((currentRoom.lowerBounds.x + currentRoom.upperBounds.x) / 2f, (currentRoom.lowerBounds.y + currentRoom.upperBounds.y) / 2f, 0f);

        // 플레이어와 가장 가까운 방에서 가장 가까운 스폰 지점을 얻음
        player.gameObject.transform.position = HelperUtilities.GetSpawnPositionNearestToPlayer(player.gameObject.transform.position);

        // 라운드 UI 띄우기
    }

    private void LevelCompleted()
    {
        // 스테이트를 다시 플레이로 바꿈
        gameState = GameState.playingLevel;


        //ㅇ 수정
        // 레벨 클리어 출력?
        if (currentDungeonLevelListIndex == 4) // 레벨 인덱스는 0부터 시작하므로 5번째 레벨은 인덱스 4
        {
            gameState = GameState.gameCleared;           
            return;
        }

        // 스크린을 페이드 아웃

        //yield return StartCoroutine(Fade(1f, 0f, 2f, new Color(0f, 0f, 0f, 0.4f)));

        // 현제 던전 레벨을 증가시킴
        currentDungeonLevelListIndex++;

        genDungeon(currentDungeonLevelListIndex);

        Managers.Popup.OpenRoundInfo();
    }

    // 게임 리스타트 메서드 추가
    private void RestartGame()
    {
        // 던전 레벨 인덱스를 초기화
        currentDungeonLevelListIndex = 0;

        // 이전 방과 현재 방 초기화
        previousRoom = null;
        currentRoom = null;
    }

    // 현재 던전 레벨 값을 호출
    public DungeonLevelSO GetCurrentDungeonLevel()
    {
        return dungeonLevelList[currentDungeonLevelListIndex];
    }
}
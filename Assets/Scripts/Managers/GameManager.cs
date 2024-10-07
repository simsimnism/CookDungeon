using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;

public class GameManager
{
    // 던전 레벨 리스트 설정
    private List<DungeonLevelSO> dungeonLevelList;
    // 초기 던전 레벨 값 (스테이지 번호)
    private int currentDungeonLevelListIndex = 0;
    //현재 플레이어 스테이터스
    private string gameState;
    //캐릭터가 움직이는지 여부
    private bool _isMoving = true;

    public bool IsMoving {  get { return _isMoving; } set { _isMoving = value; } }


    public void Init()
    {
        dungeonLevelList = new List<DungeonLevelSO>();

        // 리소스에서 던전 레벨 SO 로드
        DungeonLevelSO[] DungeonLevel = Resources.LoadAll<DungeonLevelSO>("ScriptableObjectAssets/Dungeon/Level");

        dungeonLevelList.AddRange(DungeonLevel);

        if (currentDungeonLevelListIndex >= 0 && currentDungeonLevelListIndex < dungeonLevelList.Count)
        {
            genDungeon(currentDungeonLevelListIndex);
        }
        else
        {
            Debug.LogError("유효하지 않은 던전 레벨 인덱스: " + currentDungeonLevelListIndex);
        }
    }

    void genDungeon(int dungeonLevelListIndex)
    {
        bool dungeonBuiltSucessfully = DungeonBuilder.Instance.GenerateDungeon(dungeonLevelList[dungeonLevelListIndex]);

        if (!dungeonBuiltSucessfully)
        {
            Debug.LogError("던전 생성 실패 - 지정된 방과 노드 그래프에서 던전을 만들 수 없습니다.");
        }
    }



}
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 던전 레벨 리스트
    [SerializeField] private List<DungeonLevelSO> dungeonLevelList;

    // 현재 던전 레벨 리스트
    [SerializeField] private int currentDungeonLevelListIndex = 0;

    void Start()
    {

    }

    void Update()
    {
        PlayDungeonLevel(currentDungeonLevelListIndex);
    }

    public void Init()
    {

    }

    void PlayDungeonLevel(int dungeonLevelListIndex)
    {
        bool dungeonBuiltSucessfully = Managers.Dungeon.GenerateDungeon(dungeonLevelList[dungeonLevelListIndex]);

        if (!dungeonBuiltSucessfully)
        {
            Debug.LogError("Couldn't build dungeon from specified rooms and node graphs");
        }

        //int dungeonLevel = 1;
    }
}

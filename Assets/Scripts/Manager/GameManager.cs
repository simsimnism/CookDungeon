using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*
     사용하고 싶으면 스크립트에 GameManager GM = GameManager.Instance; 을 참조
     */

    private static PlayerManager s_PlayerManager = new PlayerManager();
    private static DungeonBuilder s_DungeonBuilder = new DungeonBuilder();

    //매니저 인스턴스
    static GameManager s_Instance;

    public static GameManager Instance { get { Init(); return s_Instance; } }
    public static PlayerManager Player { get { Init(); return s_PlayerManager; } }
    public static DungeonBuilder Dungeon { get { Init(); return s_DungeonBuilder; } }

    void Start()
    {
        Init();

    }

    void Update()
    {
        
    }

    #region 초기화
    static void Init()
    {
        if (s_Instance == null)
        {
            GameObject go = GameObject.Find("GameManager");
            if (go == null)
            {
                go = new GameObject { name = "GameManager" };
                go.AddComponent<GameManager>();
            }

            DontDestroyOnLoad(go);
            s_Instance = go.GetComponent<GameManager>();
            s_PlayerManager.Init();
            s_DungeonBuilder.Init();
        }
    }
    #endregion

    void test()
    {
        //int dungeonLevel = 1;
        //GameManager.s_DungeonBuilder.GenerateDungeon(dungeonLevel);
    }
}

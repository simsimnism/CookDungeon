using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static GameManager s_GameManager = new GameManager();
    private static PlayerManager s_PlayerManager = new PlayerManager();
    private static DungeonBuilder s_DungeonBuilder = new DungeonBuilder();

    //매니저 인스턴스
    static Managers s_Instance;

    public static Managers Instance { get { Init(); return s_Instance; } }
    public static GameManager GM { get { Init(); return s_GameManager; } }
    public static PlayerManager Player { get { Init(); return s_PlayerManager; } }
    public static DungeonBuilder Dungeon { get { Init(); return s_DungeonBuilder; } }

    void Start()
    {
        Init();
    }

    #region 초기화
    static void Init()
    {
        if (s_Instance == null)
        {
            GameObject go = GameObject.Find("Manager");
            if (go == null)
            {
                go = new GameObject { name = "Manager" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_Instance = go.GetComponent<Managers>();
            s_PlayerManager.Init();
            s_DungeonBuilder.Init();
        }
    }
    #endregion


}

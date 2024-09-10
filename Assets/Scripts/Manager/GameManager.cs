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

    public ObjectPoolManager Pool;
    public PlayerController Player;

    private static PlayerManager s_PlayerManager = new PlayerManager();

    //매니저 인스턴스
    static GameManager s_Instance;

    public static GameManager Instance { get { Init(); return s_Instance; } }

    public static PlayerManager PlayerController { get { Init(); return s_PlayerManager; } }


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
        }
    }
    #endregion
}


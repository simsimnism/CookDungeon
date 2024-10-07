using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Rendering;

public class PlayerManager 
{

    // 플레이어의 상태 관련 변수
    public bool isMove;
    public float Max_speed;
    public float speed = 3.0f;
    public Vector2 inputVec;
    public string upAinme = "PlyerUp";
    public string downAinme = "PlyerDown";
    public string rightAinme = "PlyerRight";
    public string LeftAinme = "PlyerLeft";
    public string deadAinme = "PlayerDead";

    //플레이어의 상태
    public string nowState;
    public int hp = 3;
    public string gameState;
    public bool inDamage = false;

    
    public float dashSpeedMultiplier = 2.0f; 
    public float dashDuration = 0.2f; 
    public float dashCooldown = 1.0f; 


    void Start()
    {

    }

    void PlayerStates() 
    {
        if (isMove)
        {
            nowState = gameState;
        }
        return;
    }


    public void Init()
    {
        //플레이어의 초기상태
        hp = 3;
        gameState = "playing";
        inDamage = false;
    }
}

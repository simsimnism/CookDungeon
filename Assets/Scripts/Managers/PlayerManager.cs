using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    // 플레이어에 대한 정보
    public float speed = 3.0f;
    public Vector2 inputVec;
    public string deadAinme = "PlayerDead";

    public int hp;
    public string gameState = "playing";
    public bool inDamage = false;

    public float dashSpeedMultiplier = 2.0f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1.0f;



    // 플레이어 수치 초기 설정
    public void Init()
    {
        hp = 3;
        gameState = "playing";
        inDamage = false;
    }
}

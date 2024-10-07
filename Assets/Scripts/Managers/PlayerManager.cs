using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager 
{

    // 플레이어의 상태 관련 변수
    public float speed = 3.0f;
    public Vector2 inputVec;
    public string upAinme = "PlyerUp";
    public string downAinme = "PlyerDown";
    public string rightAinme = "PlyerRight";
    public string LeftAinme = "PlyerLeft";
    public string deadAinme = "PlayerDead";

    public int hp = 3;
    public string gameState = "playing";
    public bool inDamage = false;

    
    public float dashSpeedMultiplier = 2.0f; // �뽬 �� �ӵ� ���
    public float dashDuration = 0.2f; // �뽬 ���� �ð�
    public float dashCooldown = 1.0f; // �뽬 ��Ÿ��

    public void Init()
    {
        //플레이어의 초기상태
        hp = 3;
        gameState = "playing";
        inDamage = false;
    }
}

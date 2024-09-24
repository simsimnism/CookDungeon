using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static PlayerManager Instance { get; private set; }

    // 플레이어 이동 및 애니메이션 변수
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

    // 대쉬 관련 변수
    public float dashSpeedMultiplier = 2.0f; // 대쉬 시 속도 배수
    public float dashDuration = 0.2f; // 대쉬 지속 시간
    public float dashCooldown = 1.0f; // 대쉬 쿨타임

    void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 파괴되지 않음
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Init()
    {
        // 초기화 작업
        hp = 3;
        gameState = "playing";
        inDamage = false;
    }
}

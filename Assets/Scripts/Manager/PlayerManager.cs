using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager:MonoBehaviour
{

    // �̱��� �ν��Ͻ�
    public static PlayerManager Instance { get; private set; }

    // �÷��̾� �̵� �� �ִϸ��̼� ����
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

    // �뽬 ���� ����
    public float dashSpeedMultiplier = 2.0f; // �뽬 �� �ӵ� ���
    public float dashDuration = 0.2f; // �뽬 ���� �ð�
    public float dashCooldown = 0.5f; // �뽬 ��Ÿ��

    void Awake()

    {
        // �̱��� �ν��Ͻ� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ���� �ٲ� �ı����� ����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Init()
    {
        // �ʱ�ȭ �۾�
        hp = 3;
        gameState = "playing";
        inDamage = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 이동
    [HideInInspector] public PlayerController playerController;


    // 플레이어 생성
    public void Initialize()
    {
        playerController = GetComponent<PlayerController>();
    }
}

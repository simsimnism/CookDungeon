using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //이동
    [HideInInspector] public PlayerController playerController;
    private bool canMove = true;

    public void DisableMovement()
    {
        canMove = false;
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    void Update()
    {
        if (canMove)
        {
            // 플레이어의 이동 로직 처리
        }
    }
    
    // 플레이어 생성
    public void Initialize()
    {
        playerController = GetComponent<PlayerController>();
    }
}

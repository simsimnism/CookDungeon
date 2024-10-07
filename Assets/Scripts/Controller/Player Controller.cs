using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public string gameState;
    public float moveSpeed;  //움직임 속도

    void Awake()
    {
        gameState = Managers.Player.gameState;
        
    }

    private void Start()
    {

        Managers.Input.KeyAction -= OnKeyMove;
        Managers.Input.KeyAction += OnKeyMove;
    }


    //wasd로 상하좌우 이동&shift키로 달리기(달리기 수정해야 함)
    private void OnKeyMove()
    {
        if (!Managers.GM.IsMoving)
            return;

        float moveVertical = 0;
        float moveHorizontal = 0;

        if (Input.GetKey(KeyCode.W)) moveVertical = 1f;

        if (Input.GetKey(KeyCode.A))
        {
            moveHorizontal = -1f;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (Input.GetKey(KeyCode.S))
            moveVertical = -1f;

        if (Input.GetKey(KeyCode.D))
        {
            moveHorizontal = -1f;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) moveVertical = 0;
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) moveHorizontal = 0;

        if (moveVertical == 0 && moveHorizontal == 0)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, 0, Time.deltaTime * 10f);
            return;
        }

        Vector2 Direction = new Vector2(moveHorizontal, moveVertical).normalized;

        transform.Translate(Direction * moveSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = Mathf.Lerp(moveSpeed, 4, Time.deltaTime * 15f);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, 2, Time.deltaTime * 15f);
        }
    }
}

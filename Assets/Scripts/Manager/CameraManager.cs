using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 캐릭터를 중심으로 추적하는 카메라
public class CameraManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        if (Player != null )
        {
            //플레이어의 위치와 연동
            transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, -10);
        }
    }
}

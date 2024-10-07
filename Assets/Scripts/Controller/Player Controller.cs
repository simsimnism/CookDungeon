using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// 플레이어의 이동, 애니메이션, 대미지 처리, 대쉬를 담당하는 스크립트
public class PlayerController : MonoBehaviour
{
    // 이동 및 애니메이션 관련 변수들
    private float drag = 1f;
    private float decelerationFactor;
    private float Max_speed;
    private float _speed; // 현재 플레이어의 이동 속도
    Rigidbody2D rbody; // 2D 물리 엔진 처리를 위한 Rigidbody2D 컴포넌트
    bool isMoving = false; // 플레이어가 현재 이동 중인지 여부

    // PlayerManager 싱글톤 참조
    private PlayerManager pm;
    private SpriteRenderer sr; // 플레이어의 SpriteRenderer 컴포넌트 참조 (투명도 조절 용도)

    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>(); // Rigidbody2D 컴포넌트 가져오기
        sr = GetComponent<SpriteRenderer>(); // SpriteRmienderer 컴포넌트 가져오기
        rbody.drag = drag;
        pm = Managers.Player; // PlayerManager 싱글톤 인스턴스 가져오기
        _speed = pm.speed;
        Max_speed = pm.Max_speed;
        decelerationFactor = pm.decelerationFactor;
    }

    // 초기화
    void Start()
    {
        //두번 누르는거 방지
        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard;

    }

    // 매 프레임마다 호출되는 업데이트 함수
    void Update()
    {

    }

    //키보드 인풋
    void OnKeyboard()
    {
        Vector2 force = Vector2.zero; // 힘을 초기화

        // 이동을 위한 키 입력 처리
        if (Input.GetKey(KeyCode.W))
        {
            force += Vector2.up;
        }

        if (Input.GetKey(KeyCode.S))
        {
            force += Vector2.down;
        }

        if (Input.GetKey(KeyCode.A))
        {
            force += Vector2.left;
        }

        if (Input.GetKey(KeyCode.D))
        {
            force += Vector2.right;
        }

        // 키가 눌린 경우에만 힘을 가함
        if (force != Vector2.zero)
        {
            // AddForce로 힘을 가하여 캐릭터를 이동시킴
            rbody.AddForce(force * _speed);

            // 속도를 제한함
            if (rbody.velocity.magnitude > Max_speed)
            {
                rbody.velocity = rbody.velocity.normalized * Max_speed;
            }
            else
            {
                // 키 입력이 없을 때 더 빠른 감속 처리
                rbody.velocity = Vector2.Lerp(rbody.velocity, Vector2.zero, Time.deltaTime * decelerationFactor);
            }
        }
    }
}





  

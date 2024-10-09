using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using Cook.InventorySystem;
using static DG.Tweening.DOTweenAnimation;


//플레이어의 상태를 나타내는 코드(수정해야 함)
public class Player : MonoBehaviour
{

    //다른 컴포넌트 참조
    public PlayerController pc;
    public PlayerAttack pa;

    // 플레이어 이동 관련 변수(수정해야 함)
    private float speed; //
    private bool dashCooldownAction = false;
    private bool isPaused = false;
    public bool isDashing = false;
    
    private PlayerManager pm;
    private SpriteRenderer sr;
    
    GameObject player;
    Rigidbody2D rbody;

    bool isMoving = false;

    //플레이어 공격 관련 변수(수정해야 함)
    public Weapon weapon; //플레이어가 사용할 무기
    public Transform attackPoint;//공격이 발생할 위치
    public float comboResetTime = 1.0f;//콤보가 초기화되는 시간
    private float comboStep = 0;//현재 콤보 단계
    private float lastClickTime = 0;// 마지막 공격시간


    void Awake()
    {
        pm = Managers.Player;
        speed = Managers.Player.speed;
        
    }
    void Start()
    {
    }

    void Update()
    {
        
    }


    // 캐릭터의 투명도를 설정하는 메서드
    private void SetTransparency(float alpha)
    {
        Color color = sr.color;
        color.a = alpha; // 알파 값을 변경하여 투명도 설정
        sr.color = color;
    }

    //충돌관련 함수
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && !isDashing) // 적과 충돌하고 대쉬 중이 아니면
        {
            GetDamage(collision.gameObject); // 대미지 처리
        }
    }

    //플레이어가 데미지를 받는 함수
    void GetDamage(GameObject enemy)
    {
        if (pm.gameState == "playing")
        {
            pm.hp--; // 플레이어 HP 감소
            if (pm.hp > 0)
            {
                rbody.velocity = Vector2.zero; // 이동 속도 초기화
                Vector3 toPos = (transform.position - enemy.transform.position).normalized;
                rbody.AddForce(new Vector2(toPos.x * 4, toPos.y * 4), ForceMode2D.Impulse); // 적 방향 반대로 밀어냄

                pm.inDamage = true; // 대미지 상태로 전환
                Invoke("DamageEnd", 0.25f); // 일정 시간 후에 대미지 상태 종료
            }
            else
            {
                GameOver(); // HP가 0이면 게임 오버 처리
            }
        }
    }
    
    //플레이어의 데미지를 멈추는 함수
    void DamageEnd()
    {
        pm.inDamage = false; // 대미지 상태 해제
        gameObject.GetComponent<SpriteRenderer>().enabled = true; // 플레이어를 보이게 함
    }

    // 게임오버 시의 함수
    void GameOver()
    {
    pm.gameState = "gameover"; // 게임 상태를 'gameover'로 설정
    GetComponent<CircleCollider2D>().enabled = false; // 충돌 비활성화
    rbody.velocity = Vector2.zero;
    rbody.gravityScale = 1; // 중력 적용
    rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse); // 위로 튕겨나가는 효과
    Destroy(gameObject, 1.0f); // 1초 후 오브젝트 제거
    }
}

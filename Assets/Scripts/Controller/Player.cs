using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using Cook.InventorySystem;
using static DG.Tweening.DOTweenAnimation;

public class Player : MonoBehaviour
{

    //싱글톤 인스턴스
    public Player Instance {  get; private set; }

    //다른 컴포넌트 참조
    public PlayerController pc;
    public PlayerAttack pa;

    // 플레이어 이동 관련 변수
    private float speed;
    
    private bool gameState = false; 
    private bool dashCooldownAction = false;
    private bool isPaused = false;
    
    private PlayerManager pm;
    private SpriteRenderer sr;
    
    GameObject player;
    Rigidbody2D rbody;

    float axisH; // 수평 입력 값 (왼쪽/오른쪽)
    float axisV; // 수직 입력 값 (위/아래)

    bool isMoving = false;

    //플레이어 공격 관련 변수
    public Weapon weapon; //플레이어가 사용할 무기
    public Transform attackPoint;//공격이 발생할 위치
    public float comboResetTime = 1.0f;//콤보가 초기화되는 시간
    private float comboStep = 0;//현재 콤보 단계
    private float lastClickTime = 0;// 마지막 공격시간


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject );
            return;
        }
        Instance = this;
        DontDestroyOnLoad( gameObject );
        pm = Managers.Player;
        speed = Managers.Player.speed;
        
    }
    void Start()
    {
    }

    void Update()
    {
        
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

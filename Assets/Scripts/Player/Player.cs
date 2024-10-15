using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 이동
    [HideInInspector] public PlayerController playerController;
    public float invincibilityDuration = 0.7f; // 무적 상태 지속 시간
    public bool invin;
    public bool isDashing;

    //Hp바와 연동
    private HpBar hpBar;

    // Pm에서 관리
    private int MaxHP;
    private int currentHP;// 현재 체력
    private bool inDamage;
    public float moveSpeed;
    private Vector2 dashDiretion;
    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider; // 플레이어의 Collider2D 참조

    Rigidbody2D rb;

    void Awake()
    {
        MaxHP = Managers.Player.MaxHP;
        inDamage = Managers.Player.inDamage;
        invin = Managers.GM.IsInvincible;
        isDashing = GetComponent<PlayerAttack>();
        moveSpeed = Managers.Player.moveSpeed;
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer 참조 가져오기
        playerCollider = GetComponent<Collider2D>(); // Collider2D 참조 가져오기
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        currentHP = MaxHP;

        // 체력바 초기화
        hpBar = FindObjectOfType<HpBar>(); // UIManager는 그대로 두고 직접 찾습니다.
        if (hpBar != null)
        {
            hpBar.Initialize(MaxHP);
        }

    }

    void Update()
    {
        // 쉬프트 키를 누르면 무적 상태 활성화
        if (Input.GetKey(KeyCode.LeftShift) && !invin)
        {
            StartCoroutine(StartInvincibility());
        }
        if (isDashing && IsTouchingWallLayer())
        {
            StopDash();
        }
    }

    // 대쉬를 중단하는 로직
    void StopDash()
    {
        isDashing = false; // 대쉬 상태 중단
        rb.velocity = Vector2.zero; // 속도 멈춤
    }

    // 플레이어 생성
    public void Initialize()
    {
        playerController = GetComponent<PlayerController>();
    }

    // 플레이어가 데미지를 입는 로직
    public void TakeDamage(int damage)
    {
        if (!invin) // 무적 상태가 아닐 때만 데미지 입음
        {
            currentHP -= damage;
            if (currentHP < 0)
                currentHP = 0;

            if (hpBar != null)
            {
                hpBar.UpdateHealth(currentHP);
            }

            // 데미지를 입으면 무적 상태 시작
            StartCoroutine(StartInvincibility());
        }
    }

    public void Heal(int damage)
    {
        currentHP += damage;
        if (currentHP > MaxHP)
            currentHP = MaxHP;

        if (hpBar != null)
        {
            hpBar.UpdateHealth(currentHP);
        }
    }

    IEnumerator StartInvincibility()
    {
        invin = true;
        if (isDashing)
        {
            invin = true;
        }

        // 플레이어 충돌 비활성화
        playerCollider.enabled = false;

        // 무적 상태 동안 반투명 효과 적용
        float timer = 0f;
        Color originalColor = spriteRenderer.color; // 원래 색상 저장
        while (timer < invincibilityDuration)
        {
            // 반투명 효과 적용 (알파 값 변경)
            Color transparentColor = originalColor;
            transparentColor.a = 0.5f; // 알파 값 0.5로 설정하여 반투명하게
            spriteRenderer.color = transparentColor;

            yield return new WaitForSeconds(0.4f);

            // 다시 원래 상태로 복원
            spriteRenderer.color = originalColor;

            yield return new WaitForSeconds(0.4f);
            timer += 0.8f; // 전체 주기 0.8초마다 반복
        }

        // 무적 상태 해제 및 색상, 충돌 원래대로 복원
        spriteRenderer.color = originalColor;

        // 충돌을 다시 활성화하기 전에 벽 레이어를 감지
        if (IsTouchingWallLayer())
        {
            playerCollider.enabled = true; // 벽 레이어를 감지하면 충돌 다시 활성화
        }
        invin = false;
    }
    // 벽 레이어를 감지하는 함수
    bool IsTouchingWallLayer()
    {
        // 벽에 해당하는 레이어가 있다고 가정 (예: 레이어 8)
        int wallLayer = LayerMask.NameToLayer("Wall");
        LayerMask wallLayerMask = 1 << wallLayer;

        // 벽 레이어에 해당하는 물체와의 충돌을 감지
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dashDiretion, 0.5f, wallLayerMask); // 대쉬 방향으로 벽 감지
        if (hit.collider != null && hit.collider.gameObject.layer == wallLayer)
        {
            return true; // 벽 레이어에 닿았으면 true 반환
        }
        return false;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌 후에 속도를 0으로 설정하여 움직임을 막음
        rb.velocity = Vector2.zero;
    }

    public void DisableMovement()
    {
        Managers.GM.IsMoving = false;
    }

    public void EnableMovement()
    {
        Managers.GM.IsMoving = true;
    }

    void Die()
    {
        // 사망 로직
    }
}

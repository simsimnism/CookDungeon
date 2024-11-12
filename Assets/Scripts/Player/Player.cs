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

    // HP 바와 연동
    private HpBar hpBar;

    // Pm에서 관리
    private int MaxHP;
    private int currentHP; // 현재 체력
    private bool inDamage;
    public float moveSpeed;
    private Vector2 dashDiretion;
    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider; // 플레이어의 Collider2D 참조
    private Rigidbody2D rb;

    // 게임 오버 패널 추가
    public GameObject gameOverPanel; // 게임 오버 UI 패널 연결

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

        // 게임 오버 패널을 태그로 동적으로 찾음
        gameOverPanel = GameObject.FindWithTag("GameOverPanel");
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false); // 게임 오버 패널을 비활성화 상태로 시작
        }
    }

    void Update()
    {
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

            // 체력이 0이 되었을 때 죽는 로직
            if (currentHP <= 0)
            {
                Die(); // 사망 함수 호출
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
        playerCollider.enabled = true; // 충돌 다시 활성화
        invin = false;
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

    // 사망 로직
    void Die()
    {
        // 게임 오버 패널 활성화
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        // 시간 멈춤
        Time.timeScale = 0f;
    }
}

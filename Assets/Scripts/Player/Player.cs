using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D), typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerController playerController;
    public float invincibilityDuration = 0.7f; // 무적 상태 지속 시간
    public bool invin; // 무적 상태 플래그
    public bool isDashing;

    // HP 바와 연동
    private HpBar hpBar; // HP 바 인스턴스

    // 플레이어의 체력 관련 변수
    public int defaultHealAmount = 3; // 기본 회복량
    private int MaxHP;
    private int currentHP;
    private bool inDamage;
    public float moveSpeed;
    private Vector2 dashDirection;
    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider;
    private Rigidbody2D rb;

    // 게임 오버 패널
    public GameObject gameOverPanel;

    void Awake()
    {
        MaxHP = Managers.Player.MaxHP;
        inDamage = Managers.Player.inDamage;
        invin = Managers.GM.IsInvincible;
        isDashing = GetComponent<PlayerAttack>();
        moveSpeed = Managers.Player.moveSpeed;
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // 4초 후에 게임 클리어 UI 팝업을 띄우는 코루틴 시작
        StartCoroutine(SpawnGameClearPopup());
        StartCoroutine(GameClearPopup());
        currentHP = MaxHP;

        // 체력바 초기화
        hpBar = FindObjectOfType<HpBar>(); // UIManager는 그대로 두고 직접 찾습니다.
        if (hpBar != null)
        {
            hpBar.Initialize(MaxHP);
        }
    }
    private IEnumerator SpawnGameClearPopup()
    {
        yield return new WaitForSeconds(2f);

        // Managers.Popup을 통해 게임 클리어 팝업 호출
        Managers.Popup.OpenGameClear();
    }

    private IEnumerator GameClearPopup()
    {
        yield return new WaitForSeconds(2.01f);

        // Managers.Popup을 통해 게임 클리어 팝업 호출
        Managers.Popup.CloseGameClear();
    }



    // 플레이어가 데미지를 입는 로직
    public void TakeDamage(int damage)
    {
        if (!invin) // 무적 상태가 아닐 때만 데미지를 입음
        {
            currentHP -= damage;
            if (currentHP < 0)
                currentHP = 0;

            if (hpBar != null)
            {
                hpBar.UpdateHealth(currentHP);
            }
            // 체력이 0 이하가 되면 사망 처리
            if (currentHP <= 0)
            {
                Die();
                Managers.Popup.OpenGameEndUI();
            }

            // 데미지를 입은 후 무적 상태 시작
            StartCoroutine(StartInvincibility());
        }
    }

    // 체력 회복 로직
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

    // 무적 상태 시작 코루틴
    IEnumerator StartInvincibility()
    {
        invin = true;
        playerCollider.enabled = false;

        float timer = 0f;
        Color originalColor = spriteRenderer.color;
        while (timer < invincibilityDuration)
        {
            Color transparentColor = originalColor;
            transparentColor.a = 0.5f;
            spriteRenderer.color = transparentColor;

            yield return new WaitForSeconds(0.4f);

            spriteRenderer.color = originalColor;

            yield return new WaitForSeconds(0.4f);
            timer += 0.8f;
        }

        spriteRenderer.color = originalColor;
        playerCollider.enabled = true;
        invin = false;
    }

    // 체력을 기본 회복량만큼 회복시키는 함수
    public void RecoverHealth()
    {
        RecoverHealth(defaultHealAmount);
    }

    // 체력을 지정된 양만큼 회복시키는 함수
    // 체력을 회복시키는 함수
    public void RecoverHealth(int healAmount)
    {
        currentHP += healAmount;

        // 체력이 최대 체력을 초과하지 않도록 제한
        if (currentHP > MaxHP)
        {
            currentHP = MaxHP;
        }

        // 체력바 업데이트
        if (hpBar != null)
        {
            hpBar.UpdateHealth(currentHP);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.velocity = Vector2.zero; // 충돌 후 속도 0으로 설정
    }

    public void DisableMovement()
    {
        Managers.GM.IsMoving = false;
    }

    public void EnableMovement()
    {
        Managers.GM.IsMoving = true;
    }

    // 사망 처리
    public void Die()
    {
        Managers.GM.gameState = GameState.restartGame;
    }
}

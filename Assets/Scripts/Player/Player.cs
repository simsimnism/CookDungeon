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
    private HpBar hpBarInstance; // HP 바 인스턴스

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
        // HP 바를 지연하여 생성하는 코루틴 시작
        StartCoroutine(DelayedHpBarPopup());
        currentHP = MaxHP;

        // HP 바가 존재할 경우 초기화
        if (hpBarInstance != null)
        {
            hpBarInstance.Initialize(MaxHP);
        }
    }

    IEnumerator DelayedHpBarPopup()
    {
        // 0.5초 정도 지연 후 HP 바 생성 (필요 시 조정)
        yield return new WaitForSeconds(2f);

      

        // 생성된 HP 바를 검색하여 HpBar 인스턴스로 설정
        hpBarInstance = FindObjectOfType<HpBar>();

        if (hpBarInstance != null)
        {
            hpBarInstance.Initialize(MaxHP); // HP 바 초기화
        }
        else
        {
            Debug.LogError("HP 바를 찾을 수 없습니다.");
        }
    }

    // 플레이어가 데미지를 입는 로직
    public void TakeDamage(int damage)
    {
        if (!invin) // 무적 상태가 아닐 때만 데미지를 입음
        {
            currentHP -= damage;
            if (currentHP < 0)
                currentHP = 0;

            if (hpBarInstance != null)
            {
                hpBarInstance.UpdateHealth(currentHP);
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
    public void Heal(int healAmount)
    {
        currentHP += healAmount;
        if (currentHP > MaxHP)
            currentHP = MaxHP;

        if (hpBarInstance != null)
        {
            hpBarInstance.UpdateHealth(currentHP);
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
    public void RecoverHealth(int healAmount)
    {
        currentHP += healAmount;

        if (currentHP > MaxHP)
        {
            currentHP = MaxHP;
        }

        if (hpBarInstance != null)
        {
            hpBarInstance.UpdateHealth(currentHP);
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

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D), typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private HpBar hpBar; // HP 바 인스턴스

    [HideInInspector] public PlayerController playerController;
    public float invincibilityDuration = 0.7f; // 무적 상태 지속 시간
    public bool invin; // 무적 상태 플래그
    public bool isDashing;

    public LayerMask wallLayer; // "Wall" 레이어 설정

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
        moveSpeed = Managers.Player.MoveSpeed;
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
        if (hpBar != null)
        {
            hpBar.SetHealthBarValue((float)currentHP / (float)MaxHP);
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
                hpBar.SetHealthBarValue((float)currentHP / (float)MaxHP);
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
            hpBar.SetHealthBarValue((float)currentHP / (float)MaxHP);
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
            hpBar.SetHealthBarValue((float)currentHP / (float)MaxHP);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 객체가 "Wall" 레이어인지 확인
        if (((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            rb.velocity = Vector2.zero; // 이동 속도를 0으로 설정하여 멈추기
            rb.angularVelocity = 0f;

            // 충돌 상대방의 BoxCollider2D 정보 가져오기
            BoxCollider2D wallCollider = collision.collider as BoxCollider2D;
            if (wallCollider != null)
            {
                // 충돌 지점 계산
                Vector2 collisionPoint = collision.contacts[0].point;
                Vector2 wallCenter = wallCollider.bounds.center;
                Vector2 wallSize = wallCollider.bounds.size;

                // 플레이어의 위치 조정 (충돌 지점에서 조금 떨어지도록)
                if (collisionPoint.x < wallCenter.x - wallSize.x / 2) // 왼쪽에서 충돌
                {
                    transform.position = new Vector3(wallCenter.x - wallSize.x / 2 - 0.1f, transform.position.y, transform.position.z);
                }
                else if (collisionPoint.x > wallCenter.x + wallSize.x / 2) // 오른쪽에서 충돌
                {
                    transform.position = new Vector3(wallCenter.x + wallSize.x / 2 + 0.1f, transform.position.y, transform.position.z);
                }
                else if (collisionPoint.y < wallCenter.y - wallSize.y / 2) // 아래쪽에서 충돌
                {
                    transform.position = new Vector3(transform.position.x, wallCenter.y - wallSize.y / 2 - 0.1f, transform.position.z);
                }
                else if (collisionPoint.y > wallCenter.y + wallSize.y / 2) // 위쪽에서 충돌
                {
                    transform.position = new Vector3(transform.position.x, wallCenter.y + wallSize.y / 2 + 0.1f, transform.position.z);
                }
            }
        }
    }


    private IEnumerator ReenableRigidbody()
    {
        yield return new WaitForSeconds(0.1f); // 잠시 대기 후
        rb.bodyType = RigidbodyType2D.Dynamic; // Rigidbody를 Dynamic으로 복원하여 이동 가능하게 함
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

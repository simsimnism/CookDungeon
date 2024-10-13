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

    // Pm에서 관리
    private int Hp;
    private bool inDamage;
    public float moveSpeed;
    private Vector2 dashDiretion;
    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider; // 플레이어의 Collider2D 참조

    void Awake()
    {
        Hp = Managers.Player.hp;
        inDamage = Managers.Player.inDamage;
        invin = Managers.GM.IsInvincible;
        isDashing = GetComponent<PlayerAttack>();
        moveSpeed = Managers.Player.moveSpeed;
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer 참조 가져오기
        playerCollider = GetComponent<Collider2D>(); // Collider2D 참조 가져오기
    }

    void Update()
    {
        // 쉬프트 키를 누르면 무적 상태 활성화
        if (Input.GetKey(KeyCode.LeftShift) && !invin)
        {
            StartCoroutine(StartInvincibility());
        }
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
            Hp -= damage;
            Debug.Log("Player took damage: " + damage + " | Health: " + Hp);

            // 데미지를 입으면 무적 상태 시작
            StartCoroutine(StartInvincibility());
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

    void Die()
    {
        // 사망 로직
    }
}
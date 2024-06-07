using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//무적상태 지속시간 줄인것 이 시간 늘릴려면 이야기 해줘
// 이 스크립트 적용할려면 Player에 Rigidbody2D 컴포넌트 추가해야 함
public class PlayerController : MonoBehaviour
{
    public Vector2 inputVec;  // 플레이어 입력 벡터
    private Rigidbody2D rigid;  // 플레이어의 Rigidbody2D 컴포넌트
    public float speed = 3f;  // 일반 이동 속도
    public float dashSpeed = 8f;  // 대쉬 시 이동 속도
    public float dashDuration = 0.2f;  // 대쉬 지속 시간
    public float invincibilityDuration = 0.5f;  // 무적 상태 지속 시간
    public Color invincibleColor = new Color(1f, 1f, 1f, 0.5f);  // 무적 상태 동안 플레이어의 색상
    private Color originalColor;  // 플레이어의 원래 색상
    private SpriteRenderer spriteRenderer;  // 플레이어의 SpriteRenderer 컴포넌트
    private bool isDashing = false;  // 대쉬 중인지 여부
    private bool isInvincible = false;  // 무적 상태인지 여부

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();  // Rigidbody2D 컴포넌트 가져오기
        spriteRenderer = GetComponent<SpriteRenderer>();  // SpriteRenderer 컴포넌트 가져오기
        originalColor = spriteRenderer.color;  // 원래 색상 저장
    }

    void Update()
    {
        HandleInput();  // 플레이어 입력 처리

        // 왼쪽 Shift 키를 눌렀을 때 대쉬 시작
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            StartCoroutine(Dash());  // 대쉬 코루틴 시작
        }
    }

    void FixedUpdate()
    {
        // 대쉬 중이 아닐 때만 이동
        if (!isDashing)
        {
            Move();
        }
    }

    void HandleInput()
    {
        // 플레이어의 입력을 받아 inputVec에 저장
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    void Move()
    {
        // 입력에 따라 플레이어 이동
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    IEnumerator Dash()
    {
        isDashing = true;  // 대쉬 상태로 설정
        StartCoroutine(SetInvincibility(true));  // 대쉬 시작 시 무적 상태 시작

        Vector2 dashVec = inputVec.normalized * dashSpeed;  // 대쉬 속도 설정
        float dashTime = 0f;

        // 대쉬 지속 시간 동안 이동
        while (dashTime < dashDuration)
        {
            rigid.MovePosition(rigid.position + dashVec * Time.fixedDeltaTime);  // 대쉬 이동
            dashTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();  // FixedUpdate 후에 실행
        }

        isDashing = false;  // 대쉬 상태 해제
        StartCoroutine(SetInvincibility(false));  // 대쉬 종료 시 무적 상태 해제
    }

    IEnumerator SetInvincibility(bool state)
    {
        isInvincible = state;  // 무적 상태 설정
        spriteRenderer.color = state ? invincibleColor : originalColor;  // 색상 변경

        yield return new WaitForSeconds(invincibilityDuration);  // 무적 상태 지속 시간 대기

        if (!state)
        {
            isInvincible = false;  // 무적 상태 해제
            spriteRenderer.color = originalColor;  // 원래 색상으로 복원
        }
    }
}
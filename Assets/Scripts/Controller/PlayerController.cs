using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public string gameState;
    public float moveSpeed;  // 움직임 속도
    public float dashSpeed = 6f; // 대쉬 속도
    public float dashDuration = 0.5f; // 대쉬 지속 시간
    public float invincibleDuration = 1f; // 무적 지속 시간
    private bool isDashing = false; // 대쉬 중인지 여부
    private bool isInvincible = false; // 무적인지 여부
    private SpriteRenderer spriteRenderer; // 반투명 상태를 위한 SpriteRenderer

    private Vector2 dashDirection; // 대쉬 방향을 저장할 변수
    private Coroutine dashCoroutine;

    void Awake()
    {
        gameState = Managers.Player.gameState;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Managers.Input.KeyAction -= OnKeyMove;
        Managers.Input.KeyAction += OnKeyMove;
    }

    // WASD로 상하좌우 이동 & Shift키로 대쉬와 무적 상태
    private void OnKeyMove()
    {
        if (!Managers.GM.IsMoving)
            return;

        float moveVertical = 0;
        float moveHorizontal = 0;

        if (Input.GetKey(KeyCode.W)) moveVertical = 1f;

        if (Input.GetKey(KeyCode.A))
        {
            moveHorizontal = -1f;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (Input.GetKey(KeyCode.S)) moveVertical = -1f;

        if (Input.GetKey(KeyCode.D))
        {
            moveHorizontal = -1f; // 여기서 방향 오류 수정
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) moveVertical = 0;
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) moveHorizontal = 0;

        // 대각선 방향 조정
        Vector2 direction = new Vector2(moveHorizontal, moveVertical).normalized;

        // 대쉬 중이 아닐 때만 이동 방향 갱신
        if (!isDashing)
        {
            moveSpeed = 10f;
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            // 대쉬를 시작할 때 이동 방향을 저장
            dashDirection = direction;

            if (dashCoroutine != null)
            {
                StopCoroutine(dashCoroutine);
            }
            dashCoroutine = StartCoroutine(Dash());
        }
        else if (!isDashing)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, 2, Time.deltaTime * 15f);
        }
    }

    // 대쉬와 무적 상태를 관리하는 코루틴
    private IEnumerator Dash()
    {
        isDashing = true;
        isInvincible = true;
        float originalSpeed = moveSpeed;

        StartCoroutine(BecomeInvincible());

        // 대쉬 동안 속도를 빠르게 하고, 저장된 방향으로 대쉬
        float elapsedTime = 0f;

        while (elapsedTime < dashDuration)
        {
            transform.Translate(dashDirection * dashSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 대쉬 후 다시 원래 속도로 복구
        moveSpeed = originalSpeed;
        isDashing = false;
        dashCoroutine = null;
    }

    // 무적 상태와 반투명 상태를 관리하는 코루틴
    private IEnumerator BecomeInvincible()
    {
        Color originalColor = spriteRenderer.color;

        // 반투명 상태로 변경
        Color transparentColor = originalColor;
        transparentColor.a = 0.5f; // 투명도 조정
        spriteRenderer.color = transparentColor;

        yield return new WaitForSeconds(invincibleDuration);

        // 원래 상태로 복구
        spriteRenderer.color = originalColor;
        isInvincible = false;
    }

    // 무적 상태 확인 (이 함수로 외부에서 확인 가능)
    public bool IsInvincible()
    {
        return isInvincible;
    }
}

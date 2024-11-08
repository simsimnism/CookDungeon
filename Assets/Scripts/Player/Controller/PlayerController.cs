using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //아직 사용 안함
    public string gameState;

    //Pm에서 사용할 변수
    public float moveSpeed;  // 움직임 속도
    public float dashSpeed; // 대쉬 속도
    public float dashDuration; // 대쉬 지속 시간
    private bool isDashing; // 대쉬 중인지 여부
    private float transparencyFadeTime; // 투명화가 진행되는 시간
    private float transparencyHoldTime; // 투명화가 완료된 후 유지되는 시간

    //GM에서 관리할 변수
    private bool isInvincible; // 무적인지 여부

    //각 스크립트에서 직접 관리?
    public float invincibleDuration = 1f; // 무적 지속 시간

    private SpriteRenderer spriteRenderer; // 반투명 상태를 위한 SpriteRenderer
    private Vector2 dashDirection; // 대쉬 방향을 저장할 변수
    private Coroutine dashCoroutine;
    private Coroutine transparencyCoroutine;

    // 아이템 습득 관련 변수
    private PickupItem currentPickupItem; // 현재 범위 내 아이템 참조

    void Awake()
    {
        transparencyHoldTime = Managers.Player.transparencyHoldTime;
        transparencyFadeTime = Managers.Player.transparencyFadeTime;
        isInvincible = Managers.GM.IsInvincible;
        isDashing = Managers.Player.isDashing;
        dashSpeed = Managers.Player.dashSpeed;
        moveSpeed = Managers.Player.moveSpeed;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Managers.Player.Init();
        Managers.Input.KeyAction -= OnKeyMove;
        Managers.Input.KeyAction += OnKeyMove;
    }

    // WASD로 상하좌우 이동 & Shift키로 대쉬와 무적 상태 & F키로 아이템 습득
    private void OnKeyMove()
    {
        // 이동 가능 여부 체크
        if (!Managers.Player.canMove)
            return;

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
            moveHorizontal = -1f; // 방향 오류 수정
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) moveVertical = 0;
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) moveHorizontal = 0;

        // 대각선 방향 조정
        Vector2 direction = new Vector2(moveHorizontal, moveVertical).normalized;

        // 대쉬 중이 아닐 때만 이동 방향 갱신
        if (!isDashing)
        {
            moveSpeed = Managers.Player.moveSpeed;
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            // 대쉬를 시작할 때 이동 방향을 저장
            dashDirection = direction;

            if (dashCoroutine != null || transparencyCoroutine != null)
            {
                StopAllCoroutines();
            }
            dashCoroutine = StartCoroutine(Dash());
            transparencyCoroutine = StartCoroutine(Dash());
        }
        else if (!isDashing)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, 2, Time.deltaTime * 15f);
        }

        // F 키로 아이템 습득
        if (Input.GetKeyDown(KeyCode.F) && currentPickupItem != null)
        {
            currentPickupItem.Pickup(); // 아이템 습득
        }
    }

    // 대쉬와 무적 상태를 관리하는 코루틴
    private IEnumerator Dash()
    {
        if (Managers.Player.isAttacking)
            yield break;
        isDashing = true;
        isInvincible = true;
        float originalSpeed = moveSpeed;

        StartCoroutine(BecomeTransparent());
        StartCoroutine(BecomeTransparent());

        // 대쉬 동안 속도를 빠르게 하고, 저장된 방향으로 대쉬
        float elapsedTime = 0f;

        while (elapsedTime < dashDuration)
        {
            dashSpeed = Managers.Player.dashSpeed;
            transform.Translate(dashDirection * dashSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 대쉬 후 다시 원래 속도로 복구
        moveSpeed = originalSpeed;
        isDashing = false;
        dashCoroutine = null;
    }

    // 무적 상태를 관리하는 코루틴
    private IEnumerator BecomeInvincible()
    {
        yield return new WaitForSeconds(invincibleDuration);

        //무적상태 종료
        isInvincible = false;
    }

    // 투명화 상태를 관리하는 코루틴
    public IEnumerator BecomeTransparent()
    {
        Color originalColor = Color.white;

        // 투명화 시작
        float elapsedTime = 0f;
        while (elapsedTime < transparencyFadeTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0.5f, elapsedTime / transparencyFadeTime); // 투명도를 점진적으로 변경
            Color transparentColor = originalColor;
            transparentColor.a = alpha;
            spriteRenderer.color = transparentColor;
            yield return null; // 다음 프레임까지 대기
        }

        // 투명 상태 유지
        yield return new WaitForSeconds(transparencyHoldTime);

        // 원래 상태로 복구
        elapsedTime = 0f;
        while (elapsedTime < transparencyFadeTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0.5f, 1f, elapsedTime / transparencyFadeTime); // 투명도를 원래대로 복구
            Color transparentColor = originalColor;
            transparentColor.a = alpha;
            spriteRenderer.color = transparentColor;
            yield return null; // 다음 프레임까지 대기
        }
    }

    // 무적 상태 확인 (이 함수로 외부에서 확인 가능)
    public bool IsInvincible()
    {
        return isInvincible;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 PickupItem 범위에 들어왔을 때
        if (other.CompareTag("PickupItem"))
        {
            currentPickupItem = other.GetComponent<PickupItem>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 플레이어가 PickupItem 범위를 벗어났을 때
        if (other.CompareTag("PickupItem"))
        {
            currentPickupItem = null;
        }
    }
}

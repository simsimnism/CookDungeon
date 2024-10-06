using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어의 이동, 애니메이션, 대미지 처리, 대쉬를 담당하는 스크립트
public class PlayerController : MonoBehaviour
{
    // 이동 및 애니메이션 관련 변수들
    private float speed; // 현재 플레이어의 이동 속도
    private string upAinme; // 위쪽 이동 애니메이션 이름
    private string downAinme; // 아래쪽 이동 애니메이션 이름
    private string rightAinme; // 오른쪽 이동 애니메이션 이름
    private string LeftAinme; // 왼쪽 이동 애니메이션 이름
    private string deadAinme; // 플레이어가 죽을 때 실행될 애니메이션 이름

    private string nowAnimation = ""; // 현재 실행 중인 애니메이션
    private string oldAnimation = ""; // 이전에 실행된 애니메이션

    // 이동 입력값
    float axisH; // 수평 입력 값 (왼쪽/오른쪽)
    float axisV; // 수직 입력 값 (위/아래)
    public float angleZ = -90.0f; // 플레이어의 이동 방향을 결정할 각도

    Rigidbody2D rbody; // 2D 물리 엔진 처리를 위한 Rigidbody2D 컴포넌트
    bool isMoving = false; // 플레이어가 현재 이동 중인지 여부

    // PlayerManager 싱글톤 참조
    private PlayerManager playerManager;
    private SpriteRenderer spriteRenderer; // 플레이어의 SpriteRenderer 컴포넌트 참조 (투명도 조절 용도)

    // 대쉬 관련 상태 변수
    private bool isDashing = false; // 현재 대쉬 중인지 여부
    private bool dashCooldownActive = false; // 대쉬 쿨타임이 활성화되어 있는지 여부

    // 초기화
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>(); // Rigidbody2D 컴포넌트 가져오기
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer 컴포넌트 가져오기

        // PlayerManager에서 필요한 변수를 가져옴
        speed = playerManager.speed;
        deadAinme = playerManager.deadAinme;
        oldAnimation = downAinme; // 초기 애니메이션은 아래쪽을 바라보는 것으로 설정
    }

    // 매 프레임마다 호출되는 업데이트 함수
    void Update()
    {
        // 게임이 진행 중이 아니거나 대미지 처리 중이거나 대쉬 중이면 아무것도 하지 않음
        if (playerManager.gameState != "playing" || playerManager.inDamage || isDashing)
        {
            return;
        }

        // 플레이어의 이동 입력을 받음
        if (!isMoving == false)
        {
            axisH = Input.GetAxisRaw("Horizontal"); // 좌우 입력 값 (-1, 0, 1)
            axisV = Input.GetAxisRaw("Vertical"); // 상하 입력 값 (-1, 0, 1)
        }

        // Shift 키를 누르면 대쉬를 시작
        if (Input.GetKeyDown(KeyCode.LeftShift) && !dashCooldownActive)
        {
            StartCoroutine(Dash()); // 대쉬 코루틴 시작
        }

        // 현재 위치에서 이동 방향을 계산
        Vector2 fromPt = transform.position;
        Vector2 toPt = new Vector2(fromPt.x + axisH, fromPt.y + axisV);
        angleZ = GetAngle(fromPt, toPt); // 두 점 사이의 각도를 계산

        // 각도에 따라 애니메이션을 변경
        if (angleZ >= -45 && angleZ < 45)
        {
            nowAnimation = rightAinme; // 오른쪽으로 이동
        }
        else if (angleZ >= 45 && angleZ <= 135)
        {
            nowAnimation = upAinme; // 위쪽으로 이동
        }
        else if (angleZ >= -135 && angleZ <= -45)
        {
            nowAnimation = downAinme; // 아래쪽으로 이동
        }
        else
        {
            nowAnimation = LeftAinme; // 왼쪽으로 이동
        }

        // 현재 애니메이션이 이전 애니메이션과 다를 경우 변경
        if (nowAnimation != oldAnimation)
        {
            oldAnimation = nowAnimation;
            GetComponent<Animator>().Play(nowAnimation); // 애니메이션 재생
        }
    }

    // 물리적인 동작을 처리하는 FixedUpdate 함수
    void FixedUpdate()
    {
        // 게임 상태가 진행 중이 아니거나 대쉬 중이면 실행하지 않음
        if (playerManager.gameState != "playing" || isDashing)
        {
            return;
        }

        // 대미지 처리 중일 때 깜빡이는 효과
        if (playerManager.inDamage)
        {
            float val = Mathf.Sin(Time.time * 50);
            gameObject.GetComponent<SpriteRenderer>().enabled = val > 0;
            return;
        }

        // Rigidbody2D를 이용해 플레이어를 이동시킴
        rbody.velocity = new Vector2(axisH, axisV) * speed;
    }

    // 대쉬 코루틴
    private IEnumerator Dash()
    {
        isDashing = true; // 대쉬 시작
        dashCooldownActive = true; // 대쉬 쿨타임 시작

        float originalSpeed = speed; // 원래 속도를 저장
        speed *= playerManager.dashSpeedMultiplier; // 대쉬 속도로 증가

        playerManager.inDamage = true; // 대쉬 중에는 무적 상태로 전환

        SetTransparency(0.5f); // 캐릭터를 반투명하게 설정

        yield return new WaitForSeconds(playerManager.dashDuration); // 대쉬 지속 시간만큼 기다림

        speed = originalSpeed; // 속도를 원래대로 복원

        playerManager.inDamage = false; // 무적 상태 해제

        SetTransparency(1.0f); // 투명도 원상복귀

        isDashing = false; // 대쉬 종료

        yield return new WaitForSeconds(playerManager.dashCooldown); // 대쉬 쿨타임 대기
        dashCooldownActive = false; // 대쉬 쿨타임 종료
    }

    // 캐릭터의 투명도를 설정하는 메서드
    private void SetTransparency(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha; // 알파 값을 변경하여 투명도 설정
        spriteRenderer.color = color;
    }

    // 적과의 충돌을 처리하는 함수
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && !isDashing) // 적과 충돌하고 대쉬 중이 아니면
        {
            GetDamage(collision.gameObject); // 대미지 처리
        }
    }

    // 플레이어가 대미지를 받을 때 처리
    void GetDamage(GameObject enemy)
    {
        if (playerManager.gameState == "playing")
        {
            playerManager.hp--; // 플레이어 HP 감소
            if (playerManager.hp > 0)
            {
                rbody.velocity = Vector2.zero; // 이동 속도 초기화
                Vector3 toPos = (transform.position - enemy.transform.position).normalized;
                rbody.AddForce(new Vector2(toPos.x * 4, toPos.y * 4), ForceMode2D.Impulse); // 적 방향 반대로 밀어냄

                playerManager.inDamage = true; // 대미지 상태로 전환
                Invoke("DamageEnd", 0.25f); // 일정 시간 후에 대미지 상태 종료
            }
            else
            {
                GameOver(); // HP가 0이면 게임 오버 처리
            }
        }
    }

    // 대미지 상태 종료 처리
    void DamageEnd()
    {
        playerManager.inDamage = false; // 대미지 상태 해제
        gameObject.GetComponent<SpriteRenderer>().enabled = true; // 플레이어를 보이게 함
    }

    // 게임 오버 처리
    void GameOver()
    {
        playerManager.gameState = "gameover"; // 게임 상태를 'gameover'로 설정
        GetComponent<CircleCollider2D>().enabled = false; // 충돌 비활성화
        rbody.velocity = Vector2.zero;
        rbody.gravityScale = 1; // 중력 적용
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse); // 위로 튕겨나가는 효과
        GetComponent<Animator>().Play(deadAinme); // 죽는 애니메이션 재생
        Destroy(gameObject, 1.0f); // 1초 후 오브젝트 제거
    }

    // p1에서 p2까지의 각도를 계산하여 반환
    float GetAngle(Vector2 p1, Vector2 p2)
    {
        float angle;
        if (axisH != 0 || axisV != 0)
        {
            // 이동 중인 경우 각도 계산
            float dx = p2.x - p1.x;
            float dy = p2.y - p1.y;
            float rad = Mathf.Atan2(dx, dy);
            angle = rad * Mathf.Rad2Deg;
        }
        else
        {
            // 정지 중일 때 이전 각도를 유지
            angle = angleZ;
        }
        return angle;
    }
}

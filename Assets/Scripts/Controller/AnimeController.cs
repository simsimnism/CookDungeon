using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class AnimeController : MonoBehaviour
{

    private float speed;
    private PlayerManager pm;
    private SpriteRenderer sr;
    Rigidbody2D rbody;

    // 애니메이션 종류를 enum으로 정의
    private enum AnimationType { Up, Down, Right, Left, Dead }

    // 현재 실행 중인 애니메이션
    private AnimationType nowAnimation = AnimationType.Down;
    private AnimationType oldAnimation = AnimationType.Down;

    //이동 입력값
    float axisH;
    float axisV;
    public float angleZ =  - 90.0f;

    //
    bool isMoving = false;

    private bool isDashing = false;
    private bool dashCooldownActive = false;


    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        speed = Managers.Player.speed;


    }
    void Start()
    {
        StarterImage();
    }

    void Update()
    {
        PlayAnime();
    }

    void StarterImage()
    {
        // PlayerManager에서 필요한 변수를 가져옴
        if (pm != null)
        {
            speed = pm.speed;
            oldAnimation = AnimationType.Down; // 초기 애니메이션은 아래쪽을 바라보는 것으로 설정
        }
    }

    void PlayAnime()
    {
        // 게임이 진행 중이 아니거나 대미지 처리 중이거나 대쉬 중이면 아무것도 하지 않음
        if (pm.gameState != "playing" || pm.inDamage || isDashing)
        {
            return;
        }

        // 플레이어의 이동 입력을 받음
        axisH = Input.GetAxisRaw("Horizontal"); // 좌우 입력 값 (-1, 0, 1)
        axisV = Input.GetAxisRaw("Vertical");   // 상하 입력 값 (-1, 0, 1)

        // isMoving 상태 업데이트
        isMoving = axisH != 0 || axisV != 0;

        // Shift 키를 누르면 대쉬를 시작
        if (Input.GetKeyDown(KeyCode.LeftShift) && !dashCooldownActive)
        {
            StartCoroutine(Dash()); // 대쉬 코루틴 시작
        }

        // 현재 위치에서 이동 방향을 계산
        Vector2 fromPt = transform.position;
        Vector2 toPt = new Vector2(fromPt.x + axisH, fromPt.y + axisV);
        angleZ = GetAngle(fromPt, toPt); // 두 점 사이의 각도를 계산

        // 각도에 따라 애니메이션을 변경 (enum 사용)
        if (angleZ >= -45 && angleZ < 45)
        {
            nowAnimation = AnimationType.Right; // 오른쪽으로 이동
        }
        else if (angleZ >= 45 && angleZ <= 135)
        {
            nowAnimation = AnimationType.Up; // 위쪽으로 이동
        }
        else if (angleZ >= -135 && angleZ <= -45)
        {
            nowAnimation = AnimationType.Down; // 아래쪽으로 이동
        }
        else
        {
            nowAnimation = AnimationType.Left; // 왼쪽으로 이동
        }

        // 현재 애니메이션이 이전 애니메이션과 다를 경우 변경
        if (nowAnimation != oldAnimation)
        {
            oldAnimation = nowAnimation;
            GetComponent<Animator>().Play(nowAnimation.ToString()); // enum 값을 문자열로 변환하여 애니메이션 재생
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true; // 대쉬 시작
        dashCooldownActive = true; // 대쉬 쿨타임 시작

        float originalSpeed = speed; // 원래 속도를 저장
        speed *= pm.dashSpeedMultiplier; // 대쉬 속도로 증가

        pm.inDamage = true; // 대쉬 중에는 무적 상태로 전환

        SetTransparency(0.5f); // 캐릭터를 반투명하게 설정

        // 대쉬 중의 이동 속도를 직접 적용
        rbody.velocity = new Vector2(axisH, axisV).normalized * speed;

        yield return new WaitForSeconds(pm.dashDuration); // 대쉬 지속 시간만큼 기다림

        speed = originalSpeed; // 속도를 원래대로 복원

        rbody.velocity = new Vector2(axisH, axisV).normalized * speed;

        pm.inDamage = false; // 무적 상태 해제
        SetTransparency(1.0f); // 투명도 원상복귀

        isDashing = false; // 대쉬 종료

        yield return new WaitForSeconds(pm.dashCooldown); // 대쉬 쿨타임 대기
        dashCooldownActive = false; // 대쉬 쿨타임 종료
    }

    private void SetTransparency(float alpha)
    {
        Color color = sr.color;
        color.a = alpha; // 알파 값을 변경하여 투명도 설정
        sr.color = color;
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

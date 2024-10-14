using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    // 플레이어 컨트롤러에서 사용할 이동 관련 변수
    public float moveSpeed = 5.0f;//이동속도
    public float dashSpeed = 3f; // 대쉬 속도
    public float dashDuration = 0.1f; // 대쉬 지속 시간
    public bool isDashing = false;// 대쉬 여부
    public float transparencyFadeTime = 0.1f; // 투명화가 진행되는 시간
    public float transparencyHoldTime = 0.5f; // 투명화가 완료된 후 유지되는 시간

    //플레이어 어택에서 사용할 변수 
    public int meleeAttackDamage = 1; //플레이어 데미지
    public float comboResetTime = 1f;//콤보 리셋 시간
    public float attackRadius = 3f;//공격 범위
    public float attackAngle = 160f;//공걱의 부채꼴 각도

    public int comboStep = 0;         // 현재 콤보 단계
    public bool isAttacking = false;  // 공격 중인지 여부
    public float lastAttackTime;      // 마지막 공격 시간
    public bool canChainCombo = false; // 콤보 연결 가능 여부
    public bool canMove;

    //공격범위 시각화를 위한 코드(이후 삭제 가능)
    public bool isShowingAttackRange = false;


    public Vector2 inputVec;
    public int MaxHP = 7;
    public int hp;
    public bool inDamage = false;


    public void DisableMovement()
    {
        canMove = false;
    }

    public void EnableMovement()
    {
        canMove=true;
    }
    



    // 플레이어 수치 초기 설정
    public void Init()
    {
        hp = 3;
        inDamage = false;
    }
}
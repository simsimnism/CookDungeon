using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private BoxCollider2D portalTrigger;

    private void Awake()
    {
        portalTrigger = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Settings.playerTag))
        {
            // GameManager의 currentDungeonLevelListIndex 사용하여 현재 레벨 확인
            if (Managers.GM.currentDungeonLevelListIndex == 4) // 5번째 레벨인 경우
            {
                // 게임 클리어 상태로 전환하고 메시지 출력
                Managers.GM.gameState = GameState.gameCleared;

                // 게임 클리어 메시지 또는 다른 UI 출력
                Managers.Popup.OpenGameClear();
            }
            else
            {
                // 다음 레벨로 진행하는 상태로 전환하고 로딩 화면 실행
                Managers.Sound.PlaySFX(Define.SFX.Teleport1);
                Managers.Popup.OpenGameLoading(); // 로딩 화면 표시
                Managers.GM.gameState = GameState.levelCompleted;
            }

            // 이전 상태를 playingLevel로 저장
            Managers.GM.previousGameState = GameState.playingLevel;
        }
    }
}



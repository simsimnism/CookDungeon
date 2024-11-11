using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    #region 룸 설정

    public const int maxChildCorridors = 3; // 방에서 이어지는 자식 복도의 최대 개수 - 최대 개수는 3개여야 하지만 권장하지 않음.
                                            // 방이 서로 맞지 않을 가능성이 높아 던전 빌드가 실패할 수 있음.
    public const float doorUnlockDelay = 1f; // 문이 잠금 해제되는 딜레이 (1초 후에 열림)

    #endregion

    #region 던전 빌드 설정
    public const int maxDungeonRebuildAttemptsForRoomGraph = 1000;
    public const int maxDungeonBuildAttempts = 10;
    #endregion

    #region 게임오브젝트 태그
    public const string playerTag = "Player";
    #endregion
}
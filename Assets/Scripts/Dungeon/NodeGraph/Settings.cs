using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    #region 룸 세팅

    public const int maxChildCorridors = 3; // 방에서 이어지는 자식 복도의 최대 개수 - 최대 개수는 3개여야 하지만 권장하지 않음.
                                            // 방이 서로 맞지 않을 가능성이 높아 던전 빌드가 실패할 수 있음.

    #endregion

    #region 던전 빌드 세팅
    public const int maxDungeonRebuildAttemptsForRoomGraph = 1000;
    public const int maxDungeonBuildAttempts = 10;
    #endregion
}
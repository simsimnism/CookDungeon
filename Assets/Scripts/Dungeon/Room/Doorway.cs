using UnityEngine;
[System.Serializable]

public class Doorway
{
    public Vector2Int position;
    public GameObject doorPrefab;

    #region Header
    [Header("복사할 상단 왼쪽 위치 좌표")]
    #endregion
    public Vector2Int doorwayStartCopyPosition;

    #region Header
    [Header("복사할 출입구 타일의 너비")]
    #endregion
    public int doorwayCopyTileWidth;

    #region Header
    [Header("복사할 출입구 타일의 높이")]
    #endregion
    public int doorwayCopyTileHeight;

    [HideInInspector]
    public bool isConnected = false;
    [HideInInspector]
    public bool isUnavailable = false;
}
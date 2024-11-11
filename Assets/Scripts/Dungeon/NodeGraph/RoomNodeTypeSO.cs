using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNodeType", menuName = "Scriptable Objects/Dungeon/Room Node Type")]
public class RoomNodeTypeSO : ScriptableObject
{
    public string roomNodeTypeName;

    #region Header
    [Header("에디터에서 보여져야하는 룸 노드 타입만 체크")]
    #endregion Header
    public bool displayInNodeGraphEditor = true;
    #region Header
    [Header("복도")]
    #endregion Header
    public bool isCorridor;
    #region Header
    [Header("복도 NS 남/북")]
    #endregion Header
    public bool isCorridorNS;
    #region Header
    [Header("복도 EW 동/서")]
    #endregion Header
    public bool isCorridorEW;
    #region Header
    [Header("입구 방")]
    #endregion Header
    public bool isEntrance;
    #region Header
    [Header("출구 방")]
    #endregion Header
    public bool isExit;
    #region Header
    [Header("보스 방")]
    #endregion Header
    public bool isBossRoom;
    #region Header
    [Header("가마솥 방")]
    #endregion Header
    public bool isCauldron;
    #region Header
    [Header("None (할당되지 않음)")]
    #endregion Header
    public bool isNone;

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(roomNodeTypeName), roomNodeTypeName);
    }
#endif
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonLevel_", menuName = "Scriptable Objects/Dungeon/Dungeon Level")]
public class DungeonLevelSO : ScriptableObject
{
    #region Header BASIC LEVEL DETAILS
    [Space(10)]
    [Header("기본 레벨 속성")]
    #endregion Header BASIC LEVEL DETAILS
    #region Tooltip
    [Tooltip("레벨의 이름입니다.")]
    #endregion Tooltip

    public string levelName;

    #region Header ROOM TEMPLATES FOR LEVEL
    [Space(10)]
    [Header("레벨 룸 템플릿 설정")]
    #endregion Header ROOM TEMPLATES FOR LEVEL
    #region Tooltip
    [Tooltip("레벨에 포함시키고 싶은 룸 템플릿을 설정합니다. 레벨의 룸 노드 그래프에 지정된 모든 룸 노드 유형에 룸 템플릿이 포함되어 있는지 확인해야 합니다.")]
    #endregion Tooltip

    public List<RoomTemplateSO> roomTemplateList;

    #region Header ROOM NODE GRAPHS FOR LEVEL
    [Space(10)]
    [Header("레벨 룸 노드 그래프 설정")]
    #endregion Header ROOM NODE GRAPHS FOR LEVEL
    #region Tooltip
    [Tooltip("이 레벨에 대한 룸 노드 그래프를 설정합니다.")]
    #endregion Tooltip

    public List<RoomNodeGraphSO> roomNodeGraphList;

    #region Validation
#if UNITY_EDITOR

    // Validate scriptable object details eneterd
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(levelName), levelName);
        if (HelperUtilities.ValidateCheckEnumerableValues(this, nameof(roomTemplateList), roomTemplateList))
            return;
        if (HelperUtilities.ValidateCheckEnumerableValues(this, nameof(roomNodeGraphList), roomNodeGraphList))
            return;

        // 지정된 노드 그래프의 모든 노드 유형에 대해 룸 템플릿이 지정되었는지 확인

        // 남/북 corridor, 동/서 corridor, 입구 타입이 지정되었는지 확인
        bool isEWCorridor = false;
        bool isNSCorridor = false;
        bool isEntrance = false;

        // 모든 룸 템플릿을 돌면서 노드 타입이 지정되었는지 확인
        foreach (RoomTemplateSO roomTemplateSO in roomTemplateList)
        {
            if (roomTemplateSO == null)
                return;

            if (roomTemplateSO.roomNodeType.isCorridorEW)
                isEWCorridor = true;
            if (roomTemplateSO.roomNodeType.isCorridorNS)
                isNSCorridor = true;
            if (roomTemplateSO.roomNodeType.isEntrance)
                isEntrance = true;
        }

        if (isEWCorridor == false)
        {
            Debug.Log("In " + this.name.ToString() + " : E/W Corridor Room Type이 지정되지 않음");
        }

        if (isNSCorridor == false)
        {
            Debug.Log("In " + this.name.ToString() + " : N/S Corridor Room Type이 지정되지 않음");
        }

        if (isEntrance == false)
        {
            Debug.Log("In " + this.name.ToString() + " : Entrance Room Type이 지정되지 않음");
        }

        // 모든 노드 그래프를 루프
        foreach (RoomNodeGraphSO roomNodeGraph in roomNodeGraphList)
        {
            if (roomNodeGraph == null)
                return;

            // 노드 그래프의 모든 노드를 루프
            foreach (RoomNodeSO roomNodeSO in roomNodeGraph.roomNodeList)
            {
                if (roomNodeSO == null)
                    continue;

                // 각 roomNode 타입에 대해 룸 템플릿이 지정되었는지 확인

                // 복도랑 입구 확인
                if (roomNodeSO.roomNodeType.isEntrance || roomNodeSO.roomNodeType.isCorridorEW || roomNodeSO.roomNodeType.isCorridorNS ||
                    roomNodeSO.roomNodeType.isCorridor || roomNodeSO.roomNodeType.isNone)
                    continue;

                bool isRoomNodeTypeFound = false;

                // 모든 룸 템플릿을 돌면서 노드 타입이 지정되었는지 확인
                foreach (RoomTemplateSO roomTemplateSO in roomTemplateList)
                {
                    if (roomTemplateSO == null)
                        continue;

                    if (roomTemplateSO.roomNodeType == roomNodeSO.roomNodeType)
                    {
                        isRoomNodeTypeFound = true;
                        break;
                    }
                }

                if (!isRoomNodeTypeFound)
                    Debug.Log("In " + this.name.ToString() + " : " + roomNodeGraph.name.ToString() + " 에 대한 룸 템플릿인 " + roomNodeSO.roomNodeType.name.ToString() + " 을 찾을 수 없습니다.");
            }
        }

    }


#endif
    #endregion Validation
}
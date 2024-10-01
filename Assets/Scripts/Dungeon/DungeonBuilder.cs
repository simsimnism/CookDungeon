using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonBuilder : MonoBehaviour
{
    public static DungeonBuilder Instance; // 임시 인스턴스

    public Dictionary<string, Room> dungeonBuilderRoomDictionary = new Dictionary<string, Room>();
    private Dictionary<string, RoomTemplateSO> roomTemplateDictionary = new Dictionary<string, RoomTemplateSO>();
    private List<RoomTemplateSO> roomTemplateList = null;
    private RoomNodeTypeListSO roomNodeTypeList;
    private bool dungeonBuildSuccessful;

    // 재우 도움핑 MonoBehaviour가 필요한데 지울려면 어떻게 해야하는가?

    private void Awake()
    {
        Instance = this;
        LoadRoomNodeTypeList();
    }

    public void Init()
    {
        LoadRoomNodeTypeList();
    }

    private void LoadRoomNodeTypeList()
    {
        //GameResources의 roomNodeTypeList를 가져옴.
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }

    public bool GenerateDungeon(DungeonLevelSO currentDungeonLevel)
    {
        roomTemplateList = currentDungeonLevel.roomTemplateList;

        // 스크립터블 오브젝트 룸 템플릿을 불러옴.
        LoadRoomTemplatesIntoDictionary();

        dungeonBuildSuccessful = false;
        int dungeonBuildAttempts = 0;

        while (!dungeonBuildSuccessful && dungeonBuildAttempts < Settings.maxDungeonBuildAttempts)
        {
            dungeonBuildAttempts++;

            // 리스트의 룸 노드 그래프를 랜덤으로 가져온다.
            RoomNodeGraphSO roomNodeGraph = SelectRandomRoomNodeGraph(currentDungeonLevel.roomNodeGraphList);

            int dungeonRebuildAttemptsForNodeGraph = 0;
            dungeonBuildSuccessful = false;

            // 던전이 성공적으로 건설되거나 노드 그래프의 최대 시도 횟수를 초과할 때까지 반복한다.
            while (!dungeonBuildSuccessful && dungeonRebuildAttemptsForNodeGraph <= Settings.maxDungeonRebuildAttemptsForRoomGraph)
            {
                // 던전 룸 게임 오브젝트랑 던전 룸 딕셔너리를 초기화 함.
                ClearDungeon();

                dungeonRebuildAttemptsForNodeGraph++;

                // 선택된 방 노드 그래프에 대해 무작위 던전 건설 시도
                dungeonBuildSuccessful = AttemptToBuildRandomDungeon(roomNodeGraph);
            }

            if (dungeonBuildSuccessful)
            {
                // Room Gameobjects 인스턴스화 시키기
                InstantiateRoomGameobjects();
            }
        }
        return dungeonBuildSuccessful;
    }

    /// <summary>
    /// 방 템플릿을 딕셔너리에 로드시키는 함수.
    /// </summary>
    private void LoadRoomTemplatesIntoDictionary()
    {
        // 룸 템플릿 딕셔너리를 초기화
        roomTemplateDictionary.Clear();

        // 룸 템플릿 딕셔너리를 로드
        foreach (RoomTemplateSO roomTemplate in roomTemplateList)
        {
            if (!roomTemplateDictionary.ContainsKey(roomTemplate.guid))
            {
                roomTemplateDictionary.Add(roomTemplate.guid, roomTemplate);
            }
            else
            {
                Debug.Log("룸 템플릿키가 중복 입력됨. 룸 템플릿 리스트 : " + roomTemplateList);
            }
        }
    }

    /// <summary>
    /// 지정된 룸 노드 그래프에 대해 무작위로 던전 빌드를 시작함.
    /// 무작위 레이아웃이 성공적으로 생성되면 true를 반환하고,
    /// 문제가 발생하고 다른 시도가 필요한 경우 false를 반환합니다.
    /// </summary>
    private bool AttemptToBuildRandomDungeon(RoomNodeGraphSO roomNodeGraph)
    {
        // openRoomNodeQueue를 생성
        Queue<RoomNodeSO> openRoomNodeQueue = new Queue<RoomNodeSO>();

        RoomNodeTypeSO temp = roomNodeTypeList.list.Find(x => x.isEntrance);
        // 룸 노드 그래프로부터 룸 노드 큐에 entrance노드를 추가한다.
        RoomNodeSO entranceNode = roomNodeGraph.GetRoomNode(temp);

        if (entranceNode != null)
        {
            openRoomNodeQueue.Enqueue(entranceNode);
        }
        else
        {
            Debug.Log("Entrance Node가 없습니다.");
            return false; // 던전 건설 실패
        }

        // 방이 겹치지않게 시작
        bool noRoomOverlaps = true;

        // Process open room nodes queue
        noRoomOverlaps = ProcessRoomsInOpenRoomNodeQueue(roomNodeGraph, openRoomNodeQueue, noRoomOverlaps);

        // 모든 룸 노드가 처리되었고 룸 중복이 없는 경우 true를 반환
        if (openRoomNodeQueue.Count == 0 && noRoomOverlaps)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 룸 노드 그래프 리스트에서 랜덤으로 방 노드 그래프를 선택하는 함수.
    /// </summary>
    private RoomNodeGraphSO SelectRandomRoomNodeGraph(List<RoomNodeGraphSO> roomNodeGraphList)
    {
        if (roomNodeGraphList.Count > 0)
        {
            return roomNodeGraphList[UnityEngine.Random.Range(0, roomNodeGraphList.Count)];
        }
        else
        {
            Debug.Log("리스트에 룸 노드 그래프가 없습니다.");
            return null;
        }
    }

    /// <summary>
    /// 던전 룸 게임오브젝트와 던전 룸 딕셔너리를 초기화하는 함수
    /// </summary>
    private void ClearDungeon()
    {
        // 인스턴스화된 던전 게임오브젝트를 파괴하고 던전 매니저 룸 딕셔너리을 지움.
        if (dungeonBuilderRoomDictionary.Count > 0)
        {
            foreach (KeyValuePair<string, Room> keyvaluepair in dungeonBuilderRoomDictionary)
            {
                Room room = keyvaluepair.Value;
                if (room.instantiatedRoom != null)
                {
                    Managers.Resource.Destroy(room.instantiatedRoom.gameObject);
                }
            }

            dungeonBuilderRoomDictionary.Clear();
        }
    }

    /// <summary>
    /// ProcessRoomsInOpenRoomNodeQueue에서 룸을 처리하고 룸이 겹치지 않으면 true를 반환하는 함수.
    /// </summary>
    private bool ProcessRoomsInOpenRoomNodeQueue(RoomNodeGraphSO roomNodeGraph, Queue<RoomNodeSO> openRoomNodeQueue, bool noRoomsOverlaps)
    {
        //룸 노드가 open room node queue에 있고 룸이 겹치는 부분이 감지되지 않음.
        while (openRoomNodeQueue.Count > 0 && noRoomsOverlaps == true)
        {
            // open room node queue에서 다음 룸 노드를 가져옴.
            RoomNodeSO roomNode = openRoomNodeQueue.Dequeue();

            // 룸 노드 그래프로부터 큐에 자식 노드를 추가 (부모 룸에 대한 링크 포함함.)
            foreach (RoomNodeSO childRoomNode in roomNodeGraph.GetChildRoomNodes(roomNode))
            {
                openRoomNodeQueue.Enqueue(childRoomNode);
            }

            // 만약 방에 Entrance 마크가 있는 경우 위치를 지정하고 룸 딕셔너리에 추가
            if (roomNode.roomNodeType.isEntrance)
            {
                RoomTemplateSO roomTemplate = GetRandomRoomTemplate(roomNode.roomNodeType);

                Room room = CreateRoomFromRoomTemplate(roomTemplate, roomNode);

                room.isPositioned = true;

                // 룸 딕셔너리에 방을 추가
                dungeonBuilderRoomDictionary.Add(room.id, room);
            }

            // 룸 타입이 Entrance가 아닌 경우
            else
            {
                // 노드에서 부모 룸을 가져옴.
                Room parentRoom = dungeonBuilderRoomDictionary[roomNode.parentRoomNodeIDList[0]];
                
                // 공간을 겹치지 않게 배치 가능한지 확인하는 작업
                noRoomsOverlaps = CanPlaceRoomWithNoOverlaps(roomNode, parentRoom); // 이거 왜 지혼자 false로 바뀜?
            }
        }
        return noRoomsOverlaps;
    }

    /// <summary>
    /// roomType과 매칭되는 룸 템플릿을 roomtemplatelist에서 랜덤으로 가져와 반환
    /// (만약 매칭되는 룸 템플릿을 찾을 수 없으면 null을 반환).
    /// </summary>
    private RoomTemplateSO GetRandomRoomTemplate(RoomNodeTypeSO roomNodeType)
    {
        List<RoomTemplateSO> matchingRoomTemplateList = new List<RoomTemplateSO>();

        // room template list를 반복
        foreach (RoomTemplateSO roomTemplate in roomTemplateList)
        {
            // 매칭되는 room templates을 추가
            if (roomTemplate.roomNodeType == roomNodeType)
            {
                matchingRoomTemplateList.Add(roomTemplate);
            }
        }

        // 리스트에 없으면 null 반환
        if (matchingRoomTemplateList.Count == 0)
            return null;

        // 리스트에서 랜덤으로 선택된 방을 반환
        return matchingRoomTemplateList[UnityEngine.Random.Range(0, matchingRoomTemplateList.Count)];
    }

    /// <summary>
    /// 던전 내에 룸 노드를 배치 시도 (배치할 수 있으면 방을 반환, 아니면 null 반환)
    /// </summary>
    private bool CanPlaceRoomWithNoOverlaps(RoomNodeSO roomNode, Room parentRoom)
    {
        // 초기화 이후 겹치는 방이 있다고 가정
        bool roomOverlaps = true;

        // 방이 겹치는 경우 - 방이 겹치지 않고 성공적으로 배치될 때까지 부모 방의 모든 사용 가능한 문에 배치 시도.

        while (roomOverlaps)
        {
            // 부모를 위한 연결되지 않은 사용 가능한 출입구를 랜덤으로 선택
            List<Doorway> unconnectedAbailableParentDoorways = GetUnconnectedAvailableDoorways(parentRoom.doorWayList).ToList();

            if (unconnectedAbailableParentDoorways.Count == 0)
            {
                // 더 이상 시도할 수 있는 문이 없으면 실패
                return false; // 방이 중복 됨
            }

            Doorway doorwayParent = unconnectedAbailableParentDoorways[UnityEngine.Random.Range(0, unconnectedAbailableParentDoorways.Count)];

            // 부모 방의 문 방향과 일치하는 노드에 대해 랜덤으로 룸 템플릿을 가져온다,
            RoomTemplateSO roomtemplate = GetRandomTemplateForRoomConsistentWithParent(roomNode, doorwayParent);

            // 방 생성
            Room room = CreateRoomFromRoomTemplate(roomtemplate, roomNode);

            // 방의 배치. 만약 방이 겹치지 않으면 true를 반환
            if (PlaceTheRoom(parentRoom, doorwayParent, room))
            {
                // 만약 방이 겹치지 않으면 false로 바꾸고 while 루프를 종료
                roomOverlaps = false;

                // 룸을 배치된 것으로 표시
                room.isPositioned = true;

                // 딕셔너리에 방 추가
                dungeonBuilderRoomDictionary.Add(room.id, room);
            }
            else
            {
                roomOverlaps = true;
            }
        }
        return true; // 겹치는 방이 없음
    }

    /// <summary>
    /// Get random room template for room node taking into account the parent doorway orientation
    /// </summary>
    private RoomTemplateSO GetRandomTemplateForRoomConsistentWithParent(RoomNodeSO roomNode, Doorway doorwayParent)
    {
        RoomTemplateSO roomtemplate = null;

        // If room node is a corridor then select random correct Corridor room template based on
        // parent doorway orientation
        if (roomNode.roomNodeType.isCorridor)
        {
            switch (doorwayParent.orientation)
            {
                case Orientation.north:
                case Orientation.south:
                    roomtemplate = GetRandomRoomTemplate(roomNodeTypeList.list.Find(x => x.isCorridorNS));
                    break;

                case Orientation.east:
                case Orientation.west:
                    roomtemplate = GetRandomRoomTemplate(roomNodeTypeList.list.Find(x => x.isCorridorEW));
                    break;

                case Orientation.none:
                    break;

                default:
                    break;
            }
        }
        //Else select random room template
        else
        {
            roomtemplate = GetRandomRoomTemplate(roomNode.roomNodeType);
        }

        return roomtemplate;
    }

    /// <summary>
    /// Place the room - returns true if the room doesn't overlap, false otherwise
    /// </summary>
    private bool PlaceTheRoom(Room parentRoom, Doorway doorwayParent, Room room)
    {
        // Get current room doorway position
        Doorway doorway = GetOppositeDoorway(doorwayParent, room.doorWayList);

        // Returns if no doorway in room opposite to parent doorway
        if (doorway == null)
        {
            // Just mark the parnet doorway as unavailable so we don't try and connect it again
            doorwayParent.isUnavailable = true;

            return false;
        }

        // Calculate 'world' grid parent doorway position
        Vector2Int parentDoorwayPosition = parentRoom.lowerBounds + doorwayParent.position - parentRoom.templateLowerBounds;

        Vector2Int adjustment = Vector2Int.zero;

        // Calculate adjustment position offset based on room doorway position that we are trying to connect
        // (e.g. if this doorway is west then we need to add (1,0) to the east parent doorway)

        switch (doorway.orientation)
        {
            case Orientation.north:
                adjustment = new Vector2Int(0, -1);
                break;


            case Orientation.east:
                adjustment = new Vector2Int(-1, 0);
                break;


            case Orientation.south:
                adjustment = new Vector2Int(0, 1);
                break;

            case Orientation.west:
                adjustment = new Vector2Int(1, 0);
                break;

            case Orientation.none:
                break;

            default:
                break;
        }

        // Calculate room lower bounds and upper bounds based on positioning to aligh with parent doorway
        room.lowerBounds = parentDoorwayPosition + adjustment + room.templateLowerBounds - doorway.position;
        room.upperBounds = room.lowerBounds + room.templateUpperBounds - room.templateLowerBounds;

        Room overlappingRoom = CheckForRoomOverlap(room);

        if (overlappingRoom == null)
        {
            // mark doorway as connected & unavailable
            doorwayParent.isConnected = true;
            doorwayParent.isUnavailable = true;

            doorway.isConnected = true;
            doorway.isUnavailable = true;

            // return true to show rooms have been connected with no overlap
            return true;
        }
        else
        {
            // Just mark the parent doorway as unavailable so we don't try and connect it again
            doorwayParent.isUnavailable = true;

            return false;
        }
    }

    /// <summary>
    /// Get the doorway from the doorway list that has the opposite orientation to doorway
    /// </summary>
    private Doorway GetOppositeDoorway(Doorway parentDoorway, List<Doorway> doorwayList)
    {
        foreach (Doorway doorwayToCheck in doorwayList)
        {
            if (parentDoorway.orientation == Orientation.east && doorwayToCheck.orientation == Orientation.west)
            {
                return doorwayToCheck;
            }
            else if (parentDoorway.orientation == Orientation.west && doorwayToCheck.orientation == Orientation.east)
            {
                return doorwayToCheck;
            }
            else if (parentDoorway.orientation == Orientation.north && doorwayToCheck.orientation == Orientation.south)
            {
                return doorwayToCheck;
            }
            else if (parentDoorway.orientation == Orientation.south && doorwayToCheck.orientation == Orientation.north)
            {
                return doorwayToCheck;
            }
        }
        return null;
    }

    /// <summary>
    /// Check for rooms that overlap the upper and lower bounds parameters, and if there are overlapping rooms then room else return null
    /// </summary>
    private Room CheckForRoomOverlap(Room roomToTest)
    {
        // Iterate through all rooms
        foreach (KeyValuePair<string, Room> keyvaluepair in dungeonBuilderRoomDictionary)
        {
            Room room = keyvaluepair.Value;

            // skip if same room as room to test or room hasn't been positioned
            if (room.id == roomToTest.id || !room.isPositioned)
                continue;

            // If room overlaps
            if (IsOverLapingRoom(roomToTest, room))
            {
                return room;
            }
        }

        //Return
        return null;
    }

    /// <summary>
    /// Check if 2 rooms overlap each other - return true if they overlap or false if they don't overlap 
    /// </summary>

    private bool IsOverLapingRoom(Room room1, Room room2)
    {
        bool isOverlappingX = IsOverLappingInterval(room1.lowerBounds.x, room1.upperBounds.x, room2.lowerBounds.x, room2.upperBounds.x);

        bool isOverlappingY = IsOverLappingInterval(room1.lowerBounds.y, room1.upperBounds.y, room2.lowerBounds.y, room2.upperBounds.y);

        if (isOverlappingX && isOverlappingY)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Check fi interval 1 overlaps interval 2 - this method is used by the IsOverLappingRoom method
    /// </summary>

    private bool IsOverLappingInterval(int imin1, int imax1, int imin2, int imax2)
    {
        if (Mathf.Max(imin1, imin2) <= Mathf.Min(imax1, imax2))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private List<string> CopyStringList(List<string> oldStringList)
    {
        List<string> newStringList = new List<string>();

        foreach (string stringVale in oldStringList)
        {
            newStringList.Add(stringVale);
        }

        return newStringList;
    }

    private List<Doorway> CopyDoorwayList(List<Doorway> oldDoorwayList)
    {
        List<Doorway> newDoorwayList = new List<Doorway>();

        foreach (Doorway doorway in oldDoorwayList)
        {
            Doorway newDoorway = new Doorway();

            newDoorway.position = doorway.position;
            newDoorway.orientation = doorway.orientation;
            newDoorway.doorPrefab = doorway.doorPrefab;
            newDoorway.isConnected = doorway.isConnected;
            newDoorway.isUnavailable = doorway.isUnavailable;
            newDoorway.doorwayStartCopyPosition = doorway.doorwayStartCopyPosition;
            newDoorway.doorwayCopyTileWidth = doorway.doorwayCopyTileWidth;
            newDoorway.doorwayCopyTileHeight = doorway.doorwayCopyTileHeight;

            newDoorwayList.Add(newDoorway);
        }

        return newDoorwayList;
    }

    /// <summary>
    /// 연결되지않은 문을 가져오는 함수
    /// </summary>
    private IEnumerable<Doorway> GetUnconnectedAvailableDoorways(List<Doorway> roomDoorwayList)
    {
        // Loop through doorway list
        foreach (Doorway doorway in roomDoorwayList)
        {
            if (!doorway.isConnected && !doorway.isUnavailable)
            {
                yield return doorway;
            }
        }
    }

    /// <summary>
    /// roomTemplate과 layoutNode를 기반으로 방을 생성하고 생성된 방을 반환하는 함수.
    /// </summary>
    private Room CreateRoomFromRoomTemplate(RoomTemplateSO roomTemplate, RoomNodeSO roomNode)
    {
        // 템플릿으로부터 방을 초기화
        Room room = new Room();

        room.templateID = roomTemplate.guid;
        room.id = roomNode.id;
        room.prefab = roomTemplate.prefab;
        room.lowerBounds = roomTemplate.lowerBounds;
        room.upperBounds = roomTemplate.upperBounds;
        room.spawnPositionArray = roomTemplate.spawnPositionArray;
        room.templateLowerBounds = roomTemplate.lowerBounds;
        room.templateUpperBounds = roomTemplate.upperBounds;
        room.childRoomIDList = CopyStringList(roomNode.childRoomNodeIDList);
        room.doorWayList = CopyDoorwayList(roomTemplate.doorwayList);

        // 방의 부모 ID 설정
        if (roomNode.parentRoomNodeIDList.Count == 0) // Entrance
        {
            room.parentRoomID = "";
            room.isPreviouslyVisited = true;
        }
        else
        {
            room.parentRoomID = roomNode.parentRoomNodeIDList[0];
        }

        return room;
    }

    /// <summary>
    /// Get a room template by room tmeplate ID, returns null if ID doesn't exist
    /// </summary>
    public RoomTemplateSO GetRoomTemplate(string roomTemplateID)
    {
        if (roomTemplateDictionary.TryGetValue(roomTemplateID, out RoomTemplateSO roomTemplate))
        {
            return roomTemplate;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Get room by roomID, if no room exists with that ID return null
    /// </summary>
    public Room GetRoomByRoomID(string roomID)
    {
        if (dungeonBuilderRoomDictionary.TryGetValue(roomID, out Room room))
        {
            return room;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Instantiate the dungeon room gameobjects from the prefabs
    /// </summary>
    private void InstantiateRoomGameobjects()
    {
        // Iterate through all dungeon rooms.
        foreach (KeyValuePair<string, Room> keyvaluepair in dungeonBuilderRoomDictionary)
        {
            Room room = keyvaluepair.Value;

            // Calculate room position (remember the room instantiatation position needs to be adjusted by the room template lower bounds)
            Vector3 roomPosition = new Vector3(room.lowerBounds.x - room.templateLowerBounds.x, room.lowerBounds.y - room.templateLowerBounds.y, 0f);

            // Instantiate room
            GameObject roomGameobject = Object.Instantiate(room.prefab, roomPosition, Quaternion.identity, transform);

            // Get instantiated room component from instantiated prefab.
            InstantiatedRoom instantiatedRoom = roomGameobject.GetComponentInChildren<InstantiatedRoom>();

            instantiatedRoom.room = room;

            // Initialise The Instantiated Room
            instantiatedRoom.Initialise(roomGameobject);

            // Save gameobject reference.
            room.instantiatedRoom = instantiatedRoom;

        }
    }


}

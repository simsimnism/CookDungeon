using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[DisallowMultipleComponent]
[RequireComponent(typeof(BoxCollider2D))]

public class InstantiatedRoom : MonoBehaviour
{
    [HideInInspector] public Room room;
    [HideInInspector] public Grid grid;
    [HideInInspector] public Tilemap groundTileMap;
    [HideInInspector] public Tilemap decorationTileMap1;
    [HideInInspector] public Tilemap decorationTileMap2;
    [HideInInspector] public Tilemap decorationTileMap3;
    [HideInInspector] public Tilemap decorationTileMap4;
    [HideInInspector] public Tilemap frontTileMap1;
    [HideInInspector] public Tilemap frontTileMap2;
    [HideInInspector] public Tilemap frontTileMap3;
    [HideInInspector] public Tilemap frontTileMap4;
    [HideInInspector] public Tilemap frontTileMap5;
    [HideInInspector] public Tilemap collisionTileMap;
    [HideInInspector] public Tilemap minimapTileMap;
    //[HideInInspector] public int[,] aStarMovementPenalty;  // 2차원 배열을 사용하여 AStar 경로 찾기에 사용할 타일맵의 이동 패널티를 저장 (현재 AStar 기법을 사용하지 않음)
    //[HideInInspector] public int[,] aStarItemObstacles; // 이동 가능한 구조물의 위치를 저장 (현재 AStar 기법을 사용하지 않음)
    [HideInInspector] public Bounds roomColliderBounds;

    #region Header OBJECT REFERENCES

    [Space(10)]
    [Header("OBJECT REFERENCES")]

    #endregion Header OBJECT REFERENCES

    #region Tooltip

    [Tooltip("Populate with the environment child placeholder gameobject ")]

    #endregion Tooltip

    [SerializeField] private GameObject environmentGameObject;

    private BoxCollider2D boxCollider2D;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();

        // Save room collider bounds
        roomColliderBounds = boxCollider2D.bounds;
    }

    // Trigger room changed event when player enters a room
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the player triggered the collider
        if (collision.tag == Settings.playerTag && room != Managers.GM.GetCurrentRoom())
        {
            // Set room as visited
            this.room.isPreviouslyVisited = true;

            // Call room changed event
            EventHandle.CallRoomChangeEvent(room);
        }
    }

    /// <summary>
    /// Initialise The Instantiated Room
    /// </summary>
    public void Initialise(GameObject roomGameobject)
    {
        PopulateTilemapMemberVariables(roomGameobject);

        BlockOffUnusedDoorWays();

        AddDoorsToRooms();

        CauldronCreate();

        DisableCollisionTilemapRenderer();
    }

    /// <summary>
    /// Populate the tilemap and grid memeber variables.
    /// </summary>
    private void PopulateTilemapMemberVariables(GameObject roomGameobject)
    {
        // Get the grid component.
        grid = roomGameobject.GetComponentInChildren<Grid>();

        // Get tilemaps in children.
        Tilemap[] tilemaps = roomGameobject.GetComponentsInChildren<Tilemap>();

        foreach (Tilemap tilemap in tilemaps)
        {
            if (tilemap.gameObject.tag == "groundTileMap")
            {
                groundTileMap = tilemap;
            }
            else if (tilemap.gameObject.tag == "decorationTileMap1")
            {
                decorationTileMap1 = tilemap;
            }
            else if (tilemap.gameObject.tag == "decorationTileMap2")
            {
                decorationTileMap2 = tilemap;
            }
            else if (tilemap.gameObject.tag == "decorationTileMap3")
            {
                decorationTileMap3 = tilemap;
            }
            else if (tilemap.gameObject.tag == "decorationTileMap4")
            {
                decorationTileMap4 = tilemap;
            }
            else if (tilemap.gameObject.tag == "frontTileMap1")
            {
                frontTileMap1 = tilemap;
            }
            else if (tilemap.gameObject.tag == "frontTileMap2")
            {
                frontTileMap2 = tilemap;
            }
            else if (tilemap.gameObject.tag == "frontTileMap3")
            {
                frontTileMap3 = tilemap;
            }
            else if (tilemap.gameObject.tag == "frontTileMap4")
            {
                frontTileMap4 = tilemap;
            }
            else if (tilemap.gameObject.tag == "frontTileMap5")
            {
                frontTileMap5 = tilemap;
            }
            else if (tilemap.gameObject.tag == "collisionTileMap")
            {
                collisionTileMap = tilemap;
            }
            else if (tilemap.gameObject.tag == "miniMapTileMap")
            {
                minimapTileMap = tilemap;
            }
            else
            {
                groundTileMap = tilemap;
            }
        }
    }

    /// <summary>
    /// Block Off Unused Doorways In The Room
    /// </summary>
    private void BlockOffUnusedDoorWays()
    {
        // Loop through all doorways
        foreach (Doorway doorway in room.doorWayList)
        {
            if (doorway.isConnected)
                continue;

            // Block unconnected doorways using tiles on tilemaps
            if (collisionTileMap != null)
            {
                BlockADoorwayOnTilemapLayer(collisionTileMap, doorway);
            }

            if (minimapTileMap != null)
            {
                BlockADoorwayOnTilemapLayer(minimapTileMap, doorway);
            }

            if (groundTileMap != null)
            {
                BlockADoorwayOnTilemapLayer(groundTileMap, doorway);
            }

            if (decorationTileMap1 != null)
            {
                BlockADoorwayOnTilemapLayer(decorationTileMap1, doorway);
            }

            if (decorationTileMap2 != null)
            {
                BlockADoorwayOnTilemapLayer(decorationTileMap2, doorway);
            }

            if (decorationTileMap3 != null)
            {
                BlockADoorwayOnTilemapLayer(decorationTileMap3, doorway);
            }

            if (decorationTileMap4 != null)
            {
                BlockADoorwayOnTilemapLayer(decorationTileMap4, doorway);
            }

            if (frontTileMap1 != null)
            {
                BlockADoorwayOnTilemapLayer(frontTileMap1, doorway);
            }

            if (frontTileMap2 != null)
            {
                BlockADoorwayOnTilemapLayer(frontTileMap2, doorway);
            }

            if (frontTileMap3 != null)
            {
                BlockADoorwayOnTilemapLayer(frontTileMap3, doorway);
            }

            if (frontTileMap4 != null)
            {
                BlockADoorwayOnTilemapLayer(frontTileMap4, doorway);
            }

            if (frontTileMap5 != null)
            {
                BlockADoorwayOnTilemapLayer(frontTileMap5, doorway);
            }
        }
    }

    /// <summary>
    /// Block doorway horizontally - for North and South doorways
    /// </summary>
    private void BlockDoorwayHorizontally(Tilemap tilemap, Doorway doorway)
    {
        Vector2Int startPosition = doorway.doorwayStartCopyPosition;

        // loop through all tiles to copy
        for (int xPos = 0; xPos < doorway.doorwayCopyTileWidth; xPos++)
        {
            for (int yPos = 0; yPos < doorway.doorwayCopyTileHeight; yPos++)
            {
                // Get rotation of tile being copied
                Matrix4x4 transformMatrix = tilemap.GetTransformMatrix(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0));

                // Copy tile
                tilemap.SetTile(new Vector3Int(startPosition.x + 1 + xPos, startPosition.y - yPos, 0), tilemap.GetTile(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0)));

                // Set rotation of tile copied
                tilemap.SetTransformMatrix(new Vector3Int(startPosition.x + 1 + xPos, startPosition.y - yPos, 0), transformMatrix);
            }
        }
    }

    /// <summary>
    /// Block doorway vertically - for East and West doorways
    /// </summary>
    private void BlockDoorwayVertically(Tilemap tilemap, Doorway doorway)
    {
        Vector2Int startPosition = doorway.doorwayStartCopyPosition;

        // loop through all tiles to copy
        for (int yPos = 0; yPos < doorway.doorwayCopyTileHeight; yPos++)
        {

            for (int xPos = 0; xPos < doorway.doorwayCopyTileWidth; xPos++)
            {
                // Get rotation of tile being copied
                Matrix4x4 transformMatrix = tilemap.GetTransformMatrix(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0));

                // Copy tile
                tilemap.SetTile(new Vector3Int(startPosition.x + xPos, startPosition.y - 1 - yPos, 0), tilemap.GetTile(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0)));

                // Set rotation of tile copied
                tilemap.SetTransformMatrix(new Vector3Int(startPosition.x + xPos, startPosition.y - 1 - yPos, 0), transformMatrix);
            }
        }
    }

    /// <summary>
    /// Block a doorway on a tilemap layer
    /// </summary>
    private void BlockADoorwayOnTilemapLayer(Tilemap tilemap, Doorway doorway)
    {
        switch (doorway.orientation)
        {
            case Orientation.north:
            case Orientation.south:
                BlockDoorwayHorizontally(tilemap, doorway);
                break;

            case Orientation.east:
            case Orientation.west:
                BlockDoorwayVertically(tilemap, doorway);
                break;

            case Orientation.none:
                break;
        }

    }

    // 복도가 아니면 문을 추가하는 함수
    private void AddDoorsToRooms()
    {
        // 방 타입이 복도라면 리턴
        if (room.roomNodeType.isCorridorEW || room.roomNodeType.isCorridorNS)
        {
            return;
        }

        // 문의 위치에 프리팹을 인스턴스화
        foreach (Doorway doorway in room.doorWayList)
        {

            // 문 프리팹이 null 값이 아니고 문이 연결되어 있는 경우
            if (doorway.doorPrefab != null && doorway.isConnected)
            {
                //float tileDistance = Settings.tileSizePixels / Settings.pixelsPerUnit;
                float tileDistance = 16 / 16;

                GameObject door = null;

                // 문의 위치를 동서남북에 따라 생성
                if (doorway.orientation == Orientation.north)
                {
                    // 부모를 방으로 하여 문을 만듬
                    door = Instantiate(doorway.doorPrefab, gameObject.transform);
                    door.transform.localPosition = new Vector3(doorway.position.x + tileDistance / 2f, doorway.position.y + tileDistance, 0f);
                }
                else if (doorway.orientation == Orientation.south)
                {
                    door = Instantiate(doorway.doorPrefab, gameObject.transform);
                    door.transform.localPosition = new Vector3(doorway.position.x + tileDistance / 2f, doorway.position.y, 0f);
                }
                else if (doorway.orientation == Orientation.east)
                {
                    door = Instantiate(doorway.doorPrefab, gameObject.transform);
                    door.transform.localPosition = new Vector3(doorway.position.x + tileDistance, doorway.position.y + tileDistance * 1.25f, 0f);
                }
                else if (doorway.orientation == Orientation.west)
                {
                    door = Instantiate(doorway.doorPrefab, gameObject.transform);
                    door.transform.localPosition = new Vector3(doorway.position.x, doorway.position.y + tileDistance * 1.25f, 0f);
                }

                // 문 컴포넌트 가져오기
                Door doorComponent = door.GetComponent<Door>();

                // 문이 보스 룸과 이어져있다면
                if (room.roomNodeType.isBossRoom)
                {
                    doorComponent.isBossRoomDoor = true;

                    // 방에 접근이 안되게 문을 잠금
                    doorComponent.LockDoor();

                    // Instantiate skull icon for minimap by door
                    // GameObject skullIcon = Instantiate(GameResources.Instance.minimapSkullPrefab, gameObject.transform);
                    // skullIcon.transform.localPosition = door.transform.localPosition;

                }
            }

        }

    }

    // 가마솥 생성
    private void CauldronCreate()
    {
        // 만약 방의 룸 노드 타입이 가마솥 방 이라면
        if (room.roomNodeType.isCauldron)
        {
            GameObject Cauldron = Managers.Resource.Instantiate("Cauldron/Cauldron", gameObject.transform);
            Cauldron.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    /// <summary>
    /// Lock the room doors
    /// </summary>
    public void LockDoors()
    {
        Door[] doorArray = GetComponentsInChildren<Door>();

        // Trigger lock doors
        foreach (Door door in doorArray)
        {
            door.LockDoor();
        }

        // Disable room trigger collider
        DisableRoomCollider();
    }

    /// <summary>
    /// Unlock the room doors
    /// </summary>
    public void UnlockDoors(float doorUnlockDelay)
    {
        StartCoroutine(UnlockDoorsRoutine(doorUnlockDelay));
    }

    /// <summary>
    /// Unlock the room doors routine
    /// </summary>
    private IEnumerator UnlockDoorsRoutine(float doorUnlockDelay)
    {
        if (doorUnlockDelay > 0f)
            yield return new WaitForSeconds(doorUnlockDelay);

        Door[] doorArray = GetComponentsInChildren<Door>();

        // Trigger open doors
        foreach (Door door in doorArray)
        {
            door.UnlockDoor();
        }

        // Enable room trigger collider
        EnableRoomCollider();
    }


    /// <summary>
    /// Disable collision tilemap renderer
    /// </summary>
    private void DisableCollisionTilemapRenderer()
    {
        // Disable collision tilemap renderer
        TilemapRenderer tmp = collisionTileMap.gameObject.GetComponent<TilemapRenderer>();
        tmp.enabled = false;
    }

    // 플레이어가 방에 들어갔을 때 트리거되는 룸 트리거 콜라이더를 활성화
    public void EnableRoomCollider()
    {
        boxCollider2D.enabled = true;
    }

    // 플레이어가 방에 들어갔을 때 트리거되는 룸 트리거 콜라이더를 비활성화
    public void DisableRoomCollider()
    {
        Debug.Log(boxCollider2D == null);
        boxCollider2D.enabled = false;
    }

    public void ActivateEnvironmentGameObjects()
    {
        if (environmentGameObject != null)
            environmentGameObject.SetActive(true);
    }

    public void DeactivateEnvironmentGameObjects()
    {
        if (environmentGameObject != null)
            environmentGameObject.SetActive(false);
    }

}

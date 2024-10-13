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
    [HideInInspector] public Tilemap groundTilemap;
    [HideInInspector] public Tilemap decoration1Tilemap;
    [HideInInspector] public Tilemap decoration2Tilemap;
    [HideInInspector] public Tilemap decoration3Tilemap;
    [HideInInspector] public Tilemap decoration4Tilemap;
    [HideInInspector] public Tilemap frontTilemap1;
    [HideInInspector] public Tilemap frontTilemap2;
    [HideInInspector] public Tilemap frontTilemap3;
    [HideInInspector] public Tilemap frontTilemap4;
    [HideInInspector] public Tilemap frontTilemap5;
    [HideInInspector] public Tilemap collisionTilemap;
    [HideInInspector] public Tilemap minimapTilemap;
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
                groundTilemap = tilemap;
            }
            else if (tilemap.gameObject.tag == "decorationTileMap1")
            {
                decoration1Tilemap = tilemap;
            }
            else if (tilemap.gameObject.tag == "decorationTileMap2")
            {
                decoration2Tilemap = tilemap;
            }
            else if (tilemap.gameObject.tag == "decorationTileMap3")
            {
                decoration3Tilemap = tilemap;
            }
            else if (tilemap.gameObject.tag == "decorationTileMap4")
            {
                decoration4Tilemap = tilemap;
            }
            else if (tilemap.gameObject.tag == "frontTileMap1")
            {
                frontTilemap1 = tilemap;
            }
            else if (tilemap.gameObject.tag == "frontTileMap2")
            {
                frontTilemap2 = tilemap;
            }
            else if (tilemap.gameObject.tag == "frontTileMap3")
            {
                frontTilemap3 = tilemap;
            }
            else if (tilemap.gameObject.tag == "frontTileMap4")
            {
                frontTilemap4 = tilemap;
            }
            else if (tilemap.gameObject.tag == "frontTileMap5")
            {
                frontTilemap5 = tilemap;
            }
            else if (tilemap.gameObject.tag == "collisionTileMap")
            {
                collisionTilemap = tilemap;
            }
            else if (tilemap.gameObject.tag == "minimapTileMap")
            {
                minimapTilemap = tilemap;
            }
            else
            {
                groundTilemap = tilemap;
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
            if (collisionTilemap != null)
            {
                BlockADoorwayOnTilemapLayer(collisionTilemap, doorway);
            }

            if (minimapTilemap != null)
            {
                BlockADoorwayOnTilemapLayer(minimapTilemap, doorway);
            }

            if (groundTilemap != null)
            {
                BlockADoorwayOnTilemapLayer(groundTilemap, doorway);
            }

            if (decoration1Tilemap != null)
            {
                BlockADoorwayOnTilemapLayer(decoration1Tilemap, doorway);
            }

            if (decoration2Tilemap != null)
            {
                BlockADoorwayOnTilemapLayer(decoration2Tilemap, doorway);
            }

            if (decoration3Tilemap != null)
            {
                BlockADoorwayOnTilemapLayer(decoration3Tilemap, doorway);
            }

            if (decoration4Tilemap != null)
            {
                BlockADoorwayOnTilemapLayer(decoration4Tilemap, doorway);
            }

            if (frontTilemap1 != null)
            {
                BlockADoorwayOnTilemapLayer(frontTilemap1, doorway);
            }

            if (frontTilemap2 != null)
            {
                BlockADoorwayOnTilemapLayer(frontTilemap2, doorway);
            }

            if (frontTilemap3 != null)
            {
                BlockADoorwayOnTilemapLayer(frontTilemap3, doorway);
            }

            if (frontTilemap4 != null)
            {
                BlockADoorwayOnTilemapLayer(frontTilemap4, doorway);
            }

            if (frontTilemap5 != null)
            {
                BlockADoorwayOnTilemapLayer(frontTilemap5, doorway);
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

    /// <summary>
    /// Disable collision tilemap renderer
    /// </summary>
    private void DisableCollisionTilemapRenderer()
    {
        // Disable collision tilemap renderer
        TilemapRenderer tmp = collisionTilemap.gameObject.GetComponent<TilemapRenderer>();
        tmp.enabled = false;
    }

    /// <summary>
    /// Disable the room trigger collider that is used to trigger when the player enters a room
    /// </summary>
    public void DisableRoomCollider()
    {
        boxCollider2D.enabled = false;
    }

    /// <summary>
    /// Enable the room trigger collider that is used to trigger when the player enters a room
    /// </summary>
    public void EnableRoomCollider()
    {
        boxCollider2D.enabled = true;
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

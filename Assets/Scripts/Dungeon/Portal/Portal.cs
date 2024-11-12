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
            Managers.GM.gameState = GameState.levelCompleted;
            Managers.GM.previousGameState = GameState.playingLevel;
        }
    }
}

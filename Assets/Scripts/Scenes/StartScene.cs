using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    private SceneManagerEx sceneManagerEx;

    void Start()
    {
        sceneManagerEx = new SceneManagerEx();
    }

    public void ChangeScene()
    {
        Managers.Sound.PlaySFX(Define.SFX.Button1);
        sceneManagerEx.LoadScene(Define.Scene.GameScene);
    }

    public void ChangeGameState()
    {
        Managers.GM.gameState = GameState.gameStarted;
    }

    void Update()
    {

    }
}
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
        sceneManagerEx.LoadScene(Define.Scene.GameScene1);
    }

    

    void Update()
    {
        
    }
}

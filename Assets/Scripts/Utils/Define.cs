using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum Scene
    {
        Unknown,
        Game,
        MainMenu,
        GameScene,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum BGM
    {
        Count
    }

    public enum SFX
    {
        Button1,
        Fire1,
        Hit1,
        Count
    }
    public enum UIEvent
    {
        Click,
        Drag,
    }

    public enum MouseEvent
    {
        Press,
        Click,
    }

    public enum CameraMode
    {
        QuarterView,
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class DestroyEvent : MonoBehaviour
{
    public event Action<DestroyEvent, DestroyedEventArgs> OnDestroyed;

    public void CallDestroyedEvent(bool playerDied, int points)
    {
        OnDestroyed?.Invoke(this, new DestroyedEventArgs() { playerDied = playerDied, points = points });
    }
}

public class DestroyedEventArgs : EventArgs
{
    public bool playerDied;
    public int points;
}
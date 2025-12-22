using System;
using UnityEngine;

public abstract class MiniGame : MonoBehaviour
{
    public event Action OnMiniGameSuccess;
    public event Action OnMiniGameFail;

    protected void RaiseSuccess()
    {
        OnMiniGameSuccess?.Invoke();
    }

    protected void RaiseFail()
    {
        OnMiniGameFail?.Invoke();
    }
}
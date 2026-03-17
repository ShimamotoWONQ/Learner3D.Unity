using System;
using UnityEngine;

public abstract class CustomAnimationScript : MonoBehaviour
{
    public event Action OnScriptCompleted;

    protected void Complete() => OnScriptCompleted?.Invoke();

    public abstract void OnPlay();
    public abstract void OnStop();
    public virtual void OnPause() { }
    public virtual void OnResume() { }
}

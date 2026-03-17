using System;
using UnityEngine;

public abstract class AnimationNodeBase : MonoBehaviour
{
    public event Action<AnimationNodeBase> OnCompleted;

    protected void NotifyCompleted() => OnCompleted?.Invoke(this);

    public abstract void Play();
    public abstract void ResetAndPlay();
    public abstract void Pause();
    public abstract void Resume();
    public abstract void Stop();
}

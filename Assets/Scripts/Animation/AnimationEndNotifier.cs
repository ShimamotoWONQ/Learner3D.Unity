using System;
using UnityEngine;

[RequireComponent( typeof(Animator) )]
public class AnimationEndNotifier : MonoBehaviour
{
    public Action OnAnimationEnded;
    [HideInInspector] public bool isAnimationEnded = false;

    public void NotifyAnimationEnded() {
        isAnimationEnded = true;
        OnAnimationEnded?.Invoke();
    }
}

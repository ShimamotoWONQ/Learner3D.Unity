using System;
using UnityEngine;

[RequireComponent( typeof(Animation) )]
public class AnimationManager : MonoBehaviour
{
    public event Action OnAllAnimationEnded;

    [SerializeField] ObjectManager objectManager;
    [SerializeField] float autoSkipTime;

    public void Init()
    {

    }

    public void PlayAnimation(int stepIndex)
    {
        AnimationEndNotifier[] notifiers = objectManager.stepObjectHolderList[stepIndex].gameObject.GetComponentsInChildren<AnimationEndNotifier>();

        if (notifiers.Length == 0) {
            Invoke(nameof(EndAnimation), autoSkipTime);
            return;
        }

        foreach (AnimationEndNotifier notifier in notifiers)
        {
            notifier.OnAnimationEnded += () => CheckIfAllAnimationEnded(notifiers);
        }
    }

    void CheckIfAllAnimationEnded(AnimationEndNotifier[] notifiers)
    {
        foreach (AnimationEndNotifier notifier in notifiers)
        {
            if (!notifier.isAnimationEnded)
                return;
        }

        EndAnimation();
    }

    void EndAnimation()
    {
        OnAllAnimationEnded?.Invoke();
    }
}

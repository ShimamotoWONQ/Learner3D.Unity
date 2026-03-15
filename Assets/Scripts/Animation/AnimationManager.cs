using System;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] ObjectManager objectManager;

    public event Action OnNextStepRequested;
    public event Action OnPrevStepRequested;

    AnimationSequence _currentSequence;
    bool _shouldPlayFromEnd;

    public void Init() { }

    /// <summary>StepManager.LoadStep から呼ばれる。非アニメーション step では sequence=null になる。</summary>
    public void PlayAnimation(int stepIndex)
    {
        UnregisterCurrentSequence();

        var holder = objectManager.stepObjectHolderList[stepIndex];
        _currentSequence = holder.GetComponentInChildren<AnimationSequence>();

        if (_currentSequence == null) return;

        _currentSequence.OnSequenceEnded += HandleSequenceEnded;
        _currentSequence.OnAtFirstStep   += HandleAtFirstStep;

        if (_shouldPlayFromEnd)
        {
            _shouldPlayFromEnd = false;
            _currentSequence.PlayFromEnd();
        }
        else
        {
            _currentSequence.Play();
        }
    }

    public void Next()
    {
        if (_currentSequence == null) { OnNextStepRequested?.Invoke(); return; }
        _currentSequence.Next();
    }

    public void Back()
    {
        if (_currentSequence == null) { OnPrevStepRequested?.Invoke(); return; }
        _currentSequence.Back();
    }

    void HandleSequenceEnded()
    {
        _shouldPlayFromEnd = false;
        OnNextStepRequested?.Invoke();
    }

    void HandleAtFirstStep()
    {
        _shouldPlayFromEnd = true;
        OnPrevStepRequested?.Invoke();
    }

    void UnregisterCurrentSequence()
    {
        if (_currentSequence == null) return;
        _currentSequence.OnSequenceEnded -= HandleSequenceEnded;
        _currentSequence.OnAtFirstStep   -= HandleAtFirstStep;
        _currentSequence = null;
    }

    void OnDestroy() => UnregisterCurrentSequence();
}

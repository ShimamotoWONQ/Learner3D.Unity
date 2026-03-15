using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSequence : MonoBehaviour
{
    [SerializeField] List<AnimationStep> steps;

    public event Action OnSequenceEnded;
    public event Action OnAtFirstStep;

    int _currentIndex = -1;
    bool _isPaused;

    AnimationStep CurrentStep =>
        (_currentIndex >= 0 && _currentIndex < steps.Count) ? steps[_currentIndex] : null;

    public void Play()
    {
        if (steps == null || steps.Count == 0)
        {
            Debug.LogError($"[AnimationSequence] steps is empty on {gameObject.name}");
            return;
        }
        GoToStep(0, reset: false);
    }

    /// <summary>現在のステップをスキップして次へ進む。</summary>
    public void Next()
    {
        if (_currentIndex < 0) return;

        StopCurrentStep();

        int nextIndex = _currentIndex + 1;
        if (nextIndex >= steps.Count)
        {
            EndSequence();
            return;
        }
        GoToStep(nextIndex, reset: false);
    }

    /// <summary>前のステップを最初から再生する。先頭にいる場合は OnAtFirstStep を発火。</summary>
    public void Back()
    {
        if (_currentIndex <= 0)
        {
            OnAtFirstStep?.Invoke();
            return;
        }

        StopCurrentStep();
        GoToStep(_currentIndex - 1, reset: true);
    }

    /// <summary>最後の step から逆方向で入場するとき（Prev 境界越え）に呼ぶ。</summary>
    public void PlayFromEnd()
    {
        if (steps == null || steps.Count == 0)
        {
            Debug.LogError($"[AnimationSequence] steps is empty on {gameObject.name}");
            return;
        }
        GoToStep(steps.Count - 1, reset: false);
    }

    public void Pause()
    {
        if (_isPaused) return;
        _isPaused = true;
        CurrentStep?.Pause();
    }

    public void Resume()
    {
        if (!_isPaused) return;
        _isPaused = false;
        CurrentStep?.Resume();
    }

    void GoToStep(int index, bool reset)
    {
        _currentIndex = index;
        var step = steps[index];
        step.OnStepCompleted += OnCurrentStepCompleted;

        if (reset)
            step.ResetAndPlay();
        else
            step.Play();
    }

    void StopCurrentStep()
    {
        var step = CurrentStep;
        if (step == null) return;
        step.OnStepCompleted -= OnCurrentStepCompleted;
        step.Stop();
    }

    void OnCurrentStepCompleted()
    {
        var step = CurrentStep;
        if (step != null)
            step.OnStepCompleted -= OnCurrentStepCompleted;
        // 自動進行しない。Next() が呼ばれるまで待機。
    }

    void EndSequence()
    {
        _currentIndex = -1;
        _isPaused = false;
        OnSequenceEnded?.Invoke();
    }
}

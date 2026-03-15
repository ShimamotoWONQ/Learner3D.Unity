using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationStepNode : MonoBehaviour
{
    public event Action<AnimationStepNode> OnCompleted;

    Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.speed = 0f; // StepManager が Play() を呼ぶまで自動再生しない
    }

    /// <summary>通常再生。Entry から再生を開始する。</summary>
    public void Play()
    {
        if (!Validate()) return;
        _animator.speed = 1f;
        _animator.Rebind();
        _animator.Update(0f);
    }

    /// <summary>前ステップへ戻る際の再生。Play() と同じく Entry から再生し直す。</summary>
    public void ResetAndPlay() => Play();

    public void Pause()
    {
        if (_animator != null) _animator.speed = 0f;
    }

    public void Resume()
    {
        if (_animator != null) _animator.speed = 1f;
    }

    public void Stop()
    {
        if (_animator != null) _animator.speed = 0f;
    }

    /// <summary>Animation Event から呼ぶ。Stop してループを防いだうえで終了を通知する。</summary>
    public void NotifyCompleted()
    {
        Stop();
        OnCompleted?.Invoke(this);
    }

    bool Validate()
    {
        if (_animator == null)
        {
            Debug.LogError($"[AnimationStepNode] Animator is null on {gameObject.name}");
            return false;
        }
        return true;
    }

#if UNITY_EDITOR
    void Reset()
    {
        var anim = GetComponent<Animator>();
        if (anim != null && anim.runtimeAnimatorController == null)
            Debug.LogWarning($"[AnimationStepNode] Animator Controller が未設定です: {gameObject.name}");
    }

    void OnValidate()
    {
        var anim = GetComponent<Animator>();
        if (anim == null) return;
        if (anim.runtimeAnimatorController == null)
            Debug.LogWarning($"[AnimationStepNode] Animator Controller が未設定です: {gameObject.name}");
    }
#endif
}

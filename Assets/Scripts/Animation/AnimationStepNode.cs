using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationStepNode : AnimationNodeBase
{
    Animator _animator;
    bool _isPlaying;
    float _prevNormalizedTime;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.speed = 0f; // StepManager が Play() を呼ぶまで自動再生しない
    }

    void Update()
    {
        if (!_isPlaying) return;
        float normalizedTime = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        // transition で 1.0 をスキップして先頭に戻った場合をループとして検出
        bool looped = _prevNormalizedTime > 0.9f && normalizedTime < 0.1f;
        _prevNormalizedTime = normalizedTime;

        if (looped)
        {
            _isPlaying = false;
            _animator.speed = 0f;
            NotifyCompleted();
        }
    }

    /// <summary>通常再生。Entry から再生を開始する。</summary>
    public override void Play()
    {
        if (!Validate()) return;
        _isPlaying = true;
        _prevNormalizedTime = 0f;
        _animator.speed = 1f;
        _animator.Rebind();
        _animator.Update(0f);
    }

    /// <summary>前ステップへ戻る際の再生。Play() と同じく Entry から再生し直す。</summary>
    public override void ResetAndPlay() => Play();

    public override void Pause()
    {
        if (_animator != null) _animator.speed = 0f;
    }

    public override void Resume()
    {
        if (_animator != null) _animator.speed = 1f;
    }

    public override void Stop()
    {
        _isPlaying = false;
        if (_animator != null) _animator.speed = 0f;
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

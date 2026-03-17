using UnityEngine;

public class CustomAnimationNode : AnimationNodeBase
{
    [SerializeField] CustomAnimationScript script;

    public override void Play()
    {
        if (!Validate()) return;
        script.OnScriptCompleted += HandleCompleted;
        script.OnPlay();
    }

    public override void ResetAndPlay() => Play();

    public override void Pause()
    {
        if (script != null) script.OnPause();
    }

    public override void Resume()
    {
        if (script != null) script.OnResume();
    }

    public override void Stop()
    {
        if (script != null)
        {
            script.OnScriptCompleted -= HandleCompleted;
            script.OnStop();
        }
    }

    void HandleCompleted()
    {
        script.OnScriptCompleted -= HandleCompleted;
        NotifyCompleted();
    }

    bool Validate()
    {
        if (script == null)
        {
            Debug.LogError($"[CustomAnimationNode] CustomAnimationScript is null on {gameObject.name}");
            return false;
        }
        return true;
    }

#if UNITY_EDITOR
    void Reset()
    {
        if (GetComponent<CustomAnimationScript>() == null)
            Debug.LogWarning($"[CustomAnimationNode] CustomAnimationScript が未設定です: {gameObject.name}");
    }
#endif
}

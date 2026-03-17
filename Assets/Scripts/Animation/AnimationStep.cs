using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimationStep
{
    [SerializeField] List<AnimationNodeBase> nodes;

    public event Action OnStepCompleted;

    int _completedCount;

    public void Play()
    {
        if (!ValidateNodes()) return;
        _completedCount = 0;
        foreach (var node in nodes)
        {
            node.OnCompleted += OnNodeCompleted;
            node.Play();
        }
    }

    public void ResetAndPlay()
    {
        if (!ValidateNodes()) return;
        _completedCount = 0;
        foreach (var node in nodes)
        {
            node.OnCompleted += OnNodeCompleted;
            node.ResetAndPlay();
        }
    }

    public void Pause()
    {
        if (nodes == null) return;
        foreach (var node in nodes)
            node?.Pause();
    }

    public void Resume()
    {
        if (nodes == null) return;
        foreach (var node in nodes)
            node?.Resume();
    }

    /// <summary>イベント登録を解除し、アニメーションを停止する。Next / Back 時に呼ぶ。</summary>
    public void Stop()
    {
        if (nodes == null) return;
        foreach (var node in nodes)
        {
            if (node == null) continue;
            node.OnCompleted -= OnNodeCompleted;
            node.Stop();
        }
    }

    void OnNodeCompleted(AnimationNodeBase node)
    {
        node.OnCompleted -= OnNodeCompleted;
        _completedCount++;
        if (_completedCount >= nodes.Count)
            OnStepCompleted?.Invoke();
    }

    bool ValidateNodes()
    {
        if (nodes == null || nodes.Count == 0)
        {
            Debug.LogError("[AnimationStep] nodes is empty");
            return false;
        }
        foreach (var node in nodes)
        {
            if (node == null)
            {
                Debug.LogError("[AnimationStep] A node in the list is null");
                return false;
            }
        }
        return true;
    }
}

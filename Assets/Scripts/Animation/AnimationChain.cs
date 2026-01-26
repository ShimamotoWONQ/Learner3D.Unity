using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimationChain : MonoBehaviour
{
    [SerializeField] List<AnimationChainNode> chainNodes;
    [SerializeField] string triggerName;

    public event Action OnEnded;

    void Start()
    {
        RegisterEvents();
    }

    public void Play()
    {
        if (chainNodes == null || chainNodes.Count == 0)
        {
            Debug.LogWarning($"[AnimationChain] chainNodes is empty on {gameObject.name}");
            OnEnded?.Invoke();
            return;
        }

        var firstNode = chainNodes[0];
        if (firstNode == null)
        {
            Debug.LogError($"[AnimationChain] First node is null on {gameObject.name}");
            return;
        }

        var animator = firstNode.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError($"[AnimationChain] Animator not found on {firstNode.name}");
            return;
        }

        animator.SetTrigger(triggerName);
    }

    void RegisterEvents()
    {
        if (chainNodes == null)
            return;

        foreach (AnimationChainNode chainNode in chainNodes)
        {
            if (chainNode != null)
            {
                chainNode.OnNextAnimationRequested += PlayNextAnimationNode;
            }
        }
    }

    void OnDestroy()
    {
        UnregisterEvents();
    }

    void UnregisterEvents()
    {
        if (chainNodes == null)
            return;

        foreach (AnimationChainNode chainNode in chainNodes)
        {
            if (chainNode != null)
            {
                chainNode.OnNextAnimationRequested -= PlayNextAnimationNode;
            }
        }
    }

    void PlayNextAnimationNode(AnimationChainNode callerNode)
    {
        if (chainNodes == null || callerNode == null)
            return;

        int currentIndex = chainNodes.IndexOf(callerNode);
        if (currentIndex == -1)
        {
            Debug.LogError($"[AnimationChain] Caller node {callerNode.name} not found in chain");
            return;
        }

        int nextIndex = currentIndex + 1;

        if (nextIndex >= chainNodes.Count)
        {
            OnEnded?.Invoke();
            return;
        }

        AnimationChainNode nextNode = chainNodes[nextIndex];
        if (nextNode == null)
        {
            Debug.LogError($"[AnimationChain] Next node at index {nextIndex} is null");
            return;
        }

        Animator nextAnimator = nextNode.GetComponent<Animator>();
        if (nextAnimator == null)
        {
            Debug.LogError($"[AnimationChain] Animator not found on {nextNode.name}");
            return;
        }

        nextAnimator.SetTrigger(triggerName);
    }
}

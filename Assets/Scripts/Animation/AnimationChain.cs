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
        ResisterEvents();
    }

    public void Play()
    {
        chainNodes[0].GetComponent<Animator>().SetTrigger(triggerName);
    }

    void ResisterEvents()
    {
        foreach (AnimationChainNode chainNode in chainNodes)
        {
            chainNode.OnNextAnimationRequested += PlayNextAnimationNode;
        }
    }

    void PlayNextAnimationNode(AnimationChainNode callerNode)
    {
        int nextIndex = chainNodes.IndexOf(callerNode) + 1;

        if (nextIndex >= chainNodes.Count) {
            OnEnded?.Invoke();
            return;
        }

        AnimationChainNode nextNode = chainNodes[nextIndex];
        Animator nextAnimator = nextNode.GetComponent<Animator>();
        nextAnimator.SetTrigger(triggerName);
    }
}

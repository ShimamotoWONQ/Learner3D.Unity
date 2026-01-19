using System;
using UnityEngine;

[RequireComponent( typeof(Animator) )]
public class AnimationChainNode : MonoBehaviour
{

    public event Action<AnimationChainNode> OnNextAnimationRequested;

    public void IdentifySelf()
    {
        Debug.Log(gameObject.name);
    }

    public void RequestNextAnimation()
    {
    	OnNextAnimationRequested?.Invoke(this);
    }
}

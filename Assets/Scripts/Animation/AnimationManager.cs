using System;
using UnityEngine;

[RequireComponent( typeof(Animation) )]
public class AnimationManager : MonoBehaviour
{
    public event Action OnAllAnimationEnded;

    [SerializeField] ObjectManager objectManager;
    [SerializeField] float autoSkipTime;

    AnimationEndNotifier[] _currentNotifiers;
    Action[] _registeredHandlers;

    public void Init()
    {

    }

    public void PlayAnimation(int stepIndex)
    {
        UnregisterPreviousListeners();

        _currentNotifiers = objectManager.stepObjectHolderList[stepIndex].gameObject.GetComponentsInChildren<AnimationEndNotifier>();

        if (_currentNotifiers.Length == 0)
        {
            Invoke(nameof(EndAnimation), autoSkipTime);
            return;
        }

        // リスナーを登録し、参照を保存
        _registeredHandlers = new Action[_currentNotifiers.Length];
        for (int i = 0; i < _currentNotifiers.Length; i++)
        {
            int index = i; // クロージャ用にローカルコピー
            _registeredHandlers[i] = () => CheckIfAllAnimationEnded();
            _currentNotifiers[index].OnAnimationEnded += _registeredHandlers[i];
        }
    }

    void UnregisterPreviousListeners()
    {
        if (_currentNotifiers == null || _registeredHandlers == null)
            return;

        for (int i = 0; i < _currentNotifiers.Length; i++)
        {
            if (_currentNotifiers[i] != null && _registeredHandlers[i] != null)
            {
                _currentNotifiers[i].OnAnimationEnded -= _registeredHandlers[i];
            }
        }

        _currentNotifiers = null;
        _registeredHandlers = null;
    }

    void CheckIfAllAnimationEnded()
    {
        if (_currentNotifiers == null)
            return;

        foreach (AnimationEndNotifier notifier in _currentNotifiers)
        {
            if (!notifier.isAnimationEnded)
                return;
        }

        EndAnimation();
    }

    void EndAnimation()
    {
        UnregisterPreviousListeners();
        OnAllAnimationEnded?.Invoke();
    }

    void OnDestroy()
    {
        UnregisterPreviousListeners();
    }
}

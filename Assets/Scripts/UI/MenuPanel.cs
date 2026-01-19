using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{

    public event Action OnResumeButtonClicked;
    public event Action OnQuitButtonClicked;
    public event Action OnNextStepButtonClicked;
    public event Action OnPrevStepButtonClicked;
    public event Action OnPlayerCameraButtonClicked;
    public event Action OnFixedCameraButtonClicked;
    public event Action OnRepositionCameraButtonClicked;
    public event Action<bool> OnCommentVisibilityToggled;

    [SerializeField] Button resumeButton;
    [SerializeField] Button quitButton;
    [SerializeField] Button nextStepButton;
    [SerializeField] Button prevStepButton;
    [SerializeField] Button playerCameraButton;
    [SerializeField] Button fixedCameraButton;
    [SerializeField] Button repositionCameraButton;
    [SerializeField] Toggle commentVisibilityToggle;

    public void Init()
    {
        ResisterEvents();
    }

    void ResisterEvents()
    {
        resumeButton.onClick.AddListener( () => OnResumeButtonClicked?.Invoke() );
        quitButton.onClick.AddListener( () => OnQuitButtonClicked?.Invoke() );
        nextStepButton.onClick.AddListener( () => OnNextStepButtonClicked?.Invoke() );
        prevStepButton.onClick.AddListener( () => OnPrevStepButtonClicked?.Invoke() );
        playerCameraButton.onClick.AddListener( () => OnPlayerCameraButtonClicked?.Invoke() );
        fixedCameraButton.onClick.AddListener( () => OnFixedCameraButtonClicked?.Invoke() );
        repositionCameraButton.onClick.AddListener( () => OnRepositionCameraButtonClicked?.Invoke() );
        commentVisibilityToggle.onValueChanged.AddListener( commentVisibility => OnCommentVisibilityToggled?.Invoke(commentVisibility) );
    }
}

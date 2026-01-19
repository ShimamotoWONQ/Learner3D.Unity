using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControlSidebar : MonoBehaviour
{
    public event Action OnNextStepButtonClicked;
    public event Action OnPrevStepButtonClicked;
    public event Action OnReloadButtonClicked;
    public event Action OnUnloadButtonClicked;
    public event Action OnInspecterButtonClicked;
    public event Action OnPlayerCameraButtonClicked;
    public event Action OnFixedCameraButtonClicked;
    public event Action OnFreezeCameraButtonClicked;
    public event Action OnRepositionCameraButtonClicked;
    public event Action OnTestButtonClicked;

    [SerializeField] Button nextStepButton;
    [SerializeField] Button prevStepButton;
    [SerializeField] Button reloadButton;
    [SerializeField] Button unloadButton;
    [SerializeField] Button inspecterButton;
    [SerializeField] Button playerCameraButton;
    [SerializeField] Button fixedCameraButton;
    [SerializeField] Button freezeCameraButton;
    [SerializeField] Button repositionCameraButton;
    [SerializeField] Button testButton;
    [SerializeField] TMP_Text consoleText;
    [SerializeField] TMP_Text stepViewerText;

    void Start()
    {
        ResisterEvents();

        OnTestButtonClicked += TestAction;
    }

    public void WriteConsole (string text)
    {
        consoleText.text += text + "\n";
    }

    public void WriteStepViewer (string text)
    {
        stepViewerText.text = text;
    }

    void ResisterEvents()
    {
        nextStepButton.onClick.AddListener( () => OnNextStepButtonClicked?.Invoke() );
        prevStepButton.onClick.AddListener( () => OnPrevStepButtonClicked?.Invoke() );
        reloadButton.onClick.AddListener( () => OnReloadButtonClicked?.Invoke() );
        unloadButton.onClick.AddListener( () => OnUnloadButtonClicked?.Invoke() );
        inspecterButton.onClick.AddListener( () => OnInspecterButtonClicked?.Invoke() );
        playerCameraButton.onClick.AddListener( () => OnPlayerCameraButtonClicked?.Invoke() );
        fixedCameraButton.onClick.AddListener( () => OnFixedCameraButtonClicked?.Invoke() );
        freezeCameraButton.onClick.AddListener( () => OnFreezeCameraButtonClicked?.Invoke() );
        repositionCameraButton.onClick.AddListener( () => OnRepositionCameraButtonClicked?.Invoke() );
        testButton.onClick.AddListener( () => OnTestButtonClicked?.Invoke() );
    }

    void TestAction () {

    }

}

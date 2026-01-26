using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Step navigation events
    public event Action OnNextStepKeyPressed;
    public event Action OnPrevStepKeyPressed;
    public event Action OnInspectorKeyPressed;

    // Camera events
    public event Action OnToggleCameraKeyPressed;
    public event Action OnRepositionCameraKeyPressed;

    // UI events
    public event Action OnMenuKeyPressed;
    public event Action OnToggleSidebarKeyPressed;
    public event Action OnFullscreenKeyPressed;

    bool isInputEnabled = true;

    public void EnableInput()
    {
        isInputEnabled = true;
    }

    public void DisableInput()
    {
        isInputEnabled = false;
    }

    void Update()
    {
        if (!isInputEnabled)
            return;

        HandleStepInput();
        HandleCameraInput();
        HandleUIInput();
    }

    void HandleStepInput()
    {
        if (Input.GetKeyDown(KeyCode.L))
            OnNextStepKeyPressed?.Invoke();

        if (Input.GetKeyDown(KeyCode.J))
            OnPrevStepKeyPressed?.Invoke();

        if (Input.GetKeyDown(KeyCode.I))
            OnInspectorKeyPressed?.Invoke();
    }

    void HandleCameraInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
            OnToggleCameraKeyPressed?.Invoke();

        if (Input.GetKeyDown(KeyCode.R))
            OnRepositionCameraKeyPressed?.Invoke();
    }

    void HandleUIInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OnMenuKeyPressed?.Invoke();

        if (Input.GetKeyDown(KeyCode.C))
            OnToggleSidebarKeyPressed?.Invoke();

        if (Input.GetKeyDown(KeyCode.F))
            OnFullscreenKeyPressed?.Invoke();
    }
}

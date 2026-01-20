using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] PlayerCamera playerCamera;
    [SerializeField] FixedCamera fixedCamera;

    [SerializeField] MenuPanel menuPanel;
    [SerializeField] InputManager inputManager;

    [SerializeField] Vector3 defaultPlayerCameraPosition;
    [SerializeField] Vector3 defaultPlayerCameraRotation;
    [SerializeField] Vector3 defaultFixedCameraPosition;
    [SerializeField] Vector3 defaultFixedCameraRotation;

    public Camera activeCamera;

    public void Init()
    {
        playerCamera.Init();
        fixedCamera.Init();

        RepositionCameras();
        EnablePlayerCamera();
        EnableUserInput();

        menuPanel.OnPlayerCameraButtonClicked += EnablePlayerCamera;
        menuPanel.OnFixedCameraButtonClicked += EnableFixedCamera;
        menuPanel.OnRepositionCameraButtonClicked += RepositionCameras;

        inputManager.OnToggleCameraKeyPressed += ToggleCamera;
        inputManager.OnRepositionCameraKeyPressed += RepositionCameras;
    }

    public void RepositionCameras()
    {
        playerCamera.transform.position = defaultPlayerCameraPosition;
        playerCamera.camera.transform.rotation = Quaternion.Euler(defaultPlayerCameraRotation);

        fixedCamera.transform.position = defaultFixedCameraPosition;
        fixedCamera.camera.transform.rotation = Quaternion.Euler(defaultFixedCameraRotation);
    }

    public void ConfigureGravityUse(bool doEnable)
    {
        if (activeCamera == fixedCamera.camera) return;

        playerCamera.ConfigureGravityUse(doEnable);
    }

    void ToggleCamera()
    {
        if (activeCamera == playerCamera.camera) EnableFixedCamera();
        else EnablePlayerCamera();
    }

    void EnablePlayerCamera()
    {
        playerCamera.gameObject.SetActive(true);
        fixedCamera.gameObject.SetActive(false);
        activeCamera = playerCamera.camera;
    }

    void EnableFixedCamera()
    {
        fixedCamera.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(false);
        activeCamera = fixedCamera.camera;
    }

    public void DisableUserInput()
    {
        playerCamera.doDisableInput = true;
        fixedCamera.doDisableInput = true;
        // Cursor.visible = true;
    }

    public void EnableUserInput()
    {
        playerCamera.doDisableInput = false;
        fixedCamera.doDisableInput = false;
        // Cursor.visible = false;
    }

    void OnDestroy()
    {
        if (menuPanel != null)
        {
            menuPanel.OnPlayerCameraButtonClicked -= EnablePlayerCamera;
            menuPanel.OnFixedCameraButtonClicked -= EnableFixedCamera;
            menuPanel.OnRepositionCameraButtonClicked -= RepositionCameras;
        }

        if (inputManager != null)
        {
            inputManager.OnToggleCameraKeyPressed -= ToggleCamera;
            inputManager.OnRepositionCameraKeyPressed -= RepositionCameras;
        }
    }
}

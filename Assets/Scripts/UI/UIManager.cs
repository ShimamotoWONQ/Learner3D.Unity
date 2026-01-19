using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] CommentSidebar commentSidebar;
    [SerializeField] CommentManager commentManager;
    [SerializeField] GuideSidebar guideSidebar;
    [SerializeField] MenuPanel menuPanel;

    [SerializeField] JSInterface jsInterface;

    [SerializeField] TMP_Text stepTitleViewer;
    [SerializeField] CameraManager cameraManager;

    public void Init()
    {
        jsInterface.OnToggleMenuRequested += ToggleMenuPanel;
        menuPanel.OnResumeButtonClicked += ToggleMenuPanel;

        Screen.fullScreenMode = FullScreenMode.Windowed;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ToggleMenuPanel();

        if (Input.GetKeyDown(KeyCode.C))
            ToggleSidebar();

        if (Input.GetKeyDown(KeyCode.F)) {
            Screen.fullScreen = !Screen.fullScreen;
            cameraManager.EnableUserInput();
        }
    }

    public void LoadUI(string stepTitle)
    {
        stepTitleViewer.text = stepTitle;

        if (commentSidebar.gameObject.activeInHierarchy)
            commentManager.OpenAllComments();
        else
            commentManager.CloseAllComments();
    }

    void ToggleSidebar()
    {
        if (commentSidebar.gameObject.activeInHierarchy)
        {
            commentSidebar.gameObject.SetActive(false);
            guideSidebar.gameObject.SetActive(true);
            commentManager.CloseAllComments();
        }
        else
        {
            commentSidebar.gameObject.SetActive(true);
            guideSidebar.gameObject.SetActive(false);
            commentManager.OpenAllComments();
        }
    }

    void ToggleMenuPanel()
    {
        if (menuPanel.gameObject.activeInHierarchy)
            HideMenuPanel();

        else
            ShowMenuPanel();
    }

    void ShowMenuPanel()
    {
        menuPanel.gameObject.SetActive(true);
        cameraManager.DisableUserInput();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HideMenuPanel()
    {
        menuPanel.gameObject.SetActive(false);
        cameraManager.EnableUserInput();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

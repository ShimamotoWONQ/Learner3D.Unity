using UnityEngine;

public class StepManager : MonoBehaviour
{
    [SerializeField] StepDetailData stepDetailData;
    [SerializeField] JSInterface jsInterface;

    [SerializeField] CameraManager cameraManager;

    [SerializeField] TerrainManager terrainManager;
    [SerializeField] ObjectManager objectManager;

    [SerializeField] CommentManager commentManager;
    [SerializeField] NoteManager noteManager;

    [SerializeField] AnimationManager animationManager;
    [SerializeField] UIManager uiManager;

    [SerializeField] CommentSidebar commentSidebar;
    [SerializeField] MenuPanel menuPanel;
    
    [SerializeField] InputManager inputManager;

    int maxStepIndex;
    int currentStepIndex;

    void OnValidate()
    {
        objectManager.Init(stepDetailData.list);
    }

    void Start()
    {
        RegisterEvents();
        
        jsInterface.NotifySceneLoaded();

        #if UNITY_EDITOR && UNITY_WEBGL

            LoadConfig loadConfig = new LoadConfig();
            loadConfig.stepIndex = 0;
            loadConfig.doShowComment = true;
            loadConfig.doAllowCommentVisibilityControl = true;

            Init(loadConfig);

        #endif
    }

    void Init(LoadConfig loadConfig)
    {
        currentStepIndex = 0;
        maxStepIndex = stepDetailData.list.Count;

        if (maxStepIndex == 0)
        {
            Debug.LogError("[StepManager] Init: stepDetailData.list is empty");
            return;
        }

        cameraManager.Init();
        terrainManager.Init();
        commentManager.Init(loadConfig.doShowComment, loadConfig.doAllowCommentVisibilityControl);
        noteManager.Init(loadConfig.doShowComment, loadConfig.doAllowCommentVisibilityControl);
        animationManager.Init();
        uiManager.Init();

        commentSidebar.Init();
        menuPanel.Init();

        int validStepIndex = ValidateStepIndex(loadConfig.stepIndex);
        LoadStep(validStepIndex);
    }

    int ValidateStepIndex(int stepIndex)
    {
        if (stepIndex < 0 || stepIndex >= maxStepIndex)
        {
            Debug.LogWarning($"[StepManager] Invalid stepIndex: {stepIndex}. Valid range: 0-{maxStepIndex - 1}. Using 0.");
            return 0;
        }
        return stepIndex;
    }

    void RegisterEvents()
    {
        jsInterface.OnLoadStepRequested += Init;
        jsInterface.OnUnloadStepRequested += UnloadStep;

        // animationManager.OnAllAnimationEnded += SkipToNextStep;

        menuPanel.OnNextStepButtonClicked += SkipToNextStep;
        menuPanel.OnPrevStepButtonClicked += SkipToPrevStep;
        menuPanel.OnQuitButtonClicked += UnloadStep;

        inputManager.OnNextStepKeyPressed += SkipToNextStep;
        inputManager.OnPrevStepKeyPressed += SkipToPrevStep;
        inputManager.OnInspectorKeyPressed += EnterInspector;
    }

    void LoadStep(int stepIndex)
    {
        if (stepIndex < 0 || stepIndex >= maxStepIndex)
        {
            Debug.LogError($"[StepManager] LoadStep: Invalid stepIndex {stepIndex}");
            return;
        }

        currentStepIndex = stepIndex;
        StepDetail stepDetail = stepDetailData.list[stepIndex];

        if (stepDetail.doRepositionCameras)
            cameraManager.RepositionCameras();

        terrainManager.EnableTerrain(stepDetail.terrainState);
        objectManager.ShowObjects(stepIndex);

        commentManager.ShowComments(stepIndex);
        commentSidebar.CreateCommentIndexes(stepIndex);
        noteManager.ShowNotes(stepIndex);

        uiManager.LoadUI(stepDetail.title);

        if (stepDetail.isAnimated)
            animationManager.PlayAnimation(stepIndex);

        jsInterface.NotifyStepLoaded(stepIndex);
    }

    public void UnloadStep()
    {
        currentStepIndex = 0;
        uiManager.HideMenuPanel();
        jsInterface.NotifyStepEnded(currentStepIndex);
    }

    public void SkipToNextStep()
    {
        currentStepIndex = (currentStepIndex + 1) % maxStepIndex;
        LoadStep(currentStepIndex);
    }

    public void SkipToPrevStep()
    {
        currentStepIndex = (currentStepIndex - 1 + maxStepIndex) % maxStepIndex;
        LoadStep(currentStepIndex);
    }

    public void EnterInspector()
    {
        cameraManager.RepositionCameras();
        terrainManager.activeTerrain.EnableSecondaryLayer();
    }

    void OnDestroy()
    {
        if (jsInterface != null)
        {
            jsInterface.OnLoadStepRequested -= Init;
            jsInterface.OnUnloadStepRequested -= UnloadStep;
        }

        if (menuPanel != null)
        {
            menuPanel.OnNextStepButtonClicked -= SkipToNextStep;
            menuPanel.OnPrevStepButtonClicked -= SkipToPrevStep;
            menuPanel.OnQuitButtonClicked -= UnloadStep;
        }

        if (inputManager != null)
        {
            inputManager.OnNextStepKeyPressed -= SkipToNextStep;
            inputManager.OnPrevStepKeyPressed -= SkipToPrevStep;
            inputManager.OnInspectorKeyPressed -= EnterInspector;
        }
    }
}

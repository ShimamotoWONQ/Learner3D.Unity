using System.Collections.Generic;
using UnityEngine;

public class CommentManager : MonoBehaviour
{
    [SerializeField] CameraManager cameraManager;
    [SerializeField] ObjectManager objectManager;

    [SerializeField] CommentDetailData commentDetailData;
    [SerializeField] GameObject commentCanvasPrefab;
    [SerializeField] CommentSidebar commentSidebar;

    [SerializeField] MenuPanel menuPanel;

    public Camera targetCamera;
    List<CommentCanvas> commentCanvasList;
    bool doShowComment;

    public void Init(bool doShowCommentInitially, bool doAllowCommentVisibilityControl)
    {
        menuPanel.OnCommentVisibilityToggled += ToggleCommentsVisibility;

        targetCamera = cameraManager.activeCamera;
        commentCanvasList = new List<CommentCanvas>();
        doShowComment = doShowCommentInitially;

        SpawnAllComments();

        if (!doShowCommentInitially)
            HideAllComments();
    }

    void SpawnAllComments()
    {
        foreach (CommentDetail detail in commentDetailData.list)
        {
            CommentCanvas commentCanvas = CreateCommentCanvas(detail);
            StepObjectHolder holder = objectManager.stepObjectHolderList[detail.stepIndex];
            commentCanvas.transform.SetParent(holder.transform);
            commentCanvasList.Add(commentCanvas);
        }
    }

    CommentCanvas CreateCommentCanvas(CommentDetail detail)
    {
        GameObject commentCanvasObject = Instantiate(
                commentCanvasPrefab,
                detail.position,
                Quaternion.identity,
                transform.parent.transform
            );

        CommentCanvas commentCanvas = commentCanvasObject.GetComponent<CommentCanvas>();
        commentCanvas.Init(detail.stepIndex, detail.title, detail.text, targetCamera);

        if (string.IsNullOrEmpty(detail.URL))
            commentCanvas.DisableURLContainer();
        else
            commentCanvas.SetURL(detail.URLTitle, detail.URL);

        return commentCanvas;
    }

    public void ShowComments(int stepIndex)
    {
        if (!doShowComment)
            return;

        var canvasesToShow = objectManager.stepObjectHolderList[stepIndex].GetComponentsInChildren<CommentCanvas>();

        foreach (CommentCanvas canvas in canvasesToShow)
        {
            canvas.gameObject.SetActive(true);
        }
    }

    void ToggleCommentsVisibility(bool doShowToggle)
    {
        if (doShowToggle) {
            ShowAllComments();
            doShowComment = true;
        }
        else
        {
            HideAllComments();
            doShowComment = false;
        }
    }

    void ShowAllComments()
    {
        foreach (CommentCanvas canvas in commentCanvasList)
        {
            canvas.gameObject.SetActive(true);
        }
    }

    public void HideAllComments()
    {
        foreach (CommentCanvas canvas in commentCanvasList)
        {
            canvas.gameObject.SetActive(false);
        }
    }

    public void OpenAllComments()
    {
        foreach (CommentCanvas canvas in commentCanvasList)
        {
            if (canvas.gameObject.activeInHierarchy)
                canvas.Open();
        }
    }

    public void CloseAllComments()
    {
        foreach (CommentCanvas canvas in commentCanvasList)
        {
            if (canvas.gameObject.activeInHierarchy)
                canvas.Close();
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent( typeof(Canvas), typeof(CanvasRotator) )]
[RequireComponent( typeof(GraphicRaycaster) )]
public class CommentCanvas : MonoBehaviour
{
    [SerializeField] URLOpener urlOpener;

    [SerializeField] GameObject commentIcon;
    [SerializeField] GameObject commentContent;
    [SerializeField] TMP_Text commentTitle;
    [SerializeField] TMP_Text commentText;
    [SerializeField] Button closeButton;

    public int stepIndex;
    Camera targetCamera;
    CanvasRotator canvasRotator;
    GraphicRaycaster graphicRaycaster;
    PointerEventData pointerEventData;

    public void Init(int stepIndex, string title, string text, Camera targetCamera)
    {
        this.stepIndex = stepIndex;
        commentTitle.text = title;
        commentText.text = text;

        this.targetCamera = targetCamera;
        gameObject.GetComponent<Canvas>().worldCamera = targetCamera;

        canvasRotator = GetComponent<CanvasRotator>();
        canvasRotator.Init(targetCamera);

        graphicRaycaster = gameObject.GetComponent<GraphicRaycaster>();
        pointerEventData = new PointerEventData(EventSystem.current);

        closeButton.onClick.AddListener( () => Close() );
    }

   void Update()
    {
        if (Input.GetMouseButtonDown(0)) CheckForClick();
    }

    void CheckForClick()
    {
        Vector3 centerOfScreen = targetCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f));
        pointerEventData.position = new Vector2(centerOfScreen.x, centerOfScreen.y);

        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerEventData, results);

        if (results.Count == 0) return;

        if (commentIcon.activeInHierarchy)
        {
            Open();
            return;
        }

        if (commentContent.activeInHierarchy)
        {
            foreach (RaycastResult result in results)
            {
                if (result.gameObject == closeButton.gameObject)
                {
                    Close();
                    return;
                }
            }
        }
    }

    public void Open()
    {
        commentIcon.SetActive(false);
        commentContent.SetActive(true);
    }

    public void Close()
    {
        commentIcon.SetActive(true);
        commentContent.SetActive(false);
    }

    public void SetURL(string URLTitle, string URL)
    {
        urlOpener.Init(URLTitle, URL, targetCamera, graphicRaycaster);
    }

    public void DisableURLContainer()
    {
        urlOpener.gameObject.SetActive(false);
    }
}

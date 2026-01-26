using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class URLOpener : MonoBehaviour
{

    [SerializeField] TMP_Text URLViewer;

    Camera targetCamera;
    GraphicRaycaster graphicRaycaster;
    PointerEventData pointerEventData;
    readonly List<RaycastResult> raycastResults = new List<RaycastResult>();
    Vector2 screenCenter;
    string URL;

    public void Init(string URLTitle, string URL, Camera targetCamera, GraphicRaycaster graphicRaycaster)
    {
        this.URL = URL;
        this.targetCamera = targetCamera;
        this.graphicRaycaster = graphicRaycaster;

        URLViewer.text = URLTitle;

        pointerEventData = new PointerEventData(EventSystem.current);
        CacheScreenCenter();
    }

    void CacheScreenCenter()
    {
        Vector3 viewportCenter = targetCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f));
        screenCenter = new Vector2(viewportCenter.x, viewportCenter.y);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) CheckForClick();
    }

    void CheckForClick()
    {
        pointerEventData.position = screenCenter;

        raycastResults.Clear();
        graphicRaycaster.Raycast(pointerEventData, raycastResults);

        foreach (RaycastResult result in raycastResults)
        {
            if (result.gameObject == gameObject)
            {
                OpenURL();
                return;
            }
        }
    }

    public void OpenURL()
    {
        Application.OpenURL(URL);
    }
}

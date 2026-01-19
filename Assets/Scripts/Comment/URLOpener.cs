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
    string URLTitle;
    string URL;

    public void Init(string URLTitle, string URL, Camera targetCamera, GraphicRaycaster graphicRaycaster)
    {
        this.URL = URL;
        this.URLTitle = URLTitle;
        this.targetCamera = targetCamera;
        this.graphicRaycaster = graphicRaycaster;

        URLViewer.text = URLTitle;

        pointerEventData = new PointerEventData(EventSystem.current);
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

        foreach (RaycastResult result in results)
        {
            if (result.gameObject == gameObject) OpenURL();
        }
    }

    public void OpenURL()
    {
        Application.OpenURL(URL);
    }
}

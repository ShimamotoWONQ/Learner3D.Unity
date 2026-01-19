using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent( typeof(Canvas), typeof(CanvasRotator) )]
public class NoteCanvas : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] TMP_Text noteTitle;
    [SerializeField] Image noteContainer;

    [Header("Color Settings")]
    [SerializeField] Color yellowContainerColor = new Color(0.85f, 0.75f, 0.18f, 0.8f);
    [SerializeField] Color yellowTextColor = Color.black;
    [SerializeField] Color blackContainerColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
    [SerializeField] Color blackTextColor = Color.white;

    public int stepIndex;
    CanvasRotator canvasRotator;

    public void Init(int stepIndex, string title, Camera targetCamera, NoteColor noteColor)
    {
        this.stepIndex = stepIndex;
        noteTitle.text = title;

        gameObject.GetComponent<Canvas>().worldCamera = targetCamera;

        canvasRotator = GetComponent<CanvasRotator>();
        canvasRotator.Init(targetCamera);

        switch (noteColor)
        {
            case NoteColor.Yellow:
                noteContainer.color = yellowContainerColor;
                noteTitle.color = yellowTextColor;
                break;

            case NoteColor.Black:
                noteContainer.color = blackContainerColor;
                noteTitle.color = blackTextColor;
                break;

            default:
                break;
        }
    }
}

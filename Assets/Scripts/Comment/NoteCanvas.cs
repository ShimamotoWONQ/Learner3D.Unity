using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent( typeof(Canvas), typeof(CanvasRotator) )]
public class NoteCanvas : MonoBehaviour
{
    [SerializeField] TMP_Text noteTitle;
    [SerializeField] Image noteContainer;

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
                noteContainer.color = new Color(r: 0.85f, g: 0.75f, b: 0.18f, a: 0.8f);
                noteTitle.color = Color.black;

                break;

            case NoteColor.Black:
                noteContainer.color = new Color(r: 0.2f, g: 0.2f, b: 0.2f, a: 0.8f);
                noteTitle.color = Color.white;

                break;

            default:
                break;

        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    [SerializeField] ObjectManager objectManager;
    [SerializeField] CameraManager cameraManager;
    [SerializeField] NoteDetailData noteDetailData;

    [SerializeField] GameObject noteCanvasPrefab;
    [SerializeField] MenuPanel menuPanel;

    public Camera targetCamera;
    List<NoteCanvas> noteCanvasList;
    bool doShowNote;

    public void Init(bool doShowNoteInitially, bool doAllowNoteVisibilityControl)
    {
        menuPanel.OnCommentVisibilityToggled += ToggleNotesVisibility;

        targetCamera = cameraManager.activeCamera;
        noteCanvasList = new List<NoteCanvas>();
        doShowNote = doShowNoteInitially;

        SpawnAllNotes();

        if (!doShowNoteInitially)
            HideAllNotes();
    }

    void SpawnAllNotes()
    {
        foreach (NoteDetail detail in noteDetailData.list)
        {
            if (detail.doShowAlways)
            {
                NoteCanvas noteCanvas = CreateNoteCanvas(detail);
                GlobalObjectHolder holder = objectManager.globalObjectHolder;
                noteCanvas.transform.SetParent(holder.transform);
                noteCanvasList.Add(noteCanvas);
                
                continue;
            }

            foreach (int stepIndex in detail.stepIndices)
            {
                NoteCanvas noteCanvas = CreateNoteCanvas(detail);
                StepObjectHolder holder = objectManager.stepObjectHolderList[stepIndex];
                noteCanvas.transform.SetParent(holder.transform);
                noteCanvasList.Add(noteCanvas);
            }
        }
    }
    
    NoteCanvas CreateNoteCanvas(NoteDetail detail)
    {
        GameObject commentCanvasObject = Instantiate(
            noteCanvasPrefab,
            detail.position,
            Quaternion.identity,
            transform.parent.transform
        );

        NoteCanvas noteCanvas = commentCanvasObject.GetComponent<NoteCanvas>();
        noteCanvas.Init(detail.stepIndex, detail.title, targetCamera, detail.noteColor);

        return noteCanvas;
    }

    public void ShowNotes(int stepIndex)
    {
        if (!doShowNote)
            return;

        var notesToShow = objectManager.stepObjectHolderList[stepIndex].GetComponentsInChildren<NoteCanvas>();

        foreach (NoteCanvas canvas in notesToShow)
        {
            canvas.gameObject.SetActive(true);
        }
    }

    void ToggleNotesVisibility(bool doShowToggle)
    {
        if (doShowToggle) {
            ShowAllNotes();
            doShowNote = true;
        }
        else
        {
            HideAllNotes();
            doShowNote = false;
        }
    }

    void ShowAllNotes()
    {
        foreach (NoteCanvas canvas in noteCanvasList)
        {
            canvas.gameObject.SetActive(true);
        }
    }

    public void HideAllNotes()
    {
        foreach (NoteCanvas canvas in noteCanvasList)
        {
            canvas.gameObject.SetActive(false);
        }
    }
}

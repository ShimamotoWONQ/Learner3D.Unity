using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NoteDetailData", menuName = "Scriptable Objects/NoteDetailData")]
public class NoteDetailData : ScriptableObject
{
    public List<NoteDetail> list = new List<NoteDetail>();
}

[Serializable]
public class NoteDetail
{
    public int stepIndex;
    public string title;
    public bool doShowAlways;
    [FormerlySerializedAs("stepIndexs")]
    public List<int> stepIndices;
    public Vector3 position;
    public NoteColor noteColor;
}

[Serializable]
public enum NoteColor {
    Yellow,
    Black
}
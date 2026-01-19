using System;
using System.Collections.Generic;
using UnityEngine;

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
    public List<int> stepIndexs;
    public Vector3 postion;
    public NoteColor noteColor;
}

[Serializable]
public enum NoteColor {
    Yellow,
    Black
}
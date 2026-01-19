using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StepDetailData", menuName = "Scriptable Objects/StepDetailData")]
public class StepDetailData : ScriptableObject
{
    public List<StepDetail> list = new List<StepDetail>();
}

[Serializable]
public class StepDetail
{
    public int stepIndex;
    public string title;
    public TerrainState terrainState;
    public bool isAnimated;
    public bool doRepositionCameras;
}

[Serializable]
public enum TerrainState {
    Flat,
    Half,
    Full
}


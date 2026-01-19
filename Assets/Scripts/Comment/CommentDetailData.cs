using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CommentDetailData", menuName = "Scriptable Objects/CommentDetailData")]
public class CommentDetailData : ScriptableObject
{

    public List<CommentDetail> list = new List<CommentDetail>();

}

[Serializable]
public class CommentDetail
{
    public int stepIndex;
    public string title;
    public string text;
    public string URL;
    public string URLTitle;
    public Vector3 postion;
}

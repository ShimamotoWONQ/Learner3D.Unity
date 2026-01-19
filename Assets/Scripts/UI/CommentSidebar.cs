using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CommentSidebar : MonoBehaviour
{
    [SerializeField] TMP_Text SumViewer;

    [SerializeField] GameObject parentObject;
    [SerializeField] GameObject commentIndexPrefab;

    public CommentDetailData commentDetailData;
    List<CommentIndex> commentIndexList;

    public void Init()
    {
        commentIndexList = new List<CommentIndex>();
    }

    public void CreateCommentIndexes(int stepIndex)
    {
        ClearCommentIndexes();

        List<CommentDetail> matchedCommentDetailList = commentDetailData.list.FindAll(detail => detail.stepIndex == stepIndex);

        ShowSum(matchedCommentDetailList.Count);

        foreach (CommentDetail commentDetail in matchedCommentDetailList)
        {
            GameObject go = Instantiate(
                commentIndexPrefab,
                commentDetail.postion,
                Quaternion.identity,
                parentObject.transform
            );

            CommentIndex index = go.GetComponent<CommentIndex>();
            index.title.text = commentDetail.title;
            index.content.text = commentDetail.text;

            commentIndexList.Add(index);
        }
    }

    void ClearCommentIndexes()
    {
        foreach (CommentIndex index in commentIndexList)
        {
            DestroyImmediate(index.gameObject);
        }

        commentIndexList.Clear();
    }

    void ShowSum (int sum)
    {
        SumViewer.text = sum.ToString();
    }
}

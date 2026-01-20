using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CommentSidebar : MonoBehaviour
{
    [SerializeField] TMP_Text sumViewer;

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
                commentDetail.position,
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
            Destroy(index.gameObject);
        }

        commentIndexList.Clear();
    }

    void ShowSum (int sum)
    {
        sumViewer.text = sum.ToString();
    }
}

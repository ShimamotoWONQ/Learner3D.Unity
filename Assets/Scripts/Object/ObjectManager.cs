using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] Vector3 basePosition;

    public GlobalObjectHolder globalObjectHolder;
    public List<StepObjectHolder> stepObjectHolderList;

    [SerializeField] GameObject prefab;

    public void Init(List<StepDetail> stepDetails)
    {
        RenameHolders(stepDetails);
    }

    public void RenameHolders(List<StepDetail> stepDetails)
    {
        for (int i = 0; i < stepDetails.Count; i++)
        {
            stepObjectHolderList[i].RenameSelf(i, stepDetails[i].title);
        }
    }

    public void ShowObjects(int stepIndex)
    {
        for (int i = 0; i < stepObjectHolderList.Count; i++)
        {
            if (stepIndex == i) stepObjectHolderList[i].gameObject.SetActive(true);
            else stepObjectHolderList[i].gameObject.SetActive(false);
        }
    }

    public void AdjustHeight()
    {
        globalObjectHolder.transform.position = basePosition;

        foreach (StepObjectHolder holder in stepObjectHolderList)
        {
            holder.transform.position = basePosition;
        }
    }

    public void AddPrefabToAll()
    {
        if(!prefab) return;

        foreach (StepObjectHolder holder in stepObjectHolderList)
        {
            Instantiate(prefab, holder.transform);
        }
    }
}

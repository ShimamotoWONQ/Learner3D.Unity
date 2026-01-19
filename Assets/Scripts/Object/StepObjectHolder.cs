using UnityEngine;

public class StepObjectHolder : MonoBehaviour
{
    public void RenameSelf(int index, string name)
    {
        string stepString;

        if (index < 10) stepString = "0" + index.ToString();
        else stepString = index.ToString();

        this.gameObject.name = stepString + " " + name;
    }
}

using UnityEngine;

public class StepObjectHolder : MonoBehaviour
{
    public void RenameSelf(int index, string name)
    {
        gameObject.name = $"{index:D2} {name}";
    }
}

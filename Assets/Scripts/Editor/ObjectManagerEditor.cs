using UnityEngine;
using UnityEditor;

[CustomEditor( typeof(ObjectManager) )]
public class ObjectManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ObjectManager objectManager = (ObjectManager)target;

        if (GUILayout.Button("Adjust Height"))
        {
            objectManager.AdjustHeight();
        }

        if (GUILayout.Button("Add Prefab To All"))
        {
            objectManager.AddPrefabToAll();
        }
    }
}
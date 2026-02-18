using UnityEngine;
using UnityEditor;

[CustomEditor( typeof(PrecastCulvertTerrain) )]
public class PrecastCulvertTerrainCustomInspector : Editor
{
    PrecastCulvertTerrain precastCulvertTerrain;

    void Init()
    {
        // variable target is defined in the base class, Editor
        precastCulvertTerrain = target as PrecastCulvertTerrain;
        precastCulvertTerrain.Init(
            terrainSize: 200,
            terrainHeight: 10,
            basePosition: new Vector3(0, 5f, 0)
        );
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Init With Default Value"))
        {
            Init();
            Debug.Log("Initialized Terrain With Default Value");
        }

        if (GUILayout.Button("Flatten Terrain With Default Value"))
        {
            precastCulvertTerrain.Flatten();
            Debug.Log("Flattened Terrain With Default Value");
        }

        if (GUILayout.Button("Dig Terrain (Half) With Default Value"))
        {
            precastCulvertTerrain.Flatten();
            precastCulvertTerrain.DigHalf();
            Debug.Log("Digged Terrain (Half) With Default Value");
        }

        if (GUILayout.Button("Dig Terrain (Full) With Default Value"))
        {
            precastCulvertTerrain.Flatten();
            precastCulvertTerrain.DigFull();
            Debug.Log("Digged Terrain (Full) With Default Value");
        }

        if (GUILayout.Button("Primary Layer"))
        {
            precastCulvertTerrain.EnablePrimaryLayer();
            Debug.Log("Enabled Primary Layer");
        }

        if (GUILayout.Button("Secondary Layer"))
        {
            precastCulvertTerrain.EnableSecondaryLayer();
            Debug.Log("Enabled Secondary Layer");
        }
    }
}

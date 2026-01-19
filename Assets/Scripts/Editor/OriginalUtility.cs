using UnityEngine;
using UnityEditor;

public class OriginalUtility : MonoBehaviour
{
    public void CreateMarkerSphere (Vector3 position, float scale) {

        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.transform.position = position;
        go.transform.localScale = new Vector3(scale, scale, scale);

    }

    public void MargeChildrenMesh (GameObject parent) {

        MeshFilter[] meshFilters = parent.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            if (meshFilters[i].gameObject == parent) {
                continue;
            }

            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            Destroy(meshFilters[i].gameObject);
        }

        Mesh mesh = new Mesh();
        mesh.name = parent.name;
        mesh.CombineMeshes(combine);
        parent.AddComponent<MeshFilter>().sharedMesh = mesh;
    }

    public void SaveMesh (Mesh mesh) {

        #if UNITY_EDITOR

        AssetDatabase.CreateAsset(mesh, "Assets/" + mesh.name + ".asset");
        AssetDatabase.SaveAssets();

        #endif

    }

    public Color ColorCodeToRGB (string colorCode) {

        if (ColorUtility.TryParseHtmlString(colorCode, out Color color)) {

            return color;

        }
        else
        {

            Debug.LogError("Cannot Convert " + colorCode + " to ColorCode");
            return Color.black;
        }

    }
}

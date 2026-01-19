using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PrecastCulvert : MonoBehaviour
{

    public float width = 1780f;

    public float height = 1320f;

    public float depth = 1000f;

    public float widthOffset = 140f;

    public float heightOffset = 160f;

    public float ductOffset = 150f;

    public bool saveMeshOnCreation = false;

    List<Vector3> anchors = new List<Vector3>();

    List<Vector3> clonedAnchors = new List<Vector3>();

    List<GameObject> segments = new List<GameObject>();

    void Start()
    {
        CreateObject();
    }

    public void CreateObject () {

        #region Calclate anchors for a side

        float w = width * 0.001f;
        float h = height * 0.001f;
        float d  = depth * 0.001f;

        float wo = widthOffset * 0.001f;
        float ho = heightOffset * 0.001f;
        float ducto = ductOffset * 0.001f;

        int[,] signList = new int[4, 2] {
            { -1,  1 },
            {  1,  1 },
            { -1, -1 },
            {  1, -1 }
        };

        for (int i = 0; i < 4; i++)
        {
            Vector3 masterAnchor = new Vector3(
                signList[i, 0] * w / 2,
                signList[i, 1] * h / 2,
                - d / 2
            );

            Vector3 firstAnchor = new Vector3(
                masterAnchor.x - signList[i, 0] * wo,
                masterAnchor.y - signList[i, 1] * ho,
                masterAnchor.z
            );

            Vector3 secondAnchor = new Vector3(
                masterAnchor.x - signList[i, 0] * (wo + ducto),
                masterAnchor.y - signList[i, 1] * ho,
                masterAnchor.z
            );

            Vector3 thirdAnchor = new Vector3(
                masterAnchor.x - signList[i, 0] * wo,
                masterAnchor.y - signList[i, 1] * (ho + ducto),
                masterAnchor.z
            );

            anchors.Add(masterAnchor);
            anchors.Add(firstAnchor);
            anchors.Add(secondAnchor);
            anchors.Add(thirdAnchor);

        }

        #endregion

        #region Create quad meshes of a side

        for (int i = 0; i < 2; i++)
        {
            CreateQuadMesh(
                anchors[(i * 4) + 1],
                anchors[(i * 4)],
                anchors[(i * 4) + (i + 1) * 4 + 1],
                anchors[(i * 4) + (i + 1) * 4]
            );

            CreateQuadMesh(
                anchors[(i * 8)],
                anchors[(i * 8) + 1],
                anchors[(i * 4) + 8],
                anchors[(i * 4) + 8 + 1]
            );
        }

        #endregion

        #region Create triangle meshes of a side

        for (int i = 0; i < 2; i++)
        {
            CreateTriangleMesh(
                anchors[(i * 12) + 1],
                anchors[(i * 12) + 2],
                anchors[(i * 12) + 3]
            );

            CreateTriangleMesh(
                anchors[(i + 1) * 4 + 1],
                anchors[(i + 1) * 4 + 3],
                anchors[(i + 1) * 4 + 2]
            );
        }

        #endregion

        #region Clone anchors and segments for another side

        foreach (Vector3 anchor in anchors)
        {
            Vector3 v = anchor + new Vector3(0, 0, d);
            clonedAnchors.Add(v);
        }

        foreach (GameObject segment in segments)
        {
            GameObject go = Instantiate(segment);
            go.transform.parent = gameObject.transform;
            go.transform.localPosition = - transform.position;
            go.transform.localRotation = Quaternion.Euler(0, 180f, 0);
        }

        #endregion

        #region Create quad meshes binds both side

        int[,] outerAnchors = new int [4,2] {
            {  0,  8 },
            {  8, 12 },
            { 12,  4 },
            {  4,  0 }
        };

        int[,] innerAnchors = new int [8,2] {
            {  2,  6 },
            {  6,  7 },
            {  7, 15 },
            { 15, 14 },
            { 14, 10 },
            { 10, 11 },
            { 11,  3 },
            {  3,  2 }
        };

        for (int i = 0; i < 4; i++)
        {

            CreateQuadMesh(
                anchors[outerAnchors[i, 0]],
                anchors[outerAnchors[i, 1]],
                clonedAnchors[outerAnchors[i, 0]],
                clonedAnchors[outerAnchors[i, 1]]
            );

        }

        for (int i = 0; i < 8; i++)
        {

            CreateQuadMesh(
                anchors[innerAnchors[i, 0]],
                anchors[innerAnchors[i, 1]],
                clonedAnchors[innerAnchors[i, 0]],
                clonedAnchors[innerAnchors[i, 1]]
            );

        }

        #endregion

        #region Marge all meshes

        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            if (meshFilters[i].gameObject == gameObject) {
                continue;
            }

            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            Destroy(meshFilters[i].gameObject);
        }

        Mesh mesh = new Mesh();
        mesh.name = "PrecastCulvert";
        mesh.CombineMeshes(combine);
        gameObject.GetComponent<MeshFilter>().sharedMesh = mesh;

        #endregion

        #region Save mesh to Assets floder if needed

        if (saveMeshOnCreation)
        {
            #if UNITY_EDITOR

            UnityEditor.AssetDatabase.CreateAsset(mesh, "Assets/" + mesh.name + ".asset");
            UnityEditor.AssetDatabase.SaveAssets();

            #endif
        }

        #endregion

    }


    void CreateQuadMesh (Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4) {

        Vector3[] vertices = new Vector3[4]
        {
            v1, v2, v3, v4
        };

        int[] triangles = new int[6]
        {
            0, 1, 2,
            3, 2, 1
        };

        Vector2[] uvs = new Vector2[4]
        {
            new Vector2(0f, 0f),
            new Vector2(1f, 0f),
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
        };

        GameObject go = new GameObject();
        go.transform.parent = transform;
        segments.Add(go);

        Mesh m = new Mesh();
        m.vertices = vertices;
        m.triangles = triangles;
        m.uv = uvs;
        m.RecalculateNormals();

        MeshFilter mf = go.AddComponent(typeof(MeshFilter)) as MeshFilter;
        mf.mesh = m;

    }

    void CreateTriangleMesh (Vector3 v1, Vector3 v2, Vector3 v3) {

        Vector3[] vertices = new Vector3[3]
        {
            v1, v2, v3
        };

        int[] triangles = new int[3]
        {
            0, 1, 2
        };
        
        Vector2[] uvs = new Vector2[3]
        {
            new Vector2(0f, 0f),
            new Vector2(0f, 1f),
            new Vector2(1f, 0f),
        };

        GameObject go = new GameObject();
        go.transform.parent = transform;
        segments.Add(go);

        Mesh m = new Mesh();
        m.vertices = vertices;
        m.triangles = triangles;
        m.uv = uvs;
        m.RecalculateNormals();
        
        MeshFilter mf = go.AddComponent(typeof(MeshFilter)) as MeshFilter;
        mf.mesh = m;

    }

}

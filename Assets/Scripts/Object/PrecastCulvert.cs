using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(MeshFilter) )]
[RequireComponent( typeof(MeshRenderer) )]
public class PrecastCulvert : MonoBehaviour
{
    private const float MillimeterToMeter = 0.001f;

    public float width = 1780f;
    public float height = 1320f;
    public float depth = 1000f;
    public float widthOffset = 140f;
    public float heightOffset = 160f;
    public float ductOffset = 150f;
    public bool saveMeshOnCreation = false;

    private readonly List<Vector3> anchors = new();
    private readonly List<Vector3> clonedAnchors = new();
    private readonly List<GameObject> segments = new();

    private MeshFilter _meshFilter;

    void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
    }

    void Start()
    {
        CreateObject();
    }

    public void CreateObject()
    {
        #region Clear previous data

        anchors.Clear();
        clonedAnchors.Clear();

        foreach (var segment in segments)
        {
            if (segment != null)
            {
                Destroy(segment);
            }
        }
        segments.Clear();

        #endregion

        #region Calculate anchors for a side

        float w = width * MillimeterToMeter;
        float h = height * MillimeterToMeter;
        float d = depth * MillimeterToMeter;

        float wo = widthOffset * MillimeterToMeter;
        float ho = heightOffset * MillimeterToMeter;
        float ducto = ductOffset * MillimeterToMeter;

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
                -d / 2
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
            int baseIndex = i * 4;
            int nextGroupIndex = (i + 1) * 4;

            CreateQuadMesh(
                anchors[baseIndex + 1],
                anchors[baseIndex],
                anchors[baseIndex + nextGroupIndex + 1],
                anchors[baseIndex + nextGroupIndex]
            );

            int baseIndex8 = i * 8;
            int offsetIndex = baseIndex + 8;

            CreateQuadMesh(
                anchors[baseIndex8],
                anchors[baseIndex8 + 1],
                anchors[offsetIndex],
                anchors[offsetIndex + 1]
            );
        }

        #endregion

        #region Create triangle meshes of a side

        for (int i = 0; i < 2; i++)
        {
            int triangleBaseIndex = i * 12;
            int midGroupIndex = (i + 1) * 4;

            CreateTriangleMesh(
                anchors[triangleBaseIndex + 1],
                anchors[triangleBaseIndex + 2],
                anchors[triangleBaseIndex + 3]
            );

            CreateTriangleMesh(
                anchors[midGroupIndex + 1],
                anchors[midGroupIndex + 3],
                anchors[midGroupIndex + 2]
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
            go.transform.SetParent(transform, false);
            go.transform.SetLocalPositionAndRotation(
                -transform.position,
                Quaternion.Euler(0, 180f, 0)
            );
        }

        #endregion

        #region Create quad meshes binds both side

        int[,] outerAnchors = new int[4, 2] {
            {  0,  8 },
            {  8, 12 },
            { 12,  4 },
            {  4,  0 }
        };

        int[,] innerAnchors = new int[8, 2] {
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

        #region Merge all meshes

        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            if (meshFilters[i].gameObject == gameObject)
            {
                continue;
            }

            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            Destroy(meshFilters[i].gameObject);
        }

        Mesh mesh = new Mesh { name = "PrecastCulvert" };
        mesh.CombineMeshes(combine);
        _meshFilter.sharedMesh = mesh;

        #endregion

        #region Save mesh to Assets folder if needed

        if (saveMeshOnCreation)
        {
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.CreateAsset(mesh, "Assets/" + mesh.name + ".asset");
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }

        #endregion
    }

    private GameObject CreateMeshSegment(Vector3[] vertices, int[] triangles, Vector2[] uvs)
    {
        var go = new GameObject();
        go.transform.SetParent(transform, false);
        segments.Add(go);

        var mesh = new Mesh
        {
            vertices = vertices,
            triangles = triangles,
            uv = uvs
        };
        mesh.RecalculateNormals();

        go.AddComponent<MeshFilter>().mesh = mesh;
        return go;
    }

    void CreateQuadMesh(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
    {
        Vector3[] vertices = new Vector3[4] { v1, v2, v3, v4 };

        int[] triangles = new int[6] { 0, 1, 2, 3, 2, 1 };

        Vector2[] uvs = new Vector2[4]
        {
            new Vector2(0f, 0f),
            new Vector2(1f, 0f),
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
        };

        CreateMeshSegment(vertices, triangles, uvs);
    }

    void CreateTriangleMesh(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        Vector3[] vertices = new Vector3[3] { v1, v2, v3 };

        int[] triangles = new int[3] { 0, 1, 2 };

        Vector2[] uvs = new Vector2[3]
        {
            new Vector2(0f, 0f),
            new Vector2(0f, 1f),
            new Vector2(1f, 0f),
        };

        CreateMeshSegment(vertices, triangles, uvs);
    }
}

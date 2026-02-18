using UnityEngine;

[RequireComponent( typeof(Terrain) )]
public class BaseTerrain : MonoBehaviour
{
    [SerializeField] Vector3 positionOffset = new Vector3(-0.5f, 0f, -0.5f);

    protected int terrainSize;
    protected int terrainHeight;
    protected Terrain terrain;
    protected TerrainData terrainData;
    protected int pointPerMeter;

    public void Init(int terrainSize, int terrainHeight, Vector3 basePosition)
    {
        this.terrainSize = terrainSize;
        this.terrainHeight = terrainHeight;

        terrain = GetComponent<Terrain>();
        terrainData = terrain.terrainData;
        terrainData.size = new Vector3(terrainSize, terrainHeight, terrainSize);

        pointPerMeter = terrainData.heightmapResolution / terrainSize;

        transform.position = new Vector3(-terrainSize/2, 0f, -terrainSize/2);
        transform.position += basePosition;
        transform.position += positionOffset;
    }
}

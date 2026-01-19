using UnityEngine;

[RequireComponent( typeof(Terrain) )]
public class PrecastCulvertTerrain : MonoBehaviour
{
    [SerializeField] TerrainLayer primaryTerrainLayer;
    [SerializeField] TerrainLayer secondlyTerrainLayer;

    [SerializeField] Vector3 positionOffset = new Vector3(-0.5f, 0f, -0.5f);
    [SerializeField] int w = 6;
    [SerializeField] int h = 20;
    [SerializeField] int wo = 3;
    [SerializeField] int ho = 3;
    [SerializeField] int d = 2;
    [SerializeField] int bw = 3;

    int terrainSize;
    int terrainHeight;
    float relativeHeight;
    Terrain terrain;
    TerrainData terrainData;
    int pointPerMeter;

    public void Init(int terrainSize, int terrainHeight, float defaultHeight, Vector3 basePosition)
    {
        this.terrainSize = terrainSize;
        this.terrainHeight = terrainHeight;
        this.relativeHeight = defaultHeight / terrainHeight;

        terrain = GetComponent<Terrain>();
        terrainData = terrain.terrainData;
        terrainData.size = new Vector3(terrainSize, terrainHeight, terrainSize);

        pointPerMeter = terrainData.heightmapResolution / terrainSize;

        transform.position = new Vector3(-terrainSize/2, 0f, -terrainSize/2);
        transform.position += basePosition;
        transform.position += positionOffset;
    }

    public void Flatten()
    {

        float[,] heights = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        for (int x = 0; x < heights.GetLength(1); x++)
        {
            for (int y = 0; y < heights.GetLength(0); y++)
            {
                heights[y, x] = relativeHeight;
            }
        }

        terrainData.SetHeights(0, 0, heights);
    }

    public void DigFull()
    {
        // DigByTrapezoidCentered(
        //     width: w,
        //     height: h,
        //     widthOffset: wo,
        //     heightOffset: ho,
        //     depth: d,
        //     bottomWidth: bw
        // );

        DigByTrapezoid(
            width: this.w,
            height: this.h + (terrainSize - this.h) / 2,
            widthOffset: this.wo,
            heightOffset: this.ho,
            depth: this.d,
            bottomWidth: this.bw
        );
    }

    public void DigHalf()
    {
        DigByTrapezoidCentered(
            width: w,
            height: h / 2,
            widthOffset: wo,
            heightOffset: ho + (h / 4),
            depth: d,
            bottomWidth: bw
        );
    }

    public void DigByTrapezoid(int width, int height, int widthOffset, int heightOffset, float depth, int bottomWidth)
    {
        int w = width * pointPerMeter;
        int h = height * pointPerMeter;
        int wo = ((terrainSize - width) / 2 + widthOffset) * pointPerMeter;
        int ho = heightOffset * pointPerMeter;

        float d = depth / terrainHeight;
        int bw = bottomWidth * pointPerMeter;
        int slopeWidth = (w - bw) / 2;
        float slopeGradient = d / slopeWidth;

        float[,] heights = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        int startWidth = wo;

        for (int x = startWidth; x < startWidth + slopeWidth; x++)
        {
            for (int y = ho; y < ho + h; y++)
            {
                heights[y, x] = relativeHeight - (slopeGradient * (x - startWidth));
            }
        }

        startWidth += slopeWidth;

        for (int x = startWidth; x < startWidth + bw; x++)
        {
            for (int y = ho; y < ho + h; y++)
            {
                heights[y, x] = relativeHeight - d;
            }
        }

        startWidth += bw;

        for (int x = startWidth; x < startWidth + slopeWidth; x++)
        {
            for (int y = ho; y < ho + h; y++)
            {
                heights[y, x] = relativeHeight - (slopeGradient * (slopeWidth - (x - startWidth)));
            }
        }

        terrainData.SetHeights(0, 0, heights);

    }

    void DigByTrapezoidCentered(int width, int height, int widthOffset, int heightOffset, float depth, int bottomWidth)
    {
        int w = width * pointPerMeter;
        int h = height * pointPerMeter;
        int wo = ((terrainSize - width) / 2 + widthOffset) * pointPerMeter;
        int ho = ((terrainSize - height) / 2 + heightOffset) * pointPerMeter;

        float d = depth / terrainHeight;
        int bw = bottomWidth * pointPerMeter;
        int slopeWidth = (w - bw) / 2;
        float slopeGradient = d / slopeWidth;

        float[,] heights = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        int startWidth = wo;

        for (int x = startWidth; x < startWidth + slopeWidth; x++)
        {
            for (int y = ho; y < ho + h; y++)
            {
                heights[y, x] = relativeHeight - (slopeGradient * (x - startWidth));
            }
        }

        startWidth += slopeWidth;

        for (int x = startWidth; x < startWidth + bw; x++)
        {
            for (int y = ho; y < ho + h; y++)
            {
                heights[y, x] = relativeHeight - d;
            }
        }

        startWidth += bw;

        for (int x = startWidth; x < startWidth + slopeWidth; x++)
        {
            for (int y = ho; y < ho + h; y++)
            {
                heights[y, x] = relativeHeight - (slopeGradient * (slopeWidth - (x - startWidth)));
            }
        }

        terrainData.SetHeights(0, 0, heights);

    }

    public void EnablePrimaryLayer()
    {
        terrainData.terrainLayers = new TerrainLayer[1]{ primaryTerrainLayer };
        terrain.Flush();
    }

    public void EnableSecondlyLayer()
    {
        terrainData.terrainLayers = new TerrainLayer[1]{ secondlyTerrainLayer };
        terrain.Flush();
    }
}

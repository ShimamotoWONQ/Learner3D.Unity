using UnityEngine;

[RequireComponent( typeof(Terrain) )]
public class PrecastCulvertTerrain : MonoBehaviour
{
    [SerializeField] TerrainLayer primaryTerrainLayer;
    [SerializeField] TerrainLayer secondaryTerrainLayer;

    [SerializeField] Vector3 positionOffset = new Vector3(-0.5f, 0f, -0.5f);
    [SerializeField] int culvertWidth = 6;
    [SerializeField] int culvertLength = 20;
    [SerializeField] int widthOffset = 3;
    [SerializeField] int lengthOffset = 3;
    [SerializeField] int digDepth = 2;
    [SerializeField] int trenchBottomWidth = 3;

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
        DigTrench(
            width: culvertWidth,
            length: culvertLength + (terrainSize - culvertLength) / 2,
            widthOffset: widthOffset,
            lengthOffset: lengthOffset,
            depth: digDepth,
            bottomWidth: trenchBottomWidth,
            centered: false
        );
    }

    public void DigHalf()
    {
        DigTrench(
            width: culvertWidth,
            length: culvertLength / 2,
            widthOffset: widthOffset,
            lengthOffset: lengthOffset + (culvertLength / 4),
            depth: digDepth,
            bottomWidth: trenchBottomWidth,
            centered: true
        );
    }

    /// <summary>
    /// 台形断面の溝を掘削する
    /// </summary>
    /// <param name="width">溝の上部幅（メートル）</param>
    /// <param name="length">溝の長さ（メートル）</param>
    /// <param name="widthOffset">X軸方向のオフセット（メートル）</param>
    /// <param name="lengthOffset">Z軸方向のオフセット（メートル）</param>
    /// <param name="depth">掘削深さ（メートル）</param>
    /// <param name="bottomWidth">溝の底部幅（メートル）</param>
    /// <param name="centered">長さ方向を中央揃えにするか</param>
    void DigTrench(int width, int length, int widthOffset, int lengthOffset, float depth, int bottomWidth, bool centered)
    {
        int trenchWidthPoints = width * pointPerMeter;
        int trenchLengthPoints = length * pointPerMeter;
        int startX = ((terrainSize - width) / 2 + widthOffset) * pointPerMeter;
        int startZ = centered
            ? ((terrainSize - length) / 2 + lengthOffset) * pointPerMeter
            : lengthOffset * pointPerMeter;

        float relativeDepth = depth / terrainHeight;
        int bottomWidthPoints = bottomWidth * pointPerMeter;
        int slopeWidthPoints = (trenchWidthPoints - bottomWidthPoints) / 2;
        float slopeGradient = relativeDepth / slopeWidthPoints;

        float[,] heights = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        int currentX = startX;

        // 左側斜面
        for (int x = currentX; x < currentX + slopeWidthPoints; x++)
        {
            float slopeDepth = slopeGradient * (x - currentX);
            for (int z = startZ; z < startZ + trenchLengthPoints; z++)
            {
                heights[z, x] = relativeHeight - slopeDepth;
            }
        }

        currentX += slopeWidthPoints;

        // 底面
        for (int x = currentX; x < currentX + bottomWidthPoints; x++)
        {
            for (int z = startZ; z < startZ + trenchLengthPoints; z++)
            {
                heights[z, x] = relativeHeight - relativeDepth;
            }
        }

        currentX += bottomWidthPoints;

        // 右側斜面
        for (int x = currentX; x < currentX + slopeWidthPoints; x++)
        {
            float slopeDepth = slopeGradient * (slopeWidthPoints - (x - currentX));
            for (int z = startZ; z < startZ + trenchLengthPoints; z++)
            {
                heights[z, x] = relativeHeight - slopeDepth;
            }
        }

        terrainData.SetHeights(0, 0, heights);
    }

    public void EnablePrimaryLayer()
    {
        terrainData.terrainLayers = new TerrainLayer[1]{ primaryTerrainLayer };
        terrain.Flush();
    }

    public void EnableSecondaryLayer()
    {
        terrainData.terrainLayers = new TerrainLayer[1]{ secondaryTerrainLayer };
        terrain.Flush();
    }
}

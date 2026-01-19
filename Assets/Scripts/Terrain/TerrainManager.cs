using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    [SerializeField] PrecastCulvertTerrain precastCulvertTerrain;
    [SerializeField] PrecastCulvertTerrain flatTerrain;
    [SerializeField] PrecastCulvertTerrain fullDiggedTerrain;
    [SerializeField] PrecastCulvertTerrain halfDiggedTerrain;
    [SerializeField] Vector3 basePosition;
    [SerializeField] float defaultHeight = 3f;

    public PrecastCulvertTerrain activeTerrain;

    readonly int TERRAIN_SIZE = 200;
    readonly int TERRAIN_HEIGHT = 10;

    public void Init()
    {
        flatTerrain.Init(TERRAIN_SIZE, TERRAIN_HEIGHT, defaultHeight, basePosition);
        halfDiggedTerrain.Init(TERRAIN_SIZE, TERRAIN_HEIGHT, defaultHeight, basePosition);
        fullDiggedTerrain.Init(TERRAIN_SIZE, TERRAIN_HEIGHT, defaultHeight, basePosition);
    }

    public void EnableTerrain(TerrainState terrainState)
    {
        switch (terrainState)
        {
            case TerrainState.Flat:
                flatTerrain.gameObject.SetActive(true);
                halfDiggedTerrain.gameObject.SetActive(false);
                fullDiggedTerrain.gameObject.SetActive(false);

                break;

            case TerrainState.Half:
                halfDiggedTerrain.gameObject.SetActive(true);
                flatTerrain.gameObject.SetActive(false);
                fullDiggedTerrain.gameObject.SetActive(false);

                break;

            case TerrainState.Full:
                fullDiggedTerrain.gameObject.SetActive(true);
                flatTerrain.gameObject.SetActive(false);
                halfDiggedTerrain.gameObject.SetActive(false);

                break;

            default:
                break;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    [SerializeField] PrecastCulvertTerrain flatTerrain;
    [SerializeField] PrecastCulvertTerrain halfDiggedTerrain;
    [SerializeField] PrecastCulvertTerrain fullDiggedTerrain;
    [SerializeField] Vector3 basePosition;
    [SerializeField] float defaultHeight = 3f;

    public PrecastCulvertTerrain activeTerrain;

    const int TerrainSize = 200;
    const int TerrainHeight = 10;

    Dictionary<TerrainState, PrecastCulvertTerrain> terrainMap;

    public void Init()
    {
        terrainMap = new Dictionary<TerrainState, PrecastCulvertTerrain>
        {
            { TerrainState.Flat, flatTerrain },
            { TerrainState.Half, halfDiggedTerrain },
            { TerrainState.Full, fullDiggedTerrain }
        };

        foreach (var terrain in terrainMap.Values)
        {
            terrain.Init(TerrainSize, TerrainHeight, defaultHeight, basePosition);
        }
    }

    public void EnableTerrain(TerrainState terrainState)
    {
        foreach (var (state, terrain) in terrainMap)
        {
            bool isActive = state == terrainState;
            terrain.gameObject.SetActive(isActive);
        }

        activeTerrain = terrainMap[terrainState];
    }
}

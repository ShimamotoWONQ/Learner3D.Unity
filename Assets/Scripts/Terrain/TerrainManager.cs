using System;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    [SerializeField] List<TerrainEntry> terrainEntries;
    [SerializeField] Vector3 basePosition;
    [SerializeField] int terrainSize = 200;
    [SerializeField] int terrainHeight = 20;

    public BaseTerrain activeTerrain { get; private set; }

    Dictionary<TerrainState, BaseTerrain> terrainMap;

    public void Init()
    {
        terrainMap = new Dictionary<TerrainState, BaseTerrain>();
        foreach (var entry in terrainEntries)
        {
            terrainMap[entry.state] = entry.terrain;
        }

        foreach (var terrain in terrainMap.Values)
        {
            terrain.Init(terrainSize, terrainHeight, basePosition);
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

[Serializable]
public class TerrainEntry
{
    public TerrainState state;
    public BaseTerrain terrain;
}

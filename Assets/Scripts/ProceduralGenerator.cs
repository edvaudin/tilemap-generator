using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralGenerator : MonoBehaviour
{
    private int[,] terrainMap;

    public Vector3Int tileMapSize;

    public Tilemap landMap;
    public Tilemap seaMap;
    public Tilemap forestMap;
    public Tile landTile;
    public Tile seaTile;
    public Tile forestTile;

    int width;
    int height;

    public void Simulate()
    {
        ClearMap(false);
        width = tileMapSize.x;
        height = tileMapSize.y;
        if (terrainMap == null)
        {
            terrainMap = new int[width, height];
            InitialPosition();
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (terrainMap[x, y] == 0)
                {
                    seaMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), seaTile);
                }
                else if (terrainMap[x, y] == 1)
                {
                    landMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), landTile);
                }
                else if (terrainMap[x, y] == 2)
                {
                    forestMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), forestTile);
                }
            }
        }
    }

    public void InitialPosition()
    {
        int seed = Random.Range(0, 10000);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                terrainMap[x, y] = Mathf.RoundToInt(Mathf.PerlinNoise(tileMapSize.x, tileMapSize.y));
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Simulate();
        }
        if (Input.GetMouseButtonDown(1))
        {
            ClearMap(true);
        }
    }

    public void ClearMap(bool complete)
{
    landMap.ClearAllTiles();
    seaMap.ClearAllTiles();

    if (complete)
    {
        terrainMap = null;
    }
}
}

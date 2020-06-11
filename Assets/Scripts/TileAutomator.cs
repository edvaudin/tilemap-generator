using System.CodeDom.Compiler;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileAutomator : MonoBehaviour
{
    [Range(0, 100)] public int initialChance;

    [Range(1, 8)] public int birthLimit;
    [Range(1, 8)] public int deathLimit;

    [Range(1, 10)]
    public int repetitions;

    private int[,] terrainMap;

    public Vector3Int tileMapSize;

    public Tilemap topMap;
    public Tilemap botMap;
    public Tile topTile;
    public Tile botTile;

    int width;
    int height;

    public void Simulate(int repetitions)
    {
        ClearMap(false);
        width = tileMapSize.x;
        height = tileMapSize.y;

        if (terrainMap == null)
        {
            terrainMap = new int[width, height];
            InitialPosition();
        }

        for (int i = 0; i < repetitions; i++)
        {
            terrainMap = GenerateTilePosition(terrainMap);
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (terrainMap[x, y] == 1)
                {
                    topMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), topTile);
                    botMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), botTile);
                }
            }
        }
    }
    public void InitialPosition()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                terrainMap[x, y] = Random.Range(1, 101) < initialChance ? 1 : 0;
            }
        }
    }

    public int[,] GenerateTilePosition(int[,] oldMap)
    {
        int[,] newMap = new int[width, height];
        int neighbours;
        BoundsInt bounds = new BoundsInt(-1, -1, 0, 3, 3, 1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                neighbours = 0;
                foreach (var b in bounds.allPositionsWithin)
                {
                    if (b.x == 0 && b.y == 0) continue;
                    if (x + b.x >= 0 && x + b.x < width && y + b.y >= 0 && y + b.y < height)
                    {
                        neighbours += oldMap[x + b.x, y + b.y];
                    }
                    else
                    {
                        //neighbours++;
                    }
                }
                if (oldMap[x, y] == 1)
                {
                    if (neighbours < deathLimit)
                    {
                        newMap[x, y] = 0;
                    }
                    //else if (neighbours > deathLimit + 1) //Leave this commented out to encourage land masses
                    //{
                    //    newMap[x, y] = 0;
                    //}
                    else
                    {
                        newMap[x, y] = 1;
                    }
                }
                if (oldMap[x, y] == 0)
                {
                    if (neighbours > birthLimit) // set to '>' to encourage land masses, set to '==' for Conway's GoL rules.
                    {
                        newMap[x, y] = 1;
                    }
                    else
                    {
                        newMap[x, y] = 0;
                    }
                }
            }
        }
        return newMap;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Simulate(repetitions);
        }
        if (Input.GetMouseButtonDown(1))
        {
            ClearMap(true);
        }
    }

    public void ClearMap(bool complete)
    {
        topMap.ClearAllTiles();
        botMap.ClearAllTiles();

        if (complete)
        {
            terrainMap = null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralGeneration : MonoBehaviour
{
    [SerializeField] Tilemap walls;
    [SerializeField] Tilemap ground;
    [SerializeField] Grid grid;
    [SerializeField] RuleTile[] tiles;
    int index1 = 0;
    int index2 = 0;
    int index0 = 0;
    [SerializeField] Transform player;

    // so cave doesnt generate over player
    int playerChunkX = -3;
    int playerChunkY = -3;

    int[, ,] chunks = new int[, ,] {
    {
        { 1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1 },
        { 1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1 },
        { 1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1 },
        { 1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1 },
        { 1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1 },
        { 1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1 },
        { 1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1 },
        { 1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1 },
        { 1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1 },
    },
    {
        { 1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1 },
        { 1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1 },
        { 1,1,1,1,1,0,2,0,3,0,0,1,1,1,1,1 },
        { 1,1,1,1,0,0,0,0,0,0,0,1,1,1,1,1 },
        { 1,1,1,0,0,0,0,0,0,0,0,0,0,0,1,1 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,3,0,0,2,0,0,2,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0 },
        { 0,0,3,0,0,0,0,0,2,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0 },
        { 1,1,1,0,0,0,0,0,0,0,0,0,1,1,1,1 },
        { 1,1,1,1,0,0,3,0,0,0,0,0,1,1,1,1 },
        { 1,1,1,1,1,0,0,0,0,0,0,0,1,1,1,1 },
        { 1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1 },
        { 1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1 },
    },
    {
        { 1,1,1,1,1,4,4,4,4,4,4,1,1,1,1,1 },
        { 1,1,1,1,1,4,4,4,4,4,4,1,1,1,1,1 },
        { 1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1 },
        { 1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1 },
        { 1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1 },
        { 4,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4 },
        { 4,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4 },
        { 4,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4 },
        { 4,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4 },
        { 4,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4 },
        { 4,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4 },
        { 1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1 },
        { 1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1 },
        { 1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1 },
        { 1,1,1,1,1,4,4,4,4,4,4,1,1,1,1,1 },
        { 1,1,1,1,1,4,4,4,4,4,4,1,1,1,1,1 },
    },


    };

    const int chunksLength = 3;

    int[,,] spawnChunks = new int[,,] {
    { // player spawn
        { 1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1 },
        { 1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1 },
        { 1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1 },
        { 1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1 },
        { 1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,7,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,5,0,0,0,0,6,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1 },
        { 1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1 },
        { 1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1 },
        { 1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1 },
        { 1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1 },
    },
    { // cave entrance
        { 1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1 },
        { 1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1 },
        { 1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1 },
        { 1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1 },
        { 1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,8,0,0,0,6,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 0,0,0,2,0,0,0,0,0,0,3,0,0,0,0,0 },
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        { 1,1,1,1,0,0,0,0,2,0,0,0,1,1,1,1 },
        { 1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1 },
        { 1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1 },
        { 1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1 },
        { 1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1 },
    },
    };

// Start is called before the first frame update
    void Awake()
    {
        CreateTerrain();
        CreatePlayerChunk(Random.Range(-2, 2), Random.Range(-2, 2));
        CreateCaveEntranceChunk(Random.Range(-2, 2), Random.Range(-2, 2));
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    // 16 x 16 chunks
    void CreateChunk(int x, int y) {
        index0 = Random.Range(0, chunksLength);
        for (int i = x * 32; i < (x * 32) + 32; i += 2) {
            for (int j = y * 32; j < (y * 32) + 32; j += 2) {
                GenerateTile(i, j, index1, index2, chunks);
                //print(index2);
                index2++;
            }
            index1++;
            index2 = 0;
        }
        index1 = 0;
    }

    void CreatePlayerChunk(int x, int y) {
        index0 = 0;
        for (int i = x * 32; i < (x * 32) + 32; i += 2) {
            for (int j = y * 32; j < (y * 32) + 32; j += 2) {
                GenerateTile(i, j, index1, index2, spawnChunks);
                //print(index2);
                index2++;
            }
            index1++;
            index2 = 0;
        }
        index1 = 0;
    }

    void CreateCaveEntranceChunk(int x, int y) {
        index0 = 1;
        while (x == playerChunkX && y == playerChunkY) {
            x = Random.Range(-2, 2);
            y = Random.Range(-2, 2);
        }

        for (int i = x * 32; i < (x * 32) + 32; i += 2) {
            for (int j = y * 32; j < (y * 32) + 32; j += 2) {
                GenerateTile(i, j, index1, index2, spawnChunks);
                //print(index2);
                index2++;
            }
            index1++;
            index2 = 0;
        }
        index1 = 0;
    }

    void CreateTerrain() {
        for (int i = -2; i < 2; i++) {
            for (int j = -2; j < 2; j++) {
                CreateChunk(i, j);
            }
        }
    }

    void GenerateTile(int x, int z, int index1, int index2, int[,,] chunkArray) {
        Vector3Int tilePos = grid.WorldToCell(new Vector3(x, 0, z));
        switch(chunkArray[index0, index1, index2]) {
            case 0: walls.SetTile(tilePos, null); break; // no tile
            case 1: walls.SetTile(tilePos, tiles[0]); break; // blue wall
            case 2: walls.SetTile(tilePos, tiles[1]); break; // tree
            case 3: walls.SetTile(tilePos, tiles[2]); break; // spaceflower
            case 4: ground.SetTile(tilePos, tiles[3]); break; // water
            case 5: walls.SetTile(tilePos, tiles[4]); break; // spaceship
            case 6: walls.SetTile(tilePos, tiles[5]); break; // crafting station
            case 7: walls.SetTile(tilePos, null); player.position = new Vector3(x, 0, z); break; // no tile, SpawnPlayer
            case 8: walls.SetTile(tilePos, tiles[6]); break; // cave entrance
            default: walls.SetTile(tilePos, null); break; // no tile
        }
    }
}

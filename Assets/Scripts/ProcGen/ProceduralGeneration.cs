using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class ProceduralGeneration : MonoBehaviour
{
    [SerializeField] Tilemap walls;
    [SerializeField] Tilemap ground;
    [SerializeField] Grid grid;
    [SerializeField] RuleTile[] tiles;
    [SerializeField] Tile[] groundTiles;
    //int index1 = 0;
    //int index2 = 0;
    //int index0 = 0;
    [SerializeField] Transform player;
    [SerializeField] Transform playerBody;
    PlayerStats playerStats;
    DayNightCycle dayNightCycle;
    Singleton singleton;

    // so cave doesnt generate over player
    //int playerChunkX = -3;
    //int playerChunkY = -3;

    // overworld
    int[,,] chunks;
    int[,,] spawnChunks;

    // caves
    int[,,] caveChunks;
    int[,,] caveSpawnChunks;

    private void Start() {
        if (GameObject.Find("Player") != null && GameObject.Find("Singleton") != null) {
            playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
            dayNightCycle = GameObject.Find("Singleton").GetComponent<DayNightCycle>();
            singleton = GameObject.Find("Singleton").GetComponent<Singleton>();
        }

        chunks = LoadChunksFile("/Resources/overworldChunks.txt");
        spawnChunks = LoadChunksFile("/Resources/overworldSpawnChunks.txt");
        caveChunks = LoadChunksFile("/Resources/caveChunks.txt");
        caveSpawnChunks = LoadChunksFile("/Resources/caveSpawnChunks.txt");
    }

    void GenerateTile(int x, int z, int tileID) {
        Vector3Int tilePos = grid.WorldToCell(new Vector3(x, 0, z));
        switch (tileID) {
            case 0: walls.SetTile(tilePos, null); break; // no tile
            case 1: walls.SetTile(tilePos, tiles[1]); break; // blue wall
            case 2: walls.SetTile(tilePos, tiles[2]); break; // tree
            case 3: walls.SetTile(tilePos, tiles[3]); break; // spaceflower
            case 4: ground.SetTile(tilePos, tiles[4]); break; // water
            case 5: walls.SetTile(tilePos, tiles[5]); break; // spaceship
            case 6: walls.SetTile(tilePos, tiles[6]); break; // crafting station
            //case 7: walls.SetTile(tilePos, null); player.position = new Vector3(x, 0, z); break; // no tile, SpawnPlayer
            case 7: walls.SetTile(tilePos, tiles[7]); break; // cave entrance
            case 8: walls.SetTile(tilePos, tiles[8]); break; // wood
            case 9: walls.SetTile(tilePos, tiles[9]); break; // cave exit
            case 10: walls.SetTile(tilePos, tiles[10]); break; // boulder
            case 11: ground.SetTile(tilePos, groundTiles[0]); break; // blue biome ground
            case 12: ground.SetTile(tilePos, groundTiles[1]); break; // cave biome ground
            default: walls.SetTile(tilePos, null); break; // no tile
        }
    }

    public int FindTileID(RuleTile tile) {
        return Array.IndexOf(tiles, tile);
    }

    public string SaveOverworld() {
        string dataToSave = "";
        Vector3Int tilePos = grid.WorldToCell(new Vector3(0, 0, 0));
        for (int i = -64; i < 64; i += 2) {
            for (int j = -64; j < 64; j += 2) {
                tilePos = grid.WorldToCell(new Vector3(j, 0, i));
                dataToSave += FindTileID((RuleTile)walls.GetTile(tilePos));

                if (ground.GetTile(tilePos).GetType().ToString().Equals("UnityEngine.RuleTile") && FindTileID((RuleTile) ground.GetTile(tilePos)) == 4) dataToSave += "|4,";
                else dataToSave += "|11,";
            }
        }
        //print(dataToSave);
        return dataToSave;
    }

    public string SaveCaves() {
        string dataToSave = "";
        Vector3Int tilePos = grid.WorldToCell(new Vector3(0, 0, 0));
        for (int i = -64; i < 64; i += 2) {
            for (int j = -64; j < 64; j += 2) {
                tilePos = grid.WorldToCell(new Vector3(j, 0, i));
                dataToSave += FindTileID((RuleTile)walls.GetTile(tilePos));

                if (ground.GetTile(tilePos).GetType().ToString().Equals("UnityEngine.RuleTile") && FindTileID((RuleTile)ground.GetTile(tilePos)) == 4) dataToSave += "|4,";
                else dataToSave += "|12,";
            }
        }
        //print(dataToSave);
        return dataToSave;
    }

    public string SavePlayerPosition() {
        string[] pos = new string[2];
        pos[0] = playerBody.position.x.ToString();
        pos[1] = playerBody.position.z.ToString();

        string dataToSave = string.Join(",", pos);
        //print(dataToSave);
        return dataToSave;
    }

    public string SavePlayerStats() {
        // oxygen, power, dayTimer, daycount, isDay, blocks broken, enemies slain
        string[] stats = new string[7];

        stats[0] = playerStats.GetOxygen().ToString();
        stats[1] = playerStats.GetPower().ToString();
        stats[2] = dayNightCycle.GetTimer().ToString();
        stats[3] = dayNightCycle.GetDayCount().ToString();
        stats[4] = dayNightCycle.IsDay() ? "1" : "0";
        stats[5] = singleton.GetScore().ToString();
        stats[6] = singleton.GetKillCount().ToString();

        string dataToSave = string.Join(",", stats);
        //print(dataToSave);
        return dataToSave;
    }

    public void LoadPlayerPosition(bool inOverworld) {
        string dataRead = File.ReadAllText(Application.dataPath + "/gameData.txt");
        dataRead = dataRead.Split("*")[inOverworld ? 2 : 4];
        string[] posArray = dataRead.Split(",");

        print(dataRead);

        player.position = new Vector3(float.Parse(posArray[0]), 0, float.Parse(posArray[1]));
        playerBody.position = new Vector3(float.Parse(posArray[0]), 0, float.Parse(posArray[1]));
    }

    public void LoadPlayerStats() {
        string dataRead = File.ReadAllText(Application.dataPath + "/gameData.txt");
        dataRead = dataRead.Split("*")[6];
        string[] statsArray = dataRead.Split(",");

        playerStats.SetOxygen(float.Parse(statsArray[0]));
        playerStats.SetPower(float.Parse(statsArray[1]));
        dayNightCycle.SetTimer(float.Parse(statsArray[2]));
        dayNightCycle.SetDayCount(int.Parse(statsArray[3]));
        dayNightCycle.SetIsDay(int.Parse(statsArray[4]) == 1);
        singleton.SetScore(int.Parse(statsArray[5]));
        singleton.SetKillCount(int.Parse(statsArray[6]));

    }

    public string SetPlayerPosition(float x, float z) {
        string data = x + "," + z;
        return data;
    }

    public string CreateOverWorld() {
        string[] dataToRead = new string[16 * 16 * 16];

        // main overworld
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 4; j++) {
                WriteChunk(i, j, GetSlice(Random.Range(0, chunks.GetLength(0)), chunks), dataToRead);
            }
        }

        // player spawn
        int playerIndex = Random.Range(0, 16);
        WriteChunk(playerIndex%4, playerIndex/4, GetSlice(0, spawnChunks), dataToRead);

        //cave spawn
        int caveIndex = -1;
        do {
            caveIndex = Random.Range(0, 16);
        } while (caveIndex == playerIndex);
        WriteChunk(caveIndex % 4, caveIndex / 4, GetSlice(1, spawnChunks), dataToRead);
        
        string playerPos = SetPlayerPosition(((playerIndex % 4)*32)+16-64, ((playerIndex / 4) * 32) + 16-64);

        string data = string.Join(",", dataToRead);
        data = data + "*" + playerPos;

        return data;
    }

    public string CreateCaves() {
        string[] dataToRead = new string[16 * 16 * 16];

        // main overworld
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 4; j++) {
                WriteChunk(i, j, GetSlice(Random.Range(0, caveChunks.GetLength(0)), caveChunks), dataToRead);
            }
        }

        // player spawn
        int playerIndex = Random.Range(0, 16);
        WriteChunk(playerIndex % 4, playerIndex / 4, GetSlice(0, caveSpawnChunks), dataToRead);

        string playerPos = SetPlayerPosition(((playerIndex % 4) * 32) + 16-64, ((playerIndex / 4) * 32) + 16-64);

        string data = string.Join(",", dataToRead);
        data = data + "*" + playerPos;

        return data;
    }

    void WriteChunk(int x, int y, int[,] newChunk, string[] data) {
        const int width = 64;

        // x increases by 1, y increases by 64
        for (int i = x * 16; i < (x + 1) * 16; i++) {
            for (int j = y * width * 16; j < (y + 1) * width * 16; j += width) {
                //print(i + ", " + j / width);
                data[i + j] = newChunk[i%16, (int) (j / width)%16].ToString();
            }
        }
    }

    public void LoadOverworld() {
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 4; j++) {
                LoadNewChunk(i, j, 1);
            }
        }
    }

    public void LoadCaves() {
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 4; j++) {
                LoadNewChunk(i, j, 3);
            }
        }
    }

    public void LoadNewChunk(int x, int y, int dataIndex) {
        string chunkString = File.ReadAllText(Application.dataPath + "/gameData.txt");
        string[] chunkStrings = chunkString.Split("*")[dataIndex].Split(",");
        string[] tileStrings;
        const int width = 64;

        // x increases by 1, y increases by 64
        for (int i = x * 16; i < (x+1)*16; i++) {
            for (int j = y * width*16; j < (y+1)*width*16; j+=width) {
                //print(i + ", " + j / width);
                string tileString = chunkStrings[i + j];
                if (tileString.Contains("|")) {
                     tileStrings = tileString.Split("|");
                    GenerateTile((i * 2) - 64, ((j * 2) / (width)) - 64, int.Parse(tileStrings[0]));
                    GenerateTile((i * 2) - 64, ((j * 2) / (width)) - 64, int.Parse(tileStrings[1]));
                } else {
                    GenerateTile((i * 2)-64, ((j * 2)/(width))-64, int.Parse(chunkStrings[i + j]));
                }
            }
        }
    }

    public int[,,] FillChunkText(int index, int[,,] array1, int[,] array2) {
        for (int i = 0; i < 16; i++) {
            for (int j = 0; j < 16; j++) {
                array1[index, i, j] = array2[i, j];
            }
        }

        return array1;
    }

    public int[,] GetSlice(int index, int[,,] array) {
        int[,] slice = new int[array.GetLength(1), array.GetLength(2)];
        for (int i = 0; i < array.GetLength(1); i++) {
            for (int j = 0; j < array.GetLength(2); j++) {
                slice[i, j] = array[index, i, j];
            }
        }

        return slice;
    }

    public void SetSlice(int index, int[,,] array, int[,] arrayToAdd) {

        for (int i = 0; i < array.GetLength(1); i++) {
            for (int j = 0; j < array.GetLength(2); j++) {
                array[index, i, j] = arrayToAdd[i, j];
            }
        }
    }
    public void SetSlice(int index, int[,,] array, string[] arrayToAdd) {

        for (int i = 0; i < array.GetLength(1); i++) {
            for (int j = 0; j < array.GetLength(2); j++) {
                array[index, i, j] = int.Parse(arrayToAdd[i*array.GetLength(1) + j]);
            }
        }
    }

    int[,,] LoadChunksFile(string filename) {
        string str = File.ReadAllText(Application.dataPath + filename);
        string[] data = str.Split("*");
        int[,,] array = new int[data.Length, 16, 16];

        for (int i = 0; i < data.Length; i++) {
            SetSlice(i, array, data[i].Split(","));
            //print(GetSlice(i, array).ToString());
        }
        
        return array;
    }
}

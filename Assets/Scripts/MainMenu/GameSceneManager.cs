using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    int score = 0;
    int killCount = 0;
    [SerializeField] Inventory inventory;
    [SerializeField] DayNightCycle dayNightCycle;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] ProceduralGeneration proceduralGeneration;
    TextMeshProUGUI pauseText;
    //[SerializeField] ProceduralGenerationCave proceduralGenerationCave;

    int difficultyLevel = 0;

    // Start is called before the first frame update
    void Awake()
    {
        //score = PlayerPrefs.GetInt("score");
    }

    private void Start() {
        pauseText = GameObject.Find("Paused").GetComponent<TextMeshProUGUI>();
        StartCoroutine(LoadFile());
    }

    //public void NewGame() {
    //    StartCoroutine(NewGameStart());
    //}

    //IEnumerator NewGameStart() {
    //    string[] dataToSave = new string[3];
    //    dataToSave[0] = "1|1,3|1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1";
    //    dataToSave[1] = proceduralGeneration.CreateOverWorld();
    //    dataToSave[2] = proceduralGeneration.CreateCaves();

    //    string data = string.Join("*", dataToSave);

    //    File.WriteAllText(Application.persistentDataPath + "/gameData.txt", data);

    //    yield return new WaitForEndOfFrame();
    //    StartCoroutine(LoadFile());
    //}

    //public void LoadNewScene() {
    //    SceneManager.LoadScene(1);
    //}

    public void LoadSceneByIndex(int index) {
        //inventory.SaveInventory();
        //dayNightCycle.SaveTimer();
        //playerStats.SaveStats();
        SceneManager.LoadScene(index);
        //PlayerPrefs.SetInt("score", score);
    }

    public int GetScore() {
        return score;
    }

    public void SetScore(int score) {
        this.score = score;
    }

    public int GetKillCount() {
        return killCount;
    }

    public void SetKillCount(int score) {
        killCount = score;
    }

    public bool FileExists() {
        return File.Exists(Application.persistentDataPath + "/gameData.txt");
    }

    public int GetDifficulty() {
        return difficultyLevel;
    }

    public void SetDifficulty(int difficulty) {
        difficultyLevel = difficulty;
    }

    //public bool HasCaves() {
    //    if (!FileExists()) return false;
    //    string data = File.ReadAllText(Application.persistentDataPath + "/gameData.txt");
    //    string[] dataArray = data.Split("*");

    //    return dataArray.Length < 5;
    //}

    // DataToSave Array: 0 = inventory & equipment, 1 = overworld tiles, 2 = overworld player position, 3 = cave tiles, 4 = cave player position
    //public void CreateNewFile() {
    //    string[] dataToSave = new string[3];
    //    dataToSave[0] = inventory.SaveInventory();
    //    dataToSave[1] = proceduralGeneration.CreateOverWorld(); // creates 2
    //    dataToSave[2] = proceduralGeneration.CreateCaves(); // creates 2

    //    string data = string.Join("*", dataToSave);

    //    File.WriteAllText(Application.persistentDataPath + "/gameData.txt", data);
    //}

    void SaveOverworld() {
        string gameData = File.ReadAllText(Application.persistentDataPath + "/gameData.txt");
        string[] gameDataArray = gameData.Split("*");

        string[] dataToSave = new string[8];
        dataToSave[0] = inventory.SaveInventory();
        dataToSave[1] = proceduralGeneration.SaveOverworld();
        dataToSave[2] = proceduralGeneration.SavePlayerPosition();
        dataToSave[3] = gameDataArray[3];
        dataToSave[4] = gameDataArray[4];
        dataToSave[5] = "1";
        dataToSave[6] = proceduralGeneration.SavePlayerStats();

        string data = string.Join("*", dataToSave);
        
        File.WriteAllText(Application.persistentDataPath + "/gameData.txt", data);
    }

    void SaveCaves() {
        string gameData = File.ReadAllText(Application.persistentDataPath + "/gameData.txt");
        string[] gameDataArray = gameData.Split("*");

        string[] dataToSave = new string[8];
        dataToSave[0] = inventory.SaveInventory();
        dataToSave[1] = gameDataArray[1];
        dataToSave[2] = gameDataArray[2];
        dataToSave[3] = proceduralGeneration.SaveCaves();
        dataToSave[4] = proceduralGeneration.SavePlayerPosition();
        dataToSave[5] = "4";
        dataToSave[6] = proceduralGeneration.SavePlayerStats();

        string data = string.Join("*", dataToSave);

        File.WriteAllText(Application.persistentDataPath + "/gameData.txt", data);
    }

    public void Save() {
        if (playerStats.IsDead()) return;
        if (IsInOverworld()) {
            SaveOverworld();
        } else {
            SaveCaves();
        }
    }

    public void LoadOverworld() {
        proceduralGeneration.LoadOverworld();
        StartCoroutine(LoadPlayerPosition());
    }

    public void LoadCaves() {
        proceduralGeneration.LoadCaves();
        StartCoroutine(LoadPlayerPosition());
    }

    public bool IsInOverworld() {
        return SceneManager.GetActiveScene().buildIndex == 1;
    }

    //IEnumerator NewFile() {
    //    yield return new WaitForEndOfFrame();
    //    CreateNewFile();
    //}

    IEnumerator LoadPlayerPosition() {
        yield return new WaitForEndOfFrame();
        playerStats.SetDead(false);
        proceduralGeneration.LoadPlayerPosition(IsInOverworld());
        proceduralGeneration.LoadPlayerStats();
        dayNightCycle.CheckDay();
        pauseText.text = "Game Paused (" + ((difficultyLevel == 0) ? "Normal)" : "Hard)");
        //print(difficultyLevel);
    }

    IEnumerator SaveFile() {
        yield return new WaitForEndOfFrame();
        if (IsInOverworld()) {
            SaveOverworld();
        } else {
            SaveCaves();
        }
    }

    IEnumerator LoadFile() {
        yield return new WaitForEndOfFrame();
        if (IsInOverworld()) {
            LoadOverworld();
        } else {
            LoadCaves();
        }
    }

    public void SaveAndLoad(int scene) {
        StartCoroutine(SaveAndLoadScene(scene));
    }

    public IEnumerator SaveAndLoadScene(int scene) {
        Save();
        yield return new WaitForEndOfFrame();
        LoadSceneByIndex(scene);
    }
}

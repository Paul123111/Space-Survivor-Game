using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneManager : MonoBehaviour
{
    [SerializeField] ProceduralGeneration proceduralGeneration;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void NewGame() {
        StartCoroutine(NewGameStart());
    }

    IEnumerator NewGameStart() {
        string[] dataToSave = new string[5];
        dataToSave[0] = "1|1,3|1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1"; // inventory
        dataToSave[1] = proceduralGeneration.CreateOverWorld(); // overworld + player position
        dataToSave[2] = proceduralGeneration.CreateCaves(); // caves + player position
        dataToSave[3] = "1"; // scene build index - 1 for overworld, 4 for caves
        dataToSave[4] = "100,100,0,0,0,0,0"; // oxygen, power, dayTimer, daycount, isDay, blocks broken, enemies slain

        string data = string.Join("*", dataToSave);

        File.WriteAllText(Application.dataPath + "/gameData.txt", data);

        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene(1);
    }

    public void LoadGame() {
        //PlayerPrefs.SetFloat("power", 100f);
        //PlayerPrefs.SetFloat("oxygen", 100f);

        string gameData = File.ReadAllText(Application.dataPath + "/gameData.txt");
        string[] gameDataArray = gameData.Split("*");

        SceneManager.LoadScene(int.Parse(gameDataArray[5]));
    }

    public void LoadNewScene() {
        SceneManager.LoadScene(1);
        //PlayerPrefs.SetInt("score", 0);

        //PlayerPrefs.SetFloat("timer", 0);
        //PlayerPrefs.SetInt("dayCount", 0);
        //PlayerPrefs.SetInt("isDay", 0);
        //PlayerPrefs.SetFloat("power", 100f);
        //PlayerPrefs.SetFloat("oxygen", 100f);

    }

    public void LoadInstructions() {
        SceneManager.LoadScene(3);
    }

    public void LoadMenu() {
        SceneManager.LoadScene(0);
    }


}

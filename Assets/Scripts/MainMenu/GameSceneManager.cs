using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    int score = 0;
    [SerializeField] Inventory inventory;
    [SerializeField] DayNightCycle dayNightCycle;
    [SerializeField] PlayerStats playerStats;

    // Start is called before the first frame update
    void Awake()
    {
        score = PlayerPrefs.GetInt("score");
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void LoadNewScene() {
        SceneManager.LoadScene(1);
    }

    public void LoadSceneByIndex(int index) {
        inventory.SaveInventory();
        dayNightCycle.SaveTimer();
        playerStats.SaveStats();
        SceneManager.LoadScene(index);
        PlayerPrefs.SetInt("score", score);
    }

    public int GetScore() {
        return score;
    }

    public void SetScore(int score) {
        this.score = score;
    }


}

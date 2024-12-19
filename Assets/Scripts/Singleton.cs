using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Singleton : MonoBehaviour
{
    GameObject gridVisual;
    GameSceneManager gameSceneManager;
    TextMeshProUGUI blocksBroken;
    TextMeshProUGUI enemiesSlain;

    //int numSingletons = 0;

    //private void Awake() {
    //    numSingletons = FindObjectsOfType<Singleton>().Length;
    //    if (numSingletons > 1) {
    //        Destroy(gameObject);
    //    } else {
    //        DontDestroyOnLoad(gameObject);
    //    }
    //}

    // Start is called before the first frame update
    void Start()
    {
        gameSceneManager = GetComponent<GameSceneManager>();
        blocksBroken = GameObject.FindWithTag("BlocksBrokenText").GetComponent<TextMeshProUGUI>();
        blocksBroken.text = "Blocks Broken:" + gameSceneManager.GetScore();
        gridVisual = GameObject.Find("GridVisual");
        enemiesSlain = GameObject.Find("EnemiesSlain").GetComponent<TextMeshProUGUI>();
    }

    public void showGrid(bool show) {
        if (gridVisual != null)
            gridVisual.SetActive(show);
    }

    public int GetScore() {
        return gameSceneManager.GetScore();
    }

    public void SetScore(int score) {
        gameSceneManager.SetScore(score);
        blocksBroken.text = "Blocks Broken:" + score;
    }

    public void SetKillCount(int kills) {
        gameSceneManager.SetKillCount(kills);
        enemiesSlain.text = "Enemies Slain:" + kills;
    }

    public int GetKillCount() {
        return gameSceneManager.GetKillCount();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Singleton : MonoBehaviour
{
    [SerializeField] GameObject gridVisual;
    GameSceneManager gameSceneManager;
    TextMeshProUGUI blocksBroken;

    // Start is called before the first frame update
    void Start()
    {
        gameSceneManager = GetComponent<GameSceneManager>();
        blocksBroken = GameObject.FindWithTag("BlocksBrokenText").GetComponent<TextMeshProUGUI>();
        blocksBroken.text = ">Blocks Broken: " + gameSceneManager.GetScore();
    }

    public void showGrid(bool show) {
        gridVisual.SetActive(show);
    }

    public int GetScore() {
        return gameSceneManager.GetScore();
    }

    public void SetScore(int score) {
        gameSceneManager.SetScore(score);
        blocksBroken.text = ">Blocks Broken: " + score;
    }
}

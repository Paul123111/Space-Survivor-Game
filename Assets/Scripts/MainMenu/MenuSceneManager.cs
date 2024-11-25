using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNewScene() {
        SceneManager.LoadScene(1);
        PlayerPrefs.SetInt("score", 0);
        PlayerPrefs.SetInt("ItemID1", 1);
        PlayerPrefs.SetInt("ItemStack1", 1);
        PlayerPrefs.SetInt("ItemID2", 3);
        PlayerPrefs.SetInt("ItemStack2", 1);
        //PlayerPrefs.SetInt("ItemID3", 6);
        //PlayerPrefs.SetInt("ItemStack3", 1);
        //PlayerPrefs.SetInt("ItemID4", 7);
        //PlayerPrefs.SetInt("ItemStack4", 1);
        //PlayerPrefs.SetInt("ItemID5", 8);
        //PlayerPrefs.SetInt("ItemStack5", 1);
        //PlayerPrefs.SetInt("ItemID6", 2);
        //PlayerPrefs.SetInt("ItemStack6", 10);
        //PlayerPrefs.SetInt("ItemID7", 0);
        //PlayerPrefs.SetInt("ItemStack7", 0);
        //PlayerPrefs.SetInt("ItemID8", 0);
        //PlayerPrefs.SetInt("ItemStack8", 0);
        //PlayerPrefs.SetInt("ItemID9", 0);
        //PlayerPrefs.SetInt("ItemStack9", 0);
        //PlayerPrefs.SetInt("ItemID0", 0);
        //PlayerPrefs.SetInt("ItemStack0", 0);

        PlayerPrefs.SetInt("ItemID3", 0);
        PlayerPrefs.SetInt("ItemStack3", 0);
        PlayerPrefs.SetInt("ItemID4", 0);
        PlayerPrefs.SetInt("ItemStack4", 0);
        PlayerPrefs.SetInt("ItemID5", 0);
        PlayerPrefs.SetInt("ItemStack5", 0);
        PlayerPrefs.SetInt("ItemID6", 0);
        PlayerPrefs.SetInt("ItemStack6", 0);
        PlayerPrefs.SetInt("ItemID7", 0);
        PlayerPrefs.SetInt("ItemStack7", 0);
        PlayerPrefs.SetInt("ItemID8", 0);
        PlayerPrefs.SetInt("ItemStack8", 0);
        PlayerPrefs.SetInt("ItemID9", 0);
        PlayerPrefs.SetInt("ItemStack9", 0);
        PlayerPrefs.SetInt("ItemID0", 0);
        PlayerPrefs.SetInt("ItemStack0", 0);

        PlayerPrefs.SetFloat("timer", 0);
        PlayerPrefs.SetInt("dayCount", 0);
        PlayerPrefs.SetInt("isDay", 0);
        PlayerPrefs.SetFloat("power", 100f);
        PlayerPrefs.SetFloat("oxygen", 100f);

        PlayerPrefs.SetInt("armour", 0);
        PlayerPrefs.SetInt("flower", 0);
        PlayerPrefs.SetInt("robot1", 0);
        PlayerPrefs.SetInt("robot2", 0);
        PlayerPrefs.SetInt("robot3", 0);
        PlayerPrefs.SetInt("robot4", 0);

    }

    public void LoadInstructions() {
        SceneManager.LoadScene(3);
    }

    public void LoadMenu() {
        SceneManager.LoadScene(0);
    }


}

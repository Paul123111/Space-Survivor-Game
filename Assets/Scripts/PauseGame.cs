using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    bool paused = false;
    bool pauseMenu = false;
    TextMeshProUGUI pausedGameText;
    TextMeshProUGUI pauseAndExitGameText;
    GameObject craftingMenu;
    GameObject fullInventory;
    Image UIUnrestrictedCursor;
    HotbarScript hotbarScript;
    GameObject pauseScreen;

    // Start is called before the first frame update
    void Start() {
        pausedGameText = GameObject.Find("GamePausedText").GetComponent<TextMeshProUGUI>();
        pauseAndExitGameText = GameObject.Find("PauseAndExitText").GetComponent<TextMeshProUGUI>();
        craftingMenu = GameObject.Find("CraftingMenu");
        UIUnrestrictedCursor = GameObject.Find("UIUnrestrictedCursor").GetComponent<Image>();
        fullInventory = GameObject.Find("FullInventory");
        hotbarScript = GameObject.Find("PlayerBody").GetComponent<HotbarScript>();
        pauseScreen = GameObject.Find("PauseMenu");

        Time.timeScale = 1.0f;
        paused = false;
        pauseMenu = false;

        StartCoroutine(HideMenu());
    }

    // Update is called once per frame
    //void Update() {

    //}

    void OnPause() {
        if (!paused && !pauseMenu) fullInventory.SetActive(true);
        TogglePause();
    }

    void OnPauseAndExitMenu() {
        TogglePauseAndExitMenu();
    }

    public void TogglePause() {
        if (pauseMenu) return;

        if (paused) {
            Time.timeScale = 1.0f;
            pausedGameText.enabled = false;
            craftingMenu.SetActive(false);
            fullInventory.SetActive(false);
            paused = false;
            hotbarScript.RefreshSlot();
        } else {
            Time.timeScale = 0.0f;
            pausedGameText.enabled = true;
            paused = true;
        }
    }

    public void TogglePauseAndExitMenu() {
        if (paused) return;
            
        if (pauseMenu) {
            Time.timeScale = 1.0f;
            pauseAndExitGameText.enabled = false;
            pauseScreen.SetActive(false);
            pauseMenu = false;
        } else {
            Time.timeScale = 0.0f;
            pauseAndExitGameText.enabled = true;
            pauseScreen.SetActive(true);
            pauseMenu = true;
        }
    }

    public bool GetPaused() {
        return paused || pauseMenu;
    }

    IEnumerator HideMenu() {
        yield return new WaitForEndOfFrame();
        craftingMenu.SetActive(false);
        fullInventory.SetActive(false);
        pauseScreen.SetActive(false);
    }
}

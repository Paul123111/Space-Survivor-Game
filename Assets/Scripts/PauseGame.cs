using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    bool paused = false;
    TextMeshProUGUI pausedGameText;
    GameObject craftingMenu;
    GameObject fullInventory;
    Image UIUnrestrictedCursor;

    // Start is called before the first frame update
    void Start() {
        pausedGameText = GameObject.Find("GamePausedText").GetComponent<TextMeshProUGUI>();
        craftingMenu = GameObject.Find("CraftingMenu");
        UIUnrestrictedCursor = GameObject.Find("UIUnrestrictedCursor").GetComponent<Image>();
        fullInventory = GameObject.Find("FullInventory");

        StartCoroutine(HideMenu());
    }

    // Update is called once per frame
    //void Update() {

    //}

    void OnPause() {
        if (!paused) fullInventory.SetActive(true);
        TogglePause();
    }

    public void TogglePause() {
        if (paused) {
            Time.timeScale = 1.0f;
            pausedGameText.enabled = false;
            UIUnrestrictedCursor.enabled = false;
            craftingMenu.SetActive(false);
            fullInventory.SetActive(false);
            paused = false;
        } else {
            Time.timeScale = 0.0f;
            pausedGameText.enabled = true;
            UIUnrestrictedCursor.enabled = true;
            paused = true;
        }
    }

    public bool GetPaused() {
        return paused;
    }

    IEnumerator HideMenu() {
        yield return new WaitForEndOfFrame();
        craftingMenu.SetActive(false);
        fullInventory.SetActive(false);
    }
}

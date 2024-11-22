using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class HideUI : MonoBehaviour
{
    GameObject scoreBoard;
    bool hidden = false;

    // Start is called before the first frame update
    void Start()
    {
        scoreBoard = GameObject.Find("ScoreBoard");
    }

    // Update is called once per frame
    void Update() {

    }

    void OnHideUI() {
        if (hidden) {
            scoreBoard.SetActive(true);
            hidden = false;
        } else {
            scoreBoard.SetActive(false);
            hidden = true;
        }
    }
}

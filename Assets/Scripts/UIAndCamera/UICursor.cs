using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICursor : MonoBehaviour
{
    [SerializeField] Camera playerCam;
    [SerializeField] Transform target;
    PauseGame pauseGame;

    // Start is called before the first frame update
    void Start()
    {
        pauseGame = GameObject.FindWithTag("Player").GetComponent<PauseGame>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseGame.GetPaused()) return;
        transform.position = playerCam.WorldToScreenPoint(target.transform.position);
    }
}

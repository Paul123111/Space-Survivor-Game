using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] float maxOxygen = 100f;
    [SerializeField] float maxPower = 100f;

    Image oxygenBar;
    Image powerBar;

    float oxygen;
    float power;

    GameSceneManager gameSceneManager;

    // Start is called before the first frame update
    void Start()
    {
        oxygenBar = GameObject.Find("OxygenBar").GetComponent<Image>();
        powerBar = GameObject.Find("PowerBar").GetComponent<Image>();

        oxygen = maxOxygen;
        power = maxPower;

        gameSceneManager = GameObject.FindWithTag("Singleton").GetComponent<GameSceneManager>();
    }

    // Update is called once per frame
    void Update()
    {
        BarLength();
        DepleteStats();
    }

    void BarLength() {
        oxygenBar.transform.localScale = new Vector3(oxygen/maxOxygen, 1, 1);
        powerBar.transform.localScale = new Vector3(power/maxPower, 1, 1);
    }

    void DepleteStats() {
        if (power > 0) {
            power -= 0.0005f;
        } else {
            oxygen -= 0.1f;
        }

        if (power < 0) {
            power = 0;
        }

        if (oxygen <= 0) {
            oxygen = 0;
            //print("Game Over");
            gameSceneManager.LoadSceneByIndex(3);
        }
    }

    public float GetOxygen() {
        return oxygen;
    }

    public void SetOxygen(float oxygen) {
        this.oxygen = oxygen;
        if (oxygen > maxOxygen)
            this.oxygen = maxOxygen;
    }

    public float GetPower() {
        return power;
    }

    public void SetPower(float power) {
        this.power = power;
        if (power > maxPower)
            this.power = maxPower;
    }
}

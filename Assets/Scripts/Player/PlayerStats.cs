using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// power bar removed to focus on oxygen bar
public class PlayerStats : MonoBehaviour
{
    [SerializeField] float maxOxygen = 100f;
    [SerializeField] float maxPower = 100f;

    Image oxygenBar;
    //Image powerBar;

    Image screenFlash;
    float alpha = 0;
    bool screenFlashBool = false;

    float oxygen = 100f;
    float power = 100f;

    [SerializeField] Animator anim;
    GameSceneManager gameSceneManager;
    Inventory inventory;
    [SerializeField] LineRenderer lineRenderer;

    bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        oxygenBar = GameObject.Find("OxygenBar").GetComponent<Image>();
        //powerBar = GameObject.Find("PowerBar").GetComponent<Image>();
        screenFlash = GameObject.Find("ScreenFlash").GetComponent<Image>();

        //oxygen = maxOxygen;
        //power = maxPower;

        //LoadStats();

        gameSceneManager = GameObject.FindWithTag("Singleton").GetComponent<GameSceneManager>();
        inventory = GameObject.FindWithTag("ItemManager").GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        BarLength();
        DepleteStats();
        UpdateScreenFlash();
    }

    void BarLength() {
        oxygenBar.transform.localScale = new Vector3(oxygen/maxOxygen, 1, 1);
        //powerBar.transform.localScale = new Vector3(power/maxPower, 1, 1);
    }

    void DepleteStats() {
        if (power > 0) {
            //power -= 0.25f * Time.deltaTime;
        } else {
            oxygen -= 0.5f*Time.deltaTime;
        }

        if (power < 0) {
            power = 0;
        }

        if (IsDead()) return;
        if (inventory.GetFlower() != null) {
            power += inventory.GetFlower().GetPowerRate() * Time.deltaTime;
            //if (power > 0) {
            oxygen += inventory.GetFlower().GetOxygenRate() * Time.deltaTime;
            //}
        }

        if (power > 100) {
            power = 100;
        }

        if (oxygen > 100) {
            oxygen = 100;
        }

        if (oxygen <= 0) {
            oxygen = 0;
            //print("Game Over");
            SetDead(true);
            //gameSceneManager.LoadSceneByIndex(2);
            StartCoroutine(GameOver());
        }
    }

    IEnumerator GameOver() {
        yield return new WaitForSeconds(3f);
        gameSceneManager.LoadSceneByIndex(2);
    }

    public bool IsDead() {
        return dead;
    }

    public void SetDead(bool dead) {
        this.dead = dead;
        if (dead) {
            anim.SetBool("isDead", true);
            lineRenderer.enabled = false;
        } else {
            anim.SetBool("isDead", false);
            lineRenderer.enabled = true;
        }
    }

    //public void SaveStats() {
    //    PlayerPrefs.SetFloat("power", power);
    //    PlayerPrefs.SetFloat("oxygen", oxygen);
    //}

    //public void MaxStats() {
    //    PlayerPrefs.SetFloat("power", maxPower);
    //    PlayerPrefs.SetFloat("oxygen", maxOxygen);
    //}

    //void LoadStats() {
    //    power = PlayerPrefs.GetFloat("power");
    //    oxygen = PlayerPrefs.GetFloat("oxygen");
    //}

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

    public void ScreenFlash() {
        alpha = 0.5f;
        screenFlashBool = true;
    }

    void UpdateScreenFlash() {
        if (screenFlashBool) {
            alpha -= Time.deltaTime;

            if (alpha <= 0) {
                alpha = 0;
                screenFlashBool = false;
            }

            screenFlash.color = new Color(1, 0, 0, alpha);
        }
    }
}

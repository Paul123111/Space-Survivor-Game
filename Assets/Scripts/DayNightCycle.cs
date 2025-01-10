using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] Light dayLight;
    [SerializeField] bool hasDayLight = true;
    [SerializeField] Light astronautLight;

    [SerializeField] float dayTimeLengthSeconds;
    [SerializeField] float nightTimeLengthSeconds;
    
    bool isDay = false;
    float timer = 0f;
    int dayCount = 0;

    //[SerializeField] GameObject enemySpawner;
    TextMeshProUGUI dayCountText;
    TextMeshProUGUI timerText;
    Singleton singleton;
    GameSceneManager gameSceneManager;

    // Start is called before the first frame update
    void Start()
    {
        dayCountText = GameObject.Find("DayCount").GetComponent<TextMeshProUGUI>();
        timerText = GameObject.Find("TimeBeforeCycle").GetComponent<TextMeshProUGUI>();
        gameSceneManager = GameObject.Find("Singleton").GetComponent<GameSceneManager>();
        
    }

    public void CheckDay() {
        StartCoroutine(CheckDayAtStart());
    }

    IEnumerator CheckDayAtStart() {
        yield return new WaitForEndOfFrame();
        //print(isDay);
        if (isDay) SetToDayTime();
        else SetToNightTime();
    }

    // Update is called once per frame
    void Update()
    {
        IncreaseTimer();
        TimerConditions();
    }

    // Switches between night and day, starts at night - daytime is dangerous
    void IncreaseTimer() {
        timer += Time.deltaTime;

        if (isDay) {
            timerText.text = ">Nighttime in: " + Mathf.Ceil(dayTimeLengthSeconds - timer);
        } else {
            timerText.text = ">Daytime in: " + Mathf.Ceil(nightTimeLengthSeconds - timer);
        }
    }

    void TimerConditions() {
        if (isDay && timer > dayTimeLengthSeconds) {
            StartCoroutine(SwitchToNightTime());
            timer = 0;
            dayCount++;
            gameSceneManager.Save();
            dayCountText.text = ">Days Survived: " + dayCount;
        } else if (!isDay && timer > nightTimeLengthSeconds) {
            timer = 0;
            StartCoroutine(SwitchToDayTime());
        }
    }

    IEnumerator SwitchToDayTime() {
        //enemySpawner.SetActive(true);
        isDay = true;
        if (gameSceneManager.IsInOverworld()) {
            while (dayLight.intensity < 0.6f && hasDayLight) {
                dayLight.intensity += 0.02f;
                yield return new WaitForSeconds(0.1f);
            }
            astronautLight.enabled = false;
        }
    }

    IEnumerator SwitchToNightTime() {
        isDay = false;
        //enemySpawner.SetActive(false);
        astronautLight.enabled = true;
        while (dayLight.intensity > 0f && hasDayLight) {
            dayLight.intensity -= 0.02f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void SetToDayTime() {
        isDay = true;
        if (gameSceneManager.IsInOverworld()) {
            astronautLight.enabled = false;
            dayLight.intensity = 0.6f;
        } else {
            astronautLight.enabled = true;
            dayLight.intensity = 0.0f;
        }
    }

    void SetToNightTime() {
        isDay = false;
        astronautLight.enabled = true;
        dayLight.intensity = 0.0f;
    }

    public bool IsDay() {
        return isDay;
    }

    public void SetIsDay(bool isDay) {
        this.isDay = isDay;
    }

    public int GetDayCount() {
        return dayCount;
    }

    public void SetDayCount(int dayCount) {
        this.dayCount = dayCount;
        dayCountText.text = ">Days Survived: " + dayCount;
    }

    //public void SaveTimer() {
    //    PlayerPrefs.SetFloat("timer", timer);
    //    PlayerPrefs.SetInt("dayCount", dayCount);
    //    PlayerPrefs.SetInt("isDay", isDay ? 1 : 0);
    //}

    //void GetTimer() {
    //    timer = PlayerPrefs.GetFloat("timer");
    //    dayCount = PlayerPrefs.GetInt("dayCount");
    //    isDay = PlayerPrefs.GetInt("isDay") != 0 ? true : false;
    //    dayCountText.text = ">Days Survived: " + dayCount;
    //}

    public float GetTimer() {
        return timer;
    }

    public void SetTimer(float timer) {
        this.timer = timer;

    }

}

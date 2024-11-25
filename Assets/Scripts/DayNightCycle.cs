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
    [SerializeField] TextMeshProUGUI dayCountText;
    [SerializeField] TextMeshProUGUI timerText;

    // Start is called before the first frame update
    void Start()
    {
        GetTimer();
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
            dayCountText.text = ">Days Survived: " + dayCount;
        } else if (!isDay && timer > nightTimeLengthSeconds) {
            timer = 0;
            StartCoroutine(SwitchToDayTime());
        }
    }

    IEnumerator SwitchToDayTime() {
        //enemySpawner.SetActive(true);
        isDay = true;
        while (dayLight.intensity < 1 && hasDayLight) {
            dayLight.intensity += 0.02f;
            yield return new WaitForSeconds(0.1f);
        }
        astronautLight.enabled = false;
    }

    IEnumerator SwitchToNightTime() {
        isDay = false;
        //enemySpawner.SetActive(false);
        astronautLight.enabled = true;
        while (dayLight.intensity > 0.2f && hasDayLight) {
            dayLight.intensity -= 0.02f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public bool IsDay() {
        return isDay;
    }

    public int GetDayCount() {
        return dayCount;
    }

    public void SaveTimer() {
        PlayerPrefs.SetFloat("timer", timer);
        PlayerPrefs.SetInt("dayCount", dayCount);
        PlayerPrefs.SetInt("isDay", isDay ? 1 : 0);
    }

    void GetTimer() {
        timer = PlayerPrefs.GetFloat("timer");
        dayCount = PlayerPrefs.GetInt("dayCount");
        isDay = PlayerPrefs.GetInt("isDay") != 0 ? true : false;
        dayCountText.text = ">Days Survived: " + dayCount;
    }
}

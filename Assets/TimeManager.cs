using UnityEngine;
using System;


public class TimeManager : MonoBehaviour
{
    
    [Header("Time Source")]
    public bool useRealTime = true;

    [Header("Real-World Time")]
    public int year;
    public int month;
    public int day;
    public int hour;
    public int minute;

    [Header("Custom Clock Time")]
    public float customTime = 0f;      // 0 to 24 hours
    public float timeSpeed = 1f;       // Speed multiplier for custom time
    public int customHour;
    public int customMinute;

    [Header("Sun Light")]
    public Light sunLight;
    public Color nightColor = new Color(0.2f, 0.3f, 0.6f);
    public Color dayColor = new Color(1f, 0.95f, 0.85f);
    public float nightIntensity = 0.2f;
    public float dayIntensity = 1f;

    private float lastDay = -1f; // tracks day change in simulation
    public int totalDaysPassed = 0; // global day counter for sun cycles

    public delegate void DayPassedHandler(int totalDays);
    public event DayPassedHandler OnDayPassed;  // event subscribers can react

    void Update()
    {
        float timeAsDecimal;

        if (useRealTime)
        {
            DateTime now = DateTime.Now;
            year = now.Year;
            month = now.Month;
            day = now.Day;
            hour = now.Hour;
            minute = now.Minute;

            timeAsDecimal = hour + (minute / 60f);
        }
        else
        {
            customTime += Time.deltaTime * timeSpeed;

            if (customTime >= 24f)
            {
                customTime = 0f;
                Debug.Log("🕛 New simulated day started.");
            }

            customHour = Mathf.FloorToInt(customTime);
            customMinute = Mathf.FloorToInt((customTime - customHour) * 60f);

            timeAsDecimal = customHour + (customMinute / 60f);

            float currentDay = Mathf.Floor(Time.time / (24f / timeSpeed));
            // Or keep your own logic: currentDay = Mathf.Floor(customTime / 24f);
            // But I recommend tracking days based on total elapsed time so it never resets

            if (currentDay > lastDay)
            {
                lastDay = currentDay;
                totalDaysPassed++;
                   Debug.Log("🌅 New simulated day detected in TimeManager.");
                Debug.Log($"☀️ Global day {totalDaysPassed} passed (Sun Cycle).");
                if (OnDayPassed != null)
        {
            Debug.Log("!! TimeManager: A new simulated day has passed@!!");
            OnDayPassed.Invoke((int)currentDay);
        }
                OnDayPassed?.Invoke(totalDaysPassed);  // notify listeners
            }
        }

        // Sun rotation & lighting (same as before)
        float sunAngle = Mathf.Lerp(-90f, 270f, timeAsDecimal / 24f);
        if (sunLight != null)
        {
            sunLight.transform.rotation = Quaternion.Euler(sunAngle, 0, 0);

            float dayFactor = Mathf.Clamp01(Mathf.InverseLerp(6f, 18f, timeAsDecimal));
            sunLight.color = Color.Lerp(nightColor, dayColor, dayFactor);
            sunLight.intensity = Mathf.Lerp(nightIntensity, dayIntensity, dayFactor);
        }
    }
}
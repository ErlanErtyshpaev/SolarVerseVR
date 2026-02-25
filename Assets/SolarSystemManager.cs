using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;   // ✅ New Input System

public class SolarSystemManager : MonoBehaviour
{
    [Header("Epoch date (start)")]
    public int epochYear = 2025;
    public int epochMonth = 1;
    public int epochDay = 1;

    [Header("Time control")]
    public double currentDaysFromEpoch = 0;  // 0 = epoch date
    public float daysPerSecond = 10f;        // скорость времени
    public bool isRunning = true;

    [Header("References")]
    public Transform sun;
    public List<PlanetOrbit> planets = new List<PlanetOrbit>();

    void Start()
    {
        UpdateAllPlanets();
        Debug.Log($"📅 Date: {GetDateString(currentDaysFromEpoch)}");
    }

    void Update()
    {
        // Управление:
        // Space — пауза/пуск
        // [  ]  — шаг -1/+1 день
        // -  =  — скорость времени

        var kb = Keyboard.current;
        if (kb != null)
        {
            if (kb.spaceKey.wasPressedThisFrame)
                isRunning = !isRunning;

            if (kb.leftBracketKey.wasPressedThisFrame)
            {
                currentDaysFromEpoch -= 1;
                isRunning = false;
                UpdateAllPlanets();
                Debug.Log($"📅 {GetDateString(currentDaysFromEpoch)}");
            }

            if (kb.rightBracketKey.wasPressedThisFrame)
            {
                currentDaysFromEpoch += 1;
                isRunning = false;
                UpdateAllPlanets();
                Debug.Log($"📅 {GetDateString(currentDaysFromEpoch)}");
            }

            // Минус/плюс скорость (на клавиатуре: "-" и "=")
            if (kb.minusKey.wasPressedThisFrame)
                daysPerSecond = Mathf.Max(0f, daysPerSecond - 5f);

            if (kb.equalsKey.wasPressedThisFrame)
                daysPerSecond += 5f;
        }

        if (isRunning)
        {
            currentDaysFromEpoch += daysPerSecond * Time.deltaTime;
            UpdateAllPlanets();
        }
    }

    void UpdateAllPlanets()
    {
        foreach (var p in planets)
            if (p) p.SetPositionByDays(currentDaysFromEpoch);
    }

    string GetDateString(double daysFromEpoch)
    {
        DateTime epoch = new DateTime(epochYear, epochMonth, epochDay);
        DateTime dt = epoch.AddDays(daysFromEpoch);
        return dt.ToString("yyyy-MM-dd");
    }

    // Публичные методы (как у тебя было)
    public string GetDateStringPublic()
    {
        DateTime epoch = new DateTime(epochYear, epochMonth, epochDay);
        DateTime dt = epoch.AddDays(currentDaysFromEpoch);
        return dt.ToString("yyyy-MM-dd");
    }

    public void UpdateAllPlanetsPublic()
    {
        foreach (var p in planets)
            if (p) p.SetPositionByDays(currentDaysFromEpoch);
    }
}

using UnityEngine;
using System.Collections.Generic;

public class LawManager : MonoBehaviour
{
    public TimeManager timeManager;

    [Header("Curfew Settings")]
    public bool curfewActive = true;
    public float curfewStartHour = 21f; // 9PM
    public float curfewEndHour = 6f;    // 6AM

    private List<string> activeLaws = new List<string>();

    void Awake()
    {
        if (timeManager == null)
            timeManager = FindObjectOfType<TimeManager>();

        if (curfewActive)
        {
            activeLaws.Add("Curfew");
            Debug.Log($"📘 Curfew law activated from {curfewStartHour} to {curfewEndHour}");
        }
    }

    public bool IsLawActive(string lawName)
    {
        return activeLaws.Contains(lawName);
    }

    public float GetLawTime(string lawName)
    {
        if (lawName == "Curfew") return curfewStartHour;
        return -1f;
    }

    public float GetLawEndTime(string lawName)
    {
        if (lawName == "Curfew") return curfewEndHour;
        return -1f;
    }

    public List<string> GetAllActiveLaws()
    {
        return activeLaws;
    }
}
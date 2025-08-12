using UnityEngine;
using System.IO;
using System;
public class SaveManager : MonoBehaviour
{
    private string filePath;

    public ResidentData residentData = new ResidentData();
    
    
 private void OnEnable()
{
    TimeManager timeManager = UnityEngine.Object.FindAnyObjectByType<TimeManager>();
    if (timeManager != null)
    {
        timeManager.OnDayPassed -= OnGlobalDayPassed; // 🔒 Prevent double subscription
        timeManager.OnDayPassed += OnGlobalDayPassed;

        Debug.Log("📡 SaveManager subscribed to TimeManager.OnDayPassed");
    }
}

    private void OnDisable()
    {
        TimeManager timeManager = UnityEngine.Object.FindAnyObjectByType<TimeManager>();
        if (timeManager != null)
        {
            timeManager.OnDayPassed -= OnGlobalDayPassed;
        }
    }
    void Awake()
    {
    filePath = Application.persistentDataPath + "/resident.json";
    LoadData();

    // TEMP TEST: Force player to be a resident
    residentData.isResident = true;
    SaveData();
    }
private void OnApplicationQuit()
{
    SaveData();
    Debug.Log("💾 Saved data on quit.");
}
    public void SaveData()
    {
        string json = JsonUtility.ToJson(residentData, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Saved: " + filePath);
    }

    public void LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            residentData = JsonUtility.FromJson<ResidentData>(json);
            Debug.Log("Loaded resident file.");
        }
        else
        {
            SaveData(); // create default file
        }
    }

     private void OnGlobalDayPassed(int totalDays)
    {
        // You decide if the player "lived" this day
        if (residentData.isResident)
        {
            residentData.daysLived++;
            SaveData();
            Debug.Log($"👤 Player days lived increased: {residentData.daysLived}");
        }
        else
        {
            Debug.Log("👤 Player is not a resident, days lived not increased.");
        }
    }
}

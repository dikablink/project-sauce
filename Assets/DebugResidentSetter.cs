using UnityEngine;

public class DebugResidentSetter : MonoBehaviour
{
    void Start()
    {
        var saveManager = FindAnyObjectByType<SaveManager>();
        saveManager.residentData.isResident = true;
        saveManager.residentData.daysLived = 30;
        saveManager.SaveData();

        Debug.Log("Test resident set: 30 days lived.");
    }
}

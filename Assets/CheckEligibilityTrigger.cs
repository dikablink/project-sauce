using UnityEngine;

public class CheckEligibilityTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
   //     Debug.Log("🟨 Trigger entered by: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("✅ Player tag matched!");

            var saveManager = Object.FindAnyObjectByType<SaveManager>();

            if (saveManager == null)
            {
                Debug.LogWarning("❌ SaveManager not found or inactive in scene!");
                return;
            }

            if (saveManager.residentData == null)
            {
                Debug.LogWarning("⚠️ residentData is null in SaveManager!");
                return;
            }

            Debug.Log("📂 SaveManager found. Checking resident status...");

            bool isResident = saveManager.residentData.isResident;
            int daysLived = saveManager.residentData.daysLived;

            Debug.Log($"📊 Resident = {isResident}, Days = {daysLived}");

            if (isResident && daysLived >= 30)
            {
                Debug.Log("🎉 You are eligible to run for office.");
            }
            else
            {
                Debug.Log("🚫 Not eligible yet.");
            }
        }
        else
        {
         //   Debug.Log("❌ Something else entered the trigger: " + other.name);
        }
    }
}
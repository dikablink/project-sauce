using UnityEngine;

public class AIDialoqueTrigger : MonoBehaviour
{
    public GameObject dialoguePanel; // just the input UI panel
    private bool playerInRange = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            dialoguePanel.SetActive(true); // Show input UI
            Debug.Log("🟢 Entered dialogue range.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
          //  playerInRange = false;

            // Instead of turning off your AI logic script, just hide the panel
          //  dialoguePanel.SetActive(false);
            Debug.Log("🔴 Left dialogue range.");
        }
    }

    public bool IsPlayerInRange() => playerInRange;
}
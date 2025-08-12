using UnityEngine;

public class AIStartSensor : MonoBehaviour
{
    private AIDebateStarter ai;
    [Tooltip("Must match the agentName in the AIDebateStarter component")]
    public string agentName; // "Amy" or "Ron"

    void Awake() => ai = GetComponent<AIDebateStarter>();

    void OnTriggerEnter(Collider other)
    {
        if (!DialogueManager.Instance || !other.CompareTag("Player")) return;
        if (agentName == "Amy")
            DialogueManager.Instance.SetPlayerNearAmy(true);
        else if (agentName == "Ron")
            DialogueManager.Instance.SetPlayerNearRon(true);

        DialogueManager.Instance.SetCurrentAI(ai);
        Debug.Log($"🟢 Player entered range of {agentName}");
    }

    void OnTriggerExit(Collider other)
    {
        if (!DialogueManager.Instance || !other.CompareTag("Player")) return;
        if (agentName == "Amy")
            DialogueManager.Instance.SetPlayerNearAmy(false);
        else if (agentName == "Ron")
            DialogueManager.Instance.SetPlayerNearRon(false);

        DialogueManager.Instance.ClearCurrentAI();
        Debug.Log($"🔴 Player left range of {agentName}");
    }
}
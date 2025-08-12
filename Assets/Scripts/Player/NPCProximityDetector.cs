using UnityEngine;

public class NPCProximityDetector : MonoBehaviour
{
    public string myName = "Amy"; // or "Ron" depending on who this is

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ron") && myName == "Amy")
        {
            Debug.Log("👀 Amy sees Ron nearby!");
        }
        else if (other.CompareTag("Amy") && myName == "Ron")
        {
            Debug.Log("👀 Ron sees Amy nearby!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ron") && myName == "Amy")
        {
            Debug.Log("👋 Amy no longer sees Ron.");
        }
        else if (other.CompareTag("Amy") && myName == "Ron")
        {
            Debug.Log("👋 Ron no longer sees Amy.");
        }
    }
}
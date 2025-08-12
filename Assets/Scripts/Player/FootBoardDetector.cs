using UnityEngine;

public class FootBoardDetector : MonoBehaviour
{
    public bool isOnBoard = false;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Skateboard"))
        {
            isOnBoard = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Skateboard"))
        {
            isOnBoard = false;
        }
    }
}
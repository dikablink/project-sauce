using UnityEngine;

public class SkateTrigAAA : MonoBehaviour
{
    public SkateboardController boardController;

    [Header("Debug")]
    public bool playerEntered = false;
    public bool playerExited = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boardController.playerOnBoard = true;
            playerEntered = true;
            playerExited = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boardController.playerOnBoard = false;
            playerExited = true;
            playerEntered = false;
        }
    }
}
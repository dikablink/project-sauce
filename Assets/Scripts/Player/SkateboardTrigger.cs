using UnityEngine;

public class SkateboardTrigger : MonoBehaviour
{
    //SAVE51925
    public SkateboardController boardController;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boardController.playerOnBoard = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boardController.playerOnBoard = false;
        }
    }
}
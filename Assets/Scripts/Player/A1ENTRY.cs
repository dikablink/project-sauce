using UnityEngine;

public class A1ENTRY : MonoBehaviour
{
    public GameObject ron;
    public CarEntrySystem carEntrySystem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ron)
        {
            Debug.Log("Ron entered car trigger");

            if (carEntrySystem != null)
            {
                carEntrySystem.SendMessage("EnterCarAsAI");
            }
        }
    }
}
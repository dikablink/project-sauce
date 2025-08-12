using UnityEngine;
[RequireComponent(typeof(Collider))]
public class A1ENTRY : MonoBehaviour
{
  //  public GameObject ron;

    public CarEntrySystem carEntrySystem;
    private void Awake()
    {
        // Auto-find if not set
        if (carEntrySystem == null)
        {
            carEntrySystem = GetComponentInParent<CarEntrySystem>();
            if (carEntrySystem == null)
                carEntrySystem = GetComponent<CarEntrySystem>();
        }

        // Ensure this collider is a trigger
        var col = GetComponent<Collider>();
        if (col != null && !col.isTrigger)
        {
            Debug.LogWarning($"[{name}] Collider was not trigger. Setting isTrigger = true.");
            col.isTrigger = true;
        }
    }
     private void OnTriggerEnter(Collider other)
    {

        if (carEntrySystem == null)
            return;
              if (carEntrySystem.isDriving || carEntrySystem.isLocked)
    {
        Debug.Log($"🔒 Entry ignored — Car isDriving={carEntrySystem.isDriving}, isLocked={carEntrySystem.isLocked}");
        return;
    }

     if (carEntrySystem.isDriving || carEntrySystem.isLocked)
    {
        Debug.Log($"🔒 Entry ignored — Car isDriving={carEntrySystem.isDriving}, isLocked={carEntrySystem.isLocked}");
        return;
    }
        if (other.CompareTag("Player"))
        {
            Debug.Log("👤 Player entered car trigger");
            carEntrySystem?.EnterCarAsPlayer();
        }
        else if (other.CompareTag("NPC"))
        {
            Debug.Log($"🚗 AI with tag A1 entered: {other.name}");
            var ai = other.GetComponent<a1carwire>() ?? other.GetComponentInParent<a1carwire>();
            if (ai == null)
            {
                Debug.LogWarning("❌ No a1carwire found on AI object.");
                return;
            }
            a1carwire aiCarWire = other.GetComponent<a1carwire>();
            bool ok = carEntrySystem.TryConfigureForAI(ai.npcBody, ai.carDriver, ai.startNode, ai.goalNode);
            if (ok)
            {
                carEntrySystem.EnterCarAsAIConfigured(); // starts & locks
            }
            if (aiCarWire != null && carEntrySystem != null)
            {
                // Transfer all needed references from the A1 NPC
                carEntrySystem.aiModel = aiCarWire.npcBody;
                carEntrySystem.aiDriver = aiCarWire.carDriver;
                carEntrySystem.startNode = aiCarWire.startNode;
                carEntrySystem.goalNode = aiCarWire.goalNode;

                Debug.Log("✅ CarEntrySystem AI references updated from A1 NPC");

                // Start AI entry process
                carEntrySystem.EnterCarAsAI();
            }
            else
            {
                Debug.LogWarning("❌ Missing a1carwire component or CarEntrySystem.");
            }
        }
    }
}
using UnityEngine;
using UnityEngine.AI;

public class a1carwire : MonoBehaviour
{
    [Header("NPC Components")]
    public GameObject npcBody; // The visible NPC body to disable when entering the car
    public NavMeshAgent agent;
    public Transform carEntryPoint;

    private NodeGraphAAA nodeGraphAAA;

    [Header("Car Components")]
    public DriverWiringAAC carDriver;
    public Transform carTargetDestination;

    [Header("State")]
    public bool isInCar = false;
    public bool hasArrived = false;

    void Start()
    {
        if (agent != null && carEntryPoint != null)
        {
            agent.SetDestination(carEntryPoint.position);
        }
    }

    void Update()
    {
        if (!isInCar && agent != null && !agent.pathPending)
        {
            float distance = Vector3.Distance(transform.position, carEntryPoint.position);

            if (distance < 1.5f)
            {
                EnterCar();
            }
        }

       if (isInCar && carDriver != null  && !hasArrived)
        {
            hasArrived = true;
            OnCarArrived();
        }
    }

    void EnterCar()
 {
    Debug.Log("🧍 NPC is entering the car.");

    npcBody.SetActive(false);
    agent.enabled = false;
    transform.position = carEntryPoint.position;
    
    if (carDriver != null)
    {
        Rigidbody rb = carDriver.GetComponent<Rigidbody>();
        if (rb != null) rb.WakeUp();

        Debug.Log("✅ Enabling car driver script...");
        carDriver.enabled = true;

       // carDriver.SetDestination(carTargetDestination.position);
    }
    else
    {
        Debug.LogWarning("❌ CarDriver not assigned!");
    }

    isInCar = true;
}

    void OnCarArrived()
    {
        Debug.Log("🚗 NPC arrived at destination.");

        // Reactivate NPC outside the car
        transform.position = carTargetDestination.position;
        npcBody.SetActive(true);
        agent.enabled = true;
        agent.SetDestination(carTargetDestination.position + Vector3.forward * 2f); // Walk forward a bit

        // You can assign another task/schedule here if needed
    }
}
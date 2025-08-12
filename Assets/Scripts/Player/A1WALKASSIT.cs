using UnityEngine;
using UnityEngine.AI;

public class A1WALKASSIT : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject car; // Assign in inspector
    public Transform carTarget; // Where to drive the car
    public bool driveCar = false;

    private bool inCar = false;

    void Update()
    {
        if (driveCar && !inCar)
        {
            // Walk to the car
            agent.SetDestination(car.transform.position);
            if (Vector3.Distance(transform.position, car.transform.position) < 2f)
            {
                EnterCar();
            }
        }

        if (inCar && car != null)
        {
            // Move the car toward the target
            car.transform.position = Vector3.MoveTowards(car.transform.position, carTarget.position, 5f * Time.deltaTime);
            car.transform.LookAt(carTarget.position);

            if (Vector3.Distance(car.transform.position, carTarget.position) < 1.5f)
            {
                ExitCar();
            }
        }
    }

    void EnterCar()
    {
        inCar = true;
        agent.enabled = false;
        gameObject.SetActive(false); // Hide NPC visually
        Debug.Log("🚗 NPC entered car");
    }

    void ExitCar()
    {
        inCar = false;
        gameObject.SetActive(true);
        agent.enabled = true;
        transform.position = car.transform.position + car.transform.right * 2f; // Exit to the side
        Debug.Log("🏠 NPC exited car");
    }
}
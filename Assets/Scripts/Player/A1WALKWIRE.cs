using UnityEngine;
using UnityEngine.AI;

public class A1WALKWIRE : MonoBehaviour
{
    public Transform target;
    public bool goToCarA;
    public Transform carTargetA;
    private NavMeshAgent agent1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NavMeshAgent agent1 = GetComponent<NavMeshAgent>();
        if (carTargetA == null)
        {
            Debug.LogWarning("yo cuh the mf car target anit there");
        }
      // if (agent != null && target != null)
        {
          //  agent1.SetDestination(target.position);
        }    
    }

    // Update is called once per frame
    void Update()
    {
        if (carTargetA == null)
        {
            Debug.LogWarning("yo cuh the mf car still anit there");
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            goToCarA = true;
        }
        Debug.Log("runingtest2");
        if (goToCarA && carTargetA != null && agent1 != null)
        {
            Debug.Log($"bro walkin to{carTargetA.position}");
            agent1.SetDestination(carTargetA.position);
        }
    }
}

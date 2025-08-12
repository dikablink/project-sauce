using System;
using UnityEngine;
using System.Collections.Generic;
public class CarEntrySystem : MonoBehaviour
{
    [Header("AI Pathfinding")]
public NodeGraphAAA nodeGraph;
public NodeRoadAAA startNode;
public NodeRoadAAA goalNode;
    [Header("Player Settings")]
    public GameObject player;
    public MonoBehaviour playerController;
    public MonoBehaviour carController;
    public GameObject carCam;
    public Transform playerSeat;

    [Header("AI Settings")]
  //  public GameObject ron;
    public GameObject aiModel; // Ron's visible model to hide
    public DriverWiringAAC aiDriver;
 //   public NodeRoadAAA aiFirstNode;
    public Transform aiSeat;

    [Header("General Settings")]
    public Rigidbody carRigidbody;
    public Transform exitPoint;
    public KeyCode enterKey = KeyCode.E;
    public KeyCode testRonKey = KeyCode.R;

    [Header("Debug")]
    public bool isDriving = false;
    public bool playerInTrigger = false;
    public bool aiInTrigger = false;
     [Header("Locking")]
    public bool lockOnFirstOccupant = true;
    public bool isLocked { get; private set; } = false;  // <- NEW
  public void ConfigureForAI(DriverWiringAAC driver, NodeRoadAAA start, NodeRoadAAA goal)
    {
        aiDriver = driver;
        startNode = start;
        goalNode = goal;
        Debug.Log("🚦 CarEntrySystem configured by NPC.");
    }

    private void Start()
    {
        if (carController != null) carController.enabled = false;
        if (aiDriver != null) aiDriver.enabled = false;

        Debug.Log("🚘 CarEntrySystem initialized.");
    }

    private void Update()
    {
        if (playerInTrigger && !isDriving && Input.GetKeyDown(enterKey))
        {
            EnterCarAsPlayer();
            Debug.Log("bro");
        }

        if (aiInTrigger && !isDriving && Input.GetKeyDown(testRonKey))
        {
            EnterCarAsAI();
        }

        if (isDriving && Input.GetKeyDown(enterKey))
        {
            ExitCarAsPlayer();
        }
    }
public void EnterCarAsAIConfigured()
    {
        isDriving = true;

        if (aiDriver != null && nodeGraph != null && startNode != null && goalNode != null)
        {
            List<NodeRoadAAA> path = nodeGraph.FindPath(startNode, goalNode);
            if (path != null && path.Count > 0)
            {
                aiDriver.SetPath(path); // <-- ensure DriverWiringAAC has SetPath(List<NodeRoadAAA>)
                aiDriver.enabled = true;

                if (carRigidbody != null) carRigidbody.WakeUp();
                if (aiSeat != null) transform.position = aiSeat.position;

                Debug.Log("✅ AI driving on configured path.");
            }
            else
            {
                Debug.LogWarning("❌ No valid path found.");
            }
        }
        else
        {
            Debug.LogWarning("❌ Missing refs (aiDriver/nodeGraph/start/goal).");
        }
    }
     public bool TryConfigureForAI(GameObject aiBody, DriverWiringAAC driver, NodeRoadAAA start, NodeRoadAAA goal)
    {
        if (lockOnFirstOccupant && isLocked)
        {
            Debug.Log($"🔒 Car locked; ignoring AI configure.");
            return false;
        }

        if (driver == null || start == null || goal == null || nodeGraph == null)
        {
            Debug.LogWarning("❌ Missing refs for AI configure (driver/start/goal/nodeGraph).");
            return false;
        }

        aiModel   = aiBody;
        aiDriver  = driver;
        startNode = start;
        goalNode  = goal;

        if (lockOnFirstOccupant)
        {
            isLocked = true;
        }
        Debug.Log($"✅ Car configured for AI. start={startNode.name}, goal={goalNode.name}, locked={isLocked}");
        return true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInTrigger = true;
            Debug.Log("👤 Player entered car trigger.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInTrigger = false;
            Debug.Log("👤 Player exited car trigger.");
        }

    }

    public void EnterCarAsPlayer()
    {
        if (lockOnFirstOccupant && isLocked && !isDriving)
        {
            Debug.Log("🔒 Car lock check triggered by: Player — already locked, blocking entry.");
            return;
        }
        isDriving = true;

        Debug.Log("🚗 Player is entering the car.");
        carCam.SetActive(true);
        player.SetActive(false);
        playerController.enabled = false;
        carController.enabled = true;

        if (carRigidbody != null)
            carRigidbody.WakeUp();

        if (playerSeat != null)
            transform.position = playerSeat.position;
              if (lockOnFirstOccupant)
    {
        isLocked = true;
        Debug.Log("✅ Car locked for: Player");
    }
    }

    public void EnterCarAsAI()
    {
        isDriving = true;

       Debug.Log("🧠 Ron is entering the car.");
        // if (aiModel != null) aiModel.SetActive(false);

        if (aiDriver != null && nodeGraph != null && startNode != null && goalNode != null)
        {
            List<NodeRoadAAA> path = nodeGraph.FindPath(startNode, goalNode);
            if (path != null && path.Count > 0)
            {
                aiDriver.SetPath(path); // <-- this needs to be implemented in DriverWiringAAC
                aiDriver.enabled = true;

                if (carRigidbody != null)
                    carRigidbody.WakeUp();

                if (aiSeat != null)
                    transform.position = aiSeat.position;

                Debug.Log("✅ Ron is now driving on path.");
            }
            else
            {
                Debug.LogWarning("❌ No valid path found.");
            }
        
               EnterCarAsAIConfigured(); // uses already-set aiDriver/start/goal
        if (lockOnFirstOccupant) isLocked = true;

    }
        else
        {
            Debug.LogWarning("❌ Pathfinding or AI references not assigned.");
        }
    }

    public void ExitCarAsPlayer()
    {
        Debug.Log("🚶 Player is exiting the car.");

        isDriving = false;
        carCam.gameObject.SetActive(false);
        carController.enabled = false;

        player.SetActive(true);
        playerController.enabled = true;

        if (exitPoint != null)
        {
            player.transform.position = exitPoint.position;
            player.transform.rotation = exitPoint.rotation;
        }
        else
        {
            player.transform.position = transform.position + transform.right * 2f;
        }
    }
}
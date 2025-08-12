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
    public Camera carCam;
    public Transform playerSeat;

    [Header("AI Settings")]
    public GameObject ron;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInTrigger = true;
            Debug.Log("👤 Player entered car trigger.");
        }
        else if (other.gameObject == ron)
        {
            aiInTrigger = true;
            Debug.Log("🤖 Ron entered car trigger.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInTrigger = false;
            Debug.Log("👤 Player exited car trigger.");
        }
        else if (other.gameObject == ron)
        {
            aiInTrigger = false;
            Debug.Log("🤖 Ron exited car trigger.");
        }
    }

    public void EnterCarAsPlayer()
    {
        isDriving = true;

        Debug.Log("🚗 Player is entering the car.");
        carCam.gameObject.SetActive(true);
        player.SetActive(false);
        playerController.enabled = false;
        carController.enabled = true;

        if (carRigidbody != null)
            carRigidbody.WakeUp();

        if (playerSeat != null)
            transform.position = playerSeat.position;
    }

    public void EnterCarAsAI()
    {
        isDriving = true;

    //    Debug.Log("🧠 Ron is entering the car.");
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
using System;
using UnityEngine;
using System.Collections.Generic;
public class DriverWiringAAC : MonoBehaviour
{
    public NodeRoadAAA currentNode;
    public float reachDistance = 3f;
    public float steerAngle = 25f;
    public float motorTorque = 800f;

     public float brakeTorque = 3000f;
  //  private List<NodeRoadAAA> nodePath = new List<NodeRoadAAA>();
    public List<NodeRoadAAA> path;
    private int pathIndex = 0;
    //private int currentPathIndex = 0;
    public float approachSlowdown = 0.3f; // 0 = no slowdown, 1 = full slowdown
    public float minSpeedFactor = 0.4f;   // how slow he can go at minimum

    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider rearLeft;
    public WheelCollider rearRight;

    internal void SetDestination(Vector3 position)
    {
        Debug.Log("VOIDLOAD");
    }

    private void FixedUpdate()
    {
        if (currentNode == null) return;

        Vector3 localTarget = transform.InverseTransformPoint(currentNode.transform.position);
        float distanceToNode = Vector3.Distance(transform.position, currentNode.transform.position);

        // Steering
        float steer = Mathf.Clamp(localTarget.x / localTarget.magnitude, -1f, 1f);
        float steerAngleApplied = steer * steerAngle;
        frontLeft.steerAngle = steerAngleApplied;
        frontRight.steerAngle = steerAngleApplied;

        // Braking / Slowing logic
        float slowdownFactor = Mathf.Clamp01(distanceToNode / 10f); // within 10 units = slow
        float speedFactor = Mathf.Lerp(minSpeedFactor, 1f, slowdownFactor * (1f - approachSlowdown));

        // Drive torque
        float torque = motorTorque * speedFactor;
        rearLeft.motorTorque = torque;
        rearRight.motorTorque = torque;

        // Switch to next node
      //  if (distanceToNode < reachDistance && currentNode.nextNode != null)
        {
       //     currentNode = currentNode.nextNode;
        }

       
      if (distanceToNode < reachDistance && path != null && pathIndex < path.Count - 1)
        {
            pathIndex++;
            currentNode = path[pathIndex];
        }
 if (distanceToNode < reachDistance && pathIndex >= path.Count - 1)
        {
            ApplyBrakes(brakeTorque); // full brake
            rearLeft.motorTorque = 0;
            rearRight.motorTorque = 0;
        }
    }
    void ApplyBrakes(float brakeAmount)
    {
        frontLeft.brakeTorque = brakeAmount;
        frontRight.brakeTorque = brakeAmount;
        rearLeft.brakeTorque = brakeAmount;
        rearRight.brakeTorque = brakeAmount;
    }

public void SetPath(List<NodeRoadAAA> newPath)
    {
        if (newPath == null || newPath.Count == 0)
        {
            Debug.LogWarning("❌ Path is null or empty.");
            return;
        }

        path = newPath;              // ✅ assign to correct `path` variable
        pathIndex = 0;
        currentNode = path[pathIndex];

        Debug.Log($"🚦 Path assigned with {path.Count} nodes. Starting at: {currentNode.name}");
    }
}
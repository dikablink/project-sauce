using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class DriverWiringAAB : MonoBehaviour
{
    [Header("Navigation Settings")]
    public Transform destination;
    public float waypointReachThreshold = 3f;

    [Header("Driving Settings")]
    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider rearLeft;
    public WheelCollider rearRight;
    public float maxMotorTorque = 1000f;
    public float maxSteerAngle = 30f;
    public float speed = 10f;

    private Rigidbody rb;
    private List<Vector3> drivePath = new List<Vector3>();
    private int currentTargetIndex = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GeneratePath();
    }

    void FixedUpdate()
    {
        if (drivePath.Count == 0) return;

        Vector3 target = drivePath[currentTargetIndex];
        Vector3 localTarget = transform.InverseTransformPoint(target);
        float steer = Mathf.Clamp(localTarget.x / localTarget.magnitude, -1f, 1f);
        float motor = Mathf.Clamp(1f - Mathf.Abs(steer), 0.5f, 1f); // Slow on hard turns

        ApplySteering(steer);
        ApplyDrive(motor);

        float distanceToTarget = Vector3.Distance(transform.position, target);
        if (distanceToTarget < waypointReachThreshold && currentTargetIndex < drivePath.Count - 1)
        {
            currentTargetIndex++;
        }
    }

    void ApplySteering(float steerInput)
    {
        float steerAngle = steerInput * maxSteerAngle;
        frontLeft.steerAngle = steerAngle;
        frontRight.steerAngle = steerAngle;
    }

    void ApplyDrive(float motorInput)
    {
        float torque = motorInput * maxMotorTorque;
        rearLeft.motorTorque = torque;
        rearRight.motorTorque = torque;
    }

    public void GeneratePath()
    {
        Debug.Log("PATH START");
        if (destination == null)
        {
            Debug.LogWarning("No destination set for AI car.");
            return;
        }

        NavMeshPath path = new NavMeshPath();
        if (NavMesh.CalculatePath(transform.position, destination.position, NavMesh.AllAreas, path))
        {
            drivePath = new List<Vector3>(path.corners);
            currentTargetIndex = 0;
            Debug.Log($"[HybridNavCarDriver] Path generated with {drivePath.Count} points.");
        }
        else
        {
            Debug.LogWarning("Path could not be calculated!");
        }
    }
}
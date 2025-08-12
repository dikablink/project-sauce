using System;
using UnityEngine;
using UnityEngine.AI;
public class DriverWiring : MonoBehaviour
{
    public Transform targetPoint;
    public float driveForce = 300f;
    public float maxSteerAngle = 30f;
    public float stopDistance = 2f;

    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider rearLeft;
    public WheelCollider rearRight;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (targetPoint == null) return;

        Vector3 localTarget = transform.InverseTransformPoint(targetPoint.position);
        float distance = Vector3.Distance(transform.position, targetPoint.position);

        float steer = Mathf.Clamp(localTarget.x / localTarget.magnitude, -1f, 1f);
        float motor = distance > stopDistance ? 1f : 0f;

        // Apply steering
        float steerAngle = steer * maxSteerAngle;
        frontLeft.steerAngle = steerAngle;
        frontRight.steerAngle = steerAngle;

        // Apply driving torque
        float torque = motor * driveForce;
        rearLeft.motorTorque = torque;
        rearRight.motorTorque = torque;

        // Apply brake when near target
        float brake = distance <= stopDistance ? 10000f : 0f;
        frontLeft.brakeTorque = brake;
        frontRight.brakeTorque = brake;
        rearLeft.brakeTorque = brake;
        rearRight.brakeTorque = brake;
    }
}
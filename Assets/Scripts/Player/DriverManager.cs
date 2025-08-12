using UnityEngine;

public class DriverManager : MonoBehaviour
{
    public Transform target;
    public float maxSteerAngle = 30f;
    public float motorTorque = 1500f;
    public float stoppingDistance = 2f;

    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider rearLeft;
    public WheelCollider rearRight;

    public Transform frontLeftMesh;
    public Transform frontRightMesh;
    public Transform rearLeftMesh;
    public Transform rearRightMesh;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = Vector3.down * 0.5f;
    }

    void FixedUpdate()
    {
        if (target == null) return;

        Vector3 localTarget = transform.InverseTransformPoint(target.position);
        float steer = Mathf.Clamp(localTarget.x / localTarget.magnitude, -1f, 1f);
        float motor = localTarget.z > stoppingDistance ? 1f : 0f;

        // Apply steering
        frontLeft.steerAngle = steer * maxSteerAngle;
        frontRight.steerAngle = steer * maxSteerAngle;

        // Apply motor torque
        rearLeft.motorTorque = motor * motorTorque;
        rearRight.motorTorque = motor * motorTorque;

        UpdateWheelMesh(frontLeft, frontLeftMesh);
        UpdateWheelMesh(frontRight, frontRightMesh);
        UpdateWheelMesh(rearLeft, rearLeftMesh);
        UpdateWheelMesh(rearRight, rearRightMesh);
    }

    void UpdateWheelMesh(WheelCollider col, Transform mesh)
    {
        Vector3 pos;
        Quaternion rot;
        col.GetWorldPose(out pos, out rot);
        mesh.position = pos;
        mesh.rotation = rot;
    }

    public void SetDestination(Transform newTarget)
    {
        target = newTarget;
    }
}
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarControllerAAC : MonoBehaviour
{
//    public Transform centerofmassref;
    public float motorTorque = 1500f;
    public float maxSteerAngle = 30f;
    public float check1;
    public float breakpress;
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    public Transform frontLeftTransform;
    public Transform frontRightTransform;
    public Transform rearLeftTransform;
    public Transform rearRightTransform;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0); // tweak if needed
    }

    void Update()
    {

      //  rb.centerOfMass = transform.InverseTransformDirection(centerofmassref.position);
        if (Input.GetKey(KeyCode.W))
        {
            motorTorque = check1;
        }
        else
        {
            motorTorque = 0;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            rearLeftWheel.brakeTorque = breakpress;
            rearRightWheel.brakeTorque = breakpress;
        }
    }
    void FixedUpdate()
    {
        float motor = Input.GetAxis("Vertical") * motorTorque;
        float steer = Input.GetAxis("Horizontal") * maxSteerAngle;

        frontLeftWheel.steerAngle = steer;
        frontRightWheel.steerAngle = steer;

        rearLeftWheel.motorTorque = motor;
        rearRightWheel.motorTorque = motor;

        UpdateWheels();
    }

    void UpdateWheels()
    {
        UpdateWheelPose(frontLeftWheel, frontLeftTransform);
        UpdateWheelPose(frontRightWheel, frontRightTransform);
        UpdateWheelPose(rearLeftWheel, rearLeftTransform);
        UpdateWheelPose(rearRightWheel, rearRightTransform);
    }

    void UpdateWheelPose(WheelCollider collider, Transform trans)
    {
        Vector3 pos;
        Quaternion rot;
        collider.GetWorldPose(out pos, out rot);
        trans.position = pos;
        trans.rotation = rot;
    }
}
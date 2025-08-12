using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class CarControllerAAD : MonoBehaviour
{
    [Header("Wheels")]
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    [Header("Engine Settings")]
    public float engineTorque = 400f;
    public float maxRPM = 7000f;
    public float minRPM = 1000f;
    public float currentRPM;

    [Header("Gears")]
    public float[] gearRatios = { 3.5f, 2.5f, 1.8f, 1.2f, 0.9f };
    public int currentGear = 0;
    public float shiftUpRPM = 6000f;
    public float shiftDownRPM = 2500f;

    [Header("Steering")]
    public float maxSteerAngle = 30f;

    [Header("Debug")]
    public float displaySpeed;
    public float rpmGainRate = 3500f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0); // optional: lower center for more stability
    }

    
    void FixedUpdate()
    {
        float accelInput = Input.GetAxis("Vertical");
        float steerInput = Input.GetAxis("Horizontal");

        float gearRatio = gearRatios[currentGear];

        UpdateRPM(accelInput);
        ApplyMotorTorque(accelInput, gearRatio);
        ApplySteering(steerInput);

        displaySpeed = rb.linearVelocity.magnitude * 3.6f; // km/h

        HandleGearShifting();
    }

    void UpdateRPM(float accelInput)
    {
        if (accelInput > 0.1f)
        {
            currentRPM += rpmGainRate * Time.fixedDeltaTime;
        }
        else
        {
            currentRPM -= rpmGainRate * 1.2f * Time.fixedDeltaTime;
        }

        currentRPM = Mathf.Clamp(currentRPM, minRPM, maxRPM);
    }

    void ApplyMotorTorque(float accelInput, float gearRatio)
    {
        float torque = engineTorque * gearRatio * accelInput;

        rearLeftWheel.motorTorque = torque;
        rearRightWheel.motorTorque = torque;
    }

    void ApplySteering(float steerInput)
    {
        float steerAngle = steerInput * maxSteerAngle;
        frontLeftWheel.steerAngle = steerAngle;
        frontRightWheel.steerAngle = steerAngle;
    }

    void HandleGearShifting()
    {
        if (currentGear < gearRatios.Length - 1 && currentRPM >= shiftUpRPM)
        {
            currentGear++;
            currentRPM = Mathf.Lerp(minRPM, maxRPM - 1500f, 0.5f);
        }
        else if (currentGear > 0 && currentRPM <= shiftDownRPM)
        {
            currentGear--;
            currentRPM = Mathf.Lerp(minRPM, maxRPM * 0.5f, 0.5f);
        }
    }
}
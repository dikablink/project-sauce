using UnityEngine;

public class CarControllerAAB : MonoBehaviour
{
    [Header("Engine Settings")]
    public float engineTorque = 500f;
    public float maxRPM = 7000f;
    public float minRPM = 1000f;
    public float currentRPM;

    [Header("Gears")]
    public float[] gearRatios = { 3.5f, 2.5f, 1.5f, 1.0f, 0.8f };
    public int currentGear = 0;
    public float shiftUpRPM = 5500f;
    public float shiftDownRPM = 2500f;

    [Header("Physics")]
    public float maxSpeed = 50f;
    public float turnStrength = 100f;
    public float rpmGainRate = 2500f;

    [Header("Debug")]
    public float displaySpeed; // scaled for Inspector
    public bool isAccelerating;
    public float speedScale = 1f; // like your 'killer' variable

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = 1000f;
        rb.linearDamping = 0.05f;
        rb.angularDamping = 0.05f;
    }

    void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Vertical");
        float steerInput = Input.GetAxis("Horizontal");

        float torqueOutput = 0f;
        isAccelerating = moveInput > 0.1f;

        float velocity = rb.linearVelocity.magnitude;
        displaySpeed = velocity * speedScale;

        // Simulate RPM climbing when accelerating
        if (isAccelerating)
        {
            currentRPM += rpmGainRate * Time.fixedDeltaTime;
        }
        else
        {
            currentRPM -= rpmGainRate * 0.8f * Time.fixedDeltaTime;
        }

        // Clamp RPM between min and max
        currentRPM = Mathf.Clamp(currentRPM, minRPM, maxRPM);

        // Compute torque
        float gearRatio = gearRatios[currentGear];
        torqueOutput = engineTorque * gearRatio;

        if (velocity < maxSpeed && isAccelerating)
        {
            Vector3 force = transform.forward * torqueOutput * Time.fixedDeltaTime;
            rb.AddForce(force);
        }

        // Steering
        rb.AddTorque(transform.up * steerInput * turnStrength * Time.fixedDeltaTime);

        HandleGearShifting();
    }

    void HandleGearShifting()
    {
        if (currentGear < gearRatios.Length - 1 && currentRPM >= shiftUpRPM)
        {
            currentGear++;
            currentRPM = Mathf.Lerp(minRPM + 500, maxRPM - 1000, 0.5f); // drop RPM a bit on shift
        }
        else if (currentGear > 0 && currentRPM <= shiftDownRPM)
        {
            currentGear--;
            currentRPM = Mathf.Lerp(minRPM, maxRPM * 0.5f, 0.5f); // raise RPM on downshift
        }
    }
}
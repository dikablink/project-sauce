using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarControllerAAA : MonoBehaviour
{
    public float acceleration = 2000f;
    public float turnStrength = 100f;
    public float maxSpeed = 50f;
    public Transform centerOfMass;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Set floaty, top-heavy physics
        rb.linearDamping = 0.05f;
        rb.angularDamping = 0.03f;

        if (centerOfMass != null)
            rb.centerOfMass = centerOfMass.localPosition;
    }

    void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        // Forward/back movement
        if (rb.linearVelocity.magnitude < maxSpeed)
            rb.AddForce(transform.forward * moveInput * acceleration * Time.fixedDeltaTime);

        // Apply turning torque (adds drift-like feel)
        rb.AddTorque(transform.up * turnInput * turnStrength * Time.fixedDeltaTime);
    }
}
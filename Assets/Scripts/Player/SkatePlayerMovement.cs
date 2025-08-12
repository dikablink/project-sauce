using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SkatePlayerMovement : MonoBehaviour
{
    //SAVE51925
    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float jumpForce = 5f;
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public CapsuleCollider playerCollider;
    public LayerMask groundLayer;            // Set this to “Ground” in Inspector
    public float groundCheckOffset = 0.1f;

    [Header("Board Status")]
    public bool isOnBoard { get; private set; }

    [Header("Board Control")]
    public KeyCode rideUpKey = KeyCode.Space;  // while on board
    public KeyCode rideDownKey = KeyCode.V;

    Rigidbody rb;
    bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 1) Movement XZ
        float mx = Input.GetAxis("Horizontal");
        float mz = Input.GetAxis("Vertical");
        Vector3 dir = transform.TransformDirection(new Vector3(mx, 0, mz));
        Vector3 vel = rb.linearVelocity;
        vel.x = dir.x * moveSpeed;
        vel.z = dir.z * moveSpeed;
        rb.linearVelocity = vel;

        // 2) Ground check
        float castDist = (playerCollider.height / 2f) + groundCheckOffset;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, castDist, groundLayer, QueryTriggerInteraction.Ignore);

        // 3) Jump (off board only)
        if (!isOnBoard && isGrounded && Input.GetKeyDown(jumpKey))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // 4) Ride up/down (when on board)
        if (isOnBoard)
        {
            Vector3 v = rb.linearVelocity;
            if (Input.GetKey(rideUpKey))   v.y =  Mathf.Abs(jumpForce);
            if (Input.GetKey(rideDownKey)) v.y = -Mathf.Abs(jumpForce);
            rb.linearVelocity = v;
        }
    }

    /// <summary>
    /// Called by trigger script when you attach/detach from board
    /// </summary>
    public void SetOnBoard(bool onBoard)
    {
        isOnBoard = onBoard;
    }
}
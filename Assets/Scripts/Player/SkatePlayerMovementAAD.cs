using UnityEngine;

public class SkatePlayerMovementAAD : MonoBehaviour
{
   [SerializeField] public SkateTrickManagerAAC trickManager;

    [Header("Player Settings")]
    public Rigidbody rb;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Detection")]
    public CapsuleCollider playerCollider;
    public LayerMask groundLayer;
    public bool isGrounded;


    [Header("Board Status")]
    public bool isOnBoard; //this dosnt ref any bro 
                           //like u only used this to say if its off which u dont elberote when it should be turned on
    void FixedUpdate()
    {
        if (trickManager.didOllie)
        {
            float maxY = trickManager.boardYAtOllie + trickManager.ollieMaxPlayerOffset;

            if (transform.position.y > maxY && rb.linearVelocity.y > 0)
            {
                Vector3 v = rb.linearVelocity;
                v.y = 0f;
                rb.linearVelocity = v;
            }
        }
    }
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.TransformDirection(new Vector3(moveX, 0, moveZ));
   //     rb.velocity = new Vector3(move.x * moveSpeed, rb.velocity.y, move.z * moveSpeed);
rb.linearVelocity = new Vector3(move.x * moveSpeed, rb.linearVelocity.y, move.z * moveSpeed);
        float castDist = (playerCollider.height / 2f) + 0.1f;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, castDist, groundLayer);

        if (Input.GetKeyDown(jumpKey) && isGrounded && !isOnBoard)//<--- look. u just say this is on without ref any other time
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
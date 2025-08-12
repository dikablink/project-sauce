using UnityEngine;
using UnityEngine.InputSystem;
public class IdvInputAAA : MonoBehaviour
{
    public Rigidbody rb;
    public float moveSpeed = 6f;
    public float jumpForce = 5f;
    public CapsuleCollider playerCollider;

    [Header("Board Detection")]
    public FootBoardDetector leftFootDetector;
    public FootBoardDetector rightFootDetector;
    public SkateInputAAB skateboardManager;

    private bool isGrounded = false;
    private bool isJumping = false;

    // Use a private field and a public getter/setter
    private bool _isOnBoard = false;
    public bool isOnBoard => _isOnBoard;  // read-only for other scripts

    public void SetIsOnBoard(bool value) // Call this from other scripts like triggers
    {
        _isOnBoard = value;
    }

    void Update()
    {
        bool feetOnBoard = (leftFootDetector != null && leftFootDetector.isOnBoard)
                        && (rightFootDetector != null && rightFootDetector.isOnBoard);

        bool boardAttached = skateboardManager != null && skateboardManager.IsBoardAttached();

        _isOnBoard = feetOnBoard && boardAttached;

        // Movement input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(moveX, 0, moveZ);
        Vector3 moveRelative = transform.TransformDirection(move);

        Vector3 currentVelocity = rb.linearVelocity;
        currentVelocity.x = moveRelative.x * moveSpeed;
        currentVelocity.z = moveRelative.z * moveSpeed;

        if (_isOnBoard)
        {
            float verticalInput = 0f;
            if (Input.GetKey(KeyCode.Space)) verticalInput += 1f;
            if (Input.GetKey(KeyCode.V)) verticalInput -= 1f;

            currentVelocity.y = verticalInput * 5f; // Up/down movement while on board
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isJumping = true;
            }
        }

        rb.linearVelocity = currentVelocity;

        isGrounded = Physics.Raycast(transform.position, Vector3.down, (playerCollider.height / 2f) + 0.1f);
        if (isGrounded) isJumping = false;
    }
}
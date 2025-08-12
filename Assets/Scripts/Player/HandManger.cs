using UnityEngine;

public class HandManager : MonoBehaviour
{
    //05-12-25 only works with obja tho
    [Header("Hand Mesh Transforms")]
    public Transform rightHand;
    public Transform leftHand;

    [Header("Movement Settings")]
    public float moveSpeed = 0.01f;
    public Vector3 movementLimit = new Vector3(1f, 2f, 1f); // Realistic human arm reach in Unity units

    [Header("Control Keys")]
    public KeyCode toggleHandControlKey = KeyCode.H;
    public KeyCode cameraLockKey = KeyCode.LeftShift;

    [Header("Runtime State (Read-Only)")]
    public bool handMovementEnabled = false;
    public bool rightHandMoving = false;
    public bool leftHandMoving = false;
    public bool isCameraLocked = false;

    [Header("Optional References")]
    public MonoBehaviour cameraLookScript; // Drag your camera movement script here

    private Vector3 rightHandStartPos;
    private Vector3 leftHandStartPos;

    private Collider rightHandCollider;
    private Collider leftHandCollider;

    void Start()
    {
        if (rightHand != null) rightHandStartPos = rightHand.localPosition;
        if (leftHand != null) leftHandStartPos = leftHand.localPosition;

        // Get the colliders for both hands
        if (rightHand != null) rightHandCollider = rightHand.GetComponent<Collider>();
        if (leftHand != null) leftHandCollider = leftHand.GetComponent<Collider>();
    }

    void Update()
    {
        // Toggle hand movement mode
        if (Input.GetKeyDown(toggleHandControlKey))
        {
            handMovementEnabled = !handMovementEnabled;
            rightHandMoving = false;
            leftHandMoving = false;

            if (!handMovementEnabled)
            {
                if (rightHand != null) rightHand.localPosition = rightHandStartPos;
                if (leftHand != null) leftHand.localPosition = leftHandStartPos;
            }
        }

        // Check for camera lock
        isCameraLocked = Input.GetKey(cameraLockKey);
        if (cameraLookScript != null)
            cameraLookScript.enabled = !isCameraLocked;

        if (!handMovementEnabled) return;

        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        float scrollDelta = Input.mouseScrollDelta.y;

        // Right hand (Right Click)
        if (Input.GetMouseButton(1))
        {
            rightHandMoving = true;
            if (rightHand != null)
                MoveHand(rightHand, rightHandStartPos, mouseDelta, scrollDelta);
        }
        else
        {
            rightHandMoving = false;
        }

        // Left hand (Left Click)
        if (Input.GetMouseButton(0))
        {
            leftHandMoving = true;
            if (leftHand != null)
                MoveHand(leftHand, leftHandStartPos, mouseDelta, scrollDelta);
        }
        else
        {
            leftHandMoving = false;
        }
    }

    void MoveHand(Transform hand, Vector3 originPos, Vector2 delta, float scroll)
    {
        Vector3 offset = new Vector3(delta.x, delta.y, scroll) * moveSpeed;
        Vector3 newLocalPos = hand.localPosition + offset;

        newLocalPos.x = Mathf.Clamp(newLocalPos.x, originPos.x - movementLimit.x, originPos.x + movementLimit.x);
        newLocalPos.y = Mathf.Clamp(newLocalPos.y, originPos.y - movementLimit.y, originPos.y + movementLimit.y);
        newLocalPos.z = Mathf.Clamp(newLocalPos.z, originPos.z - movementLimit.z, originPos.z + movementLimit.z);

        hand.localPosition = newLocalPos;
    }

    // Method to toggle the hand colliders (for when an object is picked up or dropped)
    public void ToggleHandCollision(bool enable)
    {
        if (rightHandCollider != null) rightHandCollider.enabled = enable;
        if (leftHandCollider != null) leftHandCollider.enabled = enable;
    }
    
}
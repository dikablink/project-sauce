using UnityEngine;
using UnityEngine.InputSystem;
public class PickupHandler_ScriptC : MonoBehaviour
{
    [Header("Pickup Settings")]
    public float pickupRange = 3f;
    public Transform holdPoint;
    public LayerMask pickupLayer;

    [Header("Keybinds")]
    public Key pickupKey = Key.E;
    public Key throwKey = Key.T;
    public Key rotateKey = Key.R;
    public Key cameraLockKey = Key.LeftShift;[Header("Mouse Controls")]
    public bool useRightMouseToMove = true; // Right mouse button for object movement

    [Header("Debug State Flags")]
    public bool canPickup;
    public bool isHolding;
    public bool isThrowing;
    public bool isRotating;
    public bool isCameraLocked;

    [Header("References")]
    public MouseLook cameraLookScript; // Reference to MouseLook script

    public Rigidbody HeldObject { get; private set; }

    private Camera mainCam;
    private Vector2 mouseDelta;
    private Collider heldCollider;

    [Header("Movement Settings")]
    public float followSpeed = 10f;

    [Header("Rotation Settings")]
    public float rotationSpeed = 100f;
    public float moveSensitivity = 0.01f; // Sensitivity for object position movement

    void Awake()
    {
        mainCam = Camera.main;

        if (mainCam == null)
            Debug.LogError("PickupHandler: No main camera found!");
    }

    void Update()
    {
        if (useRightMouseToMove && Mouse.current.rightButton.isPressed)
{
    MoveHeldObject(mouseDelta);
}
        if (HeldObject == null)
        {
            CheckForPickupTarget();
        }

        if (Keyboard.current[pickupKey].wasPressedThisFrame && canPickup)
        {
            Debug.Log("Pickup key pressed, attempting pickup.");
            TryPickupOrDrop();
        }

        if (Keyboard.current[throwKey].wasPressedThisFrame && isHolding)
        {
            Debug.Log("Throw key pressed.");
            ThrowHeldObject();
        }

        if (Keyboard.current[rotateKey].isPressed && isHolding)
        {
            isRotating = true;
            isCameraLocked = Keyboard.current[cameraLockKey].isPressed;

            if (cameraLookScript != null)
            {
                cameraLookScript.enabled = !isCameraLocked;
                Debug.Log(isCameraLocked ? "Camera movement disabled." : "Camera movement enabled.");
            }

            mouseDelta = Mouse.current.delta.ReadValue();
            RotateHeldObject(mouseDelta);

            if (Mouse.current.rightButton.isPressed)
            {
                MoveHeldObject(mouseDelta);
            }
        }
        else
        {
            if (isRotating && cameraLookScript != null && !cameraLookScript.enabled)
            {
                cameraLookScript.enabled = true;
                Debug.Log("Camera movement restored.");
            }

            isRotating = false;
            isCameraLocked = false;
        }
    }

    void FixedUpdate()
    {
        if (HeldObject != null)
        {
            // Parent-based movement only
        }
    }

    void CheckForPickupTarget()
    {
        Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
        RaycastHit hit;
        canPickup = Physics.Raycast(ray, out hit, pickupRange, pickupLayer);
    }

    void TryPickupOrDrop()
    {
        if (HeldObject != null)
        {
            DropHeldObject();
        }
        else
        {
            Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, pickupRange, pickupLayer))
            {
                Rigidbody rb = hit.collider.attachedRigidbody;

                if (rb != null)
                {
                    HeldObject = rb;
                    heldCollider = rb.GetComponent<Collider>();

                    if (heldCollider != null && GetComponent<Collider>() != null)
                        Physics.IgnoreCollision(heldCollider, GetComponent<Collider>(), true);

                    HeldObject.isKinematic = true;
                    HeldObject.transform.SetParent(holdPoint);
                    HeldObject.transform.localPosition = Vector3.zero;
                    HeldObject.transform.localRotation = Quaternion.identity;

                    isHolding = true;
                    isThrowing = false;
                }
            }
        }
    }

    void DropHeldObject()
    {
        if (HeldObject != null)
        {
            HeldObject.transform.SetParent(null);
            HeldObject.isKinematic = false;

            if (heldCollider != null && GetComponent<Collider>() != null)
                Physics.IgnoreCollision(heldCollider, GetComponent<Collider>(), false);

            HeldObject = null;
            heldCollider = null;
            isHolding = false;
            canPickup = false;
        }
    }

    void ThrowHeldObject()
    {
        if (HeldObject != null)
        {
            HeldObject.transform.SetParent(null);
            HeldObject.isKinematic = false;
            HeldObject.linearVelocity = mainCam.transform.forward * 10f;

            if (heldCollider != null && GetComponent<Collider>() != null)
                Physics.IgnoreCollision(heldCollider, GetComponent<Collider>(), false);

            isThrowing = true;
            isHolding = false;
            HeldObject = null;
            heldCollider = null;
            canPickup = false;
        }
    }

    void RotateHeldObject(Vector2 delta)
    {
        if (HeldObject != null)
        {
            float rotX = delta.y * rotationSpeed * Time.deltaTime;
            float rotY = -delta.x * rotationSpeed * Time.deltaTime;
            HeldObject.transform.Rotate(mainCam.transform.up, rotY, Space.World);
            HeldObject.transform.Rotate(mainCam.transform.right, rotX, Space.World);
        }
    }

    void MoveHeldObject(Vector2 delta)
    {
        if (HeldObject != null)
        {
            Vector3 moveOffset = new Vector3(delta.x, delta.y, 0f) * moveSensitivity;
            HeldObject.transform.localPosition += moveOffset;
            Debug.Log("Moving held object: " + moveOffset);
        }
    }
}

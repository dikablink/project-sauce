using UnityEngine;
using UnityEngine.InputSystem;

public class PickupManager : MonoBehaviour
{
    //05-12-25
    public HandManager handManager;
    [Header("Settings")]
    public float pickupRange = 3f;
    public Transform holdPoint;
    public LayerMask pickupLayer;
    public LayerMask layer2;

    [Header("Keybinds")]
    public Key pickupKey = Key.E;
    public Key throwKey = Key.T;
    public Key rotateKey = Key.R;
    public Key cameraLockKey = Key.LeftShift;

    [Header("Mouse Control")]
    public bool useRightMouseToMove = true;

    [Header("Debugging")]
    public bool canPickup = false; // This is the debug value

    private Camera mainCam;
    private PickupObj_ScriptA heldObject;
    private bool isRotating;
    private bool isCameraLocked;
    public MouseLook cameraLookScript; // Optional reference
void Start()
{
   if (handManager == null)
    {
        Debug.LogError("❌ No HandManager assigned to PickupManager on: " + gameObject.name);
        this.enabled = false; // Disable to stop errors & pause
        return;
    }

    Debug.Log("✅ [PickupManager] Hand Manager assigned: " + handManager.gameObject.name);
    Debug.Log("✅ [PickupManager] On GameObject: " + gameObject.name);
}

    void Awake()
    {
        mainCam = Camera.main;

        if (mainCam == null)
            Debug.LogError("PickupManager: No main camera found!");
    }

    void Update()
    {
        // Check if the player can pick up an object
        CheckCanPickup();
            Debug.DrawRay(handManager.rightHand.position, handManager.rightHand.forward * pickupRange, Color.red);
Debug.DrawRay(handManager.leftHand.position, handManager.leftHand.forward * pickupRange, Color.blue);

        if (Keyboard.current[pickupKey].wasPressedThisFrame)
        {
            if (heldObject == null && canPickup)
            {
                TryPickup();
            }
            else
            {
                Drop();
            }
        }

        if (Keyboard.current[throwKey].wasPressedThisFrame && heldObject != null)
        {
            Throw();
        }

        if (Keyboard.current[rotateKey].isPressed && heldObject != null)
        {
            isRotating = true;
            isCameraLocked = Keyboard.current[cameraLockKey].isPressed;

            if (cameraLookScript != null)
                cameraLookScript.enabled = !isCameraLocked;

            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            heldObject.Rotate(mouseDelta, 100f, mainCam.transform);
        }
        else
        {
            if (isRotating && cameraLookScript != null && !cameraLookScript.enabled)
                cameraLookScript.enabled = true;

            isRotating = false;
            isCameraLocked = false;
        }

        //if (useRightMouseToMove && heldObject != null && Mouse.current.rightButton.isPressed)
       // {
       //     Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        //    heldObject.Move(mouseDelta);
      //  }
    }
bool CheckHand(Transform hand, LayerMask layer)
{
    RaycastHit hit;

    // Draw the ray for visual debug in Scene view
    Debug.DrawRay(hand.position, hand.forward * pickupRange, Color.red); // or blue for left hand

    // Perform the SphereCast
    if (Physics.SphereCast(hand.position, 0.5f, hand.forward, out hit, pickupRange, layer))
    {
        Debug.Log("🟢 SphereCast hit: " + hit.collider.name);

        // Check if the object is pickupable
        return hit.collider.GetComponent<PickupObj_ScriptA>() != null;
    }

    return false;
}

   void CheckCanPickup()
{
    bool foundPickup = false;
    RaycastHit hit;

    // Right hand
    if (Physics.SphereCast(handManager.rightHand.position, 0.3f, handManager.rightHand.forward, out hit, pickupRange, pickupLayer | layer2))
    {
        if (hit.collider.GetComponent<PickupObj_ScriptA>() != null)
        {
            Debug.Log("🟢 Right hand sees a pickup: " + hit.collider.name);
            foundPickup = true;
        }
    }

    // Left hand
    if (Physics.SphereCast(handManager.leftHand.position, 0.3f, handManager.leftHand.forward, out hit, pickupRange, pickupLayer | layer2))
    {
        if (hit.collider.GetComponent<PickupObj_ScriptA>() != null)
        {
            Debug.Log("🟢 Left hand sees a pickup: " + hit.collider.name);
            foundPickup = true;
        }
    }

    canPickup = foundPickup;

 //   Debug.Log(canPickup ? "🟢 Can pickup object in range." : "🔴 Nothing in range to pick up.");
}

    void TryPickup()
    {
        
        RaycastHit hit;
        LayerMask allPickupLayers = pickupLayer | layer2;
        // Try Right Hand Spherecast
        if (Physics.SphereCast(handManager.rightHand.position, 0.3f, handManager.rightHand.forward, out hit, pickupRange, pickupLayer | layer2))

        {
            PickupObj_ScriptA pickup = hit.collider.GetComponent<PickupObj_ScriptA>();
            if (pickup != null)
            {
                pickup.Pickup(holdPoint);
                heldObject = pickup;
                Debug.Log("✅ Picked up object: " + pickup.name);

                if (handManager != null)
                    handManager.handMovementEnabled = true;

                // ✅ Attach to RIGHT HAND (because right hand picked it up)
                heldObject.transform.SetParent(handManager.rightHand);
                heldObject.transform.localPosition = Vector3.zero; Debug.Log("🖐️ Trying to pick up with right hand...");

                heldObject.transform.localRotation = Quaternion.identity;
                return; // Done picking up
            }
        }

        // Try Left Hand Spherecast
        if (Physics.SphereCast(handManager.leftHand.position, 0.3f, handManager.leftHand.forward, out hit, pickupRange, pickupLayer))
        {
            PickupObj_ScriptA pickup = hit.collider.GetComponent<PickupObj_ScriptA>();
            if (pickup != null)
            {
                pickup.Pickup(holdPoint);
                heldObject = pickup;

                if (handManager != null)
                    handManager.handMovementEnabled = true;

                // ✅ Attach to LEFT HAND (because left hand picked it up)
                heldObject.transform.SetParent(handManager.leftHand);
                heldObject.transform.localPosition = Vector3.zero;
                heldObject.transform.localRotation = Quaternion.identity;
                return; // Done picking up
            }
        }
    
}

    void Drop()
    {
        if (handManager != null)
            handManager.handMovementEnabled = false;
        if (heldObject != null)
        {
            heldObject.Drop();
            heldObject.transform.SetParent(null); // Detach from hand
            heldObject = null;
        }
    }

    void Throw()
    {
        if (handManager != null)
            handManager.handMovementEnabled = false;
        if (heldObject != null)
        {
            heldObject.Throw(mainCam.transform.forward * 10f);
            heldObject.transform.SetParent(null); // Detach from hand
            heldObject = null;
        }
    }
}
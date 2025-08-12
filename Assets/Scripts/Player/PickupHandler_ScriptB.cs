using UnityEngine;
using UnityEngine.InputSystem;

public class PickupHandler_ScriptB : MonoBehaviour
{
    [Header("Pickup Settings")]
    public float pickupRange = 3f;
    public Transform holdPoint;
    public LayerMask pickupLayer;

    [Header("Debug State Flags")]
    public bool canPickup;
    public bool isHolding;
    public bool isThrowing;

    public Rigidbody HeldObject { get; private set; }

    private Camera mainCam;
    private InputSystem_Actions inputActions;
    private InputAction throwAction;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Enable();

        inputActions.Player.Interact.performed += _ => TryPickupOrDrop();

        throwAction = new InputAction("Throw", binding: "<Keyboard>/t");
        throwAction.performed += _ => ThrowHeldObject();
        throwAction.Enable();

        mainCam = Camera.main;

        if (mainCam == null)
            Debug.LogError("PickupHandler: No main camera found!");
    }

    void OnDestroy()
    {
        throwAction?.Dispose();
    }

    void Update()
    {
        if (HeldObject == null)
        {
            CheckForPickupTarget();
        }
        else
        {
            HeldObject.MovePosition(holdPoint.position);
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
                    HeldObject.transform.SetParent(holdPoint);
                    HeldObject.transform.localPosition = Vector3.zero;
                    HeldObject.transform.localRotation = Quaternion.identity;
                    HeldObject.isKinematic = false;
                    HeldObject.linearVelocity = Vector3.zero;
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
            HeldObject.linearVelocity = Vector3.zero;
            HeldObject = null;
            isHolding = false;
            canPickup = false;
        }
    }

    void ThrowHeldObject()
    {
        if (HeldObject != null)
        {
            HeldObject.transform.SetParent(null);
            HeldObject.linearVelocity = mainCam.transform.forward * 10f;
            isThrowing = true;
            isHolding = false;
            HeldObject = null;
            canPickup = false;

            Invoke("ResetThrowState", 0.5f);
        }
    }

    void ResetThrowState()
    {
        isThrowing = false;
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class HeldObjectManipulator : MonoBehaviour
{
    [Header("References")]
    public PickupHandler pickupHandler;

    [Header("Drag Settings")]
    public bool enableDrag = true;
    public float dragSpeed = 0.01f;

    [Header("Rotation Settings")]
    public bool enableRotation = true;
    public float rotationSpeed = 5f;
    public bool rotateWithRightClick = true;

    private InputSystem_Actions inputActions;
    private Vector2 mouseDelta;
  void Start() {
        // ...
    }
    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void Update()
    {
        if (pickupHandler == null || pickupHandler.HeldObject == null)
            return;

        mouseDelta = inputActions.Player.Look.ReadValue<Vector2>();

        // Rotate with Right Click
        if (enableRotation && rotateWithRightClick && Mouse.current.rightButton.isPressed)
        {
            RotateHeldObject();
        }
        // Drag with mouse otherwise
        else if (enableDrag)
        {
            DragHeldObject();
        }
    }

    void DragHeldObject()
    {
        Vector3 movement = new Vector3(mouseDelta.x, mouseDelta.y, 0f) * dragSpeed;
        pickupHandler.HeldObject.transform.localPosition += movement;
    }

    void RotateHeldObject()
    {
        Vector3 rotation = new Vector3(-mouseDelta.y, mouseDelta.x, 0f) * rotationSpeed;
        pickupHandler.HeldObject.transform.Rotate(rotation, Space.Self);
    }
}
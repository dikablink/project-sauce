using UnityEngine;
using UnityEngine.InputSystem;

public class PickupObj_ScriptA : MonoBehaviour
{
    //-05-12-25
    private Rigidbody rb;
    private Collider col;

    private Vector3 initialLocalPosition;

    [Header("Movement Settings")]
    [Tooltip("Maximum offset in meters (10ft = 3.048m) from the initial position on each axis")]
    public float maxOffset = 3.048f;

    [Tooltip("Sensitivity of mouse movement when moving the object")]
    public float moveSensitivity = 0.01f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void Pickup(Transform holdPoint)
    {
        rb.isKinematic = true;
        col.isTrigger = true; // Make the collider a trigger to avoid collisions with the player
        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        StoreInitialPosition();
    }

    public void Drop()
    {
        transform.SetParent(null);
        rb.isKinematic = false;
        col.isTrigger = false; // Set the collider back to normal for physical interactions
    }

    public void Throw(Vector3 force)
    {
        Drop();
        rb.AddForce(force, ForceMode.Impulse);
    }

    public void Rotate(Vector2 delta, float rotationSpeed = 100f, Transform cameraTransform = null)
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        float rotX = delta.y * rotationSpeed * Time.deltaTime;
        float rotY = -delta.x * rotationSpeed * Time.deltaTime;

        transform.Rotate(cameraTransform.up, rotY, Space.World);
        transform.Rotate(cameraTransform.right, rotX, Space.World);
    }

    public void StoreInitialPosition()
    {
        initialLocalPosition = transform.localPosition;
    }
//this used to mvoe the obj from this script not hands if u more height or distance this ur guy check pickup tho there also another guy
  //  public void Move(Vector2 delta)
  //  {
   //     Vector3 moveOffset = new Vector3(delta.x * moveSensitivity, delta.y * moveSensitivity, 0);
   //     transform.localPosition += moveOffset;
   // }
}
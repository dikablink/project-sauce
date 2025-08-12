using UnityEngine;

public class CrouchController : MonoBehaviour
{
    [Header("Crouch Settings")]
    public KeyCode crouchKey = KeyCode.C;
    public float crouchHeight = 1f;
    public float crouchCenterY = 0.5f;
    public float crouchSpeed = 5f;

    [Header("References")]
    public CapsuleCollider playerCollider;
    public Transform playerVisual; // The visual mesh or model of the player

    private float originalHeight;
    private float originalCenterY;
    private Vector3 originalScale;
    private bool isCrouching = false;

    void Start()
    {
        if (playerCollider != null)
        {
            originalHeight = playerCollider.height;
            originalCenterY = playerCollider.center.y;
        }

        if (playerVisual != null)
        {
            originalScale = playerVisual.localScale;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(crouchKey))
        {
            isCrouching = true;
        }
        else if (Input.GetKeyUp(crouchKey))
        {
            isCrouching = false;
        }

        HandleCrouch();
    }

    void HandleCrouch()
    {
        if (playerCollider != null)
        {
            float targetHeight = isCrouching ? crouchHeight : originalHeight;
            float targetCenterY = isCrouching ? crouchCenterY : originalCenterY;

            playerCollider.height = Mathf.Lerp(playerCollider.height, targetHeight, Time.deltaTime * crouchSpeed);
            Vector3 center = playerCollider.center;
            center.y = Mathf.Lerp(center.y, targetCenterY, Time.deltaTime * crouchSpeed);
            playerCollider.center = center;
        }

        if (playerVisual != null)
        {
            Vector3 targetScale = isCrouching ? new Vector3(originalScale.x, originalScale.y * 0.7f, originalScale.z) : originalScale;
            playerVisual.localScale = Vector3.Lerp(playerVisual.localScale, targetScale, Time.deltaTime * crouchSpeed);
        }
    }
}

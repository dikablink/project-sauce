using UnityEngine;

public class FootManager : MonoBehaviour
{
    //051325 i like the footbalancing u should add keybinds for the axis fukit n make it possible to get up by hand n learn pushups 
    [Header("Foot Transforms")]
    public Transform rightFoot;
    public Transform leftFoot;

    [Header("Player Body Reference")]
    public Transform playerBody;

    [Header("Movement Settings")]
    public float moveSpeed = 0.01f;
    public Vector3 movementLimit = new Vector3(0.5f, 0.2f, 0.5f);

    [Header("Control Keys")]
    public KeyCode toggleLegControlKey = KeyCode.L;
    public KeyCode recoveryKey = KeyCode.R;
    public KeyCode cameraLockKey = KeyCode.LeftShift;

    [Header("Balance Settings")]
    public float supportRadius = 0.5f;
    public float balanceTiltThreshold = 15f;
    public float recoverySpeed = 5f;

    [Header("Auto Recovery Settings")]
    public float autoRecoveryTiltThreshold = 10f;
    public float autoRecoveryGraceTime = 0.5f;

    [Header("Runtime State")]
    public bool legMovementEnabled = false;
    public bool rightFootMoving = false;
    public bool leftFootMoving = false;
    public bool isCameraLocked = false;
    public bool isRecovering = false;

    [Header("Debug Info")]
    public float feetDistanceFromCenter = 0f;
    public bool feetAreUnderPlayer = true;
    public bool isUnbalanced = false;
    public bool shouldFall = false;
    public bool autoRecoveryReady = false;

    [Header("Optional References")]
    public MonoBehaviour cameraLookScript;

    private Vector3 rightFootStartPos;
    private Vector3 leftFootStartPos;
    private Rigidbody bodyRb;

    private Collider rightFootCollider;
    private Collider leftFootCollider;

    private float autoRecoveryTimer = 0f;

    void Start()
    {
        rightFootStartPos = rightFoot.localPosition;
        leftFootStartPos = leftFoot.localPosition;

        rightFootCollider = rightFoot.GetComponent<Collider>();
        leftFootCollider = leftFoot.GetComponent<Collider>();

        bodyRb = playerBody.GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleInputs();
        CheckBalance();
        HandleRecovery();
        HandleAutoRecovery();

        if (!legMovementEnabled || isRecovering) return;

        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        float scrollDelta = Input.mouseScrollDelta.y;

        HandleFootMovement(mouseDelta, scrollDelta);
    }

    void HandleInputs()
    {
        if (Input.GetKeyDown(toggleLegControlKey))
        {
            legMovementEnabled = !legMovementEnabled;
            rightFootMoving = false;
            leftFootMoving = false;

            if (!legMovementEnabled)
                ResetFeetPosition();
        }

        if (Input.GetKeyDown(recoveryKey))
        {
            StartRecovery();
        }

        isCameraLocked = Input.GetKey(cameraLockKey);
        if (cameraLookScript != null)
            cameraLookScript.enabled = !isCameraLocked;
    }

    void HandleFootMovement(Vector2 mouseDelta, float scroll)
    {
        // Right Foot (Right Click)
        if (Input.GetMouseButton(1))
        {
            rightFootMoving = true;
            MoveFoot(rightFoot, rightFootStartPos, mouseDelta, scroll);
        }
        else rightFootMoving = false;

        // Left Foot (Left Click)
        if (Input.GetMouseButton(0))
        {
            leftFootMoving = true;
            MoveFoot(leftFoot, leftFootStartPos, mouseDelta, scroll);
        }
        else leftFootMoving = false;
    }

    void MoveFoot(Transform foot, Vector3 originPos, Vector2 delta, float scroll)
    {
        Vector3 offset = new Vector3(delta.x, scroll, delta.y) * moveSpeed;
        Vector3 newLocalPos = foot.localPosition + offset;

        newLocalPos.x = Mathf.Clamp(newLocalPos.x, originPos.x - movementLimit.x, originPos.x + movementLimit.x);
        newLocalPos.y = Mathf.Clamp(newLocalPos.y, originPos.y - movementLimit.y, originPos.y + movementLimit.y);
        newLocalPos.z = Mathf.Clamp(newLocalPos.z, originPos.z - movementLimit.z, originPos.z + movementLimit.z);

        foot.localPosition = newLocalPos;
    }

    void ResetFeetPosition()
    {
        rightFoot.localPosition = rightFootStartPos;
        leftFoot.localPosition = leftFootStartPos;
    }

    void CheckBalance()
    {
        Vector3 feetAvgPos = (rightFoot.position + leftFoot.position) / 2f;
        Vector3 bodyXZ = new Vector3(playerBody.position.x, 0, playerBody.position.z);
        Vector3 feetXZ = new Vector3(feetAvgPos.x, 0, feetAvgPos.z);

        feetDistanceFromCenter = Vector3.Distance(bodyXZ, feetXZ);
        feetAreUnderPlayer = feetDistanceFromCenter <= supportRadius;

        float bodyTiltX = GetSignedTilt(playerBody.localEulerAngles.x);
        float bodyTiltZ = GetSignedTilt(playerBody.localEulerAngles.z);

        isUnbalanced = !feetAreUnderPlayer || Mathf.Abs(bodyTiltX) > balanceTiltThreshold || Mathf.Abs(bodyTiltZ) > balanceTiltThreshold;

        shouldFall = isUnbalanced;

        if (shouldFall)
        {
            bodyRb.freezeRotation = false;
        }
    }

    void HandleRecovery()
{
    if (!isRecovering) return;

    Quaternion targetRotation = Quaternion.Euler(0f, playerBody.localEulerAngles.y, 0f);
    playerBody.localRotation = Quaternion.Slerp(playerBody.localRotation, targetRotation, Time.deltaTime * recoverySpeed);

    float xTilt = Mathf.Abs(GetSignedTilt(playerBody.localEulerAngles.x));
    float zTilt = Mathf.Abs(GetSignedTilt(playerBody.localEulerAngles.z));

    if (xTilt < 1f && zTilt < 1f)
    {
        bodyRb.freezeRotation = true;
        isRecovering = false;

        // Reset feet back to start positions after recovery
        ResetFeetPosition();
    }
}

    void HandleAutoRecovery()
    {
        if (isRecovering) return;

        float bodyTiltX = Mathf.Abs(GetSignedTilt(playerBody.localEulerAngles.x));
        float bodyTiltZ = Mathf.Abs(GetSignedTilt(playerBody.localEulerAngles.z));

        autoRecoveryReady = feetAreUnderPlayer && bodyTiltX < autoRecoveryTiltThreshold && bodyTiltZ < autoRecoveryTiltThreshold;

        if (autoRecoveryReady)
        {
            autoRecoveryTimer += Time.deltaTime;
            if (autoRecoveryTimer >= autoRecoveryGraceTime)
            {
                StartRecovery();
                autoRecoveryTimer = 0f;
            }
        }
        else
        {
            autoRecoveryTimer = 0f;
        }
    }

    void StartRecovery()
    {
        isRecovering = true;
    }

    float GetSignedTilt(float angle)
    {
        angle = (angle > 180) ? angle - 360 : angle;
        return angle;
    }

    public void ToggleFootCollision(bool enable)
    {
        if (rightFootCollider != null) rightFootCollider.enabled = enable;
        if (leftFootCollider != null) leftFootCollider.enabled = enable;
    }
}
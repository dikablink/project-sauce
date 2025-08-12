using UnityEngine;

public class SkateboardManager : MonoBehaviour
{
    [Header("Skateboard Settings")]
    public GameObject skateboardPrefab;
    public Transform rightFoot;
    public Transform leftFoot;
    public Transform playerBody;

    [Header("Float Controls")]
    public KeyCode floatUpKey = KeyCode.Space;
    public KeyCode floatDownKey = KeyCode.V;
    public float floatAmount = 0.5f; // amount to float up/down per press

    [Header("Debug / State")]
    public bool isFloatingUp = false;
    public bool isFloatingDown = false;

    private GameObject spawnedBoard;
    public bool isAttached = false;

    private Rigidbody playerRb;

    // Track current Y level of the board
    private float currentY;

    void Start()
    {
        playerRb = playerBody.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            SpawnBoard();

        if (Input.GetKeyDown(KeyCode.K))
            ToggleAttach();

        if (isAttached && spawnedBoard != null)
        {
            UpdateBoardFollow();
            HandleFloatInput();
        }
        else
        {
            isFloatingUp = false;
            isFloatingDown = false;
        }
    }

    void SpawnBoard()
    {
        if (spawnedBoard != null)
            Destroy(spawnedBoard);

        Vector3 spawnPos = ((rightFoot.position + leftFoot.position) / 2f) + Vector3.down * 0.1f;
        spawnedBoard = Instantiate(skateboardPrefab, spawnPos, Quaternion.identity);
        currentY = spawnedBoard.transform.position.y;
    }

   void ToggleAttach()
{
    if (spawnedBoard == null) return;

    Rigidbody boardRb = spawnedBoard.GetComponent<Rigidbody>();

    if (!isAttached)
    {
        boardRb.isKinematic = false;
        isAttached = true;

        // 🔁 Attach board to player
        spawnedBoard.transform.SetParent(playerBody); // <-- THIS
    }
    else
    {
        boardRb.isKinematic = true;
        isAttached = false;

        // 🔁 Detach board from player
        spawnedBoard.transform.SetParent(null); // <-- THIS
    }
}

    void UpdateBoardFollow()
    {
        Vector3 feetAvg = (rightFoot.position + leftFoot.position) / 2f;
        Vector3 targetPos = feetAvg;
        targetPos.y = currentY; // Keep current float level on Y

        spawnedBoard.transform.position = Vector3.Lerp(spawnedBoard.transform.position, targetPos, Time.deltaTime * 10f);

        Vector3 footDir = rightFoot.position - leftFoot.position;
        footDir.y = 0f;

        if (footDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(Vector3.Cross(footDir.normalized, Vector3.up));
            targetRot *= Quaternion.Euler(0, 90, 0);
            spawnedBoard.transform.rotation = Quaternion.Slerp(spawnedBoard.transform.rotation, targetRot, Time.deltaTime * 10f);
        }
    }
    void HandleFloatInput()
    {
        // Detect float up press (one time, not hold)
        if (Input.GetKeyDown(floatUpKey))
        {
            currentY += floatAmount;
            isFloatingUp = true;
        }
        else
        {
            isFloatingUp = false;
        }

        // Detect float down press (one time, not hold)
        if (Input.GetKeyDown(floatDownKey))
        {
            currentY -= floatAmount;
            isFloatingDown = true;
        }
        else
        {
            isFloatingDown = false;
        }
    }

    public bool IsBoardAttached()
    {
        return isAttached;
    }
}
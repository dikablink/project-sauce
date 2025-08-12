using UnityEngine;

public class SkateInputAAB : MonoBehaviour
{
    [Header("Skateboard Settings")]
    public GameObject skateboardPrefab;
    public Transform rightFoot;
    public Transform leftFoot;
    public Transform playerBody;

    private GameObject spawnedBoard;
    private bool isAttached = false;

    private Rigidbody playerRb;

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
            UpdateBoardFollow();
    }

    void SpawnBoard()
    {
        if (spawnedBoard != null)
            Destroy(spawnedBoard);

        Vector3 spawnPos = ((rightFoot.position + leftFoot.position) / 2f) + Vector3.down * 0.1f;
        spawnedBoard = Instantiate(skateboardPrefab, spawnPos, Quaternion.identity);
    }

    void ToggleAttach()
    {
        if (spawnedBoard == null) return;

        Rigidbody boardRb = spawnedBoard.GetComponent<Rigidbody>();

        if (!isAttached)
        {
            boardRb.isKinematic = false;
            isAttached = true;
        }
        else
        {
            boardRb.isKinematic = true;
            isAttached = false;
        }
    }

    void UpdateBoardFollow()
    {
        Vector3 feetAvg = (rightFoot.position + leftFoot.position) / 2f;
        Vector3 targetPos = feetAvg + Vector3.down * 0.1f;

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

    public bool IsBoardAttached()
    {
        return isAttached;
    }
}
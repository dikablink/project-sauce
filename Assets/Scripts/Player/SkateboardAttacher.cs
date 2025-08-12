using UnityEngine;

public class SkateboardAttacher : MonoBehaviour
{
    public Transform skateboard; // Assign in inspector
    public Transform rightFoot;  // Assign in inspector
    public FootBoardDetector rightFootDetector; // From previous step

    public Vector3 footOffset = new Vector3(0, -0.1f, 0); // Offset to align board under foot
    public bool isAttached = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (rightFootDetector.isOnBoard)
            {
                AttachBoard();
            }
        }
    }

    void AttachBoard()
    {
        // Parent skateboard to rightFoot
        skateboard.SetParent(rightFoot);

        // Align position with foot + offset
        skateboard.localPosition = footOffset;

        // Freeze board physics while attached
        Rigidbody rb = skateboard.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        isAttached = true;
    }
}
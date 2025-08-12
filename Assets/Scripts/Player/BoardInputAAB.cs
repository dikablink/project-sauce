using UnityEngine;

public class BoardInputAAB : MonoBehaviour
{
    public Rigidbody rb;
    public Transform backPopPoint;
    public Transform frontPopPoint;
    public float popForce = 6f;
    public float torqueForce = 10f;

    public void StompBack()
    {
        if (backPopPoint != null)
        {
            rb.AddForceAtPosition(Vector3.down * popForce, backPopPoint.position, ForceMode.Impulse);
            rb.AddTorque(Vector3.right * torqueForce, ForceMode.Impulse);
            Debug.Log("StompBack triggered");
        }
    }

    public void StompFront()
    {
        if (frontPopPoint != null)
        {
            rb.AddForceAtPosition(Vector3.down * popForce, frontPopPoint.position, ForceMode.Impulse);
            rb.AddTorque(Vector3.left * torqueForce, ForceMode.Impulse);
            Debug.Log("StompFront triggered");
        }
    }
}
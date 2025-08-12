using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SkateboardController : MonoBehaviour
{
    public Rigidbody rb;
    public bool playerOnBoard;//this works fine
    public bool allowControlOffBoard = true; // ✅ this works fine

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
}

using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class SkateTrickManager : MonoBehaviour
{
    //public int timer;
    [Header("References")]
    public Rigidbody boardRb;
    public Transform board;
    public Transform player;

    public LayerMask groundMask;
    public float groundCheckDisstance = 0.2f;

    [Header("Ollie Settings")]
    public float olliePopForce = 10f;
    public float ollieWindow = 0.3f;

    [Header("Tilt Settings")]
    public float tiltAngle = 30f;
    public float tiltSpeed = 5f;

    [Header("Flip Settings")]
    public float flickRotationSpeed = 500f;
    public float flickSensitivity = 0.5f;

    [Header("Debug/State")]
    public bool isGrounded;
    public bool isOllieRising;
    public bool isFlicking;
    public bool isFirstClick;
    public bool readyforSecond;
    public bool isSecondClick;
    public bool canFlick;
    public bool isFalling;
    public bool isRising;
    public Vector3 flickDirection;
    public float currentTilt;
    public bool didOllie;
    // Compatibility vars
    public float ollieMaxPlayerOffset = 1.5f;
    public float boardYAtOllie = 0f;

    [SerializeField] private bool leftClickDown = false;
    [SerializeField] private bool rightClickDown = false;
    [SerializeField] private float clickTimer = 0f;
    [SerializeField] private bool readyforLeft;
    [SerializeField] private bool readyforRight;
    [SerializeField] private bool readyToOllie = false;

    void Update()
    {
        DetectClicks();
        HandleFlickInput();
       
    }

    void FixedUpdate()
    {
   
        isRising = !isGrounded && boardRb.linearVelocity.y > 0.1f;
        isFalling = !isGrounded && boardRb.linearVelocity.y < -0.1f;
        canFlick = !isGrounded && (isRising || isFalling);
        if (leftClickDown ^ rightClickDown)
        {
            clickTimer++;
            if (!isFirstClick && leftClickDown)
            {
                isFirstClick = true;
                Debug.Log("FIRSTL!");
                if (clickTimer <= ollieWindow)
                {
                    readyforLeft = false;
                    readyforRight = true;
                    readyforSecond = true;
                }

            }
            else if (!isFirstClick && rightClickDown)
            {
                isFirstClick = true;
                Debug.Log("FIRSTR");
                if (clickTimer <= ollieWindow)
                {
                    readyforLeft = true;
                    readyforRight = false;
                    readyforSecond = true;
                }

            }
            else if (readyforSecond && (readyforRight && rightClickDown || readyforLeft && readyforLeft))
            {
                isSecondClick = true;
                Debug.Log("SECOND!");
            }
            else
            {
                isSecondClick = false;
            }
            if (clickTimer >= ollieWindow)
            {
                readyforSecond = false;
            }
        }
        if ((!Input.GetMouseButton(0) || !Input.GetMouseButton(1)) && Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            isFirstClick = false;
            isSecondClick = false;
            readyforRight = false;
            readyforLeft = false;
         clickTimer = 0f;
        }
        isGrounded = Physics.Raycast(board.position, Vector3.down, groundCheckDisstance, groundMask);
        if (readyToOllie)
        {
           //   clickTimer = 0f;
            PerformOllie();
            readyToOllie = false;
        }


        HandleConstraints();
        ApplyTilt();

        // Compatibility update
        ollieMaxPlayerOffset = player.position.y - board.position.y;
    }

    void DetectClicks()
    {
        if (Input.GetMouseButtonDown(0)) leftClickDown = true;
        if (Input.GetMouseButtonDown(1)) rightClickDown = true;

        if (Input.GetMouseButtonUp(0)) leftClickDown = false;
        if (Input.GetMouseButtonUp(1)) rightClickDown = false;

        if (leftClickDown && rightClickDown)
        {
            //clickTimer += Time.deltaTime;

            if ((clickTimer <= ollieWindow) && isSecondClick)
            {
                readyToOllie = true;
                boardYAtOllie = board.position.y;
                clickTimer = 0f;
                Debug.Log("ollieready");
            }
        }
        else
        {
           // clickTimer = 0f;
        }
    }

    void PerformOllie()
    {
        // 🧊 Freeze first
        boardRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // 🧼 Clear tilt
        board.rotation = Quaternion.Euler(0f, board.rotation.eulerAngles.y, board.rotation.eulerAngles.z);

        // 🪶 Apply pop
        boardRb.linearDamping = 0.5f;
        boardRb.angularDamping = 1f;
        boardRb.AddForce(Vector3.up * olliePopForce, ForceMode.Impulse);
        Debug.Log("doingollie");
        isOllieRising = true;
        Invoke("ResetOllie", 0.3f);
    }

    void ResetOllie()
    {
        isOllieRising = false;
    }

    void HandleFlickInput()
    {
        if ( (Input.GetMouseButton(0) || Input.GetMouseButton(1)))//!isGrounded &&)
        {
            float flick = Input.GetAxis("Mouse X");

            //  if (Mathf.Abs(flick) > flickSensitivity)
            {
                isFlicking = true;
                flickDirection = new Vector3(0f, 0f, -flick);
                boardRb.AddTorque(flickDirection * flickRotationSpeed);
                boardRb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX;
            }
        }
        else
        {
             boardRb.constraints = RigidbodyConstraints.None;
            isFlicking = false;
        }
    }

    void ApplyTilt()
    {
        float targetAngle = 0f;
        if (Input.GetMouseButton(0))
            targetAngle = tiltAngle;
        if (Input.GetMouseButton(1))
            targetAngle = -tiltAngle;
        currentTilt = targetAngle;
        if (isGrounded && (Input.GetMouseButton(0) ||Input.GetMouseButton(1)))
        {
            boardRb.constraints = RigidbodyConstraints.FreezeRotationX; // Z unlocked for flip
        }
        else
        {
            boardRb.constraints = RigidbodyConstraints.None;
         }
        Quaternion targetRotation = Quaternion.Euler(targetAngle, board.rotation.eulerAngles.y, board.rotation.eulerAngles.z);
        boardRb.MoveRotation(Quaternion.Slerp(board.rotation, targetRotation, Time.fixedDeltaTime * tiltSpeed));
    }

    void HandleConstraints()
    {
        // if ((rightClickDown ^ leftClickDown) !)
       if(isFirstClick)
            {
            //boardRb.constraints = RigidbodyConstraints.None;
            ApplyTilt();
            }
            
            // else if (!isGrounded )
            // {
            //    //  boardRb.constraints = RigidbodyConstraints.None;
            //     //boardRb.constraints = RigidbodyConstraints.FreezeRotationX;
            // }
        else if (isOllieRising)
        {
           // boardRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
        // else if ((!isGrounded))// && isFlicking) this onlu happens when u hold n dont let go
        // {
        //    //``` boardRb.constraints = RigidbodyConstraints.None;
        //     //  boardRb.constraints = RigidbodyConstraints.FreezeRotationX; // Z unlocked for flip
        // }
        else if (isFalling)
        {
            boardRb.constraints = RigidbodyConstraints.None;
        }
        else
        {
            boardRb.constraints = RigidbodyConstraints.None;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        //isGrounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        //isGrounded = false;
    }
}
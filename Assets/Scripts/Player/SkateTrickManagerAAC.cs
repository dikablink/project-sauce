using UnityEngine;

public class SkateTrickManagerAAC : MonoBehaviour
{
[Header("References")]
public Transform player;
public Transform board;
public Rigidbody playerRb;
public SkateboardController skateboardController;

[Header("Raycast Settings")]
public float rayDistance = 1.5f;
public LayerMask boardLayer;
public float groundRayLength = 0.3f;
public LayerMask groundLayer;

[Header("Tilt & Pop Settings")]
public float tiltTorque = 10f;
public float olliePopForce = 6f;
public float maxYSpeed = 4.5f;
public float levelReturnSpeed = 5f;
public bool lockRotationAfterOllie = true;
[SerializeField] private float airDrag = 2f;
[SerializeField] private float airAngularDrag = 2f;
[SerializeField] private float groundDrag = 0f;
[SerializeField] private float groundAngularDrag = 0.05f;

[Header("Ollie Height/Flick Control")]
public float ollieHeightCap = 1.5f;
public float ollieMaxPlayerOffset = 1.0f;
public float playerYOffset = 1.0f;
public float minFlickDistance = 50f;
public float flipTorque = 250f;
    public float ollieWindow = 0.25f;
public float flickHoldThreshold = 0.15f; // How long to hold before flick is valid

    [Header("Debug Info")]
public bool leftClicked;
public bool rightClicked;
public bool didOllie;
public float boardYAtOllie;
public bool levelBoard;
public bool isBoardGrounded;
public bool isDragging;
public bool isFlick;
public bool flickPossible; // Becomes true only while one button is held
public bool flickStarted; // Flick condition met (dragging past threshold)
public bool flickReleased; // Flick completed
[SerializeField] private bool myondaboard;
[SerializeField] private bool myallowboard;
[SerializeField] private float initialOllieY;
[SerializeField] private float currentBoardY;
[SerializeField] private bool isRising;
[SerializeField] private bool isFalling = false;
 [SerializeField]private Vector2 flickStartScreenPos;
 [SerializeField]private float holdStartTime;
 [SerializeField]private bool buttonDown;
public float leftFootDistance;
public float rightFootDistance;
public float clickInterval;

[SerializeField]private Vector2 flickStart;
[SerializeField]private Vector2 flickEnd;
[SerializeField] private float firstClickTime;
[SerializeField] private float secondClickTime;
[SerializeField] private int firstClickButton = -1;
[SerializeField] private bool waitingForSecondClick = false;
    [SerializeField] private bool ollieWindowOn;
[SerializeField] private bool ollieReady = false;

private Rigidbody boardRb;
[SerializeField] private float ollieStartY;
[SerializeField] private bool trackingOllieHeight = false;

void Start()
{
if (board != null)
boardRb = board.GetComponent<Rigidbody>();

if (skateboardController != null)
myallowboard = skateboardController.allowControlOffBoard;

if (playerRb == null)
playerRb = player.GetComponent<Rigidbody>();
}

    void Update()
    {
        if (skateboardController != null)
            myondaboard = skateboardController.playerOnBoard;

        Vector3 leftOrigin = player.position + player.right * -0.3f;
        Vector3 rightOrigin = player.position + player.right * 0.3f;

        myondaboard = false;

        if (Physics.Raycast(leftOrigin, Vector3.down, out RaycastHit leftHit, rayDistance, boardLayer))
        {
            leftFootDistance = leftHit.distance;
            myondaboard = true;
        }

        if (Physics.Raycast(rightOrigin, Vector3.down, out RaycastHit rightHit, rayDistance, boardLayer))
        {
            rightFootDistance = rightHit.distance;
            myondaboard = true;
        }

        if (Input.GetMouseButtonDown(0) && !isFalling)
        {
            HandleClick(0);
            flickStart = Input.mousePosition;
            boardRb.AddTorque(board.right * tiltTorque, ForceMode.Impulse);
            buttonDown = true;
            Debug.Log("LEFT STOMP,"+firstClickTime);
        }

        if (Input.GetMouseButtonDown(1) && !isFalling)
        {
            HandleClick(1);
            buttonDown = true;
            flickStart = Input.mousePosition;
            boardRb.AddTorque(-board.right * tiltTorque, ForceMode.Impulse);
            Debug.Log("RIGHT STOMP, "+firstClickTime);
        }
        //if((isRising || isFalling) && kn

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            //  buttonDown = true;
            // flickPossible = true;
            //  flickStarted = false;
            //  flickReleased = false;
           
          
        }
        if (isRising || isFalling)
        {
            buttonDown = true;
        }
        else
        {
            buttonDown = false;
        }
        if (buttonDown && Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
        //    flickPossible = true;
           // Debug.Log("BOTH LEGS STOMPED/SMASHGED"+firstClickTime);
         }
        if ((buttonDown && clickInterval <= ollieWindow))
        {
            ollieWindowOn = true;
         }
         else
        {
            ollieWindowOn = false;
         }
        if (buttonDown && clickInterval <= ollieWindow)
        {
            flickPossible = true;
            holdStartTime = Time.time;
            flickStartScreenPos = Input.mousePosition;
        }
        else
        {
            flickPossible = false;
        }

        
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))

        {
            flickPossible = false;
            buttonDown = false;
            //Debug.Log("INPUT RESTED/LIFTED"+firstClickTime);
            //  Debug.Log("✅ Flick detected.");
            flickEnd = Input.mousePosition;
            float flickDistance = Vector2.Distance(flickStart, flickEnd);
            isFlick = clickInterval > ollieWindow && flickDistance > minFlickDistance;
        }


 
 //      buttonDown = false;
//flickPossible = false;
//flickStarted = false;
    }

    void FixedUpdate()
    {
        CheckBoardGrounded();
        float currentYVelocity = boardRb.linearVelocity.y;
        currentBoardY = board.position.y;

        if (didOllie)
        {
            float ollieHeight = currentBoardY - initialOllieY;

            if (ollieHeight >= ollieHeightCap)
            {
                Vector3 vel = boardRb.linearVelocity;
                vel.y = 0f;
                boardRb.linearVelocity = vel;

                isRising = false;
                boardRb.constraints = RigidbodyConstraints.None;
            }

            didOllie = false;
        }

        if (currentYVelocity > 0.01f)
        {
            isRising = true;
            isFalling = false;
        }
        else if (currentYVelocity <= 0.01f && isRising)
        {
            isRising = false;
            isFalling = true;
            boardRb.constraints = RigidbodyConstraints.None;
        }

        if (playerRb.linearVelocity.y > maxYSpeed + playerYOffset)
        {
            Vector3 v = playerRb.linearVelocity;
            v.y = maxYSpeed + playerYOffset;
            playerRb.linearVelocity = v;
        }

        if (ollieReady && clickInterval <= ollieWindow)
        {
            didOllie = true;
            levelBoard = true;
            ollieReady = false;
            ollieWindowOn = false;
            initialOllieY = board.position.y;
            Debug.Log("OLLIE CODE LOADED"+firstClickTime);
            Movethefuckingboard();
            Draghisbitchass();
         //   board.rotation = Quaternion.Euler(0f, board.rotation.eulerAngles.y, board.rotation.eulerAngles.z);
         //   boardRb.AddForce(Vector3.up * olliePopForce, ForceMode.Impulse);
          //  boardRb.linearDamping = airDrag;
          //  boardRb.angularDamping = airAngularDrag;
            //boardRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        if (didOllie && boardRb.linearVelocity.y <= 0 && !isFalling)
        {
            isFalling = true;
            boardRb.constraints = RigidbodyConstraints.None;
           // boardRb.constraints = RigidbodyConstraints.FreezeRotationZ;
            boardRb.linearDamping = groundDrag;
            boardRb.angularDamping = groundAngularDrag;
        }

        if (levelBoard && !lockRotationAfterOllie)
        {
            Quaternion currentRot = board.rotation;
            Quaternion targetRot = Quaternion.Euler(0f, currentRot.eulerAngles.y, currentRot.eulerAngles.z);
            board.rotation = Quaternion.Slerp(currentRot, targetRot, Time.fixedDeltaTime * levelReturnSpeed);
            Debug.Log("this works wow never new");
        }
       // isFalling = false;
}
    void Movethefuckingboard()
    {

        board.rotation = Quaternion.Euler(0f, board.rotation.eulerAngles.y, board.rotation.eulerAngles.z);
        Debug.Log("RESETX");
        boardRb.AddForce(Vector3.up * olliePopForce, ForceMode.Impulse);
        Debug.Log("movenigga");
           
    }
    void Draghisbitchass()
    {
        Debug.Log("drag");
         boardRb.linearDamping = airDrag;
            boardRb.angularDamping = airAngularDrag;
        Debug.Log("freeze!");
            boardRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }
void HandleClick(int button)
{
if (!waitingForSecondClick)
{
waitingForSecondClick = true;
firstClickButton = button;
firstClickTime = Time.time;
leftClicked = (button == 0);
rightClicked = (button == 1);
}
else if (waitingForSecondClick && button != firstClickButton)
{
secondClickTime = Time.time;
clickInterval = secondClickTime - firstClickTime;

if (clickInterval <= ollieWindow)// && myondaboard)
{
ollieReady = true;
}

waitingForSecondClick = false;
}
else
{
waitingForSecondClick = false;
}
}

    void CheckBoardGrounded()
    {
        
        isBoardGrounded = Physics.Raycast(board.position, Vector3.down, groundRayLength, groundLayer);
        if (isBoardGrounded)
        { //  boardRb.linearDamping = airDrag;
          //  boardRb.angularDamping = airAngularDrag;
            isFalling = false;
        }
    }

void ResetState()
{
didOllie = false;
levelBoard = false;
leftClicked = false;
rightClicked = false;
waitingForSecondClick = false;
firstClickButton = -1;
}
}
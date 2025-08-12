using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [SerializeField] private VehicleSO vehicleTemplate;
    [SerializeField] private float gasPedalSensitivity = 0.062f;
    [SerializeField] private float brakePedalSensitivity = 0.023f;
    [SerializeField] private float steeringWheelSensitivity = 0.049f;
    public float engineSpeed;
    public float gasPedal;
    public float brakePedal;
    public float steeringWheel;

    void Start()
    {
        
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        // Calculate Gas Pedal Input
        if (Input.GetKey(KeyCode.Space)) gasPedal += gasPedalSensitivity;
        else gasPedal -= gasPedalSensitivity;
        gasPedal = GasPedal;

        // Calculate Brake Pedal Input
        if (Input.GetKey(KeyCode.LeftAlt)) brakePedal += brakePedalSensitivity;
        else brakePedal -= brakePedalSensitivity;
        brakePedal = BrakePedal;

        // Calculate Steering Wheel Input
        if (steeringWheel < steeringWheelSensitivity && steeringWheel > steeringWheelSensitivity * -1 && ((Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A)) | (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)))) steeringWheel = 0;
        else if ((Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) | (((Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A)) | (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))) && steeringWheel > 0)) steeringWheel -= steeringWheelSensitivity;
        else if ((Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)) | (((Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A)) | (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))) && steeringWheel < 0)) steeringWheel += steeringWheelSensitivity;
        steeringWheel = SteeringWheel;
    }

    public float GasPedal
    {
        get
        {
            if (gasPedal > 1f) return 1f;
            else if (gasPedal < 0f) return 0f;
            else return gasPedal;
        }
    }

    public float BrakePedal
    {
        get
        {
            if (brakePedal > 1f) return 1f;
            else if (brakePedal < 0f) return 0f;
            else return brakePedal;
        }
    }

    public float SteeringWheel
    {
        get
        {
            if (steeringWheel > 1f) return 1f;
            else if (steeringWheel < -1f) return -1f;
            else return steeringWheel;
        }
    }
}

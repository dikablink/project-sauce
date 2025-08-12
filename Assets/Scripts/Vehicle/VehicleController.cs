using System.Collections.Generic;
using Unity.XR.Oculus.Input;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    private float turning;
    private float accel;
    private float brake;
    private float maxTurnRadius;
    private float topSpeed;
    private float tireGrip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vehicle vehicle = new Vehicle();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

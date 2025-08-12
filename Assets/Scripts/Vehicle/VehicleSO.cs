using UnityEngine;

[CreateAssetMenu(fileName = "New Vehicle", menuName = "Vehicles/Vehicle")]
public class VehicleSO : ScriptableObject
{
    [SerializeField] private float topSpeed = 200f;
    [SerializeField] private float vehicleWeight = 1200f;
    [SerializeField] private bool isManual = true;
    [SerializeField] private TransGearSO[] transmissionGears = new TransGearSO[3];
    [SerializeField] private float tireGrip = 1f;
    [SerializeField] private float turnRadius = 10.5f;

    public float GetTopSpeed() {return topSpeed;}
    public float GetVehicleWeight() {return vehicleWeight;}
    public bool GetIsManual() {return isManual;}
    public TransGearSO[] GetTransmissionGears() {return transmissionGears;}
    public float GetTireGrip() {return tireGrip;}
    public float GetTurnRadius() {return turnRadius;}
}

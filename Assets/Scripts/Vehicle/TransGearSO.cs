using UnityEngine;

[CreateAssetMenu(fileName = "New Gear", menuName = "Vehicles/Transmission Gear")]
public class TransGearSO : ScriptableObject
{
    [SerializeField] private char gearSymbol = 'N';
    [SerializeField] private float gearRatio = 0f;

    public char GetGearSymbol() {return gearSymbol;}
    public float GetGearRatio() {return gearRatio;}
}

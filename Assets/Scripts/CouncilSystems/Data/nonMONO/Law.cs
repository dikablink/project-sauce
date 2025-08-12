using UnityEngine;
[System.Serializable]
public class Law
{
    public string lawName;
    public string description;
    public bool isActive;
    public float enforcementTime; // For curfews: like 22.00f (10PM)
}
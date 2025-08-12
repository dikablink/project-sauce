using UnityEngine;  // <-- this is missing
using System;

[CreateAssetMenu(fileName = "New Law", menuName = "Council/Law Proposal")]
public class LawProposal : ScriptableObject
{
    public string lawId;
    public string lawName;
    public string description;
    public float enforcementHour;
    public bool isActive;
    public bool enforced;

    public int yesVotes;
    public int noVotes;

    public LawProposal(string name, string desc, float hour)
    {
        lawId = Guid.NewGuid().ToString();
        lawName = name;
        description = desc;
        enforcementHour = hour;
        isActive = false;
        enforced = false;
        yesVotes = 0;
        noVotes = 0;
    }
}
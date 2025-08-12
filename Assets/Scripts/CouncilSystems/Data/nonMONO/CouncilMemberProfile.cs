using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CouncilMemberProfile
{
    public string id;
    public string displayName;
    public string personality;
    public bool isPlayerControlled;

    public List<LawProposal> proposedLaws = new();
    public Dictionary<string, bool> votingHistory = new(); // LawID => Yes/No
    public int influenceScore = 0;
}
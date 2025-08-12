using UnityEngine;
using System.Collections.Generic;
public class CouncilManager : MonoBehaviour
{
    public TimeManager timeManager;
    public List<CouncilMemberProfile> memberProfiles = new();
    public List<LawProposal> lawBook = new();

    void Awake()
    {
        if (timeManager == null)
            timeManager = FindObjectOfType<TimeManager>();

        Debug.Log("📘 CouncilManager Awake");

        foreach (var law in lawBook)
        {
            Debug.Log($"📋 Law Loaded: {law.lawName} | Active: {law.isActive} | Time: {law.enforcementHour}");
        }
    }

    void Start()
    {
        CouncilMemberA1[] members = FindObjectsOfType<CouncilMemberA1>();

        foreach (var npc in members)
        {
            CouncilMemberProfile profile = new CouncilMemberProfile()
            {
                id = System.Guid.NewGuid().ToString(),
                displayName = npc.gameObject.name,
                personality = "Random for now",
                isPlayerControlled = false
            };

            npc.AssignProfile(profile);
        }

        Debug.Log("✅ All council members assigned profiles.");
    }

    void Update()
    {
        if (IsLawActive("Curfew"))
        {
            float currentHour = timeManager.useRealTime
                ? timeManager.hour + (timeManager.minute / 60f)
                : timeManager.customHour + (timeManager.customMinute / 60f);

            float curfewHour = GetLawTime("Curfew");

            if (currentHour >= curfewHour || currentHour < 6f)
            {
                Debug.Log($"🕒 Curfew is active (Hour: {currentHour:0.0}). Citizens should be home.");
            }
        }
    }

    public void StartCouncilMeeting()
    {
        Debug.Log("🧠 Council meeting started.");
    }

    public void RegisterVote(string lawId, string memberId, bool voteYes)
    {
        var member = memberProfiles.Find(m => m.id == memberId);
        if (member == null) return;

        member.votingHistory[lawId] = voteYes;

        var law = lawBook.Find(l => l.lawId == lawId);
        if (law != null)
        {
            if (voteYes) law.yesVotes++;
            else law.noVotes++;
        }
    }

    public void EnforceLaw(string lawId)
    {
        var law = lawBook.Find(l => l.lawId == lawId);
        if (law != null) law.enforced = true;
    }

    public bool IsLawActive(string lawName)
    {
        return lawBook.Exists(l => l.lawName == lawName && l.isActive);
    }

    public float GetLawTime(string lawName)
    {
        var law = lawBook.Find(l => l.lawName == lawName);
        return law != null ? law.enforcementHour : -1f;
    }

    public List<LawProposal> GetAllActiveLaws()
    {
        return lawBook.FindAll(l => l.isActive);
    }
}
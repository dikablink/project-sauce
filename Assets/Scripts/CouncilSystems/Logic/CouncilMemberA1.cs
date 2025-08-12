using UnityEngine;

public class CouncilMemberA1 : MonoBehaviour
{
    public string memberId; // must match the profile.id
    public CouncilMemberProfile profile;

    public void AssignProfile(CouncilMemberProfile loadedProfile)
    {
        profile = loadedProfile;
        memberId = profile.id;
    }

    public void Speak(string context)
    {
        // TODO: Use profile.personality and beliefs to generate dialogue
    }

    public void DebugProfile()
    {
        Debug.Log($"👤 {profile.displayName} ({profile.id}) - Personality: {profile.personality}");
    }
    public bool DecideVote(LawProposal law)
    {
        // TODO: Use LLM or simple rules
        return Random.value > 0.5f;
    }
}
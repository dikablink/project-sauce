using UnityEngine;

public class NPCState : MonoBehaviour
{
    public string id;
    public Vector3 position;
    public string currentActivity;
    public float nextDecisionTime;

    public NPCState(string id, Vector3 position, string activity, float decisionTime)
    {
        this.id = id;
        this.position = position;
        this.currentActivity = activity;
        this.nextDecisionTime = decisionTime;
    }
}
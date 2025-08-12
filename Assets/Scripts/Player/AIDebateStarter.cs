using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using TMPro;

public class AIDebateStarter : MonoBehaviour
{
    [System.Serializable]
public class DebatePayload
{
        public bool near_ai;
    public bool near_amy;
    public bool near_ron;
}
    [System.Serializable]
    public class TalkPayload
    {
        public string agent;
        public string input;
        public string emotion;
    }

    [System.Serializable]
    public class NicknamePayload
    {
        public string agent;
        public string nickname;
    }

    public string agentName = "Amy"; // or "Ron"
    public TextMeshPro overheadText;

    public void TalkToAI(string input)
    {
        StartCoroutine(SendToAI(input));
    }

    public void RequestDebateTurn()
    {
        StartCoroutine(SendDebateTurn());
    }

    public void SetNickname(string nick)
    {
        StartCoroutine(SendNickname(nick));
    }

    public void ShowOverheadText(string msg)
    {
        if (overheadText != null)
            overheadText.text = msg;
    }

    IEnumerator SendToAI(string input)
    {
        var payload = new TalkPayload {
            agent   = agentName,
            input   = input,
            emotion = "neutral"
        };
        string json = JsonUtility.ToJson(payload);
        Debug.Log($"📤 TALK JSON: {json}");

        using var req = new UnityWebRequest("http://127.0.0.1:5000/talk","POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        req.uploadHandler   = new UploadHandlerRaw(bodyRaw);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type","application/json");

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
            Debug.LogError($"❌ TALK error: {req.error}");
        else
            Debug.Log($"✅ TALK resp: {req.downloadHandler.text}");
    }

    IEnumerator SendDebateTurn()
{
    var payload = new DebatePayload {
        near_amy = DialogueManager.Instance.PlayerNearAmy,
        near_ron = DialogueManager.Instance.PlayerNearRon
    };
    string json = JsonUtility.ToJson(payload);
    Debug.Log($"📤 DEBATE JSON: {json}");

    using var req = new UnityWebRequest("http://127.0.0.1:5000/debate","POST");
    byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
    req.uploadHandler   = new UploadHandlerRaw(bodyRaw);
    req.downloadHandler = new DownloadHandlerBuffer();
    req.SetRequestHeader("Content-Type","application/json");

    yield return req.SendWebRequest();

    if (req.result != UnityWebRequest.Result.Success)
        Debug.LogError($"❌ DEBATE error: {req.error}");
    else
        Debug.Log($"🗣 DEBATE resp: {req.downloadHandler.text}");
}

    IEnumerator SendNickname(string nick)
    {
        var payload = new NicknamePayload {
            agent    = agentName,
            nickname = nick
        };
        string json = JsonUtility.ToJson(payload);
        Debug.Log($"📤 NICK JSON: {json}");

        using var req = new UnityWebRequest("http://127.0.0.1:5000/nickname","POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        req.uploadHandler   = new UploadHandlerRaw(bodyRaw);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type","application/json");

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
            Debug.LogError($"❌ NICK error: {req.error}");
        else
            Debug.Log($"✅ NICK ack: {req.downloadHandler.text}");
    }
}
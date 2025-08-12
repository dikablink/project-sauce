using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("UI")]
    public InputField inputField;

    [Header("NPC References")]
    // If left unassigned in the inspector, we'll try to find by name in Awake
    public Transform amy;
    public Transform ron;

    [Header("Proximity Settings")]
    [Tooltip("Distance under which Amy and Ron count as 'near'.")]
    public float npcDebateRadius = 5f;

    [Header("Runtime Flags (read-only)")]
    [SerializeField] bool _playerNearAmy = false;
    [SerializeField] bool _playerNearRon = false;
    [SerializeField] bool _playerNearAny = false;
    [SerializeField] bool _amyNearRon = false;
    [SerializeField] bool _ronNearAmy = false;

    public bool PlayerNearAmy  { get => _playerNearAmy; internal set => _playerNearAmy = value; }
    public bool PlayerNearRon  { get => _playerNearRon; internal set => _playerNearRon = value; }
    public bool PlayerNearAny  { get => _playerNearAny; private set => _playerNearAny = value; }
    public bool AmyNearRon     { get => _amyNearRon; private set => _amyNearRon = value; }
    public bool RonNearAmy     { get => _ronNearAmy; private set => _ronNearAmy = value; }

    [Header("Current AI")]  
    [SerializeField] AIDebateStarter _currentAI;
    public AIDebateStarter CurrentAI => _currentAI;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // auto-assign NPC references if null
        if (amy == null)
        {
            var goAmy = GameObject.Find("Amy");
            if (goAmy) amy = goAmy.transform;
            else Debug.LogWarning("⚠️ DialogueManager: 'amy' not assigned and GameObject 'Amy' not found.");
        }
        if (ron == null)
        {
            var goRon = GameObject.Find("Ron");
            if (goRon) ron = goRon.transform;
            else Debug.LogWarning("⚠️ DialogueManager: 'ron' not assigned and GameObject 'Ron' not found.");
        }
    }

    void Start()
    {
        if (inputField != null)
            inputField.onEndEdit.AddListener(HandleInput);
    }

    void Update()
    {
        // guard against missing assignments
        if (amy == null || ron == null)
            return;

        // NPC–NPC proximity each frame
        float dist = Vector3.Distance(amy.position, ron.position);
        bool near = dist <= npcDebateRadius;
        if (AmyNearRon != near)
        {
            AmyNearRon = near;
            Debug.Log($"🔍 AmyNearRon: {near} (dist {dist:F1})");
        }
        if (RonNearAmy != near)
        {
            RonNearAmy = near;
            Debug.Log($"🔍 RonNearAmy: {near} (dist {dist:F1})");
        }

        // Manual override keys
        if (Input.GetKeyDown(KeyCode.Alpha1) && amy != null)
            SetCurrentAI(amy.GetComponent<AIDebateStarter>());
        if (Input.GetKeyDown(KeyCode.Alpha2) && ron != null)
            SetCurrentAI(ron.GetComponent<AIDebateStarter>());
    }

    void HandleInput(string input)
    {
        if (CurrentAI == null || string.IsNullOrWhiteSpace(input)) return;

        Debug.Log("👤 Player: " + input);
        Debug.Log($"📢 Sending to: {CurrentAI.agentName}");

        if (input.ToLower().Contains("debate"))
        {
            if (PlayerNearAmy && PlayerNearRon && AmyNearRon && RonNearAmy)
                CurrentAI.RequestDebateTurn();
            else
                CurrentAI.ShowOverheadText("You, Amy, and Ron all need to be close together to debate.");
        }
        else if (input.ToLower().StartsWith("nickname ") || input.ToLower().StartsWith("call me "))
        {
            var parts = input.Split(' ');
            CurrentAI.SetNickname(parts[^1]);
        }
        else
        {
            CurrentAI.TalkToAI(input);
        }

        inputField.text = string.Empty;
        inputField.ActivateInputField();
    }

    public void SetCurrentAI(AIDebateStarter ai)
    {
        _currentAI = ai;
        Debug.Log($"🟢 DialogueManager set AI: {ai.agentName}");
    }

    public void ClearCurrentAI()
    {
        if (_currentAI != null)
        {
            Debug.Log($"🔴 Cleared AI: {_currentAI.agentName}");
            _currentAI = null;
        }
    }

    public void ShowMessageToPlayer(string msg)
    {
        CurrentAI?.ShowOverheadText(msg);
    }

    public void SetPlayerNearAmy(bool near)
    {
        if (PlayerNearAmy != near)
        {
            PlayerNearAmy = near;
            Debug.Log($"🔍 PlayerNearAmy: {near}");
        }
        PlayerNearAny = PlayerNearAmy || PlayerNearRon;
    }

    public void SetPlayerNearRon(bool near)
    {
        if (PlayerNearRon != near)
        {
            PlayerNearRon = near;
            Debug.Log($"🔍 PlayerNearRon: {near}");
        }
        PlayerNearAny = PlayerNearAmy || PlayerNearRon;
    }
}

using UnityEngine;

public class BehaviorWiring : MonoBehaviour
{
    public Transform homePoint;
    private LawManager lawManager;
    private bool goingHome = false;

    [Header("Movement")]
    public float speed = 2f;

    void Start()
    {
        lawManager = FindObjectOfType<LawManager>();
    }

    void Update()
    {
        // Test: press H to force go home
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log($"{name} manually told to go home (via key press).");
            goingHome = true;
        }

        // Curfew check
        if (lawManager != null && lawManager.IsLawActive("Curfew"))
        {
            float currentHour = lawManager.timeManager.useRealTime
                ? lawManager.timeManager.hour + (lawManager.timeManager.minute / 60f)
                : lawManager.timeManager.customHour + (lawManager.timeManager.customMinute / 60f);

            float curfewTime = lawManager.GetLawTime("Curfew");

            if ((currentHour >= curfewTime || currentHour < lawManager.curfewEndHour) && !goingHome)
            {
                Debug.Log($"{name} says: Uh oh, curfew hit at {currentHour:0.00}! I'm going home.");
                goingHome = true;
            }
        }

        // Move toward home if goingHome is active
        if (goingHome)
        {
            GoHome();
        }
    }

    public void GoHome()
    {
        if (homePoint == null)
        {
            Debug.LogWarning($"{name} has no homePoint set!");
            return;
        }

        Vector3 direction = (homePoint.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        transform.LookAt(homePoint.position);

        if (Vector3.Distance(transform.position, homePoint.position) < 0.5f)
        {
            Debug.Log($"{name} arrived home.");
            goingHome = false;
        }
    }

    public bool IsGoingHome()
    {
        return goingHome;
    }
}
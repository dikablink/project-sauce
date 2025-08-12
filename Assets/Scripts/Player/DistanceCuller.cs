using UnityEngine;
using System.Collections.Generic;

public class DistanceCuller : MonoBehaviour
{
    public string cullTag = "Cullable";
    public float cullDistance = 10f;

    Transform player;
    List<GameObject> cullableObjects = new List<GameObject>();

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (!playerObj)
        {
            Debug.LogError("⚠️ Player tagged 'Player' not found.");
            return;
        }

        player = playerObj.transform;

        GameObject[] objs = GameObject.FindGameObjectsWithTag(cullTag);
        foreach (GameObject obj in objs)
            cullableObjects.Add(obj);

        Debug.Log($"📍 [DistanceCuller] Found {cullableObjects.Count} '{cullTag}' objects");
    }

    void Update()
    {
        if (!player) return;

        foreach (var obj in cullableObjects)
        {
            if (!obj) continue;

            float distance = Vector3.Distance(player.position, obj.transform.position);
            bool shouldBeVisible = distance <= cullDistance;

            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer)
            {
                if (renderer.enabled != shouldBeVisible)
                {
                    renderer.enabled = shouldBeVisible;
                    Debug.Log($"📍 {(shouldBeVisible ? "Enabled" : "Disabled")} renderer on {obj.name} @ {distance:F1} units");
                }
            }
            else
            {
                Debug.LogWarning($"⚠️ {obj.name} has no Renderer component!");
            }
        }
    }
}
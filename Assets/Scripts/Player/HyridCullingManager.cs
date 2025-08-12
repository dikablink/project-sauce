using UnityEngine;
using System.Collections.Generic;
public class HyridCullingManager : MonoBehaviour
{
    public Transform playerCamera;
    public float maxDistance = 50f; // Cull objects beyond this distance
    public LayerMask cullableLayer;

    private List<Renderer> cullableObjects = new List<Renderer>();

    void Start()
    {
        var objects = GameObject.FindGameObjectsWithTag("Cullable");
        foreach (var obj in objects)
        {
            Renderer rend = obj.GetComponent<Renderer>();
            if (rend != null)
                cullableObjects.Add(rend);
        }
        Debug.Log($"[HybridCulling] Found {cullableObjects.Count} cullable objects.");
    }

    void Update()
    {
        Plane[] cameraPlanes = GeometryUtility.CalculateFrustumPlanes(playerCamera.GetComponent<Camera>());

        foreach (var rend in cullableObjects)
        {
            if (rend == null) continue;

            float dist = Vector3.Distance(playerCamera.position, rend.transform.position);
            bool withinDistance = dist <= maxDistance;

            bool withinView = GeometryUtility.TestPlanesAABB(cameraPlanes, rend.bounds);

            bool shouldRender = withinDistance && withinView;

            if (rend.enabled != shouldRender)
                rend.enabled = shouldRender;
        }
    }
}
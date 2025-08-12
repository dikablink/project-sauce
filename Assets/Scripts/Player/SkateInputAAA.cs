using UnityEngine;

public class SkateInputAAA : MonoBehaviour
{
    public Transform leftFootRayOrigin;
    //SAVE51925
    public Transform rightFootRayOrigin;
    public float raycastDistance = 1.0f;
    public LayerMask boardLayer;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click = back foot
        {
            TryPopFromFoot(leftFootRayOrigin, "BackPop");
        }

        if (Input.GetMouseButtonDown(1)) // Right click = front foot
        {
            TryPopFromFoot(rightFootRayOrigin, "FrontPop");
        }
    }

    void TryPopFromFoot(Transform footOrigin, string popType)
    {
        if (Physics.Raycast(footOrigin.position, Vector3.down, out RaycastHit hit, raycastDistance, boardLayer))
        {
            BoardInputAAB popper = hit.collider.GetComponent<BoardInputAAB>();
            if (popper != null)
            {//ion need this but this boi was tripping fuck this line 31 all my wizards hate line 31 
               // popper.ApplyPop(popType);
            }
        }
    }
}
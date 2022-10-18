using UnityEngine;

public class MouseRayCaster
{
    public Ray ray;
    public RaycastHit hit;
    public GameObject GameObjHit => hit.collider.gameObject;
    
    public void CastNewRay()
    {
        Vector3 screenMousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 screenMousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        
        ray = new Ray();
        ray.origin = worldMousePosNear;
        ray.direction = worldMousePosFar - worldMousePosNear;
        Physics.Raycast(ray, out hit);
        Debug.DrawLine(ray.origin, ray.direction * 1000 /*line lenght*/, Color.magenta);
    }
    public bool HasHitTag(string tag)
    {
        return hit.collider != null && hit.collider.CompareTag(tag);
    }
}
using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    private Vector3 dragOrigin;
    private Camera cam;
    private Plane groundPlane;

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        groundPlane = new Plane(Vector3.up, Vector3.zero);
    }

    void Update()
    {
        if (cam == null) return;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (groundPlane.Raycast(ray, out float entry))
            {
                dragOrigin = ray.GetPoint(entry);
            }
        }
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (groundPlane.Raycast(ray, out float entry))
            {
                Vector3 currentPoint = ray.GetPoint(entry);
                Vector3 difference = dragOrigin - currentPoint;
                transform.position += difference;
            }
        }
    }
}
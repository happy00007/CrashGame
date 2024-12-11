using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class RocketPathDrawer : MonoBehaviour
{
    [SerializeField] private RectTransform canvasRect; // The canvas RectTransform
    [SerializeField] private GameObject rocket;        // The rocket GameObject
    [SerializeField] private Material lineMaterial;    // Material for the line
    [SerializeField] private float lineWidth = 5f;     // Width of the line

    private LineRenderer lineRenderer;
    private Camera uiCamera; // Reference to the camera rendering the Canvas
    private List<Vector3> points = new List<Vector3>();

    private void Awake()
    {
        // Initialize LineRenderer
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.material = lineMaterial;

        // Set the camera
        Canvas canvas = canvasRect.GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;
    }

    private void Update()
    {
        // Add the current rocket position to the path
        AddPointToPath(rocket.transform.position);
    }

    private void AddPointToPath(Vector3 worldPosition)
    {
        // Convert the world position to Canvas local position
        Vector2 canvasPosition = WorldToCanvasPosition(worldPosition);

        if (points.Count == 0 || Vector2.Distance(points[points.Count - 1], canvasPosition) > 10f)
        {
            points.Add(canvasPosition);
            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPosition(points.Count - 1, canvasPosition);
        }
    }

    private Vector2 WorldToCanvasPosition(Vector3 worldPosition)
    {
        // Convert world position to screen point
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(uiCamera, worldPosition);

        // Convert screen point to Canvas local position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, uiCamera, out Vector2 localPoint);
        return localPoint;
    }
}

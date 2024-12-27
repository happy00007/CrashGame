using UnityEngine;

public class ParabolicLine : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int points = 50; // Number of points for smoothness
    [SerializeField] private float curveHeight = 5f; // Maximum height of the curve
    [SerializeField] private Transform rocket; // Starting point reference
    public Transform initialPos; // Initial position reference
    private Vector3 endPoint;

    private void Start()
    {
        if (!lineRenderer)
            lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = points;

        endPoint = new Vector3(initialPos.position.x, initialPos.position.y, initialPos.position.z);
    }

    private void Update()
    {
        DrawParabolicLine();
    }

    private void DrawParabolicLine()
    {
        if (!rocket)
            return;
        endPoint = new Vector3(initialPos.position.x, initialPos.position.y - 5.1f, initialPos.position.z);

        Vector3 startPoint = rocket.position;

        for (int i = 0; i < points; i++)
        {
            float t = i / (float)(points - 1);

            // Calculate x position using linear interpolation
            float x = Mathf.Lerp(startPoint.x, endPoint.x, t);

            // Calculate y position with slow start and exponential growth
            // The easing function is: y = curveHeight * (1 - e^(-5t))
            //float yEaseFactor = 1 - Mathf.Exp(-5 * t); 
            float yEaseFactor = 1 - Mathf.Exp(-3.5f * t);
            float y = Mathf.Lerp(startPoint.y, endPoint.y, yEaseFactor) + curveHeight * yEaseFactor;

            // Create the point and set it in the line renderer

            Vector3 point = new Vector3(x, y, startPoint.z);
            lineRenderer.SetPosition(i, point);
        }
    }
}

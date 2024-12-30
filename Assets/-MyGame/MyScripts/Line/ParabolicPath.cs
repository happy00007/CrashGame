using UnityEngine;

public class ParabolicPath : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform rocket;
    public int pointsCount = 50;
    public float maxHeight = 5f;
    public float maxDistance = 10f;

    private Vector3 initialPosition;
    public Transform initialPos;
    private void Start()
    {
        initialPosition = initialPos.position;

        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();
    }

    private void FixedUpdate()
    {
        DrawParabolicPath();
    }
    void DrawParabolicPath()
    {
        if (Vector2.Distance(initialPosition, rocket.transform.position) < 1)
            return;

        float rocketDistance = rocket.position.x - initialPosition.x;


        int pointsToDraw = Mathf.FloorToInt((rocketDistance / maxDistance) * pointsCount);

        pointsToDraw = Mathf.Min(pointsToDraw, pointsCount);

        Vector3[] positions = new Vector3[pointsToDraw];

        if (rocket.position.y <= initialPosition.y)
        {
            for (int i = 0; i < pointsToDraw; i++)
            {
                float t = (float)i / (pointsToDraw - 1);
                positions[i] = Vector3.Lerp(initialPosition, rocket.position, t);
            }
        }
        else
        {
            // Otherwise, we calculate the parabolic path
            for (int i = 0; i < pointsToDraw; i++)
            {
                // Normalized value between 0 and 1 for each point
                float t = (float)i / (pointsToDraw - 1);

                // Calculate the horizontal distance from the initial position
                float x = Mathf.Lerp(0, rocketDistance, t);

                // Calculate the corresponding y value based on the parabolic equation
                float y = (x * x) * (maxHeight / (maxDistance * maxDistance));  // Parabola equation
                //float y = (x * x) * ((rocket.position.y - initialPos.position.y) / (maxDistance * maxDistance));  // Parabola equation


                // Set the position of the line points
                positions[i] = new Vector3(initialPosition.x + x, initialPosition.y + y, initialPosition.z);
            }
        }

        // Ensure the final point is at the rocket's current position
        positions[pointsToDraw - 1] = rocket.position;

        // Update the LineRenderer
        lineRenderer.positionCount = pointsToDraw;
        lineRenderer.SetPositions(positions);
    }
}

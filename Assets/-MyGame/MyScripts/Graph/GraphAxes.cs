using UnityEngine;

public class GraphAxes : MonoBehaviour
{
    public float axisLength = 10f;

    void Start()
    {
        // Create X-axis
        GameObject xAxis = new GameObject("X-Axis");
        LineRenderer xLine = xAxis.AddComponent<LineRenderer>();
        xLine.positionCount = 2;
        xLine.SetPositions(new Vector3[] { new Vector3(-axisLength, 0, 0), new Vector3(axisLength, 0, 0) });
        xLine.startWidth = 0.05f;
        xLine.endWidth = 0.05f;

        // Create Y-axis
        GameObject yAxis = new GameObject("Y-Axis");
        LineRenderer yLine = yAxis.AddComponent<LineRenderer>();
        yLine.positionCount = 2;
        yLine.SetPositions(new Vector3[] { new Vector3(0, -axisLength, 0), new Vector3(0, axisLength, 0) });
        yLine.startWidth = 0.05f;
        yLine.endWidth = 0.05f;


        CreateGridLines(2, 10);
        AddLabels(2, 10);
    }

    void CreateGridLines(float spacing, int count)
    {
        for (int i = 1; i <= count; i++)
        {
            // Horizontal lines
            GameObject hLine = new GameObject("HLine" + i);
            LineRenderer hRenderer = hLine.AddComponent<LineRenderer>();
            float yPos = i * spacing;
            hRenderer.positionCount = 2;
            hRenderer.SetPositions(new Vector3[] { new Vector3(-axisLength, yPos, 0), new Vector3(axisLength, yPos, 0) });

            // Vertical lines
            GameObject vLine = new GameObject("VLine" + i);
            LineRenderer vRenderer = vLine.AddComponent<LineRenderer>();
            float xPos = i * spacing;
            vRenderer.positionCount = 2;
            vRenderer.SetPositions(new Vector3[] { new Vector3(xPos, -axisLength, 0), new Vector3(xPos, axisLength, 0) });
        }
    }

    void AddLabels(float spacing, int count)
    {
        for (int i = -count; i <= count; i++)
        {
            if (i == 0)
                continue;

            // X-axis labels
            GameObject xLabel = new GameObject("XLabel" + i);
            TextMesh xText = xLabel.AddComponent<TextMesh>();
            xText.text = i.ToString();
            xLabel.transform.position = new Vector3(i * spacing, -0.5f, 0);

            // Y-axis labels
            GameObject yLabel = new GameObject("YLabel" + i);
            TextMesh yText = yLabel.AddComponent<TextMesh>();
            yText.text = i.ToString();
            yLabel.transform.position = new Vector3(-0.5f, i * spacing, 0);
        }
    }


}

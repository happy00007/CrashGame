using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScaleBuilder : MonoBehaviour
{
    #region Creating Instance
    private static ScaleBuilder _instance;


    public static ScaleBuilder instance
    {
        get
        {
            if (_instance == null)
                _instance = FindFirstObjectByType<ScaleBuilder>();
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }
    #endregion

    public TMP_Text labelPrefabTxt;
    [Header("Values for X axis")]
    //public float finalValX = 11;
    public int segmentsX = 7;
    public Transform initialPtX;
    public Transform finalPtX;
    public List<TMP_Text> allLaBelsX;

    [Header("Values for Y axis")]
    //public float finalValY = 11;
    public int segmentsY = 7;
    public Transform initialPtY;
    public Transform finalPtY;
    public List<TMP_Text> allLaBelsY;

    void Start()
    {
        CreateTextObjects();
        DrawLinePointsX(LocalSettings.GRAPH_X_AXIS_LIMIT);
        DrawLinePointsY(LocalSettings.GRAPH_Y_AXIS_LIMIT);
    }

    void CreateTextObjects()
    {
        for (int i = 0; i < segmentsX + 3; i++)
        {
            GameObject obj = Instantiate(labelPrefabTxt.gameObject);
            obj.SetActive(true);
            allLaBelsX.Add(obj.GetComponent<TMP_Text>());
        }
        for (int i = 0; i < segmentsY + 3; i++)
        {
            GameObject obj = Instantiate(labelPrefabTxt.gameObject);
            obj.SetActive(true);
            allLaBelsY.Add(obj.GetComponent<TMP_Text>());
        }
    }

    public void DrawLinePointsX(float finalValX)
    {
        if (finalValX <= 0)
            finalValX = LocalSettings.GRAPH_X_AXIS_LIMIT;
        float distance = Vector3.Distance(initialPtX.position, finalPtX.position);
        float distanceFactor = distance / segmentsX;
        for (int i = 0; i < allLaBelsX.Count; i++)
        {
            Vector3 pos = new Vector3(initialPtX.position.x + i * distanceFactor, initialPtX.position.y, initialPtX.position.z);
            LocalSettings.SetPosAndRect(allLaBelsX[i].gameObject, initialPtX.GetComponent<RectTransform>(), initialPtX.parent);
            allLaBelsX[i].transform.position = pos;

            float valFactor = finalValX / (float)segmentsX;
            valFactor *= i;

            if (valFactor >= 60)
            {
                valFactor /= 60;
                allLaBelsX[i].text = i == 0 ? "" : $"{valFactor:F1}m";

            }
            else
                allLaBelsX[i].text = i == 0 ? "" : $"{valFactor:F1}s";
        }
    }

    public void DrawLinePointsY(float finalValY)
    {

        if (finalValY <= 0)
            finalValY = LocalSettings.GRAPH_Y_AXIS_LIMIT;
        //finalValY = 3.8f;
        float distance = Vector3.Distance(initialPtY.position, finalPtY.position);
        float distanceFactor = distance / segmentsY;

        for (int i = 0; i < allLaBelsY.Count; i++)
        {
            Vector3 pos = new Vector3(initialPtY.position.x, initialPtY.position.y + i * distanceFactor, initialPtY.position.z);
            LocalSettings.SetPosAndRect(allLaBelsY[i].gameObject, initialPtY.GetComponent<RectTransform>(), initialPtY.parent);
            allLaBelsY[i].transform.position = pos;

            float valFactor = finalValY / segmentsY;
            valFactor *= i;

            allLaBelsY[i].text = i == 0 ? "" : $"{valFactor:F1}x";
        }
    }
}

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] Image _smallScaleLine;
    public TMP_Text labelPrefabTxt;
    [Header("Values for X axis")]
    //public float finalValX = 11;
    public int segmentsX = 7;
    public Transform initialPtX;
    public Transform finalPtX;
    public List<TMP_Text> allLaBelsX;

    [Header("Values for Y axis")]
    //public float finalValY = 11;
    public int segmentsY = 5;
    public Transform initialPtY;
    public Transform finalPtY;
    public Transform finalPtYOrg;
    public List<TMP_Text> allLaBelsY;

    public List<GameObject> lineImages;

    void Start()
    {
        CreateTextObjects();
        //DrawLinePointsX(LocalSettings.GRAPH_X_AXIS_LIMIT);
        DrawLinePointsY(LocalSettings.GRAPH_Y_AXIS_LIMIT);
    }

    void CreateTextObjects()
    {
        //for (int i = 0; i < segmentsX + 3; i++)
        //{
        //    GameObject obj = Instantiate(labelPrefabTxt.gameObject);
        //    obj.SetActive(true);
        //    allLaBelsX.Add(obj.GetComponent<TMP_Text>());
        //}
        for (int i = 0; i < segmentsY + 5; i++)
        {
            GameObject obj = Instantiate(labelPrefabTxt.gameObject);
            obj.SetActive(true);
            allLaBelsY.Add(obj.GetComponent<TMP_Text>());

            GameObject objLine = Instantiate(_smallScaleLine.gameObject);
            objLine.SetActive(true);
            lineImages.Add(objLine);
        }
    }

    //public void DrawLinePointsX(float finalValX)
    //{
    //    if (finalValX <= 0)
    //        finalValX = LocalSettings.GRAPH_X_AXIS_LIMIT;
    //    float distance = Vector3.Distance(initialPtX.position, finalPtX.position);
    //    float distanceFactor = distance / segmentsX;
    //    for (int i = 0; i < allLaBelsX.Count; i++)
    //    {
    //        Vector3 pos = new Vector3(initialPtX.position.x + i * distanceFactor, initialPtX.position.y, initialPtX.position.z);
    //        LocalSettings.SetPosAndRect(allLaBelsX[i].gameObject, initialPtX.GetComponent<RectTransform>(), initialPtX.parent);
    //        allLaBelsX[i].transform.position = pos;

    //        float valFactor = finalValX / (float)segmentsX;
    //        valFactor *= i;

    //        if (valFactor >= 60)
    //        {
    //            valFactor /= 60;
    //            allLaBelsX[i].text = i == 0 ? "" : $"{valFactor:F1}m";

    //        }
    //        else
    //            allLaBelsX[i].text = i == 0 ? "" : $"{valFactor:F1}s";
    //    }
    //}

    public void DrawLinePointsY(float finalValY)
    {

        if (finalValY <= 0)
            finalValY = LocalSettings.GRAPH_Y_AXIS_LIMIT;
        float distance = Vector3.Distance(initialPtY.position, finalPtY.position);
        float distanceFactor = distance / segmentsY;

        for (int i = 0; i < allLaBelsY.Count; i++)
        {
            Vector3 pos = new Vector3(initialPtY.position.x, initialPtY.position.y + i * distanceFactor, initialPtY.position.z);
            LocalSettings.SetPosAndRect(allLaBelsY[i].gameObject, initialPtY.GetComponent<RectTransform>(), initialPtY.parent);
            allLaBelsY[i].transform.position = pos;

            float valFactor = finalValY / segmentsY;
            valFactor *= i;
            valFactor += (1 - ((float)i / (float)segmentsY));
            string val = $"{valFactor:F1}x";
            allLaBelsY[i].text = val;
            //Debug.LogError("Label val: " + val + "         " + ((float)i / (float)segmentsY));
            Vector3 posLine = new Vector3(initialPtY.position.x, initialPtY.position.y + (distanceFactor / 2) + (i * (distanceFactor)), initialPtY.position.z);
            LocalSettings.SetPosAndRect(lineImages[i], _smallScaleLine.GetComponent<RectTransform>(), _smallScaleLine.transform.parent);
            lineImages[i].transform.position = posLine;
        }
    }
    public void SetYOfFinalPt(float y)
    {
        finalPtY.transform.position = new Vector3(finalPtYOrg.transform.position.x, y, finalPtYOrg.transform.position.z);
    }
    public void ResetPoint()
    {
        finalPtY.transform.position = finalPtYOrg.transform.position;
    }
}

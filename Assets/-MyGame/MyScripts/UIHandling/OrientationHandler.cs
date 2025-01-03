using System;
using System.Collections;
using UnityEngine;

public class OrientationHandler : MonoBehaviour
{
    public OrientationObjets[] orientationObjets;
    [SerializeField] GameObject _rocketAnimatorForScale;
    bool _isLand;
    void Start()
    {
        //Screen.fullScreen = true;
        bool isLandscape = Screen.width > Screen.height;
        _isLand = isLandscape;
        SetOrientation(_isLand);
        StartCoroutine(SetRemainingTime());
    }

    public void SetOrientation(bool isLandscape)
    {
        for (int i = 0; i < orientationObjets.Length; i++)
        {
            RectTransform posObj = isLandscape ? orientationObjets[i].landscapePos : orientationObjets[i].portraitPos;
            LocalSettings.SetPosAndRect(orientationObjets[i].originalObj, posObj, posObj.transform.parent);
        }
        _rocketAnimatorForScale.transform.localScale = isLandscape ? Vector3.one * 50 : Vector3.one * 70;
    }


    IEnumerator SetRemainingTime()
    {
        while (true)
        {
            bool isLandscape = Screen.width > Screen.height;
            if (_isLand != isLandscape)
            {
                _isLand = isLandscape;
                SetOrientation(_isLand);
            }
            yield return new WaitForSecondsRealtime(1f);
        }
    }
}

[Serializable]
public class OrientationObjets
{
    public GameObject originalObj;
    public RectTransform portraitPos;
    public RectTransform landscapePos;
}

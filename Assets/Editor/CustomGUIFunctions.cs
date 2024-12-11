#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using UnityEngine;
using DA_Assets.FCU.Model;
using UnityEngine.UI;
using DA_Assets.Shared;

public class CustomGUIFunctions : MonoBehaviour
{
    [MenuItem("Custom Functions/Remove Script  &x")]
    static void RemoveScript()
    {
        Debug.Log("Selected Transform is on " + Selection.activeTransform.gameObject.name + ".");
        Transform[] allChildren = Selection.activeTransform.gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.GetComponent<SyncHelper>())
                DestroyImmediate(child.GetComponent<SyncHelper>());
            if (child.GetComponent<LayoutElement>())
                DestroyImmediate(child.GetComponent<LayoutElement>());
            if (child.GetComponent<HorizontalLayoutGroup>())
                DestroyImmediate(child.GetComponent<HorizontalLayoutGroup>());
            if (child.GetComponent<VerticalLayoutGroup>())
                DestroyImmediate(child.GetComponent<VerticalLayoutGroup>());
            if (child.GetComponent<CornerRounder>())
                DestroyImmediate(child.GetComponent<CornerRounder>());
            if (child.GetComponent<RectMask2D>())
                DestroyImmediate(child.GetComponent<RectMask2D>());
            if (child.GetComponent<ContentSizeFitter>())
                DestroyImmediate(child.GetComponent<ContentSizeFitter>());


        }

    }

    [MenuItem("Custom Functions/Remove Script  &x", true)]
    static bool ValidateRemoveScript()
    {
        return Selection.activeTransform != null;
    }

}
#endif

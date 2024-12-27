using UnityEngine;
using UnityEditor;

public class CopyRectTransformComponent : EditorWindow
{
    [MenuItem("Custom/Copy RectTransform Component %#c")] // Shortcut: Ctrl + Shift + C (Cmd + Shift + C on Mac)
    private static void CopyRectTransform()
    {
        GameObject selectedObject = Selection.activeGameObject;

        if (selectedObject != null)
        {
            RectTransform rectTransform = selectedObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                // Get the existing RectTransform component from the selected GameObject
                RectTransform copy = selectedObject.GetComponent<RectTransform>();

                // Use SerializedObject and SerializedProperty to copy the entire component
                SerializedObject serializedObject = new SerializedObject(rectTransform);
                SerializedObject copySerializedObject = new SerializedObject(copy);

                // Copy the properties from the original RectTransform to the existing one
                copySerializedObject.Update();
                serializedObject.Update();

                // Copy each property using SerializedProperty
                SerializedProperty prop = serializedObject.GetIterator();
                while (prop.NextVisible(true))
                {
                    copySerializedObject.CopyFromSerializedProperty(prop);
                }

                copySerializedObject.ApplyModifiedProperties();

                Debug.Log("RectTransform component copied.");
            }
            else
            {
                Debug.LogError("Selected GameObject does not have a RectTransform component.");
            }
        }
        else
        {
            Debug.LogError("No GameObject selected.");
        }
    }
}

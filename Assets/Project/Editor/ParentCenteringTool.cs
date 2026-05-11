using UnityEngine;
using UnityEditor;

public class ParentCenteringTool : Editor
{
    [MenuItem("Tools/MagicNagger/Center Parent Between Children")]
    public static void CenterParentBetweenChildren()
    {
        GameObject parent = Selection.activeGameObject;

        if (parent == null || parent.transform.childCount < 2)
        {
            Debug.LogWarning("ParentCenteringTool: Please select a parent GameObject with at least 2 children.");
            return;
        }

        Undo.RegisterFullObjectHierarchyUndo(parent, "Center Parent Between Children");

        Transform[] children = new Transform[parent.transform.childCount];
        Vector3[] worldPositions = new Vector3[parent.transform.childCount];
        Quaternion[] worldRotations = new Quaternion[parent.transform.childCount];
        Vector3[] worldScales = new Vector3[parent.transform.childCount];

        Vector3 center = Vector3.zero;

        // Capture child world data and calculate center
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            children[i] = parent.transform.GetChild(i);
            worldPositions[i] = children[i].position;
            worldRotations[i] = children[i].rotation;
            worldScales[i] = children[i].lossyScale;
            center += worldPositions[i];
        }

        center /= children.Length;

        // Move the parent to the center
        parent.transform.position = center;

        // Restore child world positions
        for (int i = 0; i < children.Length; i++)
        {
            children[i].position = worldPositions[i];
            children[i].rotation = worldRotations[i];
            // Scale is tricky if parent has scale, but usually we center to fix hierarchy
        }

        Debug.Log($"ParentCenteringTool: Centered '{parent.name}' at {center}.");
    }

    [MenuItem("Tools/MagicNagger/Center Parent Between Children", true)]
    public static bool ValidateCenterParentBetweenChildren()
    {
        return Selection.activeGameObject != null && Selection.activeGameObject.transform.childCount >= 2;
    }
}

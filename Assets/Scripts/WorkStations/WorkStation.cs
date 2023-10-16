using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEditor;


public class WorkStation : MonoBehaviour
{
    Grid Grid { get; set; }

    //
    public List<Transform> workingPositions;

    private void Awake()
    {
        Grid = FindFirstObjectByType<Grid>();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(WorkStation))]
public class WorkStationCustomEditor : Editor
{
    bool isExpanded = true;

    public override void OnInspectorGUI()
    {
        if(target is WorkStation workStation)
        {
            serializedObject.Update();
            var workingPositions = serializedObject.FindProperty("workingPositions");

            EditorGUILayout.PropertyField(workingPositions,false);

            isExpanded = EditorGUILayout.Foldout(isExpanded, "workingPositions");
            if (isExpanded)
            {
                for (int i = 0; i < workingPositions.arraySize; i++)
                {
                    var targetTransform = workingPositions.GetArrayElementAtIndex(i);

                    GUILayout.BeginHorizontal();

                    EditorGUILayout.PropertyField(targetTransform,GUIContent.none);

                    (targetTransform.objectReferenceValue as Transform).localPosition = EditorGUILayout.Vector3Field("", (targetTransform.objectReferenceValue as Transform).localPosition);

                    // Delete Button
                    if (GUILayout.Button("-", GUILayout.Width(20)))
                    {
                        DestroyImmediate((targetTransform.objectReferenceValue as Transform).gameObject);
                        workingPositions.DeleteArrayElementAtIndex(i);
                    }

                    GUILayout.EndHorizontal();
                }


                if (GUILayout.Button("AddWorkPosition"))
                {
                    var obj = new GameObject("workPosition");
                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.parent = workStation.transform;
                    workingPositions.InsertArrayElementAtIndex(workingPositions.arraySize);
                    workingPositions.GetArrayElementAtIndex(workingPositions.arraySize-1).objectReferenceValue = obj.transform;

                }
            }
            serializedObject.ApplyModifiedProperties();
        }

    }
}
#endif
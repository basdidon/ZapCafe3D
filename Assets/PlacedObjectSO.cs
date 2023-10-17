using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

[CreateAssetMenu()]
public class PlacedObjectSO : ScriptableObject
{
    [field:SerializeField] 
    public Transform PlacedObjectPrefab { get; private set; }

    List<Vector3Int> objectCells;
    public IReadOnlyList<Vector3Int> ObjectCells
    {
        get
        {
            if(objectCells == null || objectCells.Count == 0)
            {
                objectCells = new() { Vector3Int.zero };
            }
            return objectCells;    
        }
    }

    public bool AddCell(Vector3Int newCell)
    {
        if (ObjectCells != null && !ObjectCells.Contains(newCell))
        {
            objectCells.Add(newCell);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Remove(Vector3Int toRemove)
    {
        if (toRemove != Vector3Int.zero)
        {
            objectCells.Remove(toRemove);
        }
    }

    public bool SetObjectCellAt(int index, Vector3Int newValue)
    {
        if (index < 0 || index >= ObjectCells.Count)
            return false;

        if (ObjectCells.Contains(newValue))
            return false;

        objectCells[index] = newValue;
        return true;
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(PlacedObjectSO))]
public class GridObjectCustomEditor : Editor
{
    bool isExpanded = true;
    Vector3Int newCell = Vector3Int.zero;

    public override void OnInspectorGUI()
    {
        if (target is PlacedObjectSO gridObject)
        {
            serializedObject.Update();

            isExpanded = EditorGUILayout.Foldout(isExpanded, "Object Cells");


            if (!isExpanded)
                return;

            for (int i = 0; i < gridObject.ObjectCells.Count; i++)
            {
                EditorGUI.BeginDisabledGroup(i == 0);
                GUILayout.BeginHorizontal();
                gridObject.SetObjectCellAt(i, EditorGUILayout.Vector3IntField("", gridObject.ObjectCells[i]));

                if (GUILayout.Button("-", GUILayout.Width(20))) gridObject.Remove(gridObject.ObjectCells[i]);

                GUILayout.EndHorizontal();
                EditorGUI.EndDisabledGroup();
            }

            EditorGUILayout.Space();
            DrawHorizontalLine();
            GUILayout.BeginHorizontal("box");
            newCell = EditorGUILayout.Vector3IntField("", newCell);
            if (GUILayout.Button("AddCell"))
            {
                gridObject.AddCell(newCell);
            }
            GUILayout.EndHorizontal();
            DrawHorizontalLine();
            /*
            GUI.backgroundColor = Color.red;
            GUI.color = Color.white;
            if (GUILayout.Button("Clear"))
            {
                gridObject
            }
            */
        }


        serializedObject.ApplyModifiedProperties();
    }

    void DrawHorizontalLine()
    {
        var rect = EditorGUILayout.BeginHorizontal();
        Handles.color = Color.gray;
        Handles.DrawLine(new Vector2(rect.x - 15, rect.y), new Vector2(rect.width + 15, rect.y));
        EditorGUILayout.EndHorizontal();
    }
}
#endif

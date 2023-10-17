using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GridPlacement
{
    public class GridObject : MonoBehaviour
    {
        Vector3 CellSize = Vector3.one;
        List<Vector3Int> objectCells;
        public IReadOnlyList<Vector3Int> ObjectCells => objectCells;

        public void InstantiateObjectCellList()
        {
            objectCells = new() { Vector3Int.zero };
        }

        public bool AddCell(Vector3Int newCell)
        {
            if(ObjectCells != null && !ObjectCells.Contains(newCell))
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
            if(toRemove != Vector3Int.zero)
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

        readonly Vector3[] vecs = new Vector3[]
{
                new Vector3(.5f,0,.5f),
                new Vector3(.5f, 0, -.5f),
                new Vector3(-.5f, 0, -.5f),
                new Vector3(-.5f, 0, .5f),
};
        /*
        private void OnDrawGizmos()
        {
            foreach (var cellPos in ObjectCells)
            {
                Gizmos.color = cellPos == Vector3Int.zero ? Color.black : Color.white;
                var gridCenterLocal = Vector3.Scale(CellSize, cellPos) +(CellSize / 2);
                Gizmos.DrawCube(gridCenterLocal,Vector3.one);
            }
        }*/
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(GridObject))]
    public class GridObjectCustomEditor : Editor
    {
        bool isExpanded = true;
        Vector3Int newCell = Vector3Int.zero;

        public override void OnInspectorGUI()
        {
            if(target is GridObject gridObject)
            {
                serializedObject.Update();

                isExpanded = EditorGUILayout.Foldout(isExpanded, "Object Cells");

                if (gridObject.ObjectCells == null)
                {
                    if (!isExpanded)
                        return;

                    if (GUILayout.Button("CreateList"))
                    {
                        gridObject.InstantiateObjectCellList();
                    }
                }
                else
                {
                    if (!isExpanded)
                        return;

                    for (int i = 0; i < gridObject.ObjectCells.Count; i++)
                    {
                        EditorGUI.BeginDisabledGroup(i == 0);
                        GUILayout.BeginHorizontal();
                        gridObject.SetObjectCellAt(i, EditorGUILayout.Vector3IntField("", gridObject.ObjectCells[i]));

                        if (GUILayout.Button("-",GUILayout.Width(20))) gridObject.Remove(gridObject.ObjectCells[i]);

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

                    GUI.backgroundColor = Color.red;
                    GUI.color = Color.white;
                    if (GUILayout.Button("Clear"))
                    {
                        gridObject.InstantiateObjectCellList();
                    }

                }


                serializedObject.ApplyModifiedProperties();
            }
        }


        /*
        private void OnSceneGUI()
        {
            if (target is GridObject gridObject)
            {
                if (gridObject.ObjectCells == null)
                    return;

                foreach (var cellPos in gridObject.ObjectCells)
                {
                    var translatePos = vecs.Select(rectPos => cellPos + rectPos).ToArray();
                    /*
                    var rect_x = translatePos.Min(pos => pos.x);
                    var rect_y = translatePos.Min(pos => pos.z);
                    var rect_width = translatePos.Max(pos => pos.x);
                    var rect_height = translatePos.Max(pos => pos.y);
                    Rect rect = new(rect_x,rect_y,rect_width,rect_height);
                    var faceColor = cellPos == Vector3Int.zero ? Color.green : Color.gray;
                    Handles.DrawSolidRectangleWithOutline(translatePos, faceColor, Color.black);
                }

            }
        }
*/
        void DrawHorizontalLine()
        {
            var rect = EditorGUILayout.BeginHorizontal();
            Handles.color = Color.gray;
            Handles.DrawLine(new Vector2(rect.x - 15, rect.y), new Vector2(rect.width + 15, rect.y));
            EditorGUILayout.EndHorizontal();
        }
    }
#endif
}
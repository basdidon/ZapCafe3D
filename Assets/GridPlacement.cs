using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridPlacement
{
    public class GridPlacement : MonoBehaviour
    {
        public static GridPlacement Instance { get; private set; }
        public Grid Grid { get; private set; }

        public Transform target;
        public Transform DebugTransform;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            Grid = FindFirstObjectByType<Grid>();
        }

        private void Update()
        {
            var cellPos = Grid.WorldToCell(target.position);
            DebugTransform.position = Grid.GetCellCenterWorld(cellPos);
        }
    }
}
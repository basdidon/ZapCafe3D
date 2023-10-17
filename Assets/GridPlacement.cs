using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace GridPlacement
{
    [RequireComponent(typeof(Grid))]
    [RequireComponent(typeof(Tilemap))]
    public class GridPlacement : MonoBehaviour
    {
        public static GridPlacement Instance { get; private set; }
        public Grid Grid { get; private set; }
        [field: SerializeField] public Tilemap Tilemap { get; private set; }

        // Input Ref
        [field: SerializeField] InputActionReference cursorPosition { get; set; }
        [field: SerializeField] InputActionReference mouseLeftDown { get; set; }

        Dictionary<Vector2Int, GridObject> gridObjects;
        public IReadOnlyDictionary<Vector2Int, GridObject> GridObjects => gridObjects;
        public void AddGridObject(Vector2Int cellPos,GridObject gridObject)
        {
            gridObjects.Add(cellPos,gridObject);
        }

        public Texture texture;
        public Transform target;
        public Transform DebugTransform;

        public PlacedObjectSO objectSO;

        private void OnEnable()
        {
            cursorPosition.action.Enable();
            mouseLeftDown.action.Enable();
        }

        private void OnDisable()
        {
            cursorPosition.action.Disable();
            mouseLeftDown.action.Disable();
        }

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

            Grid = GetComponent<Grid>();
            Tilemap = GetComponent<Tilemap>();

            gridObjects = new();

            // input
            mouseLeftDown.action.performed += _ => {
                Debug.Log("Hi");
            };
        }

        private void Start()
        {
            DebugTransform = Instantiate(objectSO.PlacedObjectPrefab);
        }

        private void Update()
        {
            var mousePos = cursorPosition.action.ReadValue<Vector2>();
            var camRay = Camera.main.ScreenPointToRay(mousePos);

            if(Physics.Raycast(camRay,out RaycastHit hit,100f))
            {
                if (!hit.transform.CompareTag("Floor"))
                    return;

                var cellPos = Grid.WorldToCell(hit.point);
                DebugTransform.position = Grid.GetCellCenterWorld(cellPos);
            }

        }
        /*
        void OnDrawGizmosSelected()
        {
            // Draw a texture rectangle on the XY plane of the scene
            Mesh mesh = new();
            mesh.vertices = vecs;
            Gizmos.DrawMesh(mesh);
        }*/
    }
}
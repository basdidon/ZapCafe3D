using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        [SerializeField] Transform focusTile;
        [SerializeField] Vector3 focusTileOffset;
        Vector3Int focusCellPos;
        public Vector3Int FocusCellPos
        {
            get => focusCellPos;
            set
            {
                if(focusCellPos != value)
                {
                    focusCellPos = value;
                    focusTile.position = Grid.GetCellCenterWorld(value) + focusTileOffset;
                }
            }
        }

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
            focusTile.gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            if (TryGetCellByScreenPoint(out Vector3Int cellPos))
            {
                focusTile.gameObject.SetActive(true);
                FocusCellPos = cellPos;
            }
            else
            {
                focusTile.gameObject.SetActive(false);
            }
        }

        bool TryGetCellByScreenPoint(out Vector3Int targetCell)
        {
            targetCell = Vector3Int.zero;

            RaycastHit[] hits = new RaycastHit[100];
            int hitsCount;

            hitsCount = Physics.RaycastNonAlloc(Camera.main.ScreenPointToRay(cursorPosition.action.ReadValue<Vector2>()), hits, 100);

            if (hitsCount > 0)
            {
                var slicedHits = hits.Take(hitsCount);
                if (slicedHits.Any(hit => hit.transform.CompareTag("Floor")))
                {
                    var firstCellHit = slicedHits.First(hit => hit.transform.CompareTag("Floor"));
                    targetCell = Grid.WorldToCell(firstCellHit.point);
                    return true;
                }
            }

            return false;
        }
    }
}
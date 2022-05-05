using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    #region Grid itself
    private Grid _grid;                                             // The grid superimposed in the world space
    private RaycastHit? _gridHit;                                    // The grid hit raycast info struct
    public GameObject m_GridCellPrefab;
    public static bool m_CreatedGridCells = false;
    #endregion

    #region Grid cell size parameters
    [SerializeField] int m_CellSize = 1;
    private int m_GridWidth;
    private int m_GridHeight;
    private float m_GridYOffset = 0.5f;
    private float m_GridZDistance = -2.5f;
    #endregion

    #region Objects put on the grid
    private QuadFactory _quadFactory;                               // The quad factory to create a colored tile
    private List<GameObject> _cellQuads;                            // List of cell quads instantiated in the scene
    private GameObject[] m_Nodes = new GameObject[6];               // List of user-placed nodes on the grid
    private GameObject[] m_PlacableTiles = new GameObject[6];               // List of designer-placed placable tiles on the grid
    private GameObject selectionIndicator;
    #endregion

    private void CreateGridCells()
    {
        for (int x = -m_GridWidth; x <= m_GridWidth; x++)
        {
            for (int z = -m_GridHeight; z < m_GridHeight; z++)
            {
                Instantiate(m_GridCellPrefab, new Vector3(x, z, m_GridZDistance), Quaternion.identity, this.transform);
            }
        }
        m_CreatedGridCells = true;
    }

    /// <summary>
    /// Initializes the grid, quad factory, and cell quads list.
    /// </summary>
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;

            float aspectRatio = Camera.main.aspect;
            m_GridWidth = (int)(Camera.main.orthographicSize * aspectRatio) / m_CellSize;
            m_GridHeight = (int)Camera.main.orthographicSize / m_CellSize;
            _grid = new Grid(m_GridWidth, m_GridHeight, m_CellSize, 0);

            _quadFactory = GetComponent<QuadFactory>();
            _cellQuads = new List<GameObject>();

            // Generate the empty cells
            if (!m_CreatedGridCells)
            {
                CreateGridCells();
            }
            // Make sure everything's aligned for the first run
            SnapObjectsToGridEditorTime();

            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// Raycast into individual cells in the logical grid. From screen space (pixel) to world space.
    /// Stores the hit info in the class member _gridHit.
    /// </summary>
    /// <returns>True if hit a collider.</returns>
    private bool RaycastGrid()
    {
        // Test raycast into grid cells

        if (Camera.main == null) return false;
        
        Ray worldRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(worldRay.origin, worldRay.direction * 100, Color.blue);

        // bool hitSomething = Physics.Raycast(worldRay, out _gridHit, 100);
        int layerMask = ~(1 << LayerMask.NameToLayer("Ignore Raycast"));

        RaycastHit[] hits = Physics.RaycastAll(worldRay, 100, layerMask);

        if (hits.Length > 0)
        {
            foreach(RaycastHit hit in hits)
            {
                // Discard placement if there is a node there
                if (hit.collider.gameObject.CompareTag("Node"))
                {
                    return false;
                }
                if (hit.collider.gameObject.CompareTag("PlacableActorTile"))
                {
                    _gridHit = hit;
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Checks if the player's left click raycasted something in the world.
    /// Creates a colored quad at that world position if so for now. 
    /// </summary>
    private void LateUpdate()
    {
        if (MetaManager.GetCurrentScene() == 2)
        {
            HandleObjectPlacement();
        }
    }
    public void HandleObjectPlacement()
    {
        if (RaycastGrid() && Input.GetMouseButtonDown(0))
        {
            if (_gridHit == null)
            {
                return;
            }
            // Snap it to nearest grid cell (lower left corner)
            float x = Mathf.Round(_gridHit.Value.point.x);
            float y = Mathf.Round(_gridHit.Value.point.y);

            Vector3 pos = new Vector3(x, y, m_GridZDistance);

            // Spawn things there?
            // Debug.Log($"[GameEngine.cs/LateUpdate]: Hit grid cell at world x: {x}.");
            // Temporary display
            if (selectionIndicator == null)
            {
                //selectionIndicator = _quadFactory.CreateQuad(pos, _grid, ref _cellQuads);
            } else
            {
                //selectionIndicator.transform.position = pos;
            }

            GridCell _gridCell;
            _gridCell = _gridHit.Value.collider.GetComponent<GridCell>();
            
            if (!_gridCell.m_HasNode)
            {
                TurretFactory.Instance.Create(pos);
                _gridCell.m_HasNode = true;
            }
            _gridHit = null;
        }
    }

    public void SnapToGrid(ref GameObject gameObject, float offset)
    {
        float x = Mathf.Round(gameObject.transform.position.x);
        float y = Mathf.Round(gameObject.transform.position.y);
        float z = Mathf.Round(gameObject.transform.position.y);

        Vector3 pos = new Vector3(x, y, z);
        if (offset > 0)
        {
            pos.x += offset;
            pos.y += offset;
            pos.z += offset;
        }
        else if (offset < 0)
        {
            pos.x -= offset;
            pos.y -= offset;
            pos.z -= offset;
        }

        gameObject.transform.position = pos;
    }

    private void Update()
    {
        // SnapObjectsToGridEditorTime(); // Uncomment for edit mode snapping
    }

    public void SnapObjectsToGridEditorTime()
    {
        // 1. Get all Start/end nodes in a collection
        m_Nodes = GameObject.FindGameObjectsWithTag("Node");
        m_PlacableTiles = GameObject.FindGameObjectsWithTag("PlacableActorTile");

        // 2. For each object in coll, snap to its nearest grid -> should be at the center
        SnapSelectedTilesToGrid(m_Nodes);

        // 3. Same for placable actor tiles
        SnapSelectedTilesToGrid(m_PlacableTiles);
    }

    private void SnapSelectedTilesToGrid(GameObject[] selectedTiles)
    {
        for (int i = 0; i < selectedTiles.Length; i++)
        {
            GameObject m_Node = selectedTiles[i];
            float offset = 0;// m_Node.GetComponent<SphereCollider>().radius;

            SnapToGrid(ref m_Node, offset);

            // Clamp to z = -2.5f for optimal visibility in this case
            m_Node.transform.position = new Vector3(m_Node.transform.position.x, m_Node.transform.position.y, m_GridZDistance);
        }
    }
}

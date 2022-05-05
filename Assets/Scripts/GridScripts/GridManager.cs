using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    #region Grid itself
    private Grid _grid;                                             // The grid superimposed in the world space
    public RaycastHit? _gridHit;                                    // The grid hit raycast info struct
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
    private GameObject[] m_PlacableTiles = new GameObject[6];       // List of designer-placed placable tiles on the grid
    private GameObject selectionIndicator;
    public int m_MaxPlaceableActors = 3;                            // Max amount of placeable actors on a level = should depend on the level itself. Test # for now
    public int m_CurrentlyPlacedActors = 0;
    public static GameObject m_CurrentAction = null;
    public Material m_ActorMovedColorMat;                           // Just to show a difference between selecting empty tiles vs. an actor
    public Material m_PlaceableTileColorMat;                        // Red
    public bool m_RaycastHitActor = false;
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
    private bool RaycastSpecificTile(string tag, int targetLayer)
    {
        // Test raycast into grid cells
        if (Camera.main == null) return false;
        
        Ray worldRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(worldRay.origin, worldRay.direction * 100, Color.blue);

        RaycastHit[] hits = Physics.RaycastAll(worldRay, 100, 1 << targetLayer);

        if (hits.Length > 0)
        {
            foreach(RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.CompareTag(tag))
                {
                    _gridHit = hit;
                    return true;
                }
            }
        }
        _gridHit = null;
        return false;
    }

    // Also checks for a specific colliderObject name (i.e., useful for turrets that have 2 colliders)
    private bool RaycastSpecificTile(string colliderObjectName, string tag, int targetLayer)
    {
        // Test raycast into grid cells
        if (Camera.main == null) return false;

        Ray worldRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(worldRay.origin, worldRay.direction * 100, Color.blue);

        RaycastHit[] hits = Physics.RaycastAll(worldRay, 100, 1 << targetLayer);

        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.CompareTag(tag) && hit.collider.gameObject.name == colliderObjectName)
                {
                    _gridHit = hit;
                    return true;
                }
            }
        }
        _gridHit = null;
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
        if (Input.GetMouseButtonDown(0))
        {
            if (RaycastSpecificTile("Node", LayerMask.NameToLayer("Node")))
            {
                m_RaycastHitActor = false;

                // Display selector
                PlaceSelectedTileIndicator(GetSelectorSnappedGridPos());
            }
            if (RaycastSpecificTile("PlacableActorTile", LayerMask.NameToLayer("PlaceableActorTile")))
            {
                // Allows to check whether to deselect later or not
                m_RaycastHitActor = true;

                // Display selector
                PlaceSelectedTileIndicator(GetSelectorSnappedGridPos());

                // Differentiate between moving existing actor or creating a new one
                if (m_CurrentAction != null)
                {
                    print("Moving existing hero to another tile!");
                    m_CurrentAction.transform.position = GetSelectorSnappedGridPos();
                }
                else
                {
                    // Check if can place actor
                    GridCell _gridCell;
                    _gridCell = _gridHit.Value.collider.GetComponent<GridCell>();

                    if (!_gridCell.m_HasNode && m_CurrentlyPlacedActors < m_MaxPlaceableActors)
                    {
                        // Snap it to nearest grid cell (lower left corner)
                        m_CurrentAction = TurretFactory.Instance.Create(GetSelectorSnappedGridPos());
                        _gridCell.m_HasNode = true;
                        m_CurrentlyPlacedActors++;
                        PlaceActorIndicator();
                    }
                }
            }
            // Notice that it's the turret's body that has the box collider that we want, otherwise could hit its sphere collider (the hero's detection or attack radius)
            if (RaycastSpecificTile("TurretBody", LayerMask.NameToLayer("Turret")))
            {
                m_RaycastHitActor = true;

                print("Turret info log UI activated!");

                GameObject actor = _gridHit.Value.collider.gameObject;
                // TODO: Select hero/actor state
                m_CurrentAction = actor;

                // Display indicator
                PlaceActorIndicator();

                // Display its info
                print("Selected actor info: Name =" + actor.GetComponent<Turret>().m_ActorName);
            }

            // Hit empty grid cell
            HandleDeselection();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Confirm the current action
            // TODO: show pop-up confirm dialog box or something
            EndCurrentAction();
        }
    }

    private void HandleDeselection()
    {
        if (!m_RaycastHitActor)
        {
            // This is a bit weird but usage is to reset everything each frame where we
            // clicked or something else than an actor
            EndCurrentAction();
            HideSelector();
            _gridHit = null;
        }
        m_RaycastHitActor = false;
    }

    private void EndCurrentAction()
    {
        m_CurrentAction = null;  // Deselect also because user is trying to select a placeable tile to create a new actor
    }

    private void HideSelector()
    {
        if (selectionIndicator != null)
        {
            selectionIndicator.transform.position = new Vector3(1000, 1000, 1000);
        }
    }

    public Vector3 GetSelectorSnappedGridPos()
    {
        float x = Mathf.Round(_gridHit.Value.point.x);
        float y = Mathf.Round(_gridHit.Value.point.y);

        return new Vector3(x, y, m_GridZDistance);
    }

    public void PlaceSelectedTileIndicator(Vector3 position)
    {
        // Temporary display
        if (selectionIndicator == null)
        {
            selectionIndicator = _quadFactory.CreateQuad(position, _grid, ref _cellQuads);
        }
        else
        {
            selectionIndicator.transform.position = GetSelectorSnappedGridPos();
        }
        selectionIndicator.GetComponent<MeshRenderer>().material = m_PlaceableTileColorMat;
    }

    public void PlaceActorIndicator()
    {
        if (m_CurrentAction == null)
        {
            return;
        }
        PlaceSelectedTileIndicator(m_CurrentAction.transform.position);
        selectionIndicator.GetComponent<MeshRenderer>().material = m_ActorMovedColorMat;
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

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    private Grid _grid;                          // The grid superimposed in the world space
    private RaycastHit _gridHit;                 // The grid hit raycast info struct
    private QuadFactory _quadFactory;            // The quad factory to create a colored tile
    private List<GameObject> _cellQuads;         // List of cell quads instantiated in the scene
    private GameObject[] m_Nodes = new GameObject[6];            // List of user-placed nodes on the grid
    public GameObject m_GridCellPrefab;
    public static bool m_CreatedGridCells = false;

    private void CreateGridCells()
    {
        for (int x = -25; x < 25; x++)
        {
            for (int z = -25; z < 25; z++)
            {
                Instantiate(m_GridCellPrefab, new Vector3(x, z, -2.5f), Quaternion.identity, this.transform);
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
            _grid = new Grid(25, 25, 1, 0);
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

        RaycastHit[] hits = Physics.RaycastAll(worldRay.origin, worldRay.direction, 100);

        //if (hitSomething)
        //{
            // Debug.Log(_gridHit.point.ToString());
            // TowerDefenseManager.m_CurrentGameObjectClicked = null;
        //}

        if (hits.Length > 0)
        {
            foreach(RaycastHit hit in hits)
            {
                // Discard placement if there is a node there
                if (hit.collider.gameObject.CompareTag("Node"))
                {
                    return false;
                }
                _gridHit = hit; // Takes the last for now
            }
            return true;
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
            // Snap it to nearest grid cell (lower left corner)
            float x = Mathf.Round(_gridHit.point.x);
            float y = Mathf.Round(_gridHit.point.y);
            Vector3 pos = new Vector3(x, y, -2.5f);

            // Debug.Log($"[GameEngine.cs/LateUpdate]: Hit grid cell at world x: {x}.");
            _quadFactory.CreateQuad(pos, _grid, ref _cellQuads);
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

        // 2. For each object in coll, snap to its nearest grid -> should be at the center
        for (int i = 0; i < m_Nodes.Length; i++)
        {
            GameObject m_Node = m_Nodes[i];
            float offset = 0;// m_Node.GetComponent<SphereCollider>().radius;

            SnapToGrid(ref m_Node, offset);

            // Clamp to z = -2.5f for optimal visibility in this case
            m_Node.transform.position = new Vector3(m_Node.transform.position.x, m_Node.transform.position.y, -2.5f);
        }
    }
}

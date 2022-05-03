using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class GridManager : MonoBehaviour
{
    private Grid _grid;                          // The grid superimposed in the world space
    private RaycastHit _gridHit;                 // The grid hit raycast info struct
    private QuadFactory _quadFactory;            // The quad factory to create a colored tile
    private List<GameObject> _cellQuads;         // List of cell quads instantiated in the scene
    private GameObject[] m_Nodes = new GameObject[6];            // List of user-placed nodes on the grid

    /// <summary>
    /// Initializes the grid, quad factory, and cell quads list.
    /// </summary>
    private void Start()
    {
        _grid = new Grid(25, 25, 1, 0);
        _quadFactory = GetComponent<QuadFactory>();
        _cellQuads = new List<GameObject>();
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
        
        bool hitSomething = Physics.Raycast(worldRay, out _gridHit, 100);

        if (hitSomething)
        {
            // Debug.Log(_gridHit.point.ToString());   
        }

        return hitSomething;
    }

    /// <summary>
    /// Checks if the player's left click raycasted something in the world.
    /// Creates a colored quad at that world position if so for now. 
    /// </summary>
    private void LateUpdate()
    {
        HandleObjectPlacement();
    }

    public void HandleObjectPlacement()
    {
        if (RaycastGrid() && Input.GetMouseButtonDown(0))
        {
            // Snap it to nearest grid cell (lower left corner)
            float x = Mathf.Round(_gridHit.point.x);
            float y = Mathf.Round(_gridHit.point.y);
            Vector3 pos = new Vector3(x, y, -2.5f);

            Debug.Log($"[GameEngine.cs/LateUpdate]: Hit grid cell at world x: {x}.");
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
        SnapObjectsToGridEditorTime();
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

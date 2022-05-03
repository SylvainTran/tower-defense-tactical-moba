using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private Grid _grid;                          // The grid superimposed in the world space
    private RaycastHit _gridHit;                 // The grid hit raycast info struct
    private QuadFactory _quadFactory;            // The quad factory to create a colored tile
    private List<GameObject> _cellQuads;         // List of cell quads instantiated in the scene
    
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
            Debug.Log(_gridHit.point.ToString());   
        }

        return hitSomething;
    }

    /// <summary>
    /// Checks if the player's left click raycasted something in the world.
    /// Creates a colored quad at that world position if so for now. 
    /// </summary>
    private void LateUpdate()
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
}

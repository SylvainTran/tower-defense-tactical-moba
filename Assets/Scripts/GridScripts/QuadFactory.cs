using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class QuadFactory : MonoBehaviour
{
    private float width = 1;                // Quad local width
    private float height = 1;               // Quad local height
    private Mesh _cellQuadSelector;         // The quad cell mesh produced by the factory
    private int _cellQuadSelectors;         // The number of live cell meshes
    private int _maxCellQuads = 3;          // The max amount of cell quads permitted to exist in the scene
    public GameObject quadPrefab;           // The prefab for the quad, to hold the meshfilter component of the quad
    
    /// <summary>
    /// Creates a quad positioned in local position as specified by the quad's four local corner vertice positions. 
    /// </summary>
    public Mesh CreateQuad()
    {
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[] // Local space
        {
            new Vector3(0, 0, 0),
            new Vector3(width, 0, 0),
            new Vector3(0, height, 0),
            new Vector3(width, height, 0)
        };
        mesh.vertices = vertices;

        int[] tris = new int[] // clockwise order
        {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1
        };
        mesh.triangles = tris;

        Vector3[] normals = new Vector3[]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        mesh.normals = normals;

        Vector2[] uv = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        mesh.uv = uv;
        meshFilter.mesh = mesh;
        
        return mesh;
    }

    /// <summary>
    /// Creates and returns a new quad instance using a Quad GameObject prefab.
    /// </summary>
    /// <param name="pos">The world position of the new quad instance.</param>
    /// <param name="grid">The logical grid in the world.</param>
    /// <param name="cellQuads">The list of cell quads reference in the GameEngine.</param>
    /// <returns></returns>
    [CanBeNull]
    public GameObject CreateQuad(Vector3 pos, Grid grid, ref List<GameObject> cellQuads)
    {
        _maxCellQuads = grid.Width * grid.Length;
        if (_cellQuadSelectors > _maxCellQuads)
        {
            Debug.LogError("[QuadFactory.cs/CreateQuad]: Aborting CreateQuad because already exceeded max quad instances.");
            return null;
        }
        
        Vector3 lowerLeftV = new Vector3(pos.x, pos.y, pos.z);
        GameObject cellQuad = Instantiate(quadPrefab, lowerLeftV, Quaternion.identity);
        cellQuads.Add(cellQuad);
        ++_cellQuadSelectors;
        
        Debug.Log("[QuadFactory.cs/CreateQuad]: Created a new quad.");
        return cellQuad;
    }
}

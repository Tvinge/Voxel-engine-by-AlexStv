using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshData
{
    public List<Vector3> vertices = new List<Vector3>(); //wierzcho³ki trojk¹tów, 3 wierzcho³ki - 1 trójk¹t
    public List<int> triangles = new List<int>();// 2 triangles - 1 block's face
    public List<Vector2> uv = new List<Vector2>();//list of texture coordinates, 2 per triangle(lower left and upper right), 4 per face
    //same thing but for colliders
    public List<Vector3> colVertices = new List<Vector3>();
    public List<int> colTriangles = new List<int>();
    public bool useRenderDataForCol;//if true all vertices and triangles added to the render mesh get added to the collision mesh as well

    public MeshData() { }
    public void AddQuadTriangles()//generates triangles from vertices 
    {
        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 3);
        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 1);
        if (useRenderDataForCol) //adds these trangles and vertices to the collision lists(uses collision vertices)
        {
            colTriangles.Add(colVertices.Count - 4);
            colTriangles.Add(colVertices.Count - 3);
            colTriangles.Add(colVertices.Count - 2);
            colTriangles.Add(colVertices.Count - 4);
            colTriangles.Add(colVertices.Count - 2);
            colTriangles.Add(colVertices.Count - 1);
        }
    }
    public void AddVertex(Vector3 vertex)
    {
        vertices.Add(vertex);
        if(useRenderDataForCol)
        {
            colVertices.Add(vertex);
        }
    }
    public void AddTriangle(int tri)
    {
        triangles.Add(tri);
        if(useRenderDataForCol)
        {
            //Firstly it just adds to the triangle list, then if useRenderDataForCol is true we add it to the colTriangles list
            //??but we need to adjust the value by the difference between the count of the vertices and colVertices lists
            //since the triangles list entries correspond to indexes in the vertices lists we have to adjust their values
            //to match the differences in the lists.
            colTriangles.Add(tri - (vertices.Count - colVertices.Count));
        }
    }
}
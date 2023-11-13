using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class ChunkRenderer : MonoBehaviour
{
    private MeshData _meshData;
    public void Render()
    {
        Mesh mesh = new Mesh();
        mesh.SetVertices(_meshData.Verticies);
        mesh.SetTriangles(_meshData.Triangles,0,false);
        mesh.SetUVs(0,_meshData.Uvs);

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        MeshCollider meshCollider = GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        
    }

    public void UpdateMeshData(MeshData meshData)
    {
        _meshData = meshData;
    }

}

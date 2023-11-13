using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    public int[] Triangles => _triangles.ToArray();
    public Vector3[] Verticies => _verticies.ToArray();
    public Vector2[] Uvs => _uvs.ToArray();

    private List<int>_triangles = new List<int>();
    private List<Vector3>_verticies = new List<Vector3>();
    private List<Vector2>_uvs = new List<Vector2>();

    private Dictionary<BlockType,Vector2Int> _blockTexturesInfo;
    public MeshData(ChunkData chunkData,Dictionary<BlockType,Vector2Int> blockTexturesInfo)
    {
        _blockTexturesInfo = blockTexturesInfo;
        for (int x = 0;x<chunkData.ChunkWidth;x++)
        {
            for (int y = 0;y<chunkData.ChunkHeight;y++)
            {
                for (int z = 0;z<chunkData.ChunkWidth;z++)
                {
                    Vector3Int position = new Vector3Int(x,y,z);
                    if (chunkData.GetBlockAtPosition(position)!=BlockType.Air)
                        GenerateBlock(position,1f, chunkData);
                }
            }
        }
    }


    private void GenerateBlock(Vector3Int blockPos,float blockSize, ChunkData chunkData)
    {
        if (chunkData.GetBlockAtPosition(blockPos+Vector3Int.right)==BlockType.Air) GererateRightSide(blockPos,blockSize,chunkData);
        if (chunkData.GetBlockAtPosition(blockPos+Vector3Int.left)==BlockType.Air)GererateLeftSide(blockPos,blockSize,chunkData);
        if (chunkData.GetBlockAtPosition(blockPos+Vector3Int.forward)==BlockType.Air)GererateFrontSide(blockPos,blockSize,chunkData);
        if (chunkData.GetBlockAtPosition(blockPos+Vector3Int.back)==BlockType.Air)GererateBackSide(blockPos,blockSize,chunkData);
        if (chunkData.GetBlockAtPosition(blockPos+Vector3Int.down)==BlockType.Air)GenerateBottomSide(blockPos,blockSize,chunkData);
        if (chunkData.GetBlockAtPosition(blockPos+Vector3Int.up)==BlockType.Air)GererateUpSide(blockPos,blockSize,chunkData);
        
    }

    private void GererateLeftSide(Vector3Int blockPos,float blockSize, ChunkData chunkData)
    {
        _verticies.Add(blockSize*(new Vector3(0,0,0)+blockPos));
        _verticies.Add(blockSize*(new Vector3(0,0,1)+blockPos));
        _verticies.Add(blockSize*(new Vector3(0,1,0)+blockPos));
        _verticies.Add(blockSize*(new Vector3(0,1,1)+blockPos));

        AddTriangles();
        AddUvs(blockPos,blockSize,chunkData);
    }
    private void GererateRightSide(Vector3Int blockPos,float blockSize,ChunkData chunkData)
    {
        _verticies.Add(blockSize*(new Vector3(1,0,0)+blockPos));
        _verticies.Add(blockSize*(new Vector3(1,1,0)+blockPos));
        _verticies.Add(blockSize*(new Vector3(1,0,1)+blockPos));
        _verticies.Add(blockSize*(new Vector3(1,1,1)+blockPos));

        AddTriangles();
        AddUvs(blockPos,blockSize,chunkData);
    }
    private void GererateFrontSide(Vector3Int blockPos,float blockSize,ChunkData chunkData)
    {
        _verticies.Add(blockSize*(new Vector3(0,0,1)+blockPos));
        _verticies.Add(blockSize*(new Vector3(1,0,1)+blockPos));
        _verticies.Add(blockSize*(new Vector3(0,1,1)+blockPos));
        _verticies.Add(blockSize*(new Vector3(1,1,1)+blockPos));

        AddTriangles();
        AddUvs(blockPos,blockSize,chunkData);
    }
    private void GererateBackSide(Vector3Int blockPos,float blockSize,ChunkData chunkData)
    {
        _verticies.Add(blockSize*(new Vector3(0,0,0)+blockPos));
        _verticies.Add(blockSize*(new Vector3(0,1,0)+blockPos));
        _verticies.Add(blockSize*(new Vector3(1,0,0)+blockPos));
        _verticies.Add(blockSize*(new Vector3(1,1,0)+blockPos));

        AddTriangles();
        AddUvs(blockPos,blockSize,chunkData);
    }
    private void GererateUpSide(Vector3Int blockPos,float blockSize,ChunkData chunkData)
    {
        _verticies.Add(blockSize*(new Vector3(0,1,0)+blockPos));
        _verticies.Add(blockSize*(new Vector3(0,1,1)+blockPos));
        _verticies.Add(blockSize*(new Vector3(1,1,0)+blockPos));
        _verticies.Add(blockSize*(new Vector3(1,1,1)+blockPos));

        AddTriangles();
        AddUvs(blockPos,blockSize,chunkData);
    }
    private void GenerateBottomSide(Vector3Int blockPos,float blockSize,ChunkData chunkData)
    {
        if (blockPos.y==0)return;
        _verticies.Add(blockSize*(new Vector3(0,0,0)+blockPos));
        _verticies.Add(blockSize*(new Vector3(1,0,0)+blockPos));
        _verticies.Add(blockSize*(new Vector3(0,0,1)+blockPos));
        _verticies.Add(blockSize*(new Vector3(1,0,1)+blockPos));

        AddTriangles();
        AddUvs(blockPos,blockSize,chunkData);
    }

    private void AddTriangles()
    {
        
        _triangles.Add(_verticies.Count-4);
        _triangles.Add(_verticies.Count-3);
        _triangles.Add(_verticies.Count-2);

        _triangles.Add(_verticies.Count-3);
        _triangles.Add(_verticies.Count-1);
        _triangles.Add(_verticies.Count-2);
    }


    private void AddUvs(Vector3Int blockPos,float blockSize, ChunkData chunkData)
    {
        Vector2 blockUvSize = new Vector2(16f/256f,16f/256f);

        Vector2Int blockId = new Vector2Int(0,3);

        BlockType block =  chunkData.GetBlockAtPosition(blockPos);

        if (block!=BlockType.Air&&_blockTexturesInfo!=null)
        {
            if (_blockTexturesInfo.TryGetValue(block, out Vector2Int newBlockId))
            {
                blockId = newBlockId;
            }
        }

        Vector2 offset = new Vector2((float)blockId.x*blockUvSize.x,1f-(float)(blockId.y+1)*blockUvSize.y);
        

        _uvs.Add(offset);
        _uvs.Add(offset);
        _uvs.Add(offset);
        _uvs.Add(offset);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChunkData
{
    private BlockType[] _blocks;
    private Vector2Int _chunkPosition;
    private readonly int _chunkHeight;
    private readonly int _chunkWidth;

    private readonly World _world;

    public event Action changed;

    public int ChunkHeight {get=>_chunkHeight;}
    public int ChunkWidth {get => _chunkWidth;}
    public Vector2Int ChunkPosition {get=> _chunkPosition;}
    public ChunkData(ITerrainGenerator terrainGenerator,Vector2Int chunkPosition, int chunkHeight,int chunkWidth, World world)
    {
        _blocks = terrainGenerator.GenerateAt(chunkPosition,chunkHeight,chunkWidth);
        _chunkPosition = chunkPosition;
        _chunkHeight = chunkHeight;
        _chunkWidth = chunkWidth;
        _world = world;
    }

    public BlockType GetBlockAtPosition(Vector3Int position)
    {
        if (position.y<0||position.y>=_chunkHeight)
            return BlockType.Air;
        Vector2Int offset = Vector2Int.zero;
        if (position.x<0)
            offset.x = -1;
        if (position.x>=_chunkWidth)
            offset.x = 1;
        if (position.z<0)
            offset.y = -1;
        if (position.z>=_chunkWidth)
            offset.y = 1;
        if (offset.magnitude!=0)
        {
            Vector3Int newPos = new Vector3Int(Mathf.Abs(_chunkWidth-1-Mathf.Abs(position.x)),position.y,Mathf.Abs(_chunkWidth-1-Mathf.Abs(position.z)));
            int index = _world.Vector3IntToIndex(newPos);
            return _world.GetBlockInChunk(_chunkPosition+offset,index);
        }
        return _blocks[_world.Vector3IntToIndex(position)];
    }

    public BlockType GetBlockAtPosition(int index)
    {
        if (index<0||index>=_chunkHeight*_chunkWidth*_chunkWidth)
            return BlockType.Air;
        return _blocks[index];
    }
    

    public bool ModifyBlock(int blockPosition,BlockType block)
    {
        if (GetBlockAtPosition(blockPosition)==block)
            return false;
        _blocks[blockPosition]=block;
        changed?.Invoke();
        return true;
    }



}

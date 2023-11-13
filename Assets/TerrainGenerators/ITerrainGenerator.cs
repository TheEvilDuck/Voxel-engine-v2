using UnityEngine;

public interface ITerrainGenerator
{
    public BlockType[] GenerateAt(Vector2Int chunkPosition,int chunkHeight,int chunkWidth);
}

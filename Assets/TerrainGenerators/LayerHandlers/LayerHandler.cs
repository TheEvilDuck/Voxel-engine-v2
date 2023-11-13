using UnityEngine;

public abstract class LayerHandler : ScriptableObject
{
    [SerializeField] LayerHandler _next;
    public bool Handle(BlockType[] blocks,Vector3Int blockPos,int maxHeight, Vector2Int chunkCoordinates,int chunkWidth, int chunkHeight)
    {
        if (TryHandle(blocks,blockPos,maxHeight,chunkCoordinates,chunkWidth,chunkHeight))
            return true;
        if (_next!=null)
            return _next.Handle(blocks,blockPos,maxHeight,chunkCoordinates,chunkWidth,chunkHeight);
        return false;
    }
    protected abstract bool TryHandle(BlockType[] blocks,Vector3Int blockPos,int maxHeight, Vector2Int chunkCoordinates,int chunkWidth,int chunkHeight);
}


using UnityEngine;

[CreateAssetMenu(fileName = "Surface layer handler", menuName = "Layer handlers/Surface layer handler")]
public class SurfaceLayerHandler : LayerHandler
{
    [SerializeField] BlockType _surfaceBlock;
    protected override bool TryHandle(BlockType[] blocks,Vector3Int blockPos,int maxHeight, Vector2Int chunkCoordinates,int chunkWidth, int chunkHeight)
    {
        if (blockPos.y==maxHeight-1)
        {
            int index = blockPos.x+blockPos.y*chunkWidth+blockPos.z*chunkWidth*chunkHeight;
            if (blocks[index]==BlockType.Air)
            {
                blocks[index] = _surfaceBlock;
                return true;
            }
        }
        return false;
    }
}

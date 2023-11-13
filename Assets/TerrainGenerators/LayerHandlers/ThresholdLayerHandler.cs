using UnityEngine;
[CreateAssetMenu(fileName = "Threshold layer handler", menuName = "Layer handlers/Threshold layer handler")]
public class ThresholdLayerHandler : LayerHandler
{
    [SerializeField][Range(0,1)]float _maxHeight;
    [SerializeField]BlockType _resulBlock;
    [SerializeField]bool _above;
    protected override bool TryHandle(BlockType[] blocks,Vector3Int blockPos,int maxHeight, Vector2Int chunkCoordinates,int chunkWidth, int chunkHeight)
    {
        float percent = (float)blockPos.y+1f/(float)maxHeight;
        int index = blockPos.x+blockPos.y*chunkWidth+blockPos.z*chunkWidth*chunkHeight;
        if (blocks[index]==BlockType.Air
        &&(_above&&percent>=_maxHeight||!_above&&percent<=_maxHeight))
        {
            blocks[index] = _resulBlock;
            return true;
        }
        return false;
    }
}

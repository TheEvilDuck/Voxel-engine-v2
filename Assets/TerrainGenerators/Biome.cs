using UnityEngine;



[CreateAssetMenu(menuName ="Biomes/Create a biome")]
public class Biome : ScriptableObject
{
    [SerializeField]Noise _noise;
    [SerializeField]LayerHandler _startLayerHandler;
    [SerializeField]BlockType _defaultBlock;

    public BlockType[] GenerateBlocks(BlockType[] chunk,Vector2Int column,Vector2Int chunkCoordinates, int chunkHeight, int chunkWidth)
    {
        float noiseValue = _noise.GetNoiseValue(column.x+chunkCoordinates.x*chunkWidth,column.y+chunkCoordinates.y*chunkWidth);
        int height = Mathf.FloorToInt((float)chunkHeight*noiseValue);
        if (height<=0)
            height = 1;
        if (height>chunkHeight)
            height = chunkHeight;
        for (int y = 0;y<height;y++)
        {
            if (!_startLayerHandler.Handle(chunk,new Vector3Int(column.x,y,column.y),height,chunkCoordinates,chunkWidth,chunkHeight))
            {
                int index = column.x+y*chunkWidth+column.y*chunkWidth*chunkHeight;
                if (chunk[index]==BlockType.Air)
                    chunk[index] = _defaultBlock;
            }
        }
        return chunk;
    }
    
}

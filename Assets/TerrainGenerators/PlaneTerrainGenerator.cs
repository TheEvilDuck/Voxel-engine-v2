using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneTerrainGenerator : ITerrainGenerator
{
    private int _height;
    public PlaneTerrainGenerator(int height)
    {
        _height = height;
    }
    public BlockType[] GenerateAt(Vector2Int chunkPosition,int chunkHeight,int chunkWidth)
    {
        BlockType[]result = new BlockType[chunkHeight*chunkWidth*chunkWidth];

        for (int x = 0;x<chunkWidth;x++)
        {
            for (int z = 0;z<chunkWidth;z++)
            {
                for (int y = 0;y<chunkHeight;y++)
                {
                    if (y<=_height)
                        result[x+y*chunkWidth+z*chunkHeight*chunkWidth] = BlockType.Stone;
                }
            }
        }

        return result;
    }
}

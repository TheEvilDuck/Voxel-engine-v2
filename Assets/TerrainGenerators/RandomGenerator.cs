using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class RandomGenerator : ITerrainGenerator
{
    private float _min;
    private float _max;
    public RandomGenerator(float min, float max)
    {
        _min = min;
        _max = max;
    }
    public BlockType[] GenerateAt(Vector2Int chunkPosition, int chunkHeight, int chunkWidth)
    {
        BlockType[] result = new BlockType[chunkWidth*chunkWidth*chunkHeight];
        for (int x = 0;x<chunkWidth;x++)
        {
            for (int z = 0;z<chunkWidth;z++)
            {
                int min = Mathf.FloorToInt(chunkHeight*_min);
                int max = Mathf.FloorToInt(chunkHeight*_max);
                int height = new System.Random().Next(min,max);
                for (int y = 0;y<height;y++)
                {
                    result[x+y*chunkWidth+z*chunkHeight*chunkWidth] = BlockType.Stone;
                }
            }
        }
        return result;
    }
}

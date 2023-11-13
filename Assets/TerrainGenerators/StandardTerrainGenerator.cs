using UnityEngine;
using System.Collections.Generic;

public class StandardTerrainGenerator : ITerrainGenerator
{
    private TerrainGeneratorSettings _settings;
    public StandardTerrainGenerator(TerrainGeneratorSettings settings)
    {
       _settings = settings;
    }
    public BlockType[] GenerateAt(Vector2Int chunkPosition, int chunkHeight, int chunkWidth)
    {
        BlockType[] result = new BlockType[chunkWidth*chunkWidth*chunkHeight];
        for (int x = 0;x<chunkWidth;x++)
        {
            for (int z = 0;z<chunkWidth;z++)
            {
                Dictionary<Biome,float>biomeValues = new Dictionary<Biome, float>();
                foreach (Biome biome in _settings.Biomes)
                {
                    biomeValues.TryAdd(biome,1f);
                }
                foreach (NoiseMap map in _settings.Maps)
                {
                    float noiseValue = map._map.GetNoiseValue(x+chunkPosition.x*chunkWidth,z+chunkPosition.y*chunkWidth);
                    foreach (Affect affect in map._affects)
                    {
                        float decreaseAffect = 1f-affect._value;
                        float increaseAffect = 1f+affect._value;

                        float decreaseValue = (1f-noiseValue)*decreaseAffect;
                        float increaseValue = noiseValue*increaseAffect;

                        biomeValues[affect._biome]+= (decreaseValue+increaseValue)*affect._multiplier;
                    }
                }
                float maxValue = biomeValues.GetValueOrDefault(_settings.Biomes[0]);
                Biome maxBiome = _settings.Biomes[0];
                foreach (KeyValuePair<Biome,float>pair in biomeValues)
                {
                    if (pair.Value>maxValue)
                    {
                        maxValue=pair.Value;
                        maxBiome = pair.Key;
                    }
                }
                int index = x+0*chunkWidth+z*chunkWidth*chunkHeight;
                //result[index] = BlockType.Bedrock;
                result = maxBiome.GenerateBlocks(result,new Vector2Int(x,z),chunkPosition,chunkHeight,chunkWidth);
            }
        }
        return result;
    }
}

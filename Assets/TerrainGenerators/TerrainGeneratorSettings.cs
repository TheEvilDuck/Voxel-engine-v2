using UnityEngine;

[CreateAssetMenu(menuName ="Terrain generator/Create a generator settings")]
public class TerrainGeneratorSettings : ScriptableObject
{
    [SerializeField]Biome[] _biomes;
    [SerializeField]NoiseMap[] _maps;
    [SerializeField]int _seed = 0;

    public Biome[] Biomes {get =>_biomes;}
    public NoiseMap[] Maps {get => _maps;}
    public int Seed {get => _seed;}
}

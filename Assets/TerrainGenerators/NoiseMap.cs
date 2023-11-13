using UnityEngine;
[System.Serializable]
public struct Affect
{
    [SerializeField]public Biome _biome;
    [SerializeField][Range(-1,1)]public float _value;
    [SerializeField][Range(-100,100)]public float _multiplier;
}
[System.Serializable]
public class NoiseMap
{
    [SerializeField]public string _name;
    [SerializeField]public Noise _map;
    [SerializeField]public Affect[] _affects;

}

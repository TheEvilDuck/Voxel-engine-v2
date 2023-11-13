using UnityEngine;

[System.Serializable]
public class OctaveSettings
{
    public float frequency;
    public float height;
}
[System.Serializable]
public struct Noise
{
    public OctaveSettings[] _octaves;
    [Range(0.01f,10f)]public float _redistribution;
    public float GetNoiseValue(int xOffset,int yOffset)
    {
        float noiseValue = 1f;
        float amplitudeSum = 0;
        for (int i = 0;i<_octaves.GetLength(0);i++)
        {
            float xNoise = xOffset*_octaves[i].frequency;
            float yNoise = yOffset*_octaves[i].frequency;
            float calculatedNoise = Mathf.PerlinNoise(xNoise*_octaves[i].height,yNoise*_octaves[i].height)/_octaves[i].height;
            amplitudeSum+=1f/_octaves[i].height;
            noiseValue+=calculatedNoise;
        }
        float result = Mathf.Pow(noiseValue/amplitudeSum,_redistribution);
        return result;
    }
}

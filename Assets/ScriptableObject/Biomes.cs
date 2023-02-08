using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/New biome")]
public class Biomes : ScriptableObject
{
    public string BiomeName;

    [Header("Generator biomes variables")]
    public float Frequency;
    public float Amplitude;
    [Range(0, 8)]
    public int Octaves;
    [Range(2f, 3f)]
    public float Lacunarity;
    [Range(0.1f, .9f)]
    public float Persistence;
    public GameObject[] m_BlockList;
    public float[] m_BlocksThreshold;

    [Header("Extra information")]
    public bool PutObjects;
    public float ObjectsFrequency;
    public int ObjectsDensity;
    public GameObject[] m_Objects;
    public float[] m_ObjectsThreshold;

}
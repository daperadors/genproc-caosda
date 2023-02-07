using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/New biome")]
public class Biomes : ScriptableObject
{
    public string BiomeName;

    [Header("Generator biomes variables")]
    public float Frequency;
    public float Amplitude;
    public int Octaves;
    public float Lacunarity;
    public float Persistence;
    public GameObject[] m_BlockList;
    public float[] m_BlocksThreshold;

    [Header("Extra information")]
    public bool PutObjects;
    public float ObjectsFrequency;
    public GameObject[] m_Objects;
    public float[] m_ObjectsThreshold;

}
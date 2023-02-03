using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/New biome")]
public class Biomes : ScriptableObject
{
    public string BiomeName;

    [Header("Generator biomes variables")]
    public int Octaves;
    public float Frequency;
    public float Lacunarity;

    [Header("Extra information")]
    public bool PutTrees;
    public GameObject[] m_BlockList;
    public float[] m_BlocksThreshold;
}
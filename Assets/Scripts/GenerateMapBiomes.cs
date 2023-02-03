using UnityEngine;

public class GenerateMapBiomes : MonoBehaviour
{
    [Header("Biomes")]
    [SerializeField] private Biomes[] m_BiomesScriptable;
    [SerializeField] private float[] m_BiomesThreshold;

    [Header("Map info")]
    [SerializeField] private int m_Width = 50;
    [SerializeField] private int m_Height = 50;
    [SerializeField] private int m_Seed = 53467;
    [SerializeField] private float m_Scale = 35f;
    [SerializeField] private float m_Frequency = 20f;
    [SerializeField] private float m_Amplitud = 20f;
    private Vector3 m_Position;
    private int[,] m_Terrain;

    [Header("Tree info")]
    [SerializeField] private GameObject[] m_TreeList;
    [SerializeField] private float m_TreeFrequency = 10f;
    [Range(0f, 1f)]
    [SerializeField] private float m_TreeThreshold = 0.5f;
    [SerializeField, Range(10f, 0f)] private int m_TreeDensity;

    void Start()
    {
        m_Position = transform.position;
        GenerateProceduralMap();
    }

    private void GenerateProceduralMap()
    {
        m_Terrain = new int[m_Width, m_Height];
        for (int x = 0; x < m_Width; x++)
        {
            for (int y = 0; y < m_Height; y++)
            {
                int perlin = (int)Mathf.Floor(CalculatePerlinNoise(x, y, m_Scale, m_Frequency) * m_Amplitud);

                for (int z = 0; z < m_BiomesThreshold.Length; z++)
                {
                    if (CalculatePerlinNoise(x, y, m_Scale, m_Frequency) >= m_BiomesThreshold[z])
                    {
                        switch (m_BiomesThreshold[z])
                        {
                            case 0.2f:
                                GenerateBiome(0, x, y);
                                break;
                            case 0.4f:
                                GenerateBiome(1, x, y);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }
    private void GenerateBiome(int index, int x, int y)
    {
        int newPerlin = (int)Mathf.Floor(CalculatePerlinNoise(x, y, m_Scale, m_BiomesScriptable[index].Frequency) * m_Amplitud);

        float valorPerlin = y / m_Amplitud;

        int indiceBloque = 0;
        for (int j = 0; j < m_BiomesScriptable[index].m_BlockList.Length; j++)
        {
            if (valorPerlin <= m_BiomesScriptable[index].m_BlocksThreshold[j])
            {
                indiceBloque = j;
                break;
            }
        }

        GameObject block = Instantiate(m_BiomesScriptable[index].m_BlockList[indiceBloque], transform);
        block.transform.position = new Vector3(m_Position.x + x, newPerlin, m_Position.z + y);
        m_Terrain[x, y] = newPerlin;
    }
    private float CalculatePerlinNoise(int x, int y, float scale, float frequency)
    {
        return Mathf.PerlinNoise(m_Seed + (x / scale) * frequency, m_Seed + (y / scale) * frequency);
    }
}

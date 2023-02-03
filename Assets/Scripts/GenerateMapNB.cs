using UnityEngine;

public class GenerateMapNB : MonoBehaviour
{
    [Header("Map var")]
    [SerializeField] private GameObject[] m_BlockList;
    [SerializeField] private float[] m_BlockListThreshold;
    [SerializeField] private int m_Seed = 98742364;
    [SerializeField] private float m_Scale = 35f;
    [SerializeField] private float m_Frequency = 20f;
    [SerializeField] private float m_Amplitud = 20f;

    [SerializeField] private int m_SizeX = 50;
    [SerializeField] private int m_SizeZ = 50;

    private GameObject m_ActualBlock;
    private Vector3 m_Position;

    [Header("Trees var")]
    [SerializeField] private GameObject[] m_TreeList;

    [Header("Biome var")]
    [SerializeField] private float m_TreeFrequency = 10f;
    [Range(0f, 1f)]
    [SerializeField] private float m_TreeThreshold = 0.5f;
    [SerializeField, Range(10f, 0f)] private int m_TreeDensity;

    private int[,] m_Terrain;

    private void Start()
    {
        m_Position = transform.position;
        m_ActualBlock = m_BlockList[2];
        GenerateProceduralMapNoBiomas();
        GenerateTrees();
    }
    private void GenerateProceduralMapNoBiomas()
    {
        m_Terrain = new int[m_SizeX, m_SizeZ];

        for (int x = 0; x < m_SizeX; x++)
        {
            for (int z = 0; z < m_SizeZ; z++)
            {
                int y = (int)Mathf.Floor(CalculatePerlinNoise(x, z, m_Scale, m_Frequency) * m_Amplitud);

                float valorPerlin = y / m_Amplitud;

                int indiceBloque = 0;
                for (int j = 0; j < m_BlockListThreshold.Length; j++)
                {
                    if (valorPerlin <= m_BlockListThreshold[j])
                    {
                        indiceBloque = j;
                        break;
                    }
                }

                GameObject block = Instantiate(m_BlockList[indiceBloque], transform);
                block.transform.position = new Vector3(m_Position.x + x, y, m_Position.z + z);
                m_Terrain[x, z] = y;
            }
        }
    }
    private void GenerateTrees()
    {
        for (int x = 0; x < m_SizeX; x++)
        {
            for (int y = 0; y < m_SizeZ; y++)
            {

                float perlin = CalculatePerlinNoise(x, y, m_Scale, m_TreeFrequency);

                if (perlin >= m_TreeThreshold)
                {
                    if (Random.Range(0, m_TreeDensity) == 1)
                    {
                        GameObject tree = Instantiate(m_TreeList[0], transform);
                        tree.transform.position = new Vector3(x, m_Terrain[x, y] + 1, y);
                    }
                }

            }
        }
    }
    private float CalculatePerlinNoise(int x, int z, float scale, float frequency)
    {
        return Mathf.PerlinNoise(m_Seed + (x / scale) * frequency, m_Seed + (z / scale) * frequency);
    }

}


using UnityEngine;

public class GenerateMap : MonoBehaviour
{
    [Header("Map var")]
    [SerializeField] private GameObject[] m_BlockListForest;
    [SerializeField] private GameObject[] m_BlockListDesert;
    [SerializeField] private GameObject[] m_BlockListSnow;
    [SerializeField] private GameObject[] m_BlockListWetDirt;
    [SerializeField] private float[] m_BlockListThresholdForest;
    [SerializeField] private float[] m_BlockListThresholdDesert;
    [SerializeField] private float[] m_BlockListThresholdSnow;
    [SerializeField] private float[] m_BlockListThresholdWetDirt;
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
        m_ActualBlock = m_BlockListForest[2];
        GenerateProceduralMap();
        //GenerateTrees();
    }

    private void GenerateProceduralMap()
    {
        m_Terrain = new int[m_SizeX, m_SizeZ];

        for (int x = 0; x < m_SizeX; x++)
        {
            for (int z = 0; z < m_SizeZ; z++)
            {
                if (x <= m_SizeX / 4)
                {
                    GenerateBiomeForest(x, z);
                }
                else if (x <= (m_SizeX / 4) * 2 && x > m_SizeX / 4)
                {
                    GenerateBiomeDesert(x, z);
                }
                else if (x <= (m_SizeX / 4) * 3 && x > (m_SizeX / 4) * 2)
                {
                    GenerateBiomeSnow(x, z);
                }
                else if (x <= m_SizeX && x > (m_SizeX / 4) * 3)
                {
                    GenerateBiomeWetDirt(x, z);
                }

            }
        }
    }
    private void GenerateBiomeForest(int x, int z)
    {
        int y = (int)Mathf.Floor(CalculatePerlinNoise(x, z, m_Scale, m_Frequency) * m_Amplitud);

        float valorPerlin = y / m_Amplitud;

        int indiceBloque = 0;
        for (int j = 0; j < m_BlockListThresholdForest.Length; j++)
        {
            if (valorPerlin <= m_BlockListThresholdForest[j])
            {
                indiceBloque = j;
                break;
            }
        }

        GameObject block = Instantiate(m_BlockListForest[indiceBloque], transform);
        block.transform.position = new Vector3(m_Position.x + x, y, m_Position.z + z);
        m_Terrain[x, z] = y;
        GenerateTrees(x, z);
    }
    private void GenerateBiomeDesert(int x, int z)
    {
        int y = (int)Mathf.Floor(CalculatePerlinNoise(x, z, m_Scale, m_Frequency) * m_Amplitud);

        float valorPerlin = y / m_Amplitud;

        int indiceBloque = 0;
        for (int j = 0; j < m_BlockListThresholdDesert.Length; j++)
        {
            if (valorPerlin <= m_BlockListThresholdDesert[j])
            {
                indiceBloque = j;
                break;
            }
        }

        GameObject block = Instantiate(m_BlockListDesert[indiceBloque], transform);
        block.transform.position = new Vector3(m_Position.x + x, y, m_Position.z + z);
        m_Terrain[x, z] = y;
    }
    private void GenerateBiomeSnow(int x, int z)
    {
        int y = (int)Mathf.Floor(CalculatePerlinNoise(x, z, m_Scale, m_Frequency) * m_Amplitud);

        float valorPerlin = y / m_Amplitud;

        int indiceBloque = 0;
        for (int j = 0; j < m_BlockListThresholdSnow.Length; j++)
        {
            if (valorPerlin <= m_BlockListThresholdSnow[j])
            {
                indiceBloque = j;
                break;
            }
        }

        GameObject block = Instantiate(m_BlockListSnow[indiceBloque], transform);
        block.transform.position = new Vector3(m_Position.x + x, y, m_Position.z + z);
        m_Terrain[x, z] = y;
    }
    private void GenerateBiomeWetDirt(int x, int z)
    {
        int y = (int)Mathf.Floor(CalculatePerlinNoise(x, z, m_Scale, m_Frequency) * m_Amplitud);

        float valorPerlin = y / m_Amplitud;

        int indiceBloque = 0;
        for (int j = 0; j < m_BlockListThresholdWetDirt.Length; j++)
        {
            if (valorPerlin <= m_BlockListThresholdWetDirt[j])
            {
                indiceBloque = j;
                break;
            }
        }

        GameObject block = Instantiate(m_BlockListWetDirt[indiceBloque], transform);
        block.transform.position = new Vector3(m_Position.x + x, y, m_Position.z + z);
        m_Terrain[x, z] = y;
        GenerateTrees(x, z);
    }
    private void GenerateTrees(int x, int y)
    {
        //for (int x = 0; x < m_SizeX; x++)
        {
            //for (int y = 0; y < m_SizeZ; y++)
            {

                float perlin = CalculatePerlinNoise(x, y, m_Scale, m_TreeFrequency);

                //if (x >= (m_SizeX / 4) * 3 && x < (m_SizeX / 4) * 2)
                {
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
    }
    private float CalculatePerlinNoise(int x, int z, float scale, float frequency)
    {
        return Mathf.PerlinNoise(m_Seed + (x / scale) * frequency, m_Seed + (z / scale) * frequency);
    }

}


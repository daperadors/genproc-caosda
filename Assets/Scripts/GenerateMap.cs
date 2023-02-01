using UnityEngine;

public class GenerateMap : MonoBehaviour
{
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

    private void Start()
    {
        m_Position = transform.position;
        m_ActualBlock = m_BlockList[2];
        GenerateProceduralMap();
    }

    private void GenerateProceduralMap()
    {
        for (int x = 0; x < m_SizeX; x++)
        {
            for (int z = 0; z < m_SizeZ; z++)
            {

                int y = (int)Mathf.Floor(CalculatePerlinNoise(x, z) * m_Amplitud);

                /*
                for (int i = 0; i <= y; i++)
                {
                    //buscar el bloque
                    float valorPerlin = i / m_Amplitud;

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
                    block.transform.position = new Vector3(m_Position.x + x, i, m_Position.z + z);
                }
                */
                
                    //buscar el bloque
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
            }
        }
    }
    private float CalculatePerlinNoise(int x, int z)
    {
        return Mathf.PerlinNoise(m_Seed + (x / m_Scale) * m_Frequency, m_Seed + (z / m_Scale) * m_Frequency);
    }

}


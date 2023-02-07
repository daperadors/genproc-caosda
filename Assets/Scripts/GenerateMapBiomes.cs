using UnityEditor.PackageManager.UI;
using UnityEngine;

public class GenerateMapBiomes : MonoBehaviour
{

    [SerializeField] private int m_Width = 50;
    [SerializeField] private int m_Height = 50;
    [SerializeField] private int m_Seed = 53467;
    [SerializeField] private float m_Scale = 35f;

    [Header("Biomes")]
    [SerializeField] private float m_BiomeFrequency = 20f;
    [SerializeField] private Biomes[] m_BiomesScriptable;
    [SerializeField] private float[] m_BiomesThreshold;

    [Header("Map info")]
    [SerializeField] private float m_Frequency = 20f;
    [SerializeField] private float m_Amplitud = 20f;
    private Vector3 m_Position;
    private int[,] m_Terrain;
    private float[,] m_Biome;

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
        m_Biome = new float[m_Width, m_Height];
        for (int x = 0; x < m_Width; x++)
        {
            for (int y = 0; y < m_Height; y++)
            {
                m_Biome[x,y] = CalculatePerlinNoise(x, y, m_Scale, m_BiomeFrequency);
                Biomes biomaActual = GetBiome(x, y);
                
                int alturaTerreno = (int)Mathf.Floor(CalculatePerlinNoise(x, y, m_Scale, biomaActual.Frequency, biomaActual.Amplitude, biomaActual.Octaves, biomaActual.Lacunarity, biomaActual.Persistence));
                //pintar bloque
                GameObject block = Instantiate(GetBloqueBioma(biomaActual, alturaTerreno/ biomaActual.Amplitude), transform);
                GameObject block2 = Instantiate(GetBloqueBioma(biomaActual, (alturaTerreno -1)/ biomaActual.Amplitude), transform);

                block.transform.position = new Vector3(m_Position.x + x, alturaTerreno, m_Position.y + y);
                block2.transform.position = new Vector3(m_Position.x + x, alturaTerreno - 1, m_Position.y + y);

                if(biomaActual.PutObjects)
                {
                    //ruido objeto
                    //CalculatePerlinNoise()

                }
            }
        }
    }
    private Biomes GetBiome(int x, int y)
    {
        float biomeValue = m_Biome[x, y];

        int indiceBioma = 0;
        for (int i = 0; i < m_BiomesThreshold.Length; i++)
        {
            if (biomeValue <= m_BiomesThreshold[i])
            {
                indiceBioma = i;
                break;
            }
        }

        return m_BiomesScriptable[indiceBioma];
    }

    //revisar
    private GameObject GetBloqueBioma(Biomes bioma, float valorPerlin)
    {

        int indiceBloque = 0;
        for (int i = 0; i < bioma.m_BlocksThreshold.Length; i++)
        {
            if (valorPerlin <= bioma.m_BlocksThreshold[i])
            {
                indiceBloque = i;
                break;
            }
        }

        return bioma.m_BlockList[indiceBloque];
    }

    private float CalculatePerlinNoise(int x, int y, float scale, float frequency, float amplitude = 1, int octaves = 0, float lacunarity = 0f, float persistence = 0f)
    {
        float perlinValue = Mathf.PerlinNoise(m_Seed + (x / scale) * frequency, m_Seed + (y / scale) * frequency);

        for (int octave = 1; octave <= octaves; octave++)
        {
            float newFreq = frequency * lacunarity * octave;
            float xOctaveCoord = m_Seed + (x / m_Scale) * newFreq;
            float yOctaveCoord = m_Seed + (y / m_Scale) * newFreq;

            float octaveSample = Mathf.PerlinNoise(xOctaveCoord, yOctaveCoord);
            
            octaveSample = (octaveSample - .5f ) * (persistence / octave);
            perlinValue += octaveSample;
        }
        return perlinValue * amplitude;
    }
}

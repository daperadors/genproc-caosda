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
                m_Biome[x, y] = CalculatePerlinNoise(x, y, m_Scale, m_BiomeFrequency);
                Biomes biomaActual = GetByProbability<Biomes>(m_Biome[x, y], m_BiomesThreshold, m_BiomesScriptable);
                int alturaTerreno = (int)Mathf.Floor(CalculatePerlinNoise(x, y, m_Scale, biomaActual.Frequency, biomaActual.Amplitude, biomaActual.Octaves, biomaActual.Lacunarity, biomaActual.Persistence));

                //pintar bloque
                GameObject block = Instantiate(GetByProbability<GameObject>(alturaTerreno / biomaActual.Amplitude, biomaActual.m_BlocksThreshold, biomaActual.m_BlockList), transform);
                GameObject block2 = Instantiate(GetByProbability<GameObject>((alturaTerreno - 1) / biomaActual.Amplitude, biomaActual.m_BlocksThreshold, biomaActual.m_BlockList), transform);

                block.transform.position = new Vector3(m_Position.x + x, alturaTerreno, m_Position.y + y);
                block2.transform.position = new Vector3(m_Position.x + x, alturaTerreno - 1, m_Position.y + y);

                if (biomaActual.PutObjects)
                {
                    //ruido objeto
                    float perlinObject = CalculatePerlinNoise(x, y, m_Scale, biomaActual.ObjectsFrequency);
                    GenerateObject(perlinObject, biomaActual);

                }
            }
        }
    }
    private void GenerateObject(float perlin, Biomes biome)
    {
        GameObject block = Instantiate(GetByProbability<GameObject>(perlin, biome.m_ObjectsThreshold, biome.m_Objects), transform);
        print(block);
    }
    private T GetByProbability<T>(float valorPerlin, float[] arrayThreshold, T[] genericArray)
    {

        int indiceBloque = 0;
        for (int i = 0; i < arrayThreshold.Length; i++)
        {
            if (valorPerlin <= arrayThreshold[i])
            {
                indiceBloque = i;
                break;
            }
        }

        return genericArray[indiceBloque];
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

            octaveSample = (octaveSample - .5f) * (persistence / octave);
            perlinValue += octaveSample;
        }
        return perlinValue * amplitude;
    }
}

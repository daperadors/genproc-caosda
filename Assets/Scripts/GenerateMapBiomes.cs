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

    [Header("Minerals info")]
    [SerializeField] private GameObject[] m_MineralList;

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
                GameObject block = Instantiate(GetByProbability<GameObject>(alturaTerreno / biomaActual.Amplitude, biomaActual.m_BlocksThreshold, biomaActual.m_BlockList, m_MineralList, biomaActual.BiomeName == "FOREST" ? true : false), transform);
                GameObject block2 = Instantiate(GetByProbability<GameObject>((alturaTerreno - 1) / biomaActual.Amplitude, biomaActual.m_BlocksThreshold, biomaActual.m_BlockList, m_MineralList, biomaActual.BiomeName == "FOREST" ? true : false), transform);

                block.transform.position = new Vector3(m_Position.x + x, alturaTerreno, m_Position.y + y);
                block2.transform.position = new Vector3(m_Position.x + x, alturaTerreno - 1, m_Position.y + y);

                m_Terrain[x, y] = alturaTerreno;
                if (biomaActual.PutObjects)
                {
                    //ruido objeto
                    GenerateObject(biomaActual, x, y);

                }
            }
        }
    }
    private void GenerateObject(Biomes biome, int x, int y)
    {
        float perlin = CalculatePerlinNoise(x, y, m_Scale, biome.ObjectsFrequency);
        int random = Random.Range(0, biome.m_Objects.Length);
       // print(biome.m_ObjectsThreshold[random] + " "+ perlin);
        //if (perlin >= biome.m_ObjectsThreshold[random])
        {
            if (Random.Range(0, biome.ObjectsDensity) == 1)
            {

                if (m_Terrain[x, y] > biome.m_ObjectsThreshold[random] * 10)
                {
                    print(random +" "+ biome.m_Objects.Length);
                    GameObject obj = Instantiate(biome.m_Objects[random], transform);
                    obj.transform.position = new Vector3(x, (m_Terrain[x, y] + 1)+obj.transform.position.y, y);
                }
            }
        }



    }
    private T GetByProbability<T>(float valorPerlin, float[] arrayThreshold, T[] genericArray, T[] genericArrayObjects = null, bool minerals = false)
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
        if (minerals)
        {
            if (indiceBloque == 0)
            {
                float rand = Random.Range(0, 20);
                if (rand == 1)
                {
                    return genericArrayObjects[0];
                }
                else if (rand >= 5 && rand <= 8)
                {
                    return genericArrayObjects[1];
                }
                else if (rand == 9 || rand == 12)
                {
                    return genericArrayObjects[2];
                }
                else
                {
                    return genericArray[indiceBloque];
                }
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

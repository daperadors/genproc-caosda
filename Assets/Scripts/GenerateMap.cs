using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
    [SerializeField]
    //offset from the perlin map
    private float m_OffsetX;
    [SerializeField]
    private float m_OffsetY;


    //size of the area we will paint
    [SerializeField]
    private int m_Width;
    [SerializeField]
    private int m_Height;

    [Header("Biomes")]
    //Scale
    [SerializeField]
    private float m_BiomesScale = 1000f;
    //Scale
    [SerializeField]
    private float m_BiomesFrequency = 10f;

    [SerializeField]
    [Range(0f, 1f)]
    private float m_BiomaThreshold = 0.5f;

    [Header("Aparicio Arbres")]

    //Scale
    [SerializeField]
    private float m_Scale = 1000f;
    //Scale
    [SerializeField]
    private float m_Frequency = 10f;

    [SerializeField]
    [Range(0f, 1f)]
    private float m_ObjecteThreshold = 0.9f;

    //graphic
    [SerializeField]
    private GameObject[] m_Objectes;
    private List<GameObject> m_LlistaObjectes;

    void Start()
    {
        m_LlistaObjectes = new List<GameObject>();
    }

    void Update()
    {
        //regenerar perlins
        if (Input.GetKeyDown(KeyCode.P))
        {
            GenerarArbres();
        }
    }

    private void GenerarArbres()
    {
        ClearArbres();

        Debug.Log("Calculant Perlin Noise");
        for (int y = 0; y < m_Height; y++)
        {
            for (int x = 0; x < m_Width; x++)
            {
                float sample = CalculatePerlinNoise(x, y, m_Scale, m_Frequency);

                if (sample >= m_ObjecteThreshold)
                {
                    int indexObjecte = 0;
                    if (CalculatePerlinNoise(x, y, m_BiomesScale, m_BiomesFrequency) >= m_BiomaThreshold)
                        indexObjecte = 1;

                    GameObject arbre = Instantiate(m_Objectes[indexObjecte], transform);
                    arbre.transform.position = new Vector3(x, (x+y), y);
                    m_LlistaObjectes.Add(arbre);
                }
            }
        }
    }

    private float CalculatePerlinNoise(int x, int y, float scale, float frequency)
    {
        float yCoord = m_OffsetY + (y / scale) * frequency;
        float xCoord = m_OffsetX + (x / scale) * frequency;
        return Mathf.PerlinNoise(xCoord, yCoord);
    }

    private void ClearArbres()
    {
        while (m_LlistaObjectes.Count > 0)
        {
            Destroy(m_LlistaObjectes[0]);
            m_LlistaObjectes.RemoveAt(0);
        }
    }


}


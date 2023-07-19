using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    [Header("References")]
    private Mesh mesh;

    [Header("Attributes")]
    [SerializeField] private bool discreteMesh;
    [SerializeField] private float radius;
    [SerializeField] private float spacing;
    [SerializeField] private float intensity;
    [SerializeField] private float coastIntensity;
    [SerializeField] private float zoom;
    [SerializeField] private float xPosition;
    [SerializeField] private float zPosition;

    [Header("Terrain Data")]
    [SerializeField] private TerrainData[] terrainData;

    [Header("Data")]
    private Vector3[] vertices;
    private List<int>[] triangles;

    [Header("References")]
    private new MeshRenderer renderer;

    void Start()
    {
        mesh = new Mesh();
        renderer = GetComponent<MeshRenderer>();
        GetComponent<MeshFilter>().mesh = mesh;

        Debug.Log(terrainData.Length);
        triangles = new List<int>[terrainData.Length];
        Debug.Log(triangles.Length);

        for (int i = 0; i < triangles.Length; i++)
            triangles[i] = new List<int>();

        SortData(terrainData);
        
        Material[] materials = new Material[terrainData.Length];

        for (int i = 0; i < terrainData.Length; i++)
            materials[i] = terrainData[i].material;

        renderer.materials = materials;

        CreateShape();
        UpdateMesh();
    }

    void Update()
    {
    }

    private void CreateShape()
    {
        float circumference = 2f * Mathf.PI * radius;
        int circumferencePoints = Mathf.CeilToInt((circumference / spacing));

        List<Vector3> p = new List<Vector3>();

        for (int i = 0; i < circumferencePoints; i++)
        {
            float angle = Mathf.PI * 2f * i / (circumferencePoints - 1);

            float x = Mathf.Sin(angle) * radius;
            float z = Mathf.Cos(angle) * radius;
            float y = Mathf.PerlinNoise(x * zoom + xPosition, z * zoom + zPosition) * intensity;

            y -= (y * intensity);

            if (y < -.4f)
                y += .4f;

            p.Add(new Vector3(x, y, z));
        }

        for (int i = 1; i < radius; i++)
        {
            for (int j = 0; j < circumferencePoints; j++)
            {
                int index = j + (int)circumferencePoints * i;
                Vector3 dir = -p[j].normalized;
                dir.y = 0;

                Vector3 point = p[j] + (dir * i);

                if (i == 1)
                {
                    float noise = Mathf.PerlinNoise(point.x * zoom + xPosition, point.z * zoom + zPosition) * intensity;

                    if (point.x < 0)
                        point.x -= noise;
                    else
                        point.x += noise;

                    if (point.z < 0)
                        point.z -= noise;
                    else
                        point.z += noise;
                }

                point.y = Mathf.PerlinNoise(point.x * zoom + xPosition, point.z * zoom + zPosition) * intensity;

                p.Add(point);
            }
        }

        for (int i = 0; i < circumferencePoints; i++)
        {
            Vector3 point = p[i];

            float noise = Mathf.PerlinNoise(point.x * zoom + xPosition, point.z * zoom + zPosition) * coastIntensity;

            if (point.x < 0)
                point.x -= noise;
            else
                point.x += noise;

            if (point.z < 0)
                point.z -= noise;
            else
                point.z += noise;

            p[i] = point;
        }

        vertices = p.ToArray();

        //triangles = new List<int>[vertices.Length];

        for (int i = 0; i < triangles.Length; i++)
            triangles[i] = new List<int>();
        
        for (int j = 0; j < vertices.Length - 1; j++)
        {
            for (int i = 0; i < terrainData.Length; i++)
            {
                if (vertices[j].y <= terrainData[i].height)
                {
                    Debug.Log(vertices[i].y);
                    if ((int)circumferencePoints + j + 1 < vertices.Length)
                    {
                        triangles[i].Add(j);
                        triangles[i].Add(j + 1);
                        triangles[i].Add((int)circumferencePoints + j);
                        triangles[i].Add((int)circumferencePoints + j);
                        triangles[i].Add(j + 1);
                        triangles[i].Add((int)circumferencePoints + j + 1);
                    }
                    else
                    {
                        triangles[i].Add(j);
                        triangles[i].Add(j + 1);
                        triangles[i].Add(vertices.Length - 1);
                        triangles[i].Add(vertices.Length - 1);
                        triangles[i].Add(j + 1);
                        triangles[i].Add(vertices.Length - 1);
                    }
                    break;
                }
            }
        }

        for (int i = 0; i < terrainData.Length; i++)
        {
            if (vertices[i].y <= terrainData[i].height)
            {
                triangles[i].Add(0);
                triangles[i].Add((int)circumferencePoints);
                triangles[i].Add((int)circumferencePoints - 1);
                break;
            }
        }

        /*
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * zoom + xPosition, z * zoom + zPosition) * intensity;

                float xOffset = 0;
                float zOffset = 0;
                
                if (x == 0)
                    xOffset = -Mathf.PerlinNoise(x * zoom + xPosition, z * zoom + zPosition) * intensity * 2;
                else if (x == xSize)
                    xOffset = Mathf.PerlinNoise(x * zoom + xPosition, z * zoom + zPosition) * intensity * 2;

                else if (z == 0)
                    zOffset = -Mathf.PerlinNoise(x * zoom + xPosition, z * zoom + zPosition) * intensity * 2;
                else if (z == zSize)
                    zOffset = Mathf.PerlinNoise(x * zoom + xPosition, z * zoom + zPosition) * intensity * 2;

                vertices[i] = new Vector3(x + xOffset, y, z + zOffset);
                i++;
            }
        }

        int vert = 0;
        int tris = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                for (int i = 0; i < terrainData.Length; i++)
                {
                    if (vertices[vert].y <= terrainData[i].height)
                    {
                        triangles[i].Add(vert);
                        triangles[i].Add(vert + xSize + 1);
                        triangles[i].Add(vert + 1);
                        triangles[i].Add(vert + 1);
                        triangles[i].Add(vert + xSize + 1);
                        triangles[i].Add(vert + xSize + 2);
                    }
                }

                vert++;
                tris += 6;
            }
        }
        */
    }

    private void SortData(TerrainData[] data)
    {
        for (int index = 0; index < data.Length; index++)
        {
            int smallest = index;

            for (int i = index; i < data.Length; i++)
                if (data[smallest].height > data[i].height)
                    smallest = i;

            TerrainData temp = data[index];
            data[index] = data[smallest];
            data[smallest] = temp;
        }
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;

        mesh.subMeshCount = triangles.Length;
        
        Debug.Log(triangles.Length);
        for (int i = 0; i < triangles.Length; i++)
        {
            //if (triangles[i].ToArray().Length < 3) { continue; }
            int[] arr = triangles[i].ToArray();

            for (int j = 0; j < arr.Length; j++)
            {
                //Debug.Log(arr[j]);
            }
            Debug.Log("-----------");
            mesh.SetTriangles(triangles[i].ToArray(), i);
        }

        mesh.RecalculateNormals();
    }

    void OnDrawGizmos()
    {
        if (vertices == null) { return; }

        Gizmos.color = Color.green;

        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }
}

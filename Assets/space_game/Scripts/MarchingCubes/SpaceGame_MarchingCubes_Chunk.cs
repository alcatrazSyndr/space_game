using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceGame_MarchingCubes_Chunk
{
    private MeshFilter _meshFilter;
    private MeshCollider _meshCollider;
    private GameObject _chunkObject;
    private MeshRenderer _meshRenderer;
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private Vector3Int _chunkPosition;
    private float _terrainSurface;
    private int _width;
    private int _height;
    private SpaceGame_NoiseData_Sphere _myData;

    public SpaceGame_MarchingCubes_Chunk(Vector3Int position, SpaceGame_NoiseData_Sphere myData)
    {
        _myData = myData;
        _width = SpaceGame_MarchingCubes_Data.CHUNK_WIDTH;
        _height = SpaceGame_MarchingCubes_Data.CHUNK_WIDTH;
        _terrainSurface = SpaceGame_MarchingCubes_Data.TERRAIN_SURFACE;
        _chunkObject = new GameObject();
        _chunkPosition = position;
        _chunkObject.transform.position = _chunkPosition * 16;
        _chunkObject.transform.SetParent(_myData.transform);
        _chunkObject.name = "Chunk_" + position;
        _meshFilter = _chunkObject.AddComponent<MeshFilter>();
        _meshRenderer = _chunkObject.AddComponent<MeshRenderer>();
        _meshRenderer.material = Resources.Load<Material>("Materials/Terrain");
        _meshCollider = _chunkObject.AddComponent<MeshCollider>();

        CreateMeshData();
    }

    private void CreateMeshData()
    {
        ClearMeshData();

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                for (int z = 0; z < _width; z++)
                {
                    MarchCube(new Vector3Int(x, y, z));
                }
            }
        }

        BuildMesh();
    }

    private void MarchCube(Vector3Int position)
    {
        float[] cube = new float[8];
        for (int i = 0; i < 8; i++)
        {
            cube[i] = _myData.SampleNoise(_chunkPosition, position + SpaceGame_MarchingCubes_Data.CORNER_TABLE[i]);
        }

        int configIndex = GetCubeConfiguration(cube);
        if (configIndex == 0 || configIndex == 255)
            return;

        int edgeIndex = 0;
        for (int i = 0; i < 5; i++)
        {
            for (int p = 0; p < 3; p++)
            {
                int indice = SpaceGame_MarchingCubes_Data.TRIANGLE_TABLE[configIndex, edgeIndex];
                if (indice == -1)
                    return;

                Vector3 vert1 = position + SpaceGame_MarchingCubes_Data.CORNER_TABLE[SpaceGame_MarchingCubes_Data.EDGE_TABLE[indice, 0]];
                Vector3 vert2 = position + SpaceGame_MarchingCubes_Data.CORNER_TABLE[SpaceGame_MarchingCubes_Data.EDGE_TABLE[indice, 1]];
                Vector3 vertPosition;
                float vert1Sample = cube[SpaceGame_MarchingCubes_Data.EDGE_TABLE[indice, 0]];
                float vert2Sample = cube[SpaceGame_MarchingCubes_Data.EDGE_TABLE[indice, 1]];
                float difference = vert2Sample - vert1Sample;
                if (difference == 0)
                    difference = _terrainSurface;
                else
                    difference = (_terrainSurface - vert1Sample) / difference;
                vertPosition = vert1 + ((vert2 - vert1) * difference);

                vertices.Add(vertPosition);
                triangles.Add(vertices.Count - 1);
                edgeIndex++;
            }
        }
    }

    private int GetCubeConfiguration(float[] cube)
    {
        int configurationIndex = 0;
        for (int i = 0; i < 8; i++)
        {
            if (cube[i] <= _terrainSurface)
                configurationIndex |= 1 << i;
        }
        return configurationIndex;
    }

    private void ClearMeshData()
    {
        vertices.Clear();
        triangles.Clear();
    }

    private void BuildMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        _meshFilter.mesh = mesh;
        _meshCollider.sharedMesh = mesh;
    }
}

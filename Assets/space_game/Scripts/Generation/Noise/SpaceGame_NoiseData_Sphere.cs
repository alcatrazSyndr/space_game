using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpaceGame_NoiseData_Sphere : MonoBehaviour
{
    [SerializeField] private bool _debug = true;
    [SerializeField] private bool _debug_showEmpties = true;
    [SerializeField] private bool _debug_displayValues = true;
    [SerializeField] private bool _debug_drawSpheres = true;
    [SerializeField] private int _objectSize = 10;

    private Dictionary<Vector3Int, SpaceGame_MarchingCubes_Chunk> _chunks = new Dictionary<Vector3Int, SpaceGame_MarchingCubes_Chunk>();
    private int _chunkSize;
    private float[,,][,,] _objectNoiseMap;
    private bool _noiseGenerated = false;

    private void Awake()
    {
        _chunkSize = SpaceGame_MarchingCubes_Data.CHUNK_WIDTH;
    }

    private void Start()
    {
        GenerateNoise();
        GenerateChunks();
    }

    private void GenerateNoise()
    {
        float chunkBounds = ((float)_chunkSize * (float)_objectSize) / 2f;
        float radius = chunkBounds - 3f;
        Vector3 center = transform.position + new Vector3(chunkBounds, chunkBounds, chunkBounds);
        _objectNoiseMap = new float[_objectSize, _objectSize, _objectSize][,,];
        for (int i = 0; i < _objectSize; i++)
        {
            for (int n = 0; n < _objectSize; n++)
            {
                for (int m = 0; m < _objectSize; m++)
                {
                    Vector3 chunkPos = new Vector3(i * _chunkSize, n * _chunkSize, m * _chunkSize);
                    float[,,] chunkNoiseMap = new float[_chunkSize + 1, _chunkSize + 1, _chunkSize + 1];
                    for (int x = 0; x < _chunkSize + 1; x++)
                    {
                        for (int y = 0; y < _chunkSize + 1; y++)
                        {
                            for (int z = 0; z < _chunkSize + 1; z++)
                            {
                                Vector3 noisePoint = chunkPos + new Vector3(x, y, z);
                                float noiseVectorLength = (noisePoint - center).magnitude;
                                float noiseValue = 1f - Mathf.Clamp01(noiseVectorLength - radius);
                                chunkNoiseMap[x, y, z] = noiseValue;
                            }
                        }
                    }
                    _objectNoiseMap[i, n, m] = chunkNoiseMap;
                }
            }
        }
        _noiseGenerated = true;
    }

    private void GenerateChunks()
    {
        for (int x = 0; x < _objectSize; x++)
        {
            for (int y = 0; y < _objectSize; y++)
            {
                for (int z = 0; z < _objectSize; z++)
                {
                    Vector3Int chunkPos = new Vector3Int(x, y, z);
                    _chunks.Add(chunkPos, new SpaceGame_MarchingCubes_Chunk(chunkPos, this));
                }
            }
        }
    }

    public float SampleNoise(Vector3Int chunkPosition, Vector3Int vertexPosition)
    {
        return _objectNoiseMap[chunkPosition.x, chunkPosition.y, chunkPosition.z][vertexPosition.x, vertexPosition.y, vertexPosition.z];
    }

    private void OnDrawGizmos()
    {
        if (!_noiseGenerated) return;
        if (!_debug) return;

        for (int i = 0; i < _objectSize; i++)
        {
            for (int n = 0; n < _objectSize; n++)
            {
                for (int m = 0; m < _objectSize; m++)
                {
                    Vector3 chunkPos = new Vector3(i * _chunkSize, n * _chunkSize, m * _chunkSize);
                    for (int x = 0; x < _chunkSize + 1; x++)
                    {
                        for (int y = 0; y < _chunkSize + 1; y++)
                        {
                            for (int z = 0; z < _chunkSize + 1; z++)
                            {
                                if (!_debug_showEmpties && _objectNoiseMap[i, n, m][x, y, z] <= 0f) continue;

                                Vector3 pointPos = chunkPos + new Vector3(x, y, z);
                                Gizmos.color = _objectNoiseMap[i, n, m][x, y, z] > 0f ? Color.green : Color.red;
                                if (_debug_drawSpheres)
                                    Gizmos.DrawSphere(pointPos, 0.1f);
                                if (_debug_displayValues)
                                    Handles.Label(pointPos, _objectNoiseMap[i, n, m][x, y, z].ToString());
                            }
                        }
                    }
                }
            }
        }
    }
}

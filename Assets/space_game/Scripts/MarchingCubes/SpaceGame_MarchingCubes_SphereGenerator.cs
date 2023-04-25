using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpaceGame_MarchingCubes_SphereGenerator : MonoBehaviour
{
    [SerializeField] private int _objectSize = 20;

    private Dictionary<Vector3Int, SpaceGame_MarchingCubes_Chunk> _chunks = new Dictionary<Vector3Int, SpaceGame_MarchingCubes_Chunk>();

    private void Start()
    {
        Generate_Sphere();
    }

    private void Generate_Sphere()
    {
        Vector3Int center = new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
        int noiseSize = _objectSize * SpaceGame_MarchingCubes_Data.CHUNK_WIDTH;
        Vector3Int startPoint = center - new Vector3Int(noiseSize / 2, noiseSize / 2, noiseSize / 2);

        for (int x = 0; x < _objectSize; x++)
        {
            for (int y = 0; y < _objectSize; y++)
            {
                for (int z = 0; z < _objectSize; z++)
                {
                    Vector3Int chunkPos = startPoint + new Vector3Int(x * SpaceGame_MarchingCubes_Data.CHUNK_WIDTH, y * SpaceGame_MarchingCubes_Data.CHUNK_HEIGHT, z * SpaceGame_MarchingCubes_Data.CHUNK_WIDTH);
                }
            }
        }
    }
}

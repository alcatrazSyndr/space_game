using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceGame_MarchingCubes_TerrainGenerator : MonoBehaviour
{
    [SerializeField] private int _objectSize = 10;

    private Dictionary<Vector3Int, SpaceGame_MarchingCubes_Chunk> _chunks = new Dictionary<Vector3Int, SpaceGame_MarchingCubes_Chunk>();

    void Start()
    {
        Generate();
    }

    private void Generate()
    {
        for (int x = 0; x < _objectSize; x++)
        {
            for (int y = 0; y < _objectSize; y++)
            {
                for (int z = 0; z < _objectSize; z++)
                {
                    Vector3Int chunkPos = new Vector3Int(x * SpaceGame_MarchingCubes_Data.CHUNK_WIDTH, 0, z * SpaceGame_MarchingCubes_Data.CHUNK_WIDTH);
                    //_chunks.Add(chunkPos, new SpaceGame_MarchingCubes_Chunk(chunkPos));
                }
            }
        }
    }
}

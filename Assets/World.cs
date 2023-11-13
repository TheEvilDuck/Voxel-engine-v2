using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using System.Threading;
using UnityEditor;
using UnityEditor.ShortcutManagement;

public class World
{
    private readonly int _chunkWidth;
    private readonly int _chunkHeight;
    private readonly ChunkRenderer _chunkPrefab;

    public static readonly Vector2Int[] chunkDirections = {Vector2Int.left,Vector2Int.right,Vector2Int.up,Vector2Int.down};

    private ConcurrentQueue<ChunkRenderer> _renderQueue = new ConcurrentQueue<ChunkRenderer>();
    private List<Vector2Int>_chunksToUnload = new List<Vector2Int>();
    private Dictionary<Vector2Int,ChunkRenderer>_chunkRenderers = new Dictionary<Vector2Int, ChunkRenderer>();
    private Dictionary<Vector2Int,ChunkData>_chunks = new Dictionary<Vector2Int, ChunkData>();

    private Dictionary<BlockType,Vector2Int>_blockDatabase = new Dictionary<BlockType, Vector2Int>();

    private ITerrainGenerator _terrainGenerator;
    
    public World(int chunkWidth, int chunkHeight,ChunkRenderer chunkPrefab,BlockInfo[] blockDatabase, ITerrainGenerator terrainGenerator)
    {
        _chunkHeight = chunkHeight;
        _chunkWidth = chunkWidth;
        _chunkPrefab = chunkPrefab;

        _terrainGenerator = terrainGenerator;

        foreach (BlockInfo blockInfo in blockDatabase)
        {
            _blockDatabase.TryAdd(blockInfo.BlockType,blockInfo.TexturePosition);
        }
    }

    public void LoadChunksAround(Vector2Int centralChunkPosition,int loadDistance)
    {
        List<Task<ChunkData>>chunkDataTasks = new List<Task<ChunkData>>();
        for (int x = centralChunkPosition.x-loadDistance/2;x<centralChunkPosition.x+loadDistance/2;x++)
        {
            for (int y = centralChunkPosition.y-loadDistance/2;y<centralChunkPosition.y+loadDistance/2;y++)
            {
                Vector2Int position = new Vector2Int(x,y);
                if ((position-centralChunkPosition).magnitude>loadDistance)
                    continue;
                if (_chunkRenderers.ContainsKey(position))
                    continue;
                ChunkRenderer chunkRenderer = UnityEngine.Object.Instantiate(_chunkPrefab,new Vector3(position.x*_chunkWidth,0,position.y*_chunkWidth),Quaternion.identity);
                if (!_chunkRenderers.TryAdd(position,chunkRenderer))
                    continue;
                if (_chunksToUnload.Contains(position))
                    _chunksToUnload.Remove(position);
                Task<ChunkData> chunkDataTask = Task<ChunkData>.Factory.StartNew(()=>{
                    return new ChunkData(_terrainGenerator,position,_chunkHeight,_chunkWidth,this);
                });
                chunkDataTasks.Add(chunkDataTask);
            }
        }
        Task.WaitAll(chunkDataTasks.ToArray());
        Dictionary<Vector2Int,Task<MeshData>>resultTasks = new Dictionary<Vector2Int, Task<MeshData>>();
        foreach (Task<ChunkData>task in chunkDataTasks)
        { 
            ChunkData chunkData = task.Result;
            Task<MeshData> meshTask = Task<MeshData>.Factory.StartNew(()=>
            {
                return new MeshData(chunkData,_blockDatabase); 
            });
            resultTasks.Add(chunkData.ChunkPosition,meshTask);
            _chunks.TryAdd(chunkData.ChunkPosition,chunkData);
        }
        Task.WaitAll(new List<Task>(resultTasks.Values).ToArray());
        foreach (KeyValuePair<Vector2Int,Task<MeshData>>result in resultTasks)
        {
            MeshData meshData = result.Value.Result;
            ChunkRenderer chunkRenderer = _chunkRenderers.GetValueOrDefault(result.Key);
            chunkRenderer.UpdateMeshData(meshData);
            _renderQueue.Enqueue(chunkRenderer);
        }
        
    }
    public void UnloadChunksWithCenter(Vector2Int position, int loadDistance)
    {
        foreach (KeyValuePair<Vector2Int,ChunkData> chunk in _chunks)
        {
            if ((position-chunk.Key).magnitude>loadDistance)
            {
                if (!_chunksToUnload.Contains(chunk.Value.ChunkPosition))
                    _chunksToUnload.Add(chunk.Value.ChunkPosition);
            }
        }
        foreach (Vector2Int chunkPos in _chunksToUnload)
        {
            if ((chunkPos-position).magnitude<=loadDistance)
                _chunksToUnload.Remove(chunkPos);
        }
    }
   

    public void LoadChunkAtPosition(Vector2Int chunkPosition)
    {
        if (_chunkRenderers.ContainsKey(chunkPosition))
            return;
        ChunkRenderer chunkRenderer = UnityEngine.Object.Instantiate(_chunkPrefab,new Vector3(chunkPosition.x*_chunkWidth,0,chunkPosition.y*_chunkWidth),Quaternion.identity);
        _chunkRenderers.TryAdd(chunkPosition,chunkRenderer);
        
    }
    public int Vector3IntToIndex(Vector3Int blockPos)
    {
        return blockPos.x+blockPos.y*_chunkWidth+blockPos.z*_chunkWidth*_chunkHeight;
    }
    public Vector2Int WorldPositionToChunkPosition(Vector3 position)
    {
        return new Vector2Int((int)position.x/_chunkWidth,(int)position.z/_chunkWidth);
    }
    public int WorldPositionToBlockIndexInChunk(Vector3 position)
    {
        Vector2Int chunkPosition = WorldPositionToChunkPosition(position);
        return Vector3IntToIndex(new Vector3Int((int)position.x-chunkPosition.x*_chunkWidth,(int)position.y,(int)position.z-chunkPosition.y*_chunkWidth));
        


    }


    public BlockType GetBlockInChunk(Vector2Int chunkPosition, int blockIndexInChunk)
    {
        if (_chunks.TryGetValue(chunkPosition,out ChunkData chunkData))
        {
            return chunkData.GetBlockAtPosition(blockIndexInChunk);
        }
        return BlockType.Air;
    }

    private void RenderChunkFromQueue(int amount)
    {
        for (int i = 0;i<amount;i++)
        {
            if (_renderQueue.TryDequeue(out ChunkRenderer chunkRenderer))
        {
            if (chunkRenderer!=null)
                chunkRenderer.Render();
        }
        }
    }
    private void UnloadChunkFromList(int amount)
    {
        for (int i = 0;i<amount;i++)
        {
            if (_chunksToUnload.Count>0)
            {
                _chunks.Remove(_chunksToUnload[0]);
                UnityEngine.Object.Destroy(_chunkRenderers.GetValueOrDefault(_chunksToUnload[0]).gameObject);
                _chunkRenderers.Remove(_chunksToUnload[0]);
                _chunksToUnload.RemoveAt(0);
            }
        }
    }
    public void OnUpdate()
    {
        RenderChunkFromQueue(Mathf.CeilToInt(_renderQueue.Count/10f));
        UnloadChunkFromList(Mathf.CeilToInt(_chunksToUnload.Count/10));
    }

    public bool ModifyAt(Vector2Int chunkPosition, int blockPosition,BlockType block)
    {
        if (!_chunks.ContainsKey(chunkPosition))
            return false;
        ChunkData chunk = _chunks.GetValueOrDefault(chunkPosition);
        chunk.ModifyBlock(blockPosition,block);
        return true;
    }

     
}

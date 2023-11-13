using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]int _chunkHeight;
    [SerializeField]int _chunkWidth;
    [SerializeField]PlayerInput _playerInput;
    [SerializeField]ChunkRenderer _chunkPrefab;
    [SerializeField]Character _characterPrefab;
    [SerializeField]CameraManipulation _cameraManipulation;
    [SerializeField]TerrainGeneratorSettings _terrainGeneratorSettings;

    [SerializeField]BlockInfo[] _blockInfos;

    private World _world;
    private Character _playerCharacter;
    private IEnumerator Start() 
    {
        _world = new World(_chunkWidth,_chunkHeight,_chunkPrefab,_blockInfos, new StandardTerrainGenerator(_terrainGeneratorSettings));
        Vector3 startPosition = new Vector3(20f,60f,5f);

        _world.LoadChunksAround(new Vector2Int(0,0),3);

        
        yield return new WaitForSeconds(2);
        _playerCharacter = Instantiate(_characterPrefab,startPosition,Quaternion.identity);
        
        Camera.main.transform.SetParent(_playerCharacter.transform);
        Camera.main.transform.localPosition = new Vector3(0,2f,0);

        _playerInput.leftMouseButtonPressed+=OnPlayerActionMain;
        _playerInput.rightMouseButtonPressed+=OnPlayerActionSecondary;
        _playerInput.moveVectorChanged+=_playerCharacter.SetMoveVector;
        _playerInput.mouseVectorChanged+=_cameraManipulation.UpdateMouseMove;
        _playerInput.mouseVectorChanged+=_playerCharacter.UpdateMouseMove;

    }

    private void Update() 
    {
        
        _world.OnUpdate();
        if (_playerCharacter!=null)
        {
            _world.LoadChunksAround(_world.WorldPositionToChunkPosition(_playerCharacter.Position),5);
            _world.UnloadChunksWithCenter(_world.WorldPositionToChunkPosition(_playerCharacter.Position),7);
        }
    }
    private void OnPlayerActionSecondary()
    {
        TryToModifyBlock(BlockType.Air);
    }
    private void OnPlayerActionMain()
    {
        TryToModifyBlock(BlockType.Stone);
    }

    private void TryToModifyBlock(BlockType block)
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f));
        if (Physics.Raycast(ray,out RaycastHit raycastHit,100f))
        {
            Vector3 targetPos = raycastHit.point+raycastHit.normal*-0.5f;
            Vector3Int blockWorldPos = Vector3Int.FloorToInt(targetPos);
            Vector2Int chunkCoordinates = _world.WorldPositionToChunkPosition(blockWorldPos);
            _world.ModifyAt(chunkCoordinates,_world.WorldPositionToBlockIndexInChunk(blockWorldPos),block);
            Debug.Log(chunkCoordinates);
            Debug.Log(_world.WorldPositionToBlockIndexInChunk(blockWorldPos));
        }
    }
}

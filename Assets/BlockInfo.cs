using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlockInfo
{
    [SerializeField]Vector2Int _texturePosition;
    [SerializeField]BlockType _blockType;

    public Vector2Int TexturePosition => _texturePosition;
    public BlockType BlockType => _blockType;
}

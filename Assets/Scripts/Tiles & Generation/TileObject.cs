﻿using System.Collections.Generic;
using UnityEngine;

public enum TileType 
{ 
    Wall = 0, 
    Ball = 1, 
    Super = 2, 
    Air = 3, 
    Outside = 4 
}

public class TileObject
{
    public TileType type;
    public int index;

    public TileObject neighbourUp;
    public TileObject neighbourDown;
    public TileObject neighbourLeft;
    public TileObject neighbourRight;

    public List<Transform> listBlocks;
    public Transform currentBlock = null;

    public TileObject(int _index, TileType _type, List<Transform> _listBlocks)
    {
        type = _type;
        index = _index;
        listBlocks = _listBlocks;

        Init();
    }
    private void Init()
    {
        currentBlock = listBlocks[(int)type];
    }
}
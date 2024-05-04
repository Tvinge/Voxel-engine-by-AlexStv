using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; //for Blocks Serialization and saving/loading chunks

//The [Serializable] tag lets the class be saved as binary including all the private variables
[Serializable]
public class BlockGrass : Block //Inherits from Block script
{
    public BlockGrass()
        : base()//constructor?
    {
    }
public override Tile TexturePosition(Direction direction)
{
    Tile tile = new Tile();
    switch (direction)
    {
        case Direction.up:
            tile.x = 2;
            tile.y = 0;
            return tile;
        case Direction.down:
            tile.x = 1;
            tile.y = 0;
            return tile;
    }
    tile.x = 3;
    tile.y = 0;
    return tile;
}
}

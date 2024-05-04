using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; //for Blocks Serialization and saving/loading chunks

//The [Serializable] tag lets the class be saved as binary including all the private variables
[Serializable]
public class BlockAir : Block
{
    public BlockAir()
        : base()//calls base class constructor
    {
    }
    public override MeshData Blockdata
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        return meshData;
    }
    public override bool IsSolid(Block.Direction direction)
    {
        return false;
    }
}

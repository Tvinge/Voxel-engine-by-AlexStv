using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EditTerrain //?? this class is static because it's just going to be a helper class.
{
    public static WorldPos GetBlockPos(Vector3 pos)
    {
        WorldPos blockPos = new WorldPos(
            Mathf.RoundToInt(pos.x),
            Mathf.RoundToInt(pos.y),
            Mathf.RoundToInt(pos.z));
        return blockPos;
    }
    public static WorldPos GetBlockPos(RaycastHit hit, bool adjacent = false)
    {//In this function we create a new vector3 by calling MoveWithinBlock on each axis of the position.
     //However, when we raycast onto a cube block the axis of the face the raycast hits will be 0.5, exactly half way between two blocks.
     //To solve that we use MoveWithinBlock.
        Vector3 pos = new Vector3(
            MoveWithinBlock(hit.point.x, hit.normal.x, adjacent),
            MoveWithinBlock(hit.point.y, hit.normal.y, adjacent),
            MoveWithinBlock(hit.point.z, hit.normal.z, adjacent));
        return GetBlockPos(pos);
    }
    static float MoveWithinBlock(float pos, float norm, bool adjacent = false)
    {
        if(pos - (int)pos == 0.5f || pos - (int)pos == -0.5f)
        {
            if (adjacent)
            {
                pos += (norm / 2);
            }
            else
            {
                pos -= (norm / 2);
            }
        }
        return (float)pos;
    }
    public static bool SetBlock(RaycastHit hit, Block block, bool adjacent = false)
    {//This function takes a raycastHit and gets the chunk hit
        Chunk chunk = hit.collider.GetComponent<Chunk>();//if no chunk component = collider is not a chunk
        if (chunk == null)
            return false;
        WorldPos pos = GetBlockPos(hit, adjacent);
        chunk.world.SetBlock(pos.x, pos.y, pos.z, block);
        return true; //gets the chunk hit?
    }
    public static Block GetBlock(RaycastHit hit, bool adjacent = false)
    {//same thing but we return hit block
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null)
            return null;
        WorldPos pos = GetBlockPos(hit, adjacent);
        Block block = chunk.world.GetBlock(pos.x, pos.y, pos.z);
        return block;
    }
}


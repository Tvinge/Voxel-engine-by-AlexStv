using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    //defines a dictionary with WorldPos as the key and Chunk as the value
    public Dictionary<WorldPos, Chunk> chunks = new Dictionary<WorldPos, Chunk> ();
    public GameObject chunkPrefab;
    public string worldName = "World";

    public void CreateChunk(int x, int y, int z)
    {
        //the coordinates of this chunk in the world
        WorldPos worldPos = new WorldPos(x, y, z);
        //Instantiate the chunk at the coordinates using the chunk prefab
        GameObject newChunkObject = Instantiate(chunkPrefab, new Vector3(x, y, z), Quaternion.Euler(Vector3.zero) ) as GameObject;
        //Get the object's chunk component
        Chunk newChunk = newChunkObject.GetComponent<Chunk>();
        //Assign its values
        newChunk.pos = worldPos;
        newChunk.world = this;
        //Add it to the chunks dictionary with the position as the key
        chunks.Add(worldPos, newChunk);

        var terrainGen = new TerrainGen();
        newChunk = terrainGen.ChunkGen(newChunk);
        newChunk.SetBlocksUnmodified();//sets all created blocks above as unmodified
        bool loaded = Serialization.Load(newChunk);//applies all changes made by player on top of the created chunk from scratch
        //!Another option would be to put the SetBlocksUnmodified after loading. Then if there are any modified blocks they are modified since loading and are not included in the existing save file so to save the changes we would deserialize the save file to a variable and then get the current unmodified blocks and add them to the variable then save that. This would mean that we could save less often because we only save when there are new changes to save but saving would require an extra step. How you do this depends a lot on what type of game you're making so consider this part of your game carefully.
    }
    public void DestroyChunk(int x, int y, int z)
    { 
        Chunk chunk = null;
        if (chunks.TryGetValue(new WorldPos(x, y, z), out chunk))
        {
            Serialization.SaveChunk(chunk);//ensures that after destroying block it gets saved
            Object.Destroy(chunk.gameObject);
            chunks.Remove(new WorldPos(x, y, z));
        }
    }
    public Chunk GetChunk(int x, int y, int z) //searches for chunk x,y,z
    {
        WorldPos pos = new WorldPos();
        float multiple = Chunk.chunkSize;
        pos.x = Mathf.FloorToInt(x / multiple) * Chunk.chunkSize;//zwraca pozycje chunka
        pos.y = Mathf.FloorToInt(y / multiple) * Chunk.chunkSize;//?? I use the variable multiple so that the chunk size is a float when dividing because dividing two integers will give us trouble if they are negative
        pos.z = Mathf.FloorToInt(z / multiple) * Chunk.chunkSize;
        Chunk containerChunk = null;
        chunks.TryGetValue(pos, out containerChunk);//TryGetValue looks up the key in the dictionary and assigns the containerChunk with the result if found

        return containerChunk;
    }
    public Block GetBlock(int x, int y, int z) //searches for x,y,z block in Chunk
    {
        Chunk containerChunk = GetChunk(x, y, z);
        if (containerChunk != null)
        {
            Block block = containerChunk.GetBlock(
                x - containerChunk.pos.x,//x - chunk's x gives local position in the chunk
                y - containerChunk.pos.y,
                z - containerChunk.pos.z);
            return block;
        }
        else
        {
            return new BlockAir();
        }
    }
    public void SetBlock(int x, int y, int z, Block block)
    {
        Chunk chunk = GetChunk(x, y, z);
        if(chunk != null)
        {
            chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, block);
            chunk.update = true;
            //updates bordering chunks
            
            UpdateIfEqual(x - chunk.pos.x, 0, new WorldPos(x - 1, y, z));
            UpdateIfEqual(x - chunk.pos.x, Chunk.chunkSize - 1, new WorldPos(x + 1, y, z));
            UpdateIfEqual(y - chunk.pos.y, 0, new WorldPos(x, y - 1, z));
            UpdateIfEqual(y - chunk.pos.y, Chunk.chunkSize - 1, new WorldPos(x, y + 1, z));
            UpdateIfEqual(z - chunk.pos.z, 0, new WorldPos(x, y, z - 1));
            UpdateIfEqual(z - chunk.pos.z, Chunk.chunkSize - 1, new WorldPos(x, y, z + 1));
        }
    }
    void UpdateIfEqual(int value1, int value2, WorldPos pos)
    {//checks if blocks are on border of the chunk (0, chunkSize-1)
        if (value1 == value2)
        {
            Chunk chunk = GetChunk(pos.x, pos.y, pos.z);
            if (chunk != null)
                chunk.update = true;
        }
    }
}

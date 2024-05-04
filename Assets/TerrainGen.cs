using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimplexNoise;
public class TerrainGen
{
    float stoneBaseHeight = -24;
    float stoneBaseNoise = 0.05f;//0.05 for noise makes peaks around 25 blocks apart so I like it for making stone less flat
    float stoneBaseNoiseHeight = 4;//The noise height for this is 4 so the max difference between peak and valley is 4, not very much
    float stoneMountainHeight = 48;//larger height
    float stoneMountainFrequency = 0.008f;//
    float stoneMinHeight = -12;//The min height here is the lowest stone is allowed to go
    float dirtBaseHeight = 1;//minimum depth on top of the rock
    float dirtNoise = 0.04f;//the noise a little more messy than the stone with smaller peaks
    float dirtNoiseHeight = 3;
    float caveFrequency = 0.025f;
    int caveSize = 7;
    float treeFrequency = 0.2f;
    int treeDensity = 3;

    public Chunk ChunkGen(Chunk chunk)
    {//take chunk, fill it and return it 
        for (int x = chunk.pos.x - 3; x < chunk.pos.x + Chunk.chunkSize + 3; x++) //3 increases size of the chunk to allow rendering lod's parts from other chunk np. trees
        {
            for (int z = chunk.pos.z - 3; z < chunk.pos.z + Chunk.chunkSize + 3; z++)
            {
                chunk = ChunkColumnGen(chunk, x, z);
            }
        }
        return chunk;
    }
    public Chunk ChunkColumnGen(Chunk chunk, int x, int z)
    {
        int stoneHeight = Mathf.FloorToInt(stoneBaseHeight); //stone lvl
        stoneHeight += GetNoise(x, 0, z, stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));//adds mountain noise 
        if (stoneHeight < stoneMinHeight)//raises lvl below min to minimum
            stoneHeight  = Mathf.FloorToInt(stoneMinHeight);
        stoneHeight += GetNoise(x, 0, z, stoneBaseNoise, Mathf.FloorToInt(stoneBaseNoiseHeight));//applies base noise
        int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);
        dirtHeight += GetNoise(x, 100, z, dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));//adds dirt noise on top of the base dirt
        for (int y = chunk.pos.y - 8; y < chunk.pos.y +Chunk.chunkSize; y++)
        {
            //get value to base cave generation on
            int caveChance = GetNoise(x, y, z, caveFrequency, 100);
            if (y <= stoneHeight && caveSize < caveChance)
            {
                SetBlock(x, y, z, new Block(), chunk);//uses local coordinates
            }
            else if (y <= dirtHeight && caveSize < caveChance)
            {
                SetBlock(x, y, z, new BlockGrass(), chunk);
                if (y == dirtHeight && GetNoise(x, 0, z, treeFrequency, 100) < treeDensity)
                    CreateTree(x, y + 1, z, chunk);
            }
            else
            {
                SetBlock(x, y, z, new BlockAir(), chunk);
            }
        }

        return chunk;
    }
    void CreateTree(int x, int y, int z, Chunk chunk)
    {//create leaves
        for (int xi =  -2; xi <= 2; xi++)
        {
            for (int yi = 4; yi <= 8; yi++)
            {
                for (int zi = -2; zi <= 2; zi++)
                {
                    SetBlock(x + xi, y + yi, z + zi, new BlockLeaves(), chunk, true);
                }
            }
        }
        for (int yt = 0; yt < 6; yt++)
        {//creates trunk
            SetBlock(x, y + yt, z, new BlockWood(), chunk, true);
        }
    }
    public static void SetBlock(int x, int y, int z, Block block, Chunk chunk, bool replaceBlocks = false)
    {
        x -= chunk.pos.x;
        y -= chunk.pos.y;
        z -= chunk.pos.z;
        if (Chunk.InRange(x) && Chunk.InRange(y) && Chunk.InRange(z))
        {
            if (replaceBlocks || chunk.blocks[x, y, z] == null)
                chunk.SetBlock(x, y, z, block);
        }
    }

    public static int GetNoise(int x, int y, int z, float scale, int max)
    {
        return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
    }
}



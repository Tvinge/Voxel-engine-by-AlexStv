using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public static class Serialization
{
    public static string saveFolderName = "voxelGameSaves";
    public static string SaveLocation(string worldName)
    {//save to/ creates save directory
        string saveLocation = saveFolderName + "/" + worldName + "/";
        if (!Directory.Exists(saveLocation))
        {
            Directory.CreateDirectory(saveLocation);
        }
        return saveLocation;
    }
    public static string FileName(WorldPos chunkLocation)
    {//Creates name of the file/chunk and saves it 
        string fileName = chunkLocation.x + "," + chunkLocation.y + "," + chunkLocation.z + ".bin";//bin - binary file
        return fileName;
    }
    public static void SaveChunk(Chunk chunk)
    {//while modifying ensure backwards compability!!
        Save save = new Save(chunk);
        if (save.blocks.Count == 0)
            return;
        string saveFile = SaveLocation(chunk.world.worldName);
        saveFile += FileName(chunk.pos);//saves^ and names a file

        IFormatter formatter = new BinaryFormatter(); //??creates binary formater
        Stream stream = new FileStream(saveFile, FileMode.Create, FileAccess.Write, FileShare.None);//??creates filestream
        formatter.Serialize(stream, save);//serialize array before change(chunk.blocks)
        stream.Close();
    }
    public static bool Load(Chunk chunk)
    {
        string saveFile = SaveLocation(chunk.world.worldName);
        saveFile += FileName(chunk.pos);//populate a chunk with the save if there is one

        if (!File.Exists(saveFile))
            return false;

        IFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(saveFile, FileMode.Open);//deserialize file as a Block array/ ELI5 loads files to display them as cubes
        
        Save save = (Save)formatter.Deserialize(stream);//deserialize the changed blocks 
        foreach (var block in save.blocks) //set their position in the chunk to their value
        {
            chunk.blocks[block.Key.x, block.Key.y, block.Key.z] = block.Value;
        }
        stream.Close();
        return true;
    }

}

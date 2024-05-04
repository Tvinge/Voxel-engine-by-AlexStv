using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//A dictionary stores values along with a key that can be any type, then you can look up a value by its key
//making it fast to find a value without knowing its index as long as you know its key
[Serializable]
public struct WorldPos
{
    public int x, y, z;
    public WorldPos(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    public override bool Equals(object obj)
    {//used while generating chunks etc
        if (GetHashCode() == obj.GetHashCode())
            return true;
        return false;
    }
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 47;
            hash = hash * 227 + x.GetHashCode();
            hash = hash * 227 + y.GetHashCode();
            hash = hash * 227 + z.GetHashCode();
            return hash;
        }
    }
}
//What this does is check if the object being compared is a WorldPos and that x, y and z are equal to this one's x, y and z.
//This gives us a variable for positions that can quickly be compared.

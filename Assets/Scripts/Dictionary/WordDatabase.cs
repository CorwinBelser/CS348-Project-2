using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class WordDatabase {

    public Dictionary<int, List<WordCollection>> Data;

    public WordDatabase()
    {
        Data = new Dictionary<int, List<WordCollection>>();
    }

    public override string ToString()
    {
        string result = "{";
        foreach(int key in Data.Keys)
        {
            result += key + ", {" + Data[key].ToDelimitedString() + "},";
        }
        result.Remove(result.Length-1);
        result += "}";
        return result;
    }
}

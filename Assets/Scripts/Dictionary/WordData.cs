using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WordData {

    public string Value;
    public char[] Sorted;
    public int Length;

    public WordData(char[] letters)
    {
        Value = letters.ToString();
        Sorted = letters;
        Array.Sort(Sorted);
        Length = Sorted.Length;
    }

    public WordData(string word)
    {
        Value = word;
        Sorted = word.ToCharArray();
        Array.Sort(Sorted);
        Length = Sorted.Length;
    }

    public override string ToString()
    {
        return Value;
    }
}

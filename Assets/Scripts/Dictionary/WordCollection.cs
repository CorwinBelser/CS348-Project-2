using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class WordCollection {

    public char[] Letters; /* 10 sorted letters */
    public List<WordData> Words;
    public int Count {
        get {
            return Words.Count;
        }
    }

    public WordCollection()
    {
        Words = new List<WordData>();
    }

    public WordCollection(char[] letters)
    {
        Letters = letters;
        Words = new List<WordData>();
    }

    public override string ToString()
    {
        return "{" + Letters.ToString() + ", {" + Words.ToDelimitedString() + "}}";
    }
}

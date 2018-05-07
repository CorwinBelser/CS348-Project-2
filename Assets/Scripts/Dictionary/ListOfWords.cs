using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System;

[Serializable()]
public class ListOfWords {

    public Word[] Words;

    public ListOfWords()
    {

    }

    public ListOfWords(Word[] words)
    {
        Words = words;
    }
}

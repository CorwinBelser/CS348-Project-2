using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Word {

    public string value;
    public string sorted;
    public int length;
    public int gradeLevel;

    public Word() { }

    public Word(string Word, int GradeLevel = 1)
    {
        value = Word;
        value.ToLower();

        char[] temp= value.ToCharArray();
        Array.Sort(temp);
        sorted = temp.ToString();

        length = value.Length;

        gradeLevel = GradeLevel;
    }
}

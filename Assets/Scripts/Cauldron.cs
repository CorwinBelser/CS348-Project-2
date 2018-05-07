using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cauldron : MonoBehaviour
{
    [SerializeField] Text label;
    [SerializeField] Text spelledWords;
    private string word = "";

    public void AddLetter(string letter)
    {
        //TODO: make this take in the entire potion, so we can build a stack for undo/clear
        word += letter;
        label.text = word;
    }

    public void Validate()
    {
        //TODO: check word against dictionary
        bool valid = FindWordInDictionary();

        if (valid)
        {
            //TODO: animate word moving up into sky
            spelledWords.text += ", " + word;
            Clear();
        }
        else
        {
            //TODO: animate cauldron exploding
            Clear();
        }
    }

    private bool FindWordInDictionary()
    {
        //TODO:
        return true;
    }

    public void Undo()
    {
        //TODO: remove potion from stack and call Potion.Restore
    }

    public void Clear()
    {
        //TODO: remove all potions from stack and call Potion.Restore
    }
}

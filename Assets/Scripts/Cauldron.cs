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
        return WordManager.Instance.ValidWord(word);
    }

    public void Undo()
    {
        // Check that there is a letter to undo
        if (word.Length > 0)
        {
            //Remove potion from stack and call Potion.Restore
            string letter = word.Substring(word.Length - 1);
            word = word.Remove(word.Length - 1);
            label.text = word;
            Potion potion = GameController.Instance.GetPotion(letter);

            if (potion != null)
                potion.Restore();
        }
    }

    public void Clear()
    {
        for (int i = word.Length - 1; i >= 0; i--)
        {
            Undo();
        }
    }
}

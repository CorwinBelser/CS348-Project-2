using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cauldron : MonoBehaviour
{
    [SerializeField] Text label;
    [SerializeField] Text spelledWordsText;
    private string word = "";
    private List<Potion> potionsInWord = new List<Potion>();
    private List<string> spelledWords = new List<string>();
    private GameController gameController;

    private void Start()
    {
        gameController = GameController.Instance;
    }


    public void AddPotion(Potion potion)
    {
        potionsInWord.Add(potion);
        word += potion.Letter;
        label.text = word;
    }

    public void Validate()
    {
        if (!spelledWords.Contains(word))   // first check if word has already been created
        {
            bool valid = FindWordInDictionary();

            if (valid)
            {
                Debug.Log("valid");
                //TODO: animate word moving up into sky
                spelledWords.Add(word);
                spelledWordsText.text += ", " + word;
                if (word.Length >= 4)
                    gameController.AddTime();
                Clear();
            }
            else
            {
                Debug.Log("not valid");
                //TODO: animate cauldron exploding
                Clear();
            }
        }
        else
        {
            Debug.Log("already made");
            //TODO: animate cauldron exploding
            Clear();
        }
    }

    private bool FindWordInDictionary()
    {
        //return true;
        return WordManager.Instance.ValidWord(word);
    }

    public void Undo()
    {
        // Check that there is a letter to undo
        if (word.Length > 0)
        {
            // Remove last potion from stack and call Potion.Restore
            Potion potionToRestore = potionsInWord[potionsInWord.Count - 1];
            potionsInWord.RemoveAt(potionsInWord.Count - 1);
            word = word.Substring(0, word.Length - potionToRestore.Letter.Length);  // Substring is (startIndex, length)
            label.text = word;
            potionToRestore.Restore();
        }
    }

    public void Clear()
    {
        for (int i = potionsInWord.Count; i > 0; i--)
        {
            Undo();
        }
    }
}

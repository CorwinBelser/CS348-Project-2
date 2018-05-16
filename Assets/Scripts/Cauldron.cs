using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cauldron : MonoBehaviour
{
    [SerializeField] Text label;
    [SerializeField] GameObject selectionSprite;
    private string word = "";
    private bool validWord = false;
    private List<Potion> potionsInWord = new List<Potion>();
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
        CheckForValidWord();
    }

    private void CheckForValidWord()
    {
        validWord = gameController.ValidateWord(word);
        if (validWord)
            selectionSprite.SetActive(true);
        else
            selectionSprite.SetActive(false);
    }

    void OnMouseUp()
    {
        if(validWord)
            gameController.SubmitWord(word, potionsInWord.Count);
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
            CheckForValidWord();
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

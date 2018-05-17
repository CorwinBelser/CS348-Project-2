using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cauldron : MonoBehaviour
{
    [SerializeField] Text label;
    private string word = "";
    private List<Potion> potionsInWord = new List<Potion>();
    private GameController gameController;
    [SerializeField] private GameObject particleGameObject;
    private ParticleSystem bubbleParticle;
    private float emissionRate;
    private float increasePerPotion = 2f;

    private void Start()
    {
        gameController = GameController.Instance;
        bubbleParticle = particleGameObject.GetComponent<ParticleSystem>();
        emissionRate = 1f;
        UpdateBubbleEmission();
    }

    public void AddPotion(Potion potion)
    {
        /* Increase the bubble emmision */
        emissionRate += increasePerPotion;
        UpdateBubbleEmission();

        potionsInWord.Add(potion);
        word += potion.Letter;
        label.text = word;
    }

    void OnMouseUp()
    {
        gameController.ValidateWord(word, potionsInWord.Count);
    }

    public void Undo()
    {
        // Check that there is a letter to undo
        if (word.Length > 0)
        {
            emissionRate -= increasePerPotion;
            UpdateBubbleEmission();

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

    public void UpdateBubbleEmission()
    {
        ParticleSystem.EmissionModule emission = bubbleParticle.emission;
        emission.rateOverTime = emissionRate;
    }
}

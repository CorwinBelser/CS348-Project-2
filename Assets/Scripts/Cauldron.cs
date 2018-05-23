using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cauldron : MonoBehaviour
{
    [SerializeField] Text label;
    private string word = "";
    private List<Potion> potionsInWord = new List<Potion>();
    [SerializeField] private GameObject cloud;
    private List<Cloud> clouds = new List<Cloud>();
    private GameController gameController;
    [SerializeField] private GameObject particleGameObject;
    [SerializeField] private GameObject potionArcMidpoint;
    private ParticleSystem bubbleParticle;
    private float emissionRate;
    private float increasePerPotion = 2f;

    public static Cauldron Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.Log("Duplicate Cauldron instance! Destroy! Destroy!");
            Destroy(this.gameObject);
        }
    }

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


        AddCloud(potion.Letter);
        word += potion.Letter;
        //label.text = word;
    }

    private void AddCloud(string letter)
    {
        Cloud newCloud = Instantiate(cloud, transform).GetComponent<Cloud>();
        clouds.Add(newCloud);
        newCloud.Text = letter;
        newCloud.SetState(Cloud.State.RiseFromCauldron, clouds.Count);

        for(int i=0; i<clouds.Count-1; i++)
        {
            clouds[i].SetState(Cloud.State.SlideOver, clouds.Count);
        }
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

            // Remove last potion from stack
            Potion potionToRemove = potionsInWord[potionsInWord.Count - 1];
            potionsInWord.RemoveAt(potionsInWord.Count - 1);
            word = word.Substring(0, word.Length - potionToRemove.Letter.Length);  // Substring is (startIndex, length)
            //label.text = word;

            // Remove the last cloud
            Cloud cloudToRemove = clouds[clouds.Count - 1];
            clouds.RemoveAt(clouds.Count - 1);
            Destroy(cloudToRemove.gameObject);
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

    public Vector2 GetPotionArcMidpoint()
    {
        return potionArcMidpoint.transform.position;
    }
}

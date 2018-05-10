using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private Cauldron cauldron;
    [SerializeField] private Potion[] potions;
    [SerializeField] private Text spelledWords;
    [SerializeField] private Text timerText;
    private List<string> lettersInPlay = new List<string>();
    private int timer;

    public static GameController Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.Log("Duplicate GameController instance! Destroy! Destroy!");
            Destroy(this.gameObject);
        }
    }

    // Use this for initialization
    void Start() {
        timer = 75;
        InvokeRepeating("TimerTick", 1, 1);
        ResetAll();
    }

    // Update is called once per frame
    void Update() {

    }

    public void ResetAll()
    {
        cauldron.Clear();   // TODO: depending on how we animate, we may need to create a separate function here
        ResetSpelledWords();
        ResetPotions();
        lettersInPlay.Clear();  // TODO: we'll need to save this first somehow, yeah?
    }

    void ResetPotions()
    {
        // determine letter assortment
        int j = 0;
        for (int i = 0; i < potions.Length; i++)
        {
            switch (i)
            {
                case 0:
                case 1:
                    j = Random.Range(0, 5);     // vowels
                    break;
                case 2:
                case 3:
                    j = Random.Range(5, 26);    // consonants
                    break;
                case 4:
                case 5:
                    j = Random.Range(26, Constants.letters.Length);    // blends
                    break;
                default:
                    j = Random.Range(0, 26);      // fill out the rest of the potions randomly
                    break;
            }
            lettersInPlay.Add(Constants.letters[j]);
        }

        // shuffle list (may be unnecessary in the future)
        for (int i = 0; i < lettersInPlay.Count; i++)
        {
            string temp = lettersInPlay[i];
            int randomIndex = Random.Range(i, lettersInPlay.Count);
            lettersInPlay[i] = lettersInPlay[randomIndex];
            lettersInPlay[randomIndex] = temp;
        }

        for (int i = 0; i < potions.Length; i++)
        {
            potions[i].Init(lettersInPlay[i]);
        }
    }

    void ResetSpelledWords()
    {
        spelledWords.text = "";
    }

    void TimerTick()
    {
        timer--;
        timerText.text = timer.ToString();
        if (timer <= 0)
            SceneManager.LoadScene("Menu");
    }

    public void AddTime()
    {
        timer += 5;
    }

    public void QuitClick()
    { Application.Quit(); }
}

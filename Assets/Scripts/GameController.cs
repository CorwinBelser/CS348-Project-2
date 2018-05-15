using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Cauldron cauldron;
    [SerializeField]
    private Potion[] potions;
    [SerializeField]
    private Book[] books;
    [SerializeField]
    private Text timerText;
    private List<string> lettersInPlay = new List<string>();
    private int timer;
    public List<GameLoopData> History;

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
    void Start()
    {
        History = new List<GameLoopData>();
        timer = 75;
        InvokeRepeating("TimerTick", 1, 1);
        ResetAll();
    }

    public void ResetAll()
    {
        cauldron.Clear();   // TODO: depending on how we animate, we may need to create a separate function here
        ResetBooks();
        ResetPotions();
        lettersInPlay.Clear();  // TODO: we'll need to save this first somehow, yeah?
    }

    void ResetPotions()
    {
        /* Choose a random letter selection */
        string letterCollection = DictionaryManager.Instance.RandomLetterSelection();
        Debug.Log("Chosen letters: " + letterCollection);

        GameLoopData gameData = new GameLoopData(letterCollection);
        History.Add(gameData);

        /* Add all letters and blends */
        foreach (string letters in Constants.letters)
        {
            if (letterCollection.ContainsChars(letters))
                lettersInPlay.Add(letters);
        }
        Debug.Log("Giving potions: " + lettersInPlay.ToDelimitedString());

        // shuffle list (may be unnecessary in the future)
        for (int i = 0; i < lettersInPlay.Count; i++)
        {
            string temp = lettersInPlay[i];
            int randomIndex = Random.Range(i, lettersInPlay.Count);
            lettersInPlay[i] = lettersInPlay[randomIndex];
            lettersInPlay[randomIndex] = temp;
        }

        for (int i = 0; i < Mathf.Min(potions.Length, lettersInPlay.Count); i++)
        {
            potions[i].Init(lettersInPlay[i]);
        }
    }

    void ResetBooks()
    {
        foreach (Book book in books)
        {
            book.Restore();
        }
    }

    public void AddWord(string s)
    {
        /* Add the word to a free book */
        foreach (Book book in books)
        {
            if (!book.Used)
            {
                book.SetText(s);
                break;
            }
        }
        /* Add the found word to the history */
        History[History.Count - 1].AddFoundWord(s);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class GameController : MonoBehaviour
{
    [SerializeField] private Cauldron cauldron;
    [SerializeField] private Potion[] potions;
    [SerializeField] private Book[] books;
    [SerializeField] private Text timerText;
    [SerializeField] private Text roundStartText;
    [SerializeField] private GameObject resultsScreen;
    [SerializeField] private Text foundWords;
    [SerializeField] private Text missedWords;
    [SerializeField] private Button nextPageButton;
    [SerializeField] private Button lastPageButton;
    private List<string> lettersInPlay = new List<string>();
    private int timer;
    private int resultsScreenIndex;
    [SerializeField] private Sprite winSprite;
    [SerializeField] private Sprite loseSprite;
    [SerializeField] private GameObject roundEndClouds;
    public List<GameLoopData> History;

    public static GameController Instance { get; private set; }
    private static int round = 0;
    private int roundWords = 0;

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
        timer = 60;     // adjust this based on performance in previous round
        round++;
        roundWords = (5 + (int)System.Math.Floor(round / 3.0f));    // calculate # of words needed based on round
        ResetAll();     // ResetBooks() needs to color code books appropriately, ResetLetters() needs to pick a collection based on # words needed

        // display round start message (with smoke particle effect)
        roundStartText.text = "Round " + round + "\nMake " + roundWords + " Words!";
        // fade message (and particle effect)

        InvokeRepeating("TimerTick", 8, 1);
    }

    public void ValidateWord(string word, int potionCount)
    {
        if (!History[History.Count - 1].IsWordFound(word))   // first check if word has already been created
        {
            bool valid = DictionaryManager.Instance.ValidWord(word);

            if (valid)
            {
                Debug.Log("valid");
                //TODO: animate word moving up into sky
                /* Play the validWord sound effect */
                AudioManager.Instance.PlayEffect(AudioManager.SoundEffects.ValidWord);
                /* Add the found word to the history */
                History[History.Count - 1].AddFoundWord(word);
                AddWordToBook(word);
                // add extra time for creating long words and using blends
                if (word.Length >= 4)
                {
                    AddTime(word.Length + word.Length - potionCount);
                }
                cauldron.Clear();
            }
            else
            {
                Debug.Log("not valid");
                //TODO: animate cauldron exploding
                /* Play the invalidWord sound effect */
                AudioManager.Instance.PlayEffect(AudioManager.SoundEffects.InvalidWord);
                cauldron.Clear();
            }
        }
        else
        {
            Debug.Log("already made");
            //TODO: animate cauldron exploding
            /* Play the reusedWord sound effect */
            AudioManager.Instance.PlayEffect(AudioManager.SoundEffects.ReusedWord);
            cauldron.Clear();
        }
    }



    public void ResetAll()
    {
        cauldron.Clear();   // TODO: depending on how we animate, we may need to create a separate function here
        ResetBooks();
        ResetPotions();
        lettersInPlay.Clear();
    }

    void ResetPotions()
    {
        /* Choose a random letter selection */
        string letterCollection = DictionaryManager.Instance.RandomLetterSelection();
        Debug.Log("Chosen letters: " + letterCollection);

        GameLoopData gameData = new GameLoopData(letterCollection);
        History.Add(gameData);

        /* Add all letters and blends */
        int lastSingleLetterIndex = -1;
        foreach (string letters in Constants.letters)
        {
            if (letterCollection.ContainsChars(letters))
            {
                lettersInPlay.Add(letters);
                if (letters.Length == 1)
                    lastSingleLetterIndex++;
            }
        }
        Debug.Log("Giving potions: " + lettersInPlay.ToDelimitedString());

        // shuffle list of single letters, then shuffle list of blends separately
            // this ensures that we always distribute all letters before blends, if
            // we run out of potion space
        for (int i = 0; i <= lastSingleLetterIndex; i++)
        {
            string temp = lettersInPlay[i];
            int randomIndex = Random.Range(i, lastSingleLetterIndex+1);
            lettersInPlay[i] = lettersInPlay[randomIndex];
            lettersInPlay[randomIndex] = temp;
        }

        for (int i = lastSingleLetterIndex+1; i < lettersInPlay.Count; i++)
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

        // turn off excess potions
        for (int i=lettersInPlay.Count; i<potions.Length; i++)
        {
            potions[i].gameObject.SetActive(false);
        }
    }

    void ResetBooks()
    {
        for(int i=0; i<books.Length; i++)
        {
            books[i].Restore(i < roundWords);
        }
    }

    public void AddWordToBook(string word)
    {
        /* Add the word to a free book */
        foreach (Book book in books)
        {
            if (!book.Used)
            {
                book.SetText(word);
                break;
            }
        }
    }

    void TimerTick()
    {
        timer--;
        timerText.text = timer.ToString();
        if (timer <= 0)
        {
            CancelInvoke("TimerTick");
            resultsScreenIndex = 0;
            EndGame();
        }
    }

    public void AddTime(int time)
    {
        timer += time;
    }

    void EndGame()
    {
        roundEndClouds.SetActive(true);
        resultsScreen.SetActive(true);
        if(History[resultsScreenIndex].TotalNumberOfWordsFound() >= roundWords)
        {
            resultsScreen.GetComponent<SpriteRenderer>().sprite = winSprite;
        }
        else
        {
            resultsScreen.GetComponent<SpriteRenderer>().sprite = loseSprite;
            round = 0;
        }

        // enable / disable navigation buttons
        if (resultsScreenIndex == 0)
            lastPageButton.interactable = false;
        else
            lastPageButton.interactable = true;
        if (resultsScreenIndex == History.Count - 1)
            nextPageButton.interactable = false;
        else
            nextPageButton.interactable = true;

        // reset text
        foundWords.text = "FOUND WORDS";
        missedWords.text = "MISSED WORDS";
        
        // display word lists
        foreach (string foundWord in History[resultsScreenIndex].GetAllFoundWords())
        {
            foundWords.text = foundWords.text + "\n" + foundWord;
        }
        foreach (string missedWord in History[resultsScreenIndex].GetMissedGradeLevelWords())
        {
            missedWords.text = missedWords.text + "\n" + missedWord;
        }
    }

    public void NextPageClick()
    {
        resultsScreenIndex++;
        EndGame();
    }

    public void LastPageClick()
    {
        resultsScreenIndex--;
        EndGame();
    }

    public void PlayAgainClick()
    { SceneManager.LoadScene("Main"); }

    public void QuitClick()
    { Application.Quit(); }

    public void MenuClick()
    { SceneManager.LoadScene("Menu"); }
}

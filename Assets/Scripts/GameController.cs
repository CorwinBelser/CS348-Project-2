using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private Cauldron cauldron;
    [SerializeField] private Potion[] potions;
    [SerializeField] private Text spelledWords;

    // Singleton may be needed again later but not currrently
    //public static GameController Instance { get; private set; }

    //void Awake()
    //{
    //    if (Instance == null)
    //        Instance = this;
    //    else
    //    {
    //        Debug.Log("Duplicate GameController instance! Destroy! Destroy!");
    //        Destroy(this.gameObject);
    //    }
    //}

    // Use this for initialization
    void Start () {
        ResetAll();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ResetAll()
    {
        cauldron.Clear();   // TODO: depending on how we animate, we may need to create a separate function here
        ResetSpelledWords();
        ResetPotions();
    }

    void ResetPotions()
    {
        foreach (Potion p in potions)
            p.Init();   //TODO: determine the letter here instead and pass it in, to give a less-random distribution
    }

    void ResetSpelledWords()
    {
        spelledWords.text = "";
    }
}

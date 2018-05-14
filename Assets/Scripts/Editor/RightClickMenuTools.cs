using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Linq;

public class RightClickMenuTools {

    private const string WORD_LIST_PATH = "/Resources/Words/full_list.txt";
    private const string COLLECTION_LIST_PATH = "/Resources/Words/LetterCollections.txt";

	[MenuItem("Assets/Create/Letter Collection Database", priority = 21)]
    private static void CreateLetterCollectionDatabase()
    {
        Debug.Log("<color=blue>Generating Letter Collection Database...</color>");
        /* Load the full list of words */
        string[] wordList = IOHelper<string>.LoadTextFile(Application.dataPath + WORD_LIST_PATH, Environment.NewLine);
        Debug.Log(wordList.Length + " words loaded");

        /* Sort the letters of each word */
        List<string> sortedList = new List<string>();
        foreach (string word in wordList)
        {
            string sortedWord = new string(word.Distinct().OrderBy(c => c).ToArray());
            sortedList.Add(sortedWord);
        }
        Debug.Log("Sorted words into collections of letters...");

        /* Filter down to distinct collections of letters */
        List<string> filteredList = sortedList.Distinct().ToList();
        Debug.Log("Filtered letter collections down to: " + filteredList.Count);

        /* Find all collections of letters that are not a subset of another collection */
        List<string> uniqueCollections = new List<string>();
        foreach (string letters in filteredList)
        {
            if (sortedList.Where(w => w.Contains(letters) && w.Length > letters.Length).Count() == 0)
            {
                uniqueCollections.Add(letters);
            }
        }
        Debug.Log(uniqueCollections.Count + " unique collections found.");

        /* Save this list of unique collections to a text file */
        IOHelper<string>.ToTextFile(uniqueCollections.ToDelimitedString(Environment.NewLine), Application.dataPath + COLLECTION_LIST_PATH);
        Debug.Log("File saved to: " + Application.dataPath + COLLECTION_LIST_PATH);
    }
	
}

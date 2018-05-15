using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Linq;

public class RightClickMenuTools {

    private const string WORD_LIST_PATH = "/Resources/Words/DictionarySource";
    private const string GRADE_LEVEL_LIST_PATH = "/Resources/Words/gradeLevelWords.txt";
    private const string COLLECTION_LIST_PATH = "/Resources/Words/letterCollections.txt";
    private static char[] VALID_LETTERS =
    {
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
    };

	[MenuItem("Tools/Generate Letter Collection Database", priority = 1)]
    private static void CreateLetterCollectionDatabase()
    {
        Debug.Log("<color=blue>Generating Letter Collection Database...</color>");
        /* Load the full list of words */
        List<string> wordList = LoadAllWords();
        //string[] wordList = IOHelper<string>.LoadTextFile(Application.dataPath + WORD_LIST_PATH, Environment.NewLine);
        Debug.Log(wordList.Count + " words loaded");

        /* Remove any words that have illegal characters */
        for (int i = wordList.Count - 1; i >= 0; i--)
        {
            foreach (char letter in wordList[i])
            {
                if (!VALID_LETTERS.Contains(letter))
                {
                    wordList.RemoveAt(i);
                    break;
                }
            }
        }

        /* Sort the list of words alphabetically */
        wordList.Sort();

        /* Save this list of words as the grade level list */
        IOHelper<string>.ToTextFile(wordList.ToDelimitedString(Environment.NewLine), Application.dataPath + GRADE_LEVEL_LIST_PATH);

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
            bool unique = true;
            foreach (string wordToCheck in filteredList)
            {
                if (letters.Length >= wordToCheck.Length)
                    continue;
                if (!wordToCheck.ContainsChars(letters))
                {
                    unique = false;
                    break;
                }
            }

            if (unique)
                uniqueCollections.Add(letters);
        }
        Debug.Log(uniqueCollections.Count + " unique collections found.");

        /* Sort the list of unique collections */
        uniqueCollections.Sort();

        /* Save this list of unique collections to a text file */
        IOHelper<string>.ToTextFile(uniqueCollections.ToDelimitedString(Environment.NewLine), Application.dataPath + COLLECTION_LIST_PATH);
        Debug.Log("File saved to: " + Application.dataPath + COLLECTION_LIST_PATH);
    }

    private static List<string> LoadAllWords()
    {
        List<string> wordList = new List<string>();
        foreach (string path in Directory.GetFiles(Application.dataPath + WORD_LIST_PATH))
        {
            wordList.AddRange(IOHelper<string>.LoadTextFile(path, Environment.NewLine));
        }

        return wordList;
    }

    [MenuItem("Tools/Analyse Letter Collection Database", priority = 1)]
    private static void PrintLetterCollectionStats()
    {
        /* Load the list of words */
        List<string> allWords = LoadAllWords();

        /* Load the list of letter collections */
        string[] letterCollections = IOHelper<string>.LoadTextFile(Application.dataPath + COLLECTION_LIST_PATH, Environment.NewLine);
        int[] wordCounts = new int[letterCollections.Length];

        int minCollection = 0, maxCollection = 0, sumCollection = 0;

        for(int i = 0; i < letterCollections.Length; i++)
        {
            if (letterCollections[i].Length < letterCollections[minCollection].Length)
                minCollection = i;
            if (letterCollections[i].Length > letterCollections[maxCollection].Length)
                maxCollection = i;
            sumCollection += letterCollections[i].Length;
            /* Calculate the number of words that this letter collection can make */
            foreach (string word in allWords)
            {
                if (!letterCollections[i].ContainsChars(word))
                    wordCounts[i] += 1;
            }
        }

        Debug.Log("Minimum collection size: " + letterCollections[minCollection].Length + " (" + letterCollections[minCollection] + ")");
        Debug.Log("Maximum collection size: " + letterCollections[maxCollection].Length + " (" + letterCollections[maxCollection] + ")");
        Debug.Log("Average collection size: " + (sumCollection / letterCollections.Length));


        /* Sum up all lengths */
        int min = 0, max = 0, sum = 0;
        for(int i = 0; i < wordCounts.Length; i++)
        {
            if (wordCounts[i] < wordCounts[min])
                min = i;
            if (wordCounts[i] > wordCounts[max])
                max = i;
            sum += wordCounts[i];
        }

        Debug.Log("Minimum number of words in a collection: " + wordCounts[min] + " (" + letterCollections[min] + ")");
        Debug.Log("Maximum number of words in a collection: " + wordCounts[max] + " (" + letterCollections[max] + ")");
        Debug.Log("Average number of words in a collection: " + (sum / wordCounts.Length));

    }

}

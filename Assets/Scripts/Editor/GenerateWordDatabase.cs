using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;
using EditorCoroutines;

public class GenerateWordDatabase : EditorWindow
{

    private static char[] alphabet = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
    private static string[] wordList;
    private static WordDatabase database;
    public static GenerateWordDatabase Instance { get; private set; }
    private static string directoryPath;
    private static string filePath;

    /// <summary>
    /// Creates a "WordDatabase.bytes" & "WordDatabase.txt" in the current folder.
    /// </summary>
    /// <remarks>
    /// These files *must* be generated (or moved) under a "Resources" folder to be loaded
    /// </remarks>
    [MenuItem("Assets/Create/Word Database", priority = 21)]
    private static void CreateWordDatabaseAsset()
    {
        Instance = new GenerateWordDatabase();
        UnityEngine.Object selectedAsset = Selection.activeObject;
        if (selectedAsset != null)
        {
            directoryPath = AssetDatabase.GetAssetPath(selectedAsset);
            if (System.IO.Directory.Exists(directoryPath))
            {
                filePath = directoryPath + "/" + Constants.wordDatabaseFilePath + Constants.wordDatabaseFileExtension;

                /* Delete the old database file if one exists */
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);

                /* Generate the WordDatabase */
                database = new WordDatabase();
                Instance.StartCoroutine(BuildWordDatabase());
            }
        }
    }

    private static void OnFinishCoroutine()
    {
        /* TESTING: Save the database as a text file for viewing */
        IOHelper<WordDatabase>.ToTextFile(database, directoryPath + "/WordDatabase.txt");

        /* Save the database */
        IOHelper<WordDatabase>.SerializeObject(database, filePath);

        /* Refresh the asset database to show the Readme file */
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// Creates a dictionary of keys 1-5
    /// For each level, takes every fraction combination from the previous level and adds an atomic fraction to it to generate the next level
    /// </summary>
    /// <returns>
    /// WordDatabase consisting of a Dictionary with 5 levels of fraction combinations
    ///     Level 1: All fractions made up of adding 1 atomic fraction from 1/2 to 1/10
    ///     Level 2: All fractions made up of adding 2 atomic fraction
    /// </returns>
    private static IEnumerator BuildWordDatabase()
    {
        TextAsset dictionary = Resources.Load<TextAsset>("Dictionary");
        wordList = dictionary.text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

        /* Create every unique combination of 10 letters */
        for (int c1 = 0; c1 < alphabet.Length; c1++)
        {
            for (int c2 = c1 + 1; c2 < alphabet.Length; c2++)
            {
                for (int c3 = c2 + 1; c3 < alphabet.Length; c3++)
                {
                    for (int c4 = c3 + 1; c4 < alphabet.Length; c4++)
                    {
                        for (int c5 = c4 + 1; c5 < alphabet.Length; c5++)
                        {
                            for (int c6 = c5 + 1; c6 < alphabet.Length; c6++)
                            {
                                for (int c7 = c6 + 1; c7 < alphabet.Length; c7++)
                                {
                                    for (int c8 = c7 + 1; c8 < alphabet.Length; c8++)
                                    {
                                        for (int c9 = c8 + 1; c9 < alphabet.Length; c9++)
                                        {
                                            for (int c10 = c9 + 1; c10 < alphabet.Length; c10++)
                                            {
                                                char[] letters = new[] { alphabet[c1], alphabet[c2], alphabet[c3], alphabet[c4], alphabet[c5], alphabet[c6], alphabet[c7], alphabet[c8], alphabet[c9], alphabet[c10] };
                                                Debug.Log(new string(letters));
                                                WordCollection wordCollection = FindPossibleWordsFromLetters(letters);
                                                if (wordCollection.Count > 4)
                                                {
                                                    if (!database.Data.ContainsKey(wordCollection.Count))
                                                        database.Data[wordCollection.Count] = new List<WordCollection>();
                                                    database.Data[wordCollection.Count].Add(wordCollection);
                                                }
                                                yield return new WaitForSeconds(0f);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        OnFinishCoroutine();
        yield return null;
    }

    private static WordCollection FindPossibleWordsFromLetters(char[] letters)
    {
        WordCollection wordData = new WordCollection(letters);
        foreach (string word in wordList)
        {
            bool isFound = true;
            foreach (char letter in word)
            {
                if (!letters.Contains(letter))
                {
                    isFound = false;
                    break;
                }
            }
            if (isFound)
                wordData.Words.Add(new WordData(word));
        }
        Debug.Log("  Found " + wordData.Count + " words");
        return wordData;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.IO;
using System.Xml.Serialization;

public class WordManager : MonoBehaviour {

    private Word[] words; /* Holds the entire English language */
    private const string DICTIONARY_PATH = "/Resources/Dictionary.xml";

    public static WordManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            TryLoadDictionary();
        }
        else
        {
            Debug.Log("Duplicate instance of WordManager! Destroying " + this.name);
            Destroy(this.gameObject);
        }
    }

    private void TryLoadDictionary()
    {
        //Check if the Dictionary object exists
        if (File.Exists(Application.dataPath + DICTIONARY_PATH))
            DeSerializeDictionary();
        else
            BuildDictionary();
    }

    private void BuildDictionary()
    {
        List<Word> wordList = new List<Word>();
        string directory = Application.dataPath + "/Resources/Words/";
        foreach (string path in Directory.GetFiles(directory))
        {
            if (path.Contains("grade_") && !path.Contains(".meta"))
            {
                Debug.Log("Loading file: " + path);
                //Get the grade level
                int gradeLevel = 1;
                int startIndex = path.LastIndexOf('/') + "/grade_".Length;
                string sub = path.Substring(startIndex, path.Length - ".txt".Length - startIndex);
                //Debug.Log("sub: _" + sub + "_");
                //0123456789
                //grade_1.txt
                int.TryParse(sub, out gradeLevel);
                //Debug.Log("Grade Level: " + gradeLevel);
                foreach(string word in System.IO.File.ReadAllLines(path))
                {
                    //Debug.Log("Word: _" + word + "_");
                    wordList.Add(new Word(word, gradeLevel));
                }
            }
        }

        //Save the dictionary to the persistantDataPath for easier loading next time
        words = wordList.ToArray();
        SerializeDictionary();
    }

    public bool ValidWord(string word)
    {
        return words.Where(w => w.value.ToLower() == word.ToLower()).Count() > 0;
    }

    private void SerializeDictionary()
    {
        ListOfWords listOfWords = new ListOfWords(words);

        // Make the XmlSerializer and StringWriter.
        XmlSerializer xml_serializer =
            new XmlSerializer(typeof(ListOfWords));
        using (StringWriter string_writer = new StringWriter())
        {
            // Serialize.
            xml_serializer.Serialize(string_writer, listOfWords);

            // Write to the file.
            File.WriteAllText(Application.dataPath + DICTIONARY_PATH, string_writer.ToString());
        }
    }

    private void DeSerializeDictionary()
    {
        string data = File.ReadAllText(Application.dataPath + DICTIONARY_PATH);
        // Deserialize the serialization.
        XmlSerializer xml_serializer =
            new XmlSerializer(typeof(ListOfWords));
        using (StringReader string_reader =
            new StringReader(data))
        {
            ListOfWords listOfWords =
                (ListOfWords)(xml_serializer.Deserialize(string_reader));

            words = listOfWords.Words;
        }
    }
}

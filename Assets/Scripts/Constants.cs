using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    [SerializeField] private Sprite[] serializedPotionSprites;
    public static Sprite[] potionSprites;   // your move, Unity
    public static string[] letters = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "G", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
    public static string[] blends;  //TODO: should this just continue from letters?

    public static Color[] potionColors = new Color[] { Color.blue, Color.green, Color.red, Color.yellow };


    private void Awake()
    {
        potionSprites = serializedPotionSprites;
    }
}

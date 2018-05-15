using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Book : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Text text;
    private bool isUsed = false;

    public bool Used
    { get { return isUsed; } }


    public void SetText(string s)
    {
        text.text = s;
        isUsed = true;
    }

    public void Restore()
    {
        isUsed = false;
        text.text = "";
        int i = Random.Range(0, Constants.bookSprites.Length);
        sprite.sprite = Constants.bookSprites[i];
    }
}

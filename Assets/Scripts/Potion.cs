using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Potion : MonoBehaviour
{
    [SerializeField] private Cauldron cauldron;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Text label;
    private string letter = "";
    //private bool used = false;

    public string Letter
    { get { return letter; } }

    public void Init(string s)
    {
        //Restore();
        letter = s;
        label.text = letter;
        int i = Random.Range(0, Constants.potionSprites.Length);
        sprite.sprite = Constants.potionSprites[i];
        i = Random.Range(0, Constants.potionColors.Length);
        sprite.color = Constants.potionColors[i];
        gameObject.SetActive(true);
    }

    private void OnMouseUp()
    {
        //if(!used)
        //{
            //TODO: animate to cauldron
            //used = true;
            cauldron.AddPotion(this);
            //sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
            //label.color = new Color(label.color.r, label.color.g, label.color.b, 0.5f);
        //}
    }

    public void Restore()   // "Reset" is a protected keyword
    {
        //used = false;
        //sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
        //label.color = new Color(label.color.r, label.color.g, label.color.b, 1f);
    }
}

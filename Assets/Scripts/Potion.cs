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
    private Vector3 startPosition;
    private bool isMoving = false;
    //private bool used = false;

    public string Letter
    { get { return letter; } }

    public void Init(string s)
    {
        Restore();
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
        //        cauldron.AddPotion(this);
        isMoving = true;
            //sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
            //label.color = new Color(label.color.r, label.color.g, label.color.b, 0.5f);
        //}
    }

    public void Restore()   // "Reset" is a protected keyword
    {
        transform.position = startPosition;
        isMoving = false;
        //used = false;
        //sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
        //label.color = new Color(label.color.r, label.color.g, label.color.b, 1f);
    }

    void Awake()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (isMoving)
            MoveToCauldron();
        else
            ReturnToShelf();        
    }

    private void MoveToCauldron()
    {
        Vector3 destination = cauldron.gameObject.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * 20f);

        if (IsWithin(transform.position, destination))  // add potion when destination is reached
        {
            isMoving = false;
            cauldron.AddPotion(this);
        }
    }

    private void ReturnToShelf()
    {
        if (IsWithin(transform.position, startPosition))
            return;

        transform.position = Vector3.MoveTowards(transform.position, startPosition, Time.deltaTime * 20f);
    }

    private bool IsWithin(Vector3 obj1, Vector3 obj2)
    {
        float tolerance = 0.15f;
        if ((Mathf.Abs(obj1.x - obj2.x) < tolerance) && (Mathf.Abs(obj1.y - obj2.y) < tolerance))
            return true;
        else
            return false;
    }
}

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
    private Vector2 startPosition;
    private Coroutine animationCR;

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
        if (animationCR == null)
            animationCR = StartCoroutine(MoveToCauldron());
    }

    public void Restore()   // "Reset" is a protected keyword
    {
        if (animationCR != null)
        {
            StopCoroutine(animationCR);
            animationCR = null;
        }
        transform.position = startPosition;
        transform.rotation = Quaternion.identity;
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
    }

    void Awake()
    {
        startPosition = transform.position;
    }

    private IEnumerator MoveToCauldron()
    {
        /* Trigger a throw sound effect */
        AudioManager.Instance.PlayEffect(AudioManager.SoundEffects.PotionThrow);
        Vector2 destination = cauldron.gameObject.transform.position;
        Quaternion rotation = Quaternion.Euler(new Vector3(0f, 0f, Random.Range(1f, 30f)));
        foreach (Vector2 position in CoolStuff.PositionOverParabola(startPosition, cauldron.GetPotionArcMidpoint(), destination, .5f))
        {
            transform.position = position;
            transform.Rotate(rotation.eulerAngles);
            yield return null;
        }

        /* Trigger a bottle break sound effect */
        AudioManager.Instance.PlayEffect(AudioManager.SoundEffects.PotionBreak);
        /* The potion is now at the cauldron */
        cauldron.AddPotion(this);
        yield return new WaitForSeconds(.5f);

        /* Trigger the return animation */
        animationCR = StartCoroutine(ReturnToShelf());
    }

    private IEnumerator ReturnToShelf()
    {
        //while (!IsWithin(transform.position, startPosition))
        //{
        //    transform.position = Vector2.MoveTowards(transform.position, startPosition, Time.deltaTime * 20f);
        //    yield return null;
        //}

        /* Kill the alpha */
        Color original = sprite.color;
        sprite.color = new Color(original.r, original.g, original.b, 0f);

        /* Snap into place */
        transform.position = startPosition;
        transform.rotation = Quaternion.identity;

        /* Fade the potion back in */
        for (float alpha = 0f; alpha < 1f; alpha += Time.deltaTime) /* 2 seconds to reach full alpha */
        {
            sprite.color = new Color(original.r, original.g, original.b, alpha);
            yield return null;
        }
        /* Snap to original color */
        sprite.color = original;
        animationCR = null;
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

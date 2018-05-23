using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cloud : MonoBehaviour
{
    // cloud behaves as a state machine
    public enum State { RiseFromCauldron, Bob, SlideOver, MoveToBook };

    [SerializeField] private float width;
    [SerializeField] private float speed;
    [SerializeField] private Text text;
    private Vector2 targetPosition;
    private Vector2 cauldronPosition;
    private State state;

    public string Text
    { set { text.text = value; } }

    public Vector2 TargetPosition
    { set { targetPosition = value; } }

    void Awake()
    {
        cauldronPosition = transform.parent.transform.position;
    }

    public void SetState(State newState, int totalClouds)
    {
        switch (newState)
        {
            case (State.RiseFromCauldron):
                targetPosition = new Vector2((0.5f * width) * (totalClouds - 1), 2.5f) + cauldronPosition;
                break;
            case (State.Bob):

                break;
            case (State.SlideOver):
                targetPosition = new Vector2(transform.position.x - (0.5f * width), transform.position.y);
                break;
            case (State.MoveToBook):

                break;
        }
        state = newState;
    }


    void Update()
    {
        switch(state)
        {
            case (State.RiseFromCauldron):
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 20f);
                break;
            case (State.Bob):

                break;
            case (State.SlideOver):
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 20f);
                break;
            case (State.MoveToBook):

                break;
        }
        // random bobbing movement

        // move to book and destroy
    }

}

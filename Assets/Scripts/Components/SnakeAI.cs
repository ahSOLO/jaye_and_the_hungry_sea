using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeAI : MonoBehaviour
{
    [SerializeField] GameObject headTop;
    [SerializeField] GameObject headBottom;

    [SerializeField] float openingSpeed;
    [SerializeField] float closingSpeed;

    float openTopYDestination;
    [SerializeField] float closedTopYDestination;
    float openBottomYDestination;
    [SerializeField] float closedBottomYDestination;

    [SerializeField] float stayOpenTimerMax;
    [SerializeField] float stayClosedTimerMax;
    [SerializeField] float shakingTimerMax;

    Vector3 initialPosition;

    float timer;
    bool enteringState;
    enum State { opening, open, shaking, closing, closed }
    State state;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        state = State.open;
        enteringState = true;
        openTopYDestination = headTop.transform.position.y;
        openBottomYDestination = headBottom.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        
        switch (state)
        {
            case State.opening:
                headTop.transform.position = new Vector3(
                    headTop.transform.position.x, 
                    Mathf.MoveTowards(headTop.transform.position.y, openTopYDestination, openingSpeed), 
                    headTop.transform.position.z
                    );
                headBottom.transform.position = new Vector3(
                    headBottom.transform.position.x,
                    Mathf.MoveTowards(headBottom.transform.position.y, openBottomYDestination, openingSpeed),
                    headBottom.transform.position.z
                    );
                if (headTop.transform.position.y == openTopYDestination && headBottom.transform.position.y == openBottomYDestination)
                {
                    state = State.open;
                    enteringState = true;
                }
                break;
            case State.open:
                if (enteringState)
                {
                    timer = stayOpenTimerMax;
                    enteringState = false;
                }
                if (timer <= 0)
                {
                    state = State.shaking;
                    enteringState = true;
                }
                break;
            case State.shaking:
                if (enteringState)
                {
                    timer = shakingTimerMax;
                    enteringState = false;
                }
                transform.position = new Vector3(initialPosition.x + Random.Range(-0.1f, 0.1f), initialPosition.y + Random.Range(-0.1f, 0.1f), initialPosition.z);
                if (timer <= 0)
                {
                    transform.position = initialPosition;
                    state = State.closing;
                    enteringState = true;
                }
                break;
            case State.closing:
                headTop.transform.position = new Vector3(
                    headTop.transform.position.x, 
                    Mathf.MoveTowards(headTop.transform.position.y, closedTopYDestination, closingSpeed), 
                    headTop.transform.position.z
                    );
                headBottom.transform.position = new Vector3(
                    headBottom.transform.position.x,
                    Mathf.MoveTowards(headBottom.transform.position.y, closedBottomYDestination, closingSpeed),
                    headBottom.transform.position.z
                    );
                if (headTop.transform.position.y == closedTopYDestination && headBottom.transform.position.y == closedBottomYDestination)
                {
                    state = State.closed;
                    enteringState = true;
                }
                break;
            case State.closed:
                if (enteringState)
                {
                    timer = stayClosedTimerMax;
                    enteringState = false;
                }
                if (timer <= 0)
                {
                    state = State.opening;
                    enteringState = true;
                }
                break;
        }
    }
}

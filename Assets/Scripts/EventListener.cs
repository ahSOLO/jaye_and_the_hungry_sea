using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour
{
    public List<EventAndResponse> eventAndResponses = new List<EventAndResponse>();
    
    private void OnEnable()
    {
        if (eventAndResponses.Count >= 1)
        {
            foreach (EventAndResponse eAndR in eventAndResponses)
            {
                eAndR.gameEvent.Register(this);
            }
        }
    }

    private void OnDisable()
    {
        if (eventAndResponses.Count >= 1)
        {
            foreach (EventAndResponse eAndR in eventAndResponses)
            {
                eAndR.gameEvent.Unregister(this);
            }
        }
    }

    [ContextMenu("Raise Events")]
    public void OnEventOccurs(Event passedEvent)
    {
        for (int i = eventAndResponses.Count - 1; i >= 0; i--)
        {
            // Check if the passed event is the correct one
            if (passedEvent == eventAndResponses[i].gameEvent)
            {
                eventAndResponses[i].EventRaised();
            }
        }
    }
}

[System.Serializable]
public class EventAndResponse
{
    public string name;
    public Event gameEvent;
    public UnityEvent response;
    // public ResponseWithString responseForSentString;
    // public ResponseWithInt responseForSentInt;
    public ResponseWithFloat responseForSentFloat;
    // public ResponseWithBool responseForSentBool;

    public void EventRaised()
    {
        // default/generic
        if (response.GetPersistentEventCount() >= 1) // always check if at least 1 object is listening for the event
        {
            response.Invoke();
        }

        /*
        // string
        if (responseForSentString.GetPersistentEventCount() >= 1)
        {
            responseForSentString.Invoke(gameEvent.sentString);
        }

        // int
        if (responseForSentInt.GetPersistentEventCount() >= 1)
        {
            responseForSentInt.Invoke(gameEvent.sentInt);
        }
        */

        // float
        if (responseForSentFloat.GetPersistentEventCount() >= 1)
        {
            responseForSentFloat.Invoke(gameEvent.sentFloat);
        }

        /*
        // bool
        if (responseForSentBool.GetPersistentEventCount() >= 1)
        {
            responseForSentBool.Invoke(gameEvent.sentBool);
        }
        */
    }
}

[System.Serializable]
public class ResponseWithString : UnityEvent<string>
{
}

[System.Serializable]
public class ResponseWithInt : UnityEvent<int>
{
}

[System.Serializable]
public class ResponseWithFloat : UnityEvent<float>
{
}

[System.Serializable]
public class ResponseWithBool : UnityEvent<bool>
{
}
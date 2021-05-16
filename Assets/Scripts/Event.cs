using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEvent", menuName = "Game Event", order = 52)]
public class Event : ScriptableObject
{
    public string sentString;
    public int sentInt;
    public float sentFloat;
    public bool sentBool;

    private HashSet<EventListener> eListeners = new HashSet<EventListener>();

    public void Register(EventListener listener)
    {
        eListeners.Add(listener);
    }

    public void Unregister(EventListener listener)
    {
        eListeners.Remove(listener);
    }

    public void Occurred()
    {
        foreach (var eListener in eListeners)
        {
            eListener.OnEventOccurs(this);
        }
    }

    public void Destroyed()
    {
        foreach (var eListener in eListeners)
        {
            foreach (var eAndR in eListener.eventAndResponses)
            {
                eAndR.gameEvent.Unregister(eListener);
            }
        }
    }
}

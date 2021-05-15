using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEvent", menuName = "Game Event", order = 52)]
public class Event : ScriptableObject
{
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
            eListener.OnEventOccurs();
        }
    }

    public void Destroyed()
    {
        foreach (var eListener in eListeners)
        {
            eListener.gEvent.Unregister(eListener);
        }
    }
}

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Event))]
public class EventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        Event e = target as Event;

        if (GUILayout.Button("Raise"))
        {
            e.Occurred();
        }
    }
}
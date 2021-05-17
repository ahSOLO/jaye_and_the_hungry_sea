using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager iM;

    public Dictionary<int, Bottle> inventory = new Dictionary<int, Bottle>();
    public int currentNote;
    public TextAsset defaultText;
    // private GameObject player;

    // Start is called before the first frame update
    void OnEnable()
    {
        DontDestroyOnLoad(gameObject); // Persist between scenes

        //Assign Singleton
        if (iM == null) iM = this;
        else Destroy(gameObject);

        currentNote = 1;
    }

    public TextAsset GetNoteContent(int noteId)
    {
        if (inventory.ContainsKey(currentNote))
        {
            return inventory[currentNote].textAsset;
        }
        else return defaultText;
    } 

    public string GetNoteTitle(int nodeId)
    {
        if (inventory.ContainsKey(currentNote))
        {
            return inventory[currentNote].title;
        }
        else return "Note #" + currentNote.ToString();
    }

    public void AddNote(Bottle bottle)
    {
        if (!inventory.ContainsKey(bottle.id)) inventory.Add(bottle.id, bottle);
        currentNote = bottle.id;
    }
}

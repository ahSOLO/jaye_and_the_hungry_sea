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
    // public List<Bottle> inventory = new List<Bottle>();
    private GameObject player;

    // Start is called before the first frame update
    void OnEnable()
    {
        DontDestroyOnLoad(gameObject); // Persist between scenes
        SceneManager.sceneLoaded += OnSceneLoad;

        //Assign Singleton
        if (iM == null) iM = this;
        else Destroy(gameObject);

        currentNote = 1;
    }

    public void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public TextAsset ParseNote(int noteId)
    {
        if (inventory.ContainsKey(currentNote) && inventory[currentNote].isCollected)
        {
            return inventory[currentNote].textAsset;
        }
        else return defaultText;
    } 

    public string ParseTitle(int nodeId)
    {
        if (inventory.ContainsKey(currentNote) && inventory[currentNote].isCollected)
        {
            return inventory[currentNote].title;
        }
        else return "Note #" + currentNote.ToString();
    }

}

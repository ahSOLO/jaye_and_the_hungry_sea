using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BottleProperties : MonoBehaviour
{
    public int bottleId;
    public TextAsset text;
    public string title;

    Bottle bottle = new Bottle();
    
    void Start()
    {
        bottle.id = bottleId;
        bottle.textAsset = text;
        bottle.title = title;
    }

    // Collect the bottle - called when player collides with bottle.
    public void Collect()
    {
        // isCollected is used to determine whether the note can be shown to the player in the inventory menu.
        bottle.isCollected = true;
        // Add the bottle to inventory if it hasn't already been collected.
        if (!InventoryManager.iM.inventory.ContainsKey(bottleId)) InventoryManager.iM.inventory.Add(bottleId, bottle);
        InventoryManager.iM.currentNote = bottleId;
        Destroy(gameObject);
    }
}

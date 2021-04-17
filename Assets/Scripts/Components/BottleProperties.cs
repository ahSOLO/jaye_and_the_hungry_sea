using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleProperties : MonoBehaviour
{
    public int bottleId;
    public TextAsset text;
    public string title;

    Bottle bottle = new Bottle();
    
    // Start is called before the first frame update
    void Start()
    {
        bottle.id = bottleId;
        bottle.textAsset = text;
        bottle.title = title;
    }

    public void Collect()
    {
        bottle.isCollected = true;
        InventoryManager.iM.inventory.Add(bottleId, bottle);
        InventoryManager.iM.currentNote = bottleId;
        Destroy(gameObject);
    }
}

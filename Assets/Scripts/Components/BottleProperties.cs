using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleProperties : MonoBehaviour
{
    public int bottleId;

    Bottle bottle = new Bottle();
    
    // Start is called before the first frame update
    void Start()
    {
        bottle.id = bottleId;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Collect()
    {
        bottle.isCollected = true;
        InventoryManager.iM.inventory.Add(bottle);
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager iM;

    public GameObject player;
    public List<Bottle> inventory = new List<Bottle>();

    // Start is called before the first frame update
    void OnEnable()
    {
        //Assign Singleton
        if (iM == null) iM = this;
        else Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

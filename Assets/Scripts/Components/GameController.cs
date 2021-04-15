using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController gC;
    
    // Start is called before the first frame update
    void Start()
    {
        //Assign Singleton
        if (gC == null) gC = this;
        else Destroy(gameObject);

        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

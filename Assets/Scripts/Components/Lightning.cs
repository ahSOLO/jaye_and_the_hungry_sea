using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void StartFlash()
    {
        EffectsController.eC.isFlashing = true;
    }

    void StopFlash()
    {
        EffectsController.eC.isFlashing = false;
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}

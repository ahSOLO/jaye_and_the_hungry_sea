using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    // Called in Lightning animation event.
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

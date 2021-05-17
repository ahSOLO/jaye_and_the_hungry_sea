using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class Lightning : MonoBehaviour
{
    [SerializeField] BoolGameEvent LightningEffect;
    
    // Called in Lightning animation event.
    void StartFlash()
    {
        LightningEffect.Raise(true);
    }

    void StopFlash()
    {
        LightningEffect.Raise(false);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InControlSingleton : MonoBehaviour
{
    public static InControlSingleton iC;

    // Start is called before the first frame update

    private void Awake()
    {
        if (iC == null) iC = this;
        else Destroy(gameObject);        
    }
}

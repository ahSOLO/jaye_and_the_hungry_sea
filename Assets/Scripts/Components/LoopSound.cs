using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopSound : MonoBehaviour
{
    public AudioSource sound;
    public float delayBetweenLoops;
    private float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        timer = delayBetweenLoops;
    }

    // Update is called once per frame
    void Update()
    {
        if (!sound.isPlaying)
        {
            timer -= Time.deltaTime;
        }

        if (timer < 0)
        {
            sound.Play();
            timer = delayBetweenLoops;
        }
    }
}

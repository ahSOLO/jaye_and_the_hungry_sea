using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    public static FadeController fC;

    public GameObject faderObj;
    private Image faderImg;

    // Start is called before the first frame update
    void Start()
    {
        fC = this;
        
        faderObj.SetActive(true);
        faderImg = faderObj.GetComponent<Image>();

        // Fade in the Scene
        StartCoroutine(Fade(0f, 3f));
    }

    public IEnumerator Fade(float target, float timer)
    {
        float currentTime = 0f;
        float start = faderImg.color.a;

        while (currentTime < timer)
        {
            currentTime += Time.deltaTime;
            Color newColor = new Color(faderImg.color.r, faderImg.color.g, faderImg.color.b, Mathf.SmoothStep(start, target, currentTime / timer));
            faderImg.color = newColor;
            yield return null;
        }
        yield break;
    }
}

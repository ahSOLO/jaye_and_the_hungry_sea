using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FontContainer : MonoBehaviour
{
    public static FontContainer fC;

    public TMP_FontAsset defaultFont;
    public TMP_FontAsset anxietyFont;
    public TMP_FontAsset paranoiaFont;
    public TMP_FontAsset guiltFont;
    public TMP_FontAsset liesFont;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (fC == null) fC = this;
        else Destroy(this);
    }

}

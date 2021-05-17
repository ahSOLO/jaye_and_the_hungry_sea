using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue : MonoBehaviour
{
    public string content;
    public float duration;
    public int characterId = 1;
    public int barkId;
    public int triggerId;
    public bool pauseGame;
}

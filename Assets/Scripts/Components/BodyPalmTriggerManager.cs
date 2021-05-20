using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPalmTriggerManager : MonoBehaviour
{
    public bool isTouchingPlayer = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isTouchingPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isTouchingPlayer = false;
        }
    }
}

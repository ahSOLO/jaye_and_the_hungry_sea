using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyTorsoTriggerManager : MonoBehaviour
{
    public bool isTouchingLargeTrigger = false;
    public bool isTouchingMediumTrigger = false;
    public bool isTouchingSmallTrigger = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "BodyTriggerLarge")
        {
            isTouchingLargeTrigger = true;
        }

        if (collision.tag == "BodyTriggerMedium")
        {
            isTouchingMediumTrigger = true;
        }

        if (collision.tag == "BodyTriggerSmall")
        {
            isTouchingSmallTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "BodyTriggerLarge")
        {
            isTouchingLargeTrigger = false;
        }

        if (collision.tag == "BodyTriggerMedium")
        {
            isTouchingMediumTrigger = false;
        }

        if (collision.tag == "BodyTriggerSmall")
        {
            isTouchingSmallTrigger = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOfHandsAI : MonoBehaviour
{
    bool isMoving = false;
    [SerializeField] float moveSpeed = 2.4f;
    float adjustedMoveSpeed;

    private void Start()
    {
        adjustedMoveSpeed = moveSpeed - (0.15f * GameController.gC.fails);
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            transform.Translate(Vector2.up * adjustedMoveSpeed * Time.fixedDeltaTime, Space.World);
        }
    }

    public void StartMovement()
    {
        isMoving = true;
    }
}

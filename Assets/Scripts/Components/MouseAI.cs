using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAI : MonoBehaviour
{
    [SerializeField] Vector3 position1;
    [SerializeField] Vector3 position2;
    bool isTargetingPos1 = true;
    Vector3 targetPosition;
    [SerializeField] float rotationSpeed = 180f;
    [SerializeField] float moveSpeed = 2.5f;
    enum State { rotating, moving };
    State state;
    
    void Start()
    {
        state = State.rotating;
        targetPosition = position1;
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case State.rotating:
                var oldRotation = transform.rotation;
                transform.rotation = Helper.RotateTowardsOnZAxis(targetPosition, transform.position, transform.rotation, 90f, rotationSpeed * Time.fixedDeltaTime);
                if (Quaternion.Angle(oldRotation, transform.rotation) == 0f)
                {
                    state = State.moving;
                }
                break;
            case State.moving:
                transform.Translate(-transform.up * moveSpeed * Time.fixedDeltaTime, Space.World);
                break;
        }
    }

    void Update()
    {
        switch (state)
        {
            case State.moving:
                if ((transform.position - targetPosition).sqrMagnitude < 1)
                {
                    state = State.rotating;
                    if (isTargetingPos1)
                    {
                        targetPosition = position2;
                        isTargetingPos1 = false;
                    } 
                    else
                    {
                        targetPosition = position1;
                        isTargetingPos1 = true;
                    }
                }
                break;
        }
    }
}

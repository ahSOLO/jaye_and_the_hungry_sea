using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 4f;
    [SerializeField] private float rotationSpeed = 2.5f;
    [SerializeField] private float acceleration = 0.05f;
    private float speed;
    private Rigidbody2D rb;
    private Vector3 inputDirection;
    private Vector3 lastDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    // Update is called once per frame
    void Update()
    {
        inputDirection.x = Input.GetAxisRaw("Horizontal");
        inputDirection.y = Input.GetAxisRaw("Vertical");
    }

    // Move function
    void MoveCharacter()
    {        
        if (inputDirection != Vector3.zero)
        {
            lastDirection = inputDirection;
            float angle = -90 + Mathf.Atan2(lastDirection.y, lastDirection.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            rb.MoveRotation(Quaternion.RotateTowards(this.transform.rotation, rotation, rotationSpeed));

            speed = Mathf.Lerp(speed, maxSpeed, acceleration);
        } 
        else
        {
            speed = Mathf.Lerp(speed, 0, acceleration);
        }

        rb.velocity = transform.up * speed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement variables
    [SerializeField] private float maxSpeed = 4f;
    [SerializeField] private float rotationSpeed = 2.5f;
    [SerializeField] private float acceleration = 0.05f;
    private float speed;
    private Vector3 inputDirection;
    private Vector3 lastDirection;
    
    // Components
    private Rigidbody2D rb;
    private Animator anim;

    // Wall variables
    [SerializeField] private GameObject background;   
    private Vector3 velocityOffset;
    private bool isTouchingWall;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        MoveCharacter();

        // FixedUpdate happens before OnTriggerStay so this defaults to false each frame
        isTouchingWall = false;
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

            anim.SetBool("isRowing", true);
        } 
        else
        {
            speed = Mathf.Lerp(speed, 0, acceleration);

            anim.SetBool("isRowing", false);
        }

        if (!isTouchingWall && velocityOffset != Vector3.zero)
        {
            velocityOffset += -velocityOffset * acceleration * 2;
            if (velocityOffset.sqrMagnitude < 0.05f)
            {
                velocityOffset = Vector3.zero;
            }
        }

        rb.velocity = transform.up * speed + velocityOffset;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Bottle")
        {
            collision.collider.gameObject.GetComponent<BottleProperties>().Invoke("Collect", 0);
            Debug.Log("Collected a Bottle!");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall-H")
        {
            isTouchingWall = true;
            Vector3 wallPosition = collision.gameObject.transform.position;
            if (wallPosition.x < transform.position.x)
            {
                velocityOffset += Vector3.right * acceleration * 2;
            } 
            else if (wallPosition.x > transform.position.x)
            {
                velocityOffset += Vector3.left * acceleration * 2;
            }
        }
        else if (collision.gameObject.tag == "Wall-V")
        {
            isTouchingWall = true;
            Vector3 wallPosition = collision.gameObject.transform.position;
            if (wallPosition.y < transform.position.y)
            {
                velocityOffset += Vector3.up * acceleration * 2;
            }
            else if (wallPosition.y > transform.position.y)
            {
                velocityOffset += Vector3.down * acceleration * 2;
            }
        }
    }
}

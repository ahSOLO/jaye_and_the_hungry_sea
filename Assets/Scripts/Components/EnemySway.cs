using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySway : MonoBehaviour
{
    [SerializeField] private float swayHorizontal = 0f;
    [SerializeField] private bool startLeft = false;
    [SerializeField] private float swaySpeed = 0.1f;
    private int xDirection;
    private Vector2 leftBound;
    private Vector2 rightBound;
    private Rigidbody2D rb;
    private float dirChangeGrace;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        leftBound = new Vector2(transform.position.x - swayHorizontal, transform.position.y);
        rightBound = new Vector2(transform.position.x + swayHorizontal, transform.position.y);

        if (startLeft) xDirection = -1;
        else xDirection = 1;

        dirChangeGrace = 0;
    }

    private void FixedUpdate()
    {
        if (swayHorizontal == 0) return;
        
        if (transform.position.x < leftBound.x || transform.position.x > rightBound.x )
        {
            if (dirChangeGrace <= 0)
            {
                xDirection *= -1;
                dirChangeGrace = 1f;
            }
        }
        rb.velocity = Vector3.right * xDirection * swaySpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (dirChangeGrace > 0)
        {
            dirChangeGrace -= Time.deltaTime;
        }
    }
}

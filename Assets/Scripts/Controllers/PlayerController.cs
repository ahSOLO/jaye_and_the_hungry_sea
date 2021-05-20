using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using ScriptableObjectArchitecture;

public class PlayerController : MonoBehaviour
{
    // Static var
    public static PlayerController pC;

    // Actions
    public PlayerActions pActions;

    // Movement variables
    [SerializeField] FloatVariable maxSpeedVar;
    [SerializeField] FloatVariable fastRowMultiplierVar;
    [SerializeField] private float rotationSpeed = 2.6f;
    [SerializeField] private float acceleration = 0.017f;
    public bool canSteer;

    private float maxSpeed;
    private float fastRowMultiplier;
    private bool rowFast;
    private float speed;
    private Vector3 inputDirection;
    private Vector3 lastDirection;
    private bool turnLeft;
    private bool turnRight;

    // Physics variables
    private Vector2 bounceDir;
    [SerializeField] private float bounceMagMax = 3f;
    private float bounceMag = 3f;

    // Anim variables
    private enum animState { idle = 0, rowing = 1, fastRowing = 2}
    private animState aState;
    [SerializeField] private GameObject boatLight;
    private Animator lightAnim;

    // Components
    private Rigidbody2D rb;
    private Animator anim;

    // Wall variables  
    private Vector3 velocityOffset;
    private bool isTouchingWall;

    // Health variables
    [SerializeField] private IntVariable maxHealth;
    [SerializeField] private IntVariable currentHealth;
    private bool isInvulnerable;
    private float invulnerableTimerMax = 1.5f;
    private float invulnerableTimer;

    // Creak variables
    private float creakTimer;
    [SerializeField] private float avgCreakTime = 4f;

    // Body variables
    public List<BodyAI> attachedBodies = new List<BodyAI>();

    // Events
    [SerializeField] GameEvent Creak;
    [SerializeField] BottleGameEvent CollectBottle;
    [SerializeField] Collision2DGameEvent HitByEnemy;
    [SerializeField] Collision2DGameEvent HitHardDebris;
    [SerializeField] Collider2DGameEvent HitBySkull;
    [SerializeField] GameEvent PlayerDamaged;
    [SerializeField] GameEvent PlayerHealed;
    [SerializeField] GameEvent SkullCancelChase;
    [SerializeField] DialogueGameEvent EnterDialogue;
    [SerializeField] GameEvent RainIncrease;
    [SerializeField] GameEvent RainDecrease;
    [SerializeField] GameEvent LevelEnd;
    [SerializeField] GameEvent DetachBodies;

    // Start is called before the first frame update
    void OnEnable()
    {
        pC = this;
        
        // col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (boatLight)
            lightAnim = boatLight.GetComponent<Animator>();

        rowFast = false;

        aState = animState.idle;

        bounceDir = Vector2.zero;

        creakTimer = avgCreakTime;

        maxSpeed = maxSpeedVar.Value;
        fastRowMultiplier = fastRowMultiplierVar.Value;

        // Prevent player from steering for first 3 seconds of scene.
        StartCoroutine(allowSteerTimer(3f));

        // Bind control mappings
        CreatePlayerActions();
        
        // Default starting health is 3, add extra health for previous failures, carry over health from previous levels if higher.
        currentHealth.Value = Mathf.Max(3 + Mathf.Min(GameController.gC.fails, 2), currentHealth.Value);
    }

    private void FixedUpdate()
    {
        MoveCharacter();

        // FixedUpdate happens before OnTriggerStay so this defaults to false each frame.
        isTouchingWall = false;
    }

    // Update is called once per frame
    void Update()
    {  
        var inputDevice = InputManager.ActiveDevice;
        
        if (canSteer)
        {
            inputDirection.x = pActions.Horizontal.RawValue;
            inputDirection.y = pActions.Vertical.RawValue;
            turnLeft = pActions.TurnLeft.IsPressed;
            turnRight = pActions.TurnRight.IsPressed;
        }

        if (invulnerableTimer > 0)
        {
            invulnerableTimer -= Time.deltaTime;
            if (invulnerableTimer < 0)
            {
                isInvulnerable = false;
            }
        }

        if (pActions.RowFast.IsPressed)
        {
            rowFast = true;
        }
        else
        {
            rowFast = false;
        }

        anim.SetInteger("state", (int) aState);
        if (lightAnim)
            lightAnim.SetInteger("state", (int) aState);

        // Creak sound
        if (rb.velocity.sqrMagnitude > 4)
        {
            creakTimer -= Time.deltaTime;
            if (creakTimer < 0)
            {
                Creak.Raise();
                creakTimer = Random.Range(avgCreakTime - 1f, avgCreakTime + 1f);
            }
        }
    }

    private void OnDestroy()
    {
        pActions.Destroy();
    }

    // Move function
    void MoveCharacter()
    {
        // Disable movement if character recently hit enemy, bounce character away from collision point
        if (isInvulnerable)
        {
            rb.velocity = bounceDir * bounceMag;
            bounceMag *= 0.95f;
            aState = animState.idle;
            return;
        }

        if (inputDirection != Vector3.zero)
        {            
            lastDirection = inputDirection;
            float angle = -90 + Mathf.Atan2(lastDirection.y, lastDirection.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            rb.MoveRotation(Quaternion.RotateTowards(this.transform.rotation, rotation, rotationSpeed));

            if (rowFast)
            {
                speed = Mathf.Lerp(speed, maxSpeed * fastRowMultiplier, acceleration * fastRowMultiplier);
                aState = animState.fastRowing;
            }
            else
            {
                speed = Mathf.Lerp(speed, maxSpeed, acceleration);
                aState = animState.rowing;
            }
        }
        else
        {
            speed = Mathf.Lerp(speed, 0, acceleration);

            if (turnLeft)
            {
                rb.MoveRotation(rb.rotation + (rotationSpeed / 2));
            }
            else if (turnRight)
            {
                rb.MoveRotation(rb.rotation - (rotationSpeed / 2));
            }

            aState = animState.idle;
        }

        // Decrease velocity offset by 3x default acceleration if player is not touching a wall
        if (!isTouchingWall && velocityOffset != Vector3.zero)
        {
            velocityOffset += -velocityOffset * acceleration * 3.0f;
            if (velocityOffset.sqrMagnitude < 0.05f)
            {
                velocityOffset = Vector3.zero;
            }
        }

        rb.velocity = transform.up * speed + velocityOffset;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {        
        GameObject colObject = collision.collider.gameObject;

        if (colObject.tag == "Bottle")
        {
            var bottleProperties = colObject.GetComponent<BottleProperties>();
            CollectBottle.Raise(bottleProperties.bottle);
            bottleProperties.onPickUp?.Raise();
            colObject.SetActive(false);
        }
        else if (colObject.tag == "Enemy" && !isInvulnerable)
        {
            HitByEnemy.Raise(collision);
        }
        else if (colObject.tag == "HardDebris")
        {
            HitHardDebris.Raise(collision);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "RainUp")
        {
            RainIncrease.Raise();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "RainDown")
        {
            RainDecrease.Raise();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "LevelEnd")
        {
            LevelEnd.Raise();
        }
        else if (collision.gameObject.tag == "Dialogue")
        {
            Dialogue d = collision.gameObject.GetComponent<Dialogue>();
            EnterDialogue.Raise(d);
            d.onPickUp?.Raise();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "ChasingSkull" && !isInvulnerable)
        {
            HitBySkull.Raise(collision);
        }
        else if (collision.gameObject.tag == "SkullCancelChase")
        {
            SkullCancelChase.Raise();
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall-H" && isInvulnerable == false)
        {
            isTouchingWall = true;
            Vector3 wallPosition = collision.gameObject.transform.position;
            if (wallPosition.x < transform.position.x)
            {
                velocityOffset += Vector3.right * acceleration * 2.5f;
            } 
            else if (wallPosition.x > transform.position.x)
            {
                velocityOffset += Vector3.left * acceleration * 2.5f;
            }
        }
        else if (collision.gameObject.tag == "Wall-V" && isInvulnerable == false)
        {
            isTouchingWall = true;
            Vector3 wallPosition = collision.gameObject.transform.position;
            if (wallPosition.y < transform.position.y)
            {
                velocityOffset += Vector3.up * acceleration * 2.5f;
            }
            else if (wallPosition.y > transform.position.y)
            {
                velocityOffset += Vector3.down * acceleration * 2.5f;
            }
        }
    }

    public void BounceAway(Collision2D collision)
    {
        velocityOffset = Vector3.zero;
        Vector2 dir = collision.contacts[0].point - new Vector2(transform.position.x, transform.position.y);
        bounceDir = -dir.normalized;
        bounceMag = bounceMagMax;
    }

    public void BounceAway(Collider2D collider)
    {
        velocityOffset = Vector3.zero;
        Vector2 dir = collider.attachedRigidbody.position - new Vector2(transform.position.x, transform.position.y);
        bounceDir = -dir.normalized;
        bounceMag = bounceMagMax;
    }

    public void TakeDamage()
    {
        currentHealth.Value--;
        PlayerDamaged.Raise();

        if (currentHealth.Value < 1)
        {
            invulnerableTimerMax = 2f;
            EffectsController.eC.StartCoroutine(EffectsController.eC.PlayerDeath(2f, transform.position));
            GameController.gC.fails++;
            Invoke("Deactivate", 2f);
        }

        invulnerableTimer = invulnerableTimerMax;
        isInvulnerable = true;
        speed = 0f;

        CinemachineShake.cSInstance.ShakeCamera(5f, invulnerableTimerMax);
        invulnerableTimerMax = 1.5f;
    }

    public void Heal()
    {
        if (currentHealth.Value < maxHealth.Value)
        {
            currentHealth.Value++;
            PlayerHealed.Raise();
        }
    }

    public IEnumerator allowSteerTimer(float timer)
    {
        canSteer = false;
        
        yield return new WaitForSeconds(timer);

        canSteer = true;
    }

    public void attachBody(BodyAI body)
    {
        attachedBodies.Add(body);
        maxSpeed *= body.slowDownMultiplier;
    }

    public void detachBodies()
    {
        if (attachedBodies.Count == 0) return;

        DetachBodies.Raise();

        foreach (var body in attachedBodies)
        {
            body.DetachBody();
        }

        attachedBodies.Clear();
        maxSpeed = maxSpeedVar.Value;
    }

    private void CreatePlayerActions()
    {
        pActions = new PlayerActions();

        pActions.Up.AddDefaultBinding(InputControlType.LeftStickUp);
        pActions.Up.AddDefaultBinding(InputControlType.DPadUp);
        pActions.Up.AddDefaultBinding(Key.W);
        pActions.Up.AddDefaultBinding(Key.UpArrow);

        pActions.Down.AddDefaultBinding(InputControlType.LeftStickDown);
        pActions.Down.AddDefaultBinding(InputControlType.DPadDown);
        pActions.Down.AddDefaultBinding(Key.S);
        pActions.Down.AddDefaultBinding(Key.DownArrow);

        pActions.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
        pActions.Left.AddDefaultBinding(InputControlType.DPadLeft);
        pActions.Left.AddDefaultBinding(Key.A);
        pActions.Left.AddDefaultBinding(Key.LeftArrow);

        pActions.Right.AddDefaultBinding(InputControlType.LeftStickRight);
        pActions.Right.AddDefaultBinding(InputControlType.DPadRight);
        pActions.Right.AddDefaultBinding(Key.D);
        pActions.Right.AddDefaultBinding(Key.RightArrow);

        pActions.RowFast.AddDefaultBinding(InputControlType.Action1);
        pActions.RowFast.AddDefaultBinding(Key.LeftShift);

        pActions.Inventory.AddDefaultBinding(InputControlType.Action4);
        pActions.Inventory.AddDefaultBinding(Key.I);

        pActions.TurnLeft.AddDefaultBinding(InputControlType.LeftTrigger);
        pActions.TurnLeft.AddDefaultBinding(Key.Q);

        pActions.TurnRight.AddDefaultBinding(InputControlType.RightTrigger);
        pActions.TurnRight.AddDefaultBinding(Key.E);

        pActions.CallLightning.AddDefaultBinding(InputControlType.Action3);
        pActions.CallLightning.AddDefaultBinding(Key.Space);

        pActions.PauseMenu.AddDefaultBinding(InputControlType.Command);
        pActions.PauseMenu.AddDefaultBinding(Key.Escape);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}

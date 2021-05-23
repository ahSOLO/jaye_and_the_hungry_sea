using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

[System.Serializable]
public class BodyAI : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject leftWrist;
    Rigidbody2D leftWristRB;
    [SerializeField] GameObject leftPalm;
    BodyPalmTriggerManager leftPalmTriggers;
    [SerializeField] GameObject rightWrist;
    [SerializeField] GameObject rightPalm;
    Rigidbody2D rightWristRB;
    BodyPalmTriggerManager rightPalmTriggers;
    [SerializeField] GameObject torso;
    BodyTorsoTriggerManager torsoTriggers;

    [SerializeField] float wristRotationSpeed = 2f;
    [SerializeField] float chasingSpeed = 0.33f;
    [SerializeField] float attachmentSpeed = 3.6f;
    float detachmentSpeed;
    [SerializeField] public float slowDownMultiplier = 0.85f;

    [SerializeField] BodyAIGameEvent bodyAttached;
    SpriteRenderer[] childRenderers;

    enum State { idle, chasing, attached, detached }
    State state;

    enum BodyType { small, medium, large }
    [SerializeField] BodyType bodyType;
    
    // Start is called before the first frame update
    void Start()
    {
        leftWristRB = leftWrist.GetComponent<Rigidbody2D>();
        rightWristRB = rightWrist.GetComponent<Rigidbody2D>();
        leftPalmTriggers = leftPalm.GetComponent<BodyPalmTriggerManager>();
        rightPalmTriggers = leftPalm.GetComponent<BodyPalmTriggerManager>();
        torsoTriggers = torso.GetComponent<BodyTorsoTriggerManager>();

        detachmentSpeed = attachmentSpeed * 10f;
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case State.idle:
                break;
            case State.chasing:
                leftWrist.transform.rotation = Helper.RotateTowardsOnZAxis(player, leftWrist, 90f, wristRotationSpeed);
                leftWristRB.velocity = Vector2.Lerp(leftWristRB.velocity, (player.transform.position - leftWrist.transform.position).normalized * chasingSpeed, chasingSpeed);
                rightWrist.transform.rotation = Helper.RotateTowardsOnZAxis(player, rightWrist, 90f, wristRotationSpeed);
                rightWristRB.velocity = Vector2.Lerp(rightWristRB.velocity, (player.transform.position - leftWrist.transform.position).normalized * chasingSpeed, chasingSpeed);
                break;
            case State.attached:
                leftWrist.transform.rotation = Helper.RotateTowardsOnZAxis(player, leftWrist, 90f, wristRotationSpeed * 3);
                rightWrist.transform.rotation = Helper.RotateTowardsOnZAxis(player, rightWrist, 90f, wristRotationSpeed * 3);
                if (leftPalmTriggers.isTouchingPlayer == false)
                {
                    var leftDistance = player.transform.position - leftPalm.transform.position;
                    leftWristRB.velocity = Vector2.Lerp(leftWristRB.velocity, leftDistance.normalized * attachmentSpeed, attachmentSpeed / 5);
                }
                if (rightPalmTriggers.isTouchingPlayer == false)
                {
                    var rightDistance = player.transform.position - rightPalm.transform.position;
                    rightWristRB.velocity = Vector2.Lerp(rightWristRB.velocity, rightDistance.normalized * attachmentSpeed, attachmentSpeed / 5);
                }
                break;
        }        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.idle:
                switch (bodyType)
                {
                    case BodyType.small:
                        if (torsoTriggers.isTouchingSmallTrigger)
                        {
                            state = State.chasing;
                        }
                        break;
                    case BodyType.medium:
                        if (torsoTriggers.isTouchingMediumTrigger)
                        {
                            state = State.chasing;
                        }
                        break;
                    case BodyType.large:
                        if (torsoTriggers.isTouchingLargeTrigger)
                        {
                            state = State.chasing;
                        }
                        break;
                }
                break;
            case State.chasing:
                switch (bodyType)
                {
                    case BodyType.small:
                        if (!torsoTriggers.isTouchingSmallTrigger)
                        {
                            state = State.idle;
                        }
                        break;
                    case BodyType.medium:
                        if (!torsoTriggers.isTouchingMediumTrigger)
                        {
                            state = State.idle;
                        }
                        break;
                    case BodyType.large:
                        if (!torsoTriggers.isTouchingLargeTrigger)
                        {
                            state = State.idle;
                        }
                        break;
                }
                if (CollidedWithPlayer())
                {
                bodyAttached.Raise(this);
                state = State.attached;
                }
                break;
        }
        
    }

    bool CollidedWithPlayer()
    {
        return leftPalmTriggers.isTouchingPlayer || rightPalmTriggers.isTouchingPlayer;
    }

    public void DetachBody()
    {
        state = State.detached;

        var leftDistance = player.transform.position - leftPalm.transform.position;
        leftWristRB.velocity = -leftDistance.normalized * detachmentSpeed;

        var rightDistance = player.transform.position - rightPalm.transform.position;
        rightWristRB.velocity = -rightDistance.normalized * detachmentSpeed;

        StartCoroutine(FadeOutBody());
    }

    public IEnumerator FadeOutBody()
    {
        childRenderers = GetComponentsInChildren<SpriteRenderer>();
        yield return new WaitForSeconds(0.5f);
        
        var timer = 2f;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            foreach (var spriteRenderer in childRenderers)
            {
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Mathf.Max(spriteRenderer.color.a - (0.5f * Time.deltaTime), 0f));
            }
            yield return null;
        }

        Destroy(gameObject);
    }

    public void AddGravityAndDestroy()
    {
        leftWristRB.gravityScale = 3f;
        rightWristRB.gravityScale = 3f;
        torso.GetComponent<Rigidbody2D>().gravityScale = 7f;
        Destroy(gameObject, 3f);
    }
}

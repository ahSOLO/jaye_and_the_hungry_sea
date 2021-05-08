using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SkullAI : MonoBehaviour
{
    private StateMachine _stateMachine;
    public static SkullAI sAI;
    
    GameObject player;
    public GameObject hitParticle;
    [SerializeField] GameObject patrolDestination;
    
    [SerializeField] GameObject leftEye;
    [SerializeField] GameObject rightEye;

    [SerializeField] float eyeRotationLeftBound = -45f;
    [SerializeField] float eyeRotationRightBound = 45f;

    [SerializeField] float eyeRotationTimeMax = 5f;
    [SerializeField] float eyeRotationSpeed = 0.6f;

    [SerializeField] float moveSpeed = 0.02f;
    [SerializeField] float rotationSpeed = 0.1f;

    LayerMask visionMask;

    float leaveVisionRangeTime;

    private void Start()
    {
        sAI = this;

        player = GameObject.FindGameObjectWithTag("Player");

        // State machine initialization
        _stateMachine = new StateMachine();

        var patrol = new Patrol(this, patrolDestination, player, leftEye, rightEye, eyeRotationLeftBound, eyeRotationRightBound, eyeRotationTimeMax, eyeRotationSpeed, moveSpeed, rotationSpeed);
        var chase = new Chase(this, player, leftEye, rightEye, eyeRotationLeftBound, eyeRotationRightBound, eyeRotationSpeed, moveSpeed, rotationSpeed);

        AddT(patrol, chase, () => {
            if (PlayerInEyeRange())
            {
                leaveVisionRangeTime = 0f;
                return true;
            }
            return false;
            });
        AddT(chase, patrol, () => {
            if (!PlayerInEyeRange()) leaveVisionRangeTime += Time.deltaTime;
            if (leaveVisionRangeTime > 0.66f) return true;
            return false;
            });

        _stateMachine.SetState(chase);

        void AddT(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);

        visionMask = LayerMask.GetMask("Player", "BlockSkullVision");
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedTick();
    }

    private void Update()
    {
        _stateMachine.Tick();
    }

    public Vector3 SetPatrolDestination()
    {
        return new Vector3(UnityEngine.Random.Range(player.transform.position.x - 10f, player.transform.position.x + 10f) , UnityEngine.Random.Range(player.transform.position.y - 10f, player.transform.position.y + 10f), 0f);
    }

    private bool PlayerInEyeRange()
    {
        var leftEyeCol = Physics2D.OverlapCircle(leftEye.transform.position, 10f, LayerMask.GetMask("Player"));
        var rightEyeCol = Physics2D.OverlapCircle(rightEye.transform.position, 10f, LayerMask.GetMask("Player"));

        if (leftEyeCol != null)
        {
            var dir = player.transform.position - leftEye.transform.position;
            if (Vector3.Angle(-transform.up, dir) > eyeRotationLeftBound && Vector3.Angle(-transform.up, dir) < eyeRotationRightBound)
            {
                var hit1 = Physics2D.Raycast(leftEye.transform.position, -leftEye.transform.up, 10f, visionMask);
                // -30 to 30 is the inner angle of the 2D light
                var hit2 = Physics2D.Raycast(leftEye.transform.position, Quaternion.AngleAxis(-30f, Vector3.forward) * -leftEye.transform.up, 10f, visionMask);
                var hit3 = Physics2D.Raycast(leftEye.transform.position, Quaternion.AngleAxis(30f, Vector3.forward) * -leftEye.transform.up, 10f, visionMask);
                var hit4 = Physics2D.Raycast(leftEye.transform.position, Quaternion.AngleAxis(-15f, Vector3.forward) * -leftEye.transform.up, 10f, visionMask);
                var hit5 = Physics2D.Raycast(leftEye.transform.position, Quaternion.AngleAxis(15f, Vector3.forward) * -leftEye.transform.up, 10f, visionMask);

                return (hit1 && hit1.collider.CompareTag("Player")) || (hit2 && hit2.collider.CompareTag("Player")) || (hit3 && hit3.collider.CompareTag("Player")) || (hit4 && hit4.collider.CompareTag("Player")) || (hit5 && hit5.collider.CompareTag("Player"));
            }
        }

        if (rightEyeCol != null)
        {
            var dir = player.transform.position - rightEye.transform.position;
            if (Vector3.Angle(-transform.up, dir) > eyeRotationLeftBound && Vector3.Angle(-transform.up, dir) < eyeRotationRightBound)
            {
                var hit1 = Physics2D.Raycast(rightEye.transform.position, -rightEye.transform.up, 10f, visionMask);
                // -30 to 30 is the inner angle of the 2D light
                var hit2 = Physics2D.Raycast(rightEye.transform.position, Quaternion.AngleAxis(-30f, Vector3.forward) * -rightEye.transform.up, 10f, visionMask);
                var hit3 = Physics2D.Raycast(rightEye.transform.position, Quaternion.AngleAxis(30f, Vector3.forward) * -rightEye.transform.up, 10f, visionMask);
                var hit4 = Physics2D.Raycast(rightEye.transform.position, Quaternion.AngleAxis(-15f, Vector3.forward) * -rightEye.transform.up, 10f, visionMask);
                var hit5 = Physics2D.Raycast(rightEye.transform.position, Quaternion.AngleAxis(15f, Vector3.forward) * -rightEye.transform.up, 10f, visionMask);

                return (hit1 && hit1.collider.CompareTag("Player")) || (hit2 && hit2.collider.CompareTag("Player")) || (hit3 && hit3.collider.CompareTag("Player")) || (hit4 && hit4.collider.CompareTag("Player")) || (hit5 && hit5.collider.CompareTag("Player"));
            }
        }

        return false;
    }

    public void SkullHit()
    {
        GameObject.Instantiate(hitParticle, transform.position, transform.rotation);

        switch (UnityEngine.Random.Range(0, 4))
        {
            case 0:
                transform.position = new Vector3(player.transform.position.x - 30f, player.transform.position.y, transform.position.z);
                break;
            case 1:
                transform.position = new Vector3(player.transform.position.x + 30f, player.transform.position.y, transform.position.z);
                break;
            case 2: 
                transform.position = new Vector3(player.transform.position.x - 20f, player.transform.position.y - 20f, transform.position.z);
                break;
            case 3:
                transform.position = new Vector3(player.transform.position.x + 20f, player.transform.position.y + 20f, transform.position.z);
                break;
        }
    }

}

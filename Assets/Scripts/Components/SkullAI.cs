using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullAI : MonoBehaviour
{
    GameObject player;
    
    [SerializeField] GameObject leftEye;
    [SerializeField] GameObject rightEye;

    [SerializeField] float eyeRotationLeftBound = -45;
    [SerializeField] float eyeRotationRightBound = 45;

    [SerializeField] float eyeRotationTimeMax = 5f;
    [SerializeField] float eyeRotationSpeed = 0.6f;
    float rotationTime;
    bool rotateLeft;

    [SerializeField] float moveSpeed = 0.02f;
    [SerializeField] float rotationSpeed = 0.1f;

    private void Start()
    {
        rotateLeft = true;
        rotationTime = 0f;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        transform.position += -transform.up * moveSpeed;

        Vector3 difference = player.transform.position - transform.position;
        float targetZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, targetZ + 90f), rotationSpeed);

        // float headZ = Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.z, targetZ, rotationSpeed);
        // Add 90 degrees to correct for sprite facing downwards
        // transform.rotation = Quaternion.Euler(0f, 0f, targetZ + 90f);

        //float angleToPlayer = Vector3.SignedAngle(transform.position, player.transform.position, Vector3.forward) + 90f;
        //float headZ = Mathf.MoveTowardsAngle(transform.localEulerAngles.z, angleToPlayer, rotationSpeed);
        //Vector3 rotateDestination = new Vector3(0, 0, headZ);
        //transform.localEulerAngles = rotateDestination;

        float eyeRotationBound;
        if (rotateLeft) eyeRotationBound = eyeRotationLeftBound;
        else eyeRotationBound = eyeRotationRightBound;

        float eyeZ = Mathf.MoveTowardsAngle(leftEye.transform.localEulerAngles.z, eyeRotationBound, eyeRotationSpeed);

        Vector3 eyeRotateDestination = new Vector3(0, 0, eyeZ);
        leftEye.transform.localEulerAngles = eyeRotateDestination;
        rightEye.transform.localEulerAngles = eyeRotateDestination;
    }

    private void Update()
    {
        rotationTime += Time.deltaTime;

        if (rotationTime >= eyeRotationTimeMax)
        {
            rotateLeft = !rotateLeft;
            rotationTime = 0f;
        }
    }
}

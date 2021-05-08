using UnityEngine;

public class Patrol : IState
{
    SkullAI _skull;
    GameObject _player;
    GameObject _patrolDestination;

    GameObject _leftEye;
    GameObject _rightEye;

    float _eyeRotationLeftBound;
    float _eyeRotationRightBound;
    float _eyeRotationTimeMax;
    float _eyeRotationSpeed;
    float _moveSpeed;
    float _rotationSpeed;

    float rotationTime;
    bool rotateLeft;

    public Patrol(SkullAI skull, GameObject patrolDestination, GameObject player, GameObject leftEye, GameObject rightEye, float eyeRotationLeftBound, float eyeRotationRightBound, float eyeRotationTimeMax, float eyeRotationSpeed,
    float moveSpeed, float rotationSpeed)
    {
        rotateLeft = true;
        rotationTime = 0f;

        _skull = skull;
        _patrolDestination = patrolDestination;
        _player = player;
        _leftEye = leftEye;
        _rightEye = rightEye;
        _eyeRotationLeftBound = eyeRotationLeftBound;
        _eyeRotationRightBound = eyeRotationRightBound;
        _eyeRotationTimeMax = eyeRotationTimeMax;
        _eyeRotationSpeed = eyeRotationSpeed;
        _moveSpeed = moveSpeed;
        _rotationSpeed = rotationSpeed;
    }

    public void OnEnter()
    {
        _patrolDestination.transform.position = _skull.SetPatrolDestination();
        rotationTime = 0f;
    }

    public void FixedTick()
    {
        _skull.transform.position += -_skull.transform.up * _moveSpeed;
        _skull.transform.rotation = Helper.RotateTowardsOnZAxis(_patrolDestination, _skull.gameObject, 90f, _rotationSpeed);

        if ((Vector3.SqrMagnitude(_patrolDestination.transform.position - _skull.transform.position) < 25f) 
            || Vector3.SqrMagnitude(_player.transform.position - _patrolDestination.transform.position) > 360f)
        {
            _patrolDestination.transform.position = _skull.SetPatrolDestination();
        }

        float eyeRotationBound;
        if (rotateLeft) eyeRotationBound = _eyeRotationLeftBound;
        else eyeRotationBound = _eyeRotationRightBound;

        float eyeZ = Mathf.MoveTowardsAngle(_leftEye.transform.localEulerAngles.z, eyeRotationBound, _eyeRotationSpeed);

        Vector3 eyeRotateDestination = new Vector3(0, 0, eyeZ);
        _leftEye.transform.localEulerAngles = eyeRotateDestination;
        _rightEye.transform.localEulerAngles = eyeRotateDestination;
    }

    public void Tick()
    {
        // Update Timer
        rotationTime += Time.deltaTime;

        if (rotationTime >= _eyeRotationTimeMax)
        {
            rotateLeft = !rotateLeft;
            rotationTime = 0f;
        }
    }

    public void OnExit()
    {

    }
}

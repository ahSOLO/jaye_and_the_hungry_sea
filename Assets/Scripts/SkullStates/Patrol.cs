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

    float speedDifficultyAdjustment;


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

        speedDifficultyAdjustment = GameController.gC.fails * -0.15f;
    }

    public void OnEnter()
    {
        _patrolDestination.transform.position = _skull.SetPatrolDestination();
        rotationTime = 0f;
    }

    public void FixedTick()
    {
        // Head forward and rotate towards the patrol destination
        _skull.transform.position += -_skull.transform.up * Mathf.Max(_moveSpeed + speedDifficultyAdjustment, 1) * Time.fixedDeltaTime;
        _skull.transform.rotation = Helper.RotateTowardsOnZAxis(_patrolDestination, _skull.gameObject, 90f, _rotationSpeed * Time.fixedDeltaTime);

        // Rotate the eyes in a scanning pattern from side to side
        float eyeRotationBound;
        if (rotateLeft) eyeRotationBound = _eyeRotationLeftBound;
        else eyeRotationBound = _eyeRotationRightBound;

        float eyeZ = Mathf.MoveTowardsAngle(_leftEye.transform.localEulerAngles.z, eyeRotationBound, _eyeRotationSpeed * Time.fixedDeltaTime);

        Vector3 eyeRotateDestination = new Vector3(0, 0, eyeZ);
        _leftEye.transform.localEulerAngles = eyeRotateDestination;
        _rightEye.transform.localEulerAngles = eyeRotateDestination;

        // If skull is not chasing, set the patrol destination to the beginning of the map
        if (_skull.cancelChase)
        {
            _patrolDestination.transform.position = Vector3.zero;
            return;
        }

        // Set a new patrol destination if the skull has reached the previous destination or the current destination is too far from the player.
        if ((Vector3.SqrMagnitude(_patrolDestination.transform.position - _skull.transform.position) < 25f) 
            || Vector3.SqrMagnitude(_player.transform.position - _patrolDestination.transform.position) > 360f)
        {
            _patrolDestination.transform.position = _skull.SetPatrolDestination();
        }
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

using UnityEngine;

public class Chase : IState
{
    SkullAI _skull;
    GameObject _player;

    GameObject _leftEye;
    GameObject _rightEye;

    float _eyeRotationLeftBound;
    float _eyeRotationRightBound;
    float _eyeRotationSpeed;
    float _moveSpeed;
    float _rotationSpeed;

    float imageOffset = 90f;

    public Chase(SkullAI skull, GameObject player, GameObject leftEye, GameObject rightEye, float eyeRotationLeftBound, float eyeRotationRightBound, float eyeRotationSpeed,
        float moveSpeed, float rotationSpeed)
    {
        _skull = skull;
        _player = player;
        _leftEye = leftEye;
        _rightEye = rightEye;
        _eyeRotationLeftBound = eyeRotationLeftBound;
        _eyeRotationRightBound = eyeRotationRightBound;
        _eyeRotationSpeed = eyeRotationSpeed;
        _moveSpeed = moveSpeed;
        _rotationSpeed = rotationSpeed;
    }

    public void OnEnter()
    {

    }

    public void FixedTick()
    {
        _skull.transform.position += -_skull.transform.up * _moveSpeed;
        _skull.transform.rotation = Helper.RotateTowardsOnZAxis(_player, _skull.gameObject, imageOffset, _rotationSpeed);

        _leftEye.transform.rotation = Helper.RotateTowardsOnZAxis(_player, _leftEye, imageOffset, _eyeRotationSpeed);
        _rightEye.transform.rotation = Helper.RotateTowardsOnZAxis(_player, _rightEye, imageOffset, _eyeRotationSpeed);

        _leftEye.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Clamp(Helper.NormalizeAngle(_leftEye.transform.localEulerAngles.z), _eyeRotationLeftBound, _eyeRotationRightBound));
        _rightEye.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Clamp(Helper.NormalizeAngle(_rightEye.transform.localEulerAngles.z), _eyeRotationLeftBound, _eyeRotationRightBound));
    }

    public void Tick()
    {
    }

    public void OnExit()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static Quaternion RotateTowardsOnZAxis(GameObject target, GameObject current, float degOffset, float rotationSpeed)
    {
        Vector3 difference = target.transform.position - current.transform.position;
        float targetZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        return Quaternion.RotateTowards(current.transform.rotation, Quaternion.Euler(0f, 0f, targetZ + degOffset), rotationSpeed);
    }

    public static Quaternion RotateTowardsOnZAxis(Vector3 target, Vector3 current, Quaternion rotation, float degOffset, float rotationSpeed)
    {
        Vector3 difference = target - current;
        float targetZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        return Quaternion.RotateTowards(rotation, Quaternion.Euler(0f, 0f, targetZ + degOffset), rotationSpeed);
    }

    //return angle in range -180 to 180
    public static float NormalizeAngle(float a)
    {
        if (a > 180f) return a - 360f;
        if (a < -180f) return a + 360f;
        return a;
    }
}

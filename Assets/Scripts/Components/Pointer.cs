using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{
    public static Pointer p;

    public Vector3 targetPosition;
    private RectTransform pointerRectTransform;
    private Image pointerImage;
    [SerializeField] private float pointerRefreshTimerMax = 1f;
    private float pointerRefreshTimer;

    private void Awake()
    {
        p = this;
        
        pointerRectTransform = GetComponent<RectTransform>();
        pointerImage = GetComponent<Image>();

        UpdateTarget();
    }

    private void Update()
    {
        pointerRefreshTimer -= Time.deltaTime;
        if (pointerRefreshTimer <= 0f)
        {
            UpdateTarget();
            pointerRefreshTimer = pointerRefreshTimerMax;

            var currentPos = Camera.main.transform.position;
            currentPos.z = 0f;

            if (Vector3.Distance(currentPos, targetPosition) > 30f)
            {
                pointerImage.enabled = false;
            }
            else
            {
                pointerImage.enabled = true;
            }
        }

        if (pointerImage.enabled == false) return;

        float borderSize = 16f;
        Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(targetPosition);
        bool isOffScreen = targetPositionScreenPoint.x <= borderSize || targetPositionScreenPoint.x >= Screen.width - borderSize || targetPositionScreenPoint.y <= borderSize || targetPositionScreenPoint.y >= Screen.height - borderSize;

        if (isOffScreen)
        {
            pointerImage.enabled = true;
            RotatePointerTowardsTargetPosition();

            Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;
            if (cappedTargetScreenPosition.x <= borderSize) cappedTargetScreenPosition.x = borderSize;
            if (cappedTargetScreenPosition.x >= Screen.width - borderSize) cappedTargetScreenPosition.x = Screen.width - borderSize;
            if (cappedTargetScreenPosition.y <= borderSize) cappedTargetScreenPosition.y = borderSize;
            if (cappedTargetScreenPosition.y >= Screen.height - borderSize) cappedTargetScreenPosition.y = Screen.height - borderSize;

            pointerRectTransform.position = cappedTargetScreenPosition;
            pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, 0f);
        }
        else
        {
            pointerImage.enabled = false;
        }
    }

    public void ShowPointer()
    {
        pointerImage.enabled = true;
    }

    public void HidePointer()
    {
        pointerImage.enabled = false;
    }

    public void UpdateTarget()
    {
        targetPosition = ClosestBottlePosition();
    }
    
    private Vector3 ClosestBottlePosition()
    {
        var bottlesInScene = GameObject.FindGameObjectsWithTag("Bottle");

        if (bottlesInScene.Length == 0) return new Vector3(0, 1000f, 0);
        
        float min = Mathf.Infinity;
        GameObject closest = bottlesInScene[0];

        foreach (var bottle in bottlesInScene)
        {
            Vector3 toPosition = bottle.transform.position;
            Vector3 fromPosition = Camera.main.transform.position;
            fromPosition.z = 0f;
            float distance = Vector3.Distance(fromPosition, toPosition);
            if (distance < min)
            {
                min = distance;
                closest = bottle;
            }
        }

        return closest.transform.position;
    }
    
    private void RotatePointerTowardsTargetPosition()
    {
        Vector3 toPosition = targetPosition;
        Vector3 fromPosition = Camera.main.transform.position;
        fromPosition.z = 0f;
        Vector3 dir = (toPosition - fromPosition).normalized;
        float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) % 360;
        pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);
    }
}

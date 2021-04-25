using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake cSInstance { get; private set; }

    private CinemachineVirtualCamera cVC;
    private CinemachineBasicMultiChannelPerlin cVCBasicMCPerlin;
    private float shakeTimer;

    private void Awake()
    {
        cSInstance = this;

        cVC = GetComponent<CinemachineVirtualCamera>();    
        cVCBasicMCPerlin = cVC.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        cVCBasicMCPerlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            // Lerp from start amplitude to no amplitude
            cVCBasicMCPerlin.m_AmplitudeGain = Mathf.Lerp(cVCBasicMCPerlin.m_AmplitudeGain, 0f, Time.deltaTime / shakeTimer);
            if (shakeTimer <= 0f)
            {
                cVCBasicMCPerlin.m_AmplitudeGain = 0f;  
            }
        }
    }
}

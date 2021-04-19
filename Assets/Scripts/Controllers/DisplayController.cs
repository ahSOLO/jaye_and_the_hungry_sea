using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class DisplayController : MonoBehaviour
{
    public static DisplayController dC;
    
    private CinemachineVirtualCamera cVC = null;
    private Camera mainCam;
    private float orthoMinimum = 5f;
    private float orthoMaximum = 7.5f;
    private float startScreenHeight;

    private void OnEnable()
    {
        //Assign Singleton
        if (dC == null) dC = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject); // Persist between scenes

        SceneManager.sceneLoaded += OnSceneLoad;

        startScreenHeight = Screen.height;
    }

    public void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        GameObject vCam = GameObject.FindGameObjectWithTag("vCam");
        if (vCam != null)
        {
            cVC = GameObject.FindGameObjectWithTag("vCam").GetComponent<CinemachineVirtualCamera>();
        }

        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        if (Screen.height > 720)
        {
            float orthoTarget = Mathf.Clamp(Screen.height / 144, orthoMinimum, orthoMaximum);
            if (cVC != null)
            {
                cVC.m_Lens.OrthographicSize = orthoTarget;
            }
            mainCam.orthographicSize = orthoTarget;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            RefreshResolution();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    void RefreshResolution()
    {
        if (Screen.fullScreen)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            float orthoTarget = Mathf.Clamp(startScreenHeight / 144, orthoMinimum, orthoMaximum);
            if (cVC != null)
            {
                cVC.m_Lens.OrthographicSize = orthoTarget;
            }
            mainCam.orthographicSize = orthoTarget;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            float orthoTarget = Mathf.Clamp(Screen.currentResolution.height / 144, orthoMinimum, orthoMaximum);
            if (cVC != null)
            {
                cVC.m_Lens.OrthographicSize = orthoTarget;
            }
            mainCam.orthographicSize = orthoTarget;
        }
    }
}

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
    private float orthoMinimum = 5f; // Orthographic camera size for a 720p display with 1 unit = 72 pixels
    private float orthoMaximum = 7.5f; // Orthographic camera size for a 1080p display with 1 unit = 72 pixels
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

        // Set the default screen height if the game is not initially ran in 720p.
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
            ToggleFullScreen();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    public void ToggleFullScreen()
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

    /*
    public void ToggleFullScreenButton()
    {
        GameObject vCam = GameObject.FindGameObjectWithTag("vCam");
        if (vCam != null)
        {
            cVC = GameObject.FindGameObjectWithTag("vCam").GetComponent<CinemachineVirtualCamera>();
        }

        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

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
    */
}

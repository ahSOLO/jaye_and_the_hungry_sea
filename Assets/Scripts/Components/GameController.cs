using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController gC;
    public GameObject pauseMenu;
    private bool isPaused = false;
    private bool isAxisInUse = false;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        DontDestroyOnLoad(gameObject); // Persist between scenes
        SceneManager.sceneLoaded += OnSceneLoad;

        //Assign Singleton
        if (gC == null) gC = this;
        else Destroy(gameObject);

        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("PauseMenu") != 0)
        {
            if (isAxisInUse) return;
            switch (isPaused)
            {
                case false:
                    PauseGame();
                    break;
                case true:
                    UnpauseGame();
                    break;
            }
            isAxisInUse = true;
        }
        else
        {
            isAxisInUse = false;
        }
    }

    // On Scene Load, do the following...
    public void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "1_Level1")
        {
            AudioController.aC.PlayMusic(AudioController.aC.firstLevelMusic, 0.6f);
        }
    }

    void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
        AudioController.aC.musicSource.volume = 0.2f;
        AudioController.aC.PauseActiveSources();
    }

    void UnpauseGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
        AudioController.aC.musicSource.volume = 0.6f;
        AudioController.aC.UnPauseAudioSources();
    }
}

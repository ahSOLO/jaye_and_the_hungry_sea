using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController gC;

    public enum GameState {title, pauseMenu, boating, notes};
    public GameState gState;

    private bool isPaused = false;
    private bool isPauseAxisInUse = false;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        DontDestroyOnLoad(gameObject); // Persist between scenes
        SceneManager.sceneLoaded += OnSceneLoad;

        //Assign Singleton
        if (gC == null) gC = this;
        else Destroy(gameObject);

        Application.targetFrameRate = 60;

        gState = GameState.title;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("PauseMenu") == 1 && gState == GameState.boating)
        {
            if (isPauseAxisInUse) return;
            switch (isPaused)
            {
                case false:
                    PauseGame();
                    gState = GameState.pauseMenu;
                    UIManager.uIM.OpenPauseMenu();
                    break;
                case true:
                    UnPauseGame();
                    gState = GameState.boating;
                    UIManager.uIM.ClosePauseMenu();
                    break;
            }
            isPauseAxisInUse = true;
        }
        else
        {
            isPauseAxisInUse = false;
        }
    }

    // On Scene Load, do the following...
    public void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "1_Level1")
        {
            AudioController.aC.PlayMusic(AudioController.aC.firstLevelMusic, 0.6f);
            gState = GameState.boating;
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
        AudioController.aC.musicSource.volume = 0.2f;
        AudioController.aC.PauseActiveSources();
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
        isPaused = false;
        AudioController.aC.musicSource.volume = 0.6f;
        AudioController.aC.UnPauseAudioSources();
    }

    public void RestartScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

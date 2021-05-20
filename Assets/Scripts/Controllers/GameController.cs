using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ScriptableObjectArchitecture;

public class GameController : MonoBehaviour
{
    public static GameController gC;

    public enum GameState {title, cutscene, pauseMenu, boating, notes};
    public GameState gState;

    // Progression and health tracking
    public int progress;
    public int fails;
    [SerializeField] IntVariable currentPlayerHealth;

    // Events
    [SerializeField] GameEvent TutorialStart;

    void OnEnable()
    {
        //Assign Singleton
        if (gC == null) gC = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject); // Persist between scenes
        SceneManager.sceneLoaded += OnSceneLoad;
        Application.targetFrameRate = 60;

        gState = GameState.title;
        progress = 0;
        fails = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.pC != null && PlayerController.pC.pActions.PauseMenu.WasPressed && (gState == GameState.boating || gState == GameState.pauseMenu))
        {
            switch (gState)
            {
                case GameState.boating:
                    PauseGame();
                    gState = GameState.pauseMenu;
                    UIManager.uIM.OpenPauseMenu();
                    break;
                case GameState.pauseMenu:
                    UnPauseGame();
                    gState = GameState.boating;
                    UIManager.uIM.ClosePauseMenu();
                    break;
            }
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    // Set default game state for scene and initialize any scene-start coroutines
    public void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "0_Title")
        {
            gState = GameState.title;
            fails = 0;
            currentPlayerHealth.Value = 3;
        }
        
        else if (SceneManager.GetActiveScene().name == "1_Introduction")
        {
            fails = 0;
            gState = GameState.cutscene;
            progress = Mathf.Min(progress, 1);
        }

        else if (SceneManager.GetActiveScene().name == "2_Level1")
        {
            TutorialStart.Raise();
            gState = GameState.boating;
        }

        else if (SceneManager.GetActiveScene().name == "3_Cutscene1")
        {
            fails = 0;
            gState = GameState.cutscene;
            progress = Mathf.Min(progress, 2);
        }

        else if (SceneManager.GetActiveScene().name == "4_Level2")
        {
            gState = GameState.boating;
        }

        else if (SceneManager.GetActiveScene().name == "5_Cutscene2")
        {
            fails = 0;
            gState = GameState.cutscene;
            progress = Mathf.Min(progress, 3);
        }

        else if (SceneManager.GetActiveScene().name == "6_Level3")
        {
            gState = GameState.boating;
        }

        else if (SceneManager.GetActiveScene().name == "7_Cutscene3")
        {
            fails = 0;
            gState = GameState.cutscene;
            progress = Mathf.Min(progress, 4);
        }

        else if (SceneManager.GetActiveScene().name == "8_Level4")
        {
            gState = GameState.boating;
        }

        else if (SceneManager.GetActiveScene().name == "Epilogue")
        {
            gState = GameState.cutscene;
        }
    }

    // PauseGame is used by the pause menu as well as the note screen.
    public void PauseGame()
    {
        Time.timeScale = 0;
        AudioController.aC.musicSource.volume = 0.2f;
        AudioController.aC.PauseActiveSources();
    }

    public void UnPauseGame()
    {
        // Must refer to singleton for use on button
        gC.gState = GameState.boating;
        Time.timeScale = 1;
        AudioController.aC.musicSource.volume = 0.6f;
        AudioController.aC.UnPauseAudioSources();
    }

    public void RestartScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Asynchronously load the next scene and delay scene switch until the timer is completed
    public IEnumerator LoadNextSceneAsync(float timer)
    {
        AsyncOperation asyncLoad;
        if (SceneManager.sceneCountInBuildSettings > SceneManager.GetActiveScene().buildIndex + 1)
        {
            asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            asyncLoad = SceneManager.LoadSceneAsync(0);
        }
        asyncLoad.allowSceneActivation = false;
        
        yield return new WaitForSeconds(timer);

        asyncLoad.allowSceneActivation = true;
    }

    public void LoadTitle()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    
    public void LoadLevelOne()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadLevelTwo()
    {
        SceneManager.LoadScene(3);
    }

    public void LoadLevelThree()
    {
        SceneManager.LoadScene(5);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void EndLevel()
    {
        StartCoroutine(LoadNextSceneAsync(3f));
    }
}

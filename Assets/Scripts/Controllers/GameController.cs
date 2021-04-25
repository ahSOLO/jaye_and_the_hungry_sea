using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController gC;

    public enum GameState {title, cutscene, pauseMenu, boating, notes};
    public GameState gState;

    public int progress;
    
    // Start is called before the first frame update
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
        }
        
        else if (SceneManager.GetActiveScene().name == "1_Introduction")
        {
            gState = GameState.cutscene;
            progress = Mathf.Max(progress, 1);
        }

        else if (SceneManager.GetActiveScene().name == "2_Level1")
        {
            StartCoroutine(Tutorial());
            gState = GameState.boating;
        }

        else if (SceneManager.GetActiveScene().name == "3_Cutscene1")
        {
            gState = GameState.cutscene;
            progress = Mathf.Max(progress, 2);
        }

        else if (SceneManager.GetActiveScene().name == "4_Level2")
        {
            gState = GameState.boating;
        }

        else if (SceneManager.GetActiveScene().name == "5_Cutscene2")
        {
            gState = GameState.cutscene;
            progress = Mathf.Max(progress, 3);
        }

        else if (SceneManager.GetActiveScene().name == "6_Level3")
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
        GameController.gC.gState = GameState.boating;
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
        SceneManager.LoadScene(2);
    }

    public void LoadLevelTwo()
    {
        SceneManager.LoadScene(4);
    }

    public void LoadLevelThree()
    {
        SceneManager.LoadScene(6);
    }

    public void Quit()
    {
        Application.Quit();
    }

    // Tutorial for the first level
    public IEnumerator Tutorial()
    {
        yield return new WaitForSeconds(4f);

        UIManager.uIM.helperMessage.text = "Move with WASD or the Controller left-stick";
        UIManager.uIM.helperMessageTimer = 7f;

        yield return new WaitForSeconds(9f);

        UIManager.uIM.helperMessage.text = "To turn without moving, press Q and E, or use the Left and Right controller triggers";
        UIManager.uIM.helperMessageTimer = 12f;

        yield return new WaitForSeconds(14f);

        UIManager.uIM.helperMessage.text = "To row faster, hold Left-Shift, A (XBox), or X (PlayStation)";
        UIManager.uIM.helperMessageTimer = 12f;

        yield return new WaitForSeconds(14f);

        UIManager.uIM.helperMessage.text = "Press F to toggle fullscreen";
        UIManager.uIM.helperMessageTimer = 12f;
    }
}

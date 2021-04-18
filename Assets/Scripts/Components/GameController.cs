using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController gC;

    public enum GameState {title, pauseMenu, boating, notes};
    public GameState gState;

    private bool isPauseAxisInUse = false;
    
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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("PauseMenu") == 1 && (gState == GameState.boating || gState == GameState.pauseMenu))
        {
            if (isPauseAxisInUse) return;
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
            isPauseAxisInUse = true;
        }
        else
        {
            isPauseAxisInUse = false;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    // On Scene Load, do the following...
    public void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "2_Level1")
        {
            StartCoroutine(Tutorial());
            gState = GameState.boating;
        }
    }

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
    public IEnumerator LoadNextSceneAsync(float timer)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        asyncLoad.allowSceneActivation = false;

        yield return new WaitForSeconds(timer);

        asyncLoad.allowSceneActivation = true;
    }

    public void LoadTitle()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public IEnumerator Tutorial()
    {
        yield return new WaitForSeconds(3f);

        UIManager.uIM.helperMessage.text = "Move with WASD or the Controller left-stick";
        UIManager.uIM.helperMessageTimer = 7f;

        yield return new WaitForSeconds(8f);

        UIManager.uIM.helperMessage.text = "To turn without moving, press Q and E, or use the Left and Right controller triggers";
        UIManager.uIM.helperMessageTimer = 9f;

        yield return new WaitForSeconds(10f);

        UIManager.uIM.helperMessage.text = "To row faster, hold Left-Shift, A (XBox), or X (PlayStation)";
        UIManager.uIM.helperMessageTimer = 9f;

        yield return new WaitForSeconds(10f);

        UIManager.uIM.helperMessage.text = "Head north. Collect bottled notes. Avoid dangers.";
        UIManager.uIM.helperMessageTimer = 9f;
    }
}

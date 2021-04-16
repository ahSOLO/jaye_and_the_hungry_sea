using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController gC;
    
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
        
    }

    // On Scene Load, do the following...
    public void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "1_Level1")
        {
            AudioController.aC.PlayMusic(AudioController.aC.firstLevelMusic, 0.6f);
        }
    }
}

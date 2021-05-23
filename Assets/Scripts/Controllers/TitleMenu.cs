using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleMenu : MonoBehaviour
{
    public GameObject levelMenu;
    public GameObject playButton;
    public GameObject levelOneButton;
    public GameObject levelTwoButton;
    public GameObject levelThreeButton;
    public GameObject levelFourButton;
    public GameObject selectLevelButton;
    public GameObject howToButton;
    public GameObject howToPopup;
    public GameObject creditsButton;
    public GameObject creditsPopup;
    
    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(playButton);
        
        if (GameController.gC.progress > 1)
        {
            levelTwoButton.SetActive(true);
        }

        if (GameController.gC.progress > 2)
        {
            levelThreeButton.SetActive(true);
        }

        if (GameController.gC.progress > 3)
        {
            levelFourButton.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenLevelMenu()
    {
        levelMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(levelOneButton);
    }

    public void CloseLevelMenu()
    {
        levelMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selectLevelButton);
    }

    public void OpenHowTo()
    {
        howToPopup.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(howToPopup);
    }

    public void CloseHowTo()
    {
        howToPopup.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(howToButton);
    }

    public void OpenCredits()
    {
        creditsPopup.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(creditsPopup);
    }

    public void CloseCredits()
    {
        creditsPopup.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(creditsButton);
    }
}

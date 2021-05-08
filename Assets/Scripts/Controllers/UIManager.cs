using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Static var
    public static UIManager uIM;

    // UI vars
    public GameObject helperMessageObj;
    public TextMeshProUGUI helperMessage;
    public float helperMessageTimer;
    public GameObject dialogueBox;
    public GameObject defaultMsgObj;
    public TextMeshProUGUI defaultMsgText;
    public GameObject anxietyMsgObj;
    public TextMeshProUGUI anxietyMsgText;
    public GameObject paranoiaMsgObj;
    public TextMeshProUGUI paranoiaMsgText;
    public GameObject guiltMsgObj;
    public TextMeshProUGUI guiltMsgText;
    private float dialogueTimer;
    public GameObject[] hearts;
    private Image[] heartImages = new Image[5];
    public Sprite heartIcon;
    public Sprite heartIconInverse;

    // Pause Menu vars
    public GameObject pauseMenu;
    public GameObject resumeButton;
    public GameObject backToTitleButton;

    // Note menu vars
    public GameObject noteDisplay;
    public GameObject noteDisplayTextObj;
    public TextMeshProUGUI noteDisplayText;
    public GameObject noteTitleObj;
    public TextMeshProUGUI noteTitle;
    private bool noteOpen;
    public int maxNotes = 10;

    // Start is called before the first frame update
    void OnEnable()
    {
        uIM = this;

        noteOpen = false;

        noteDisplayText = noteDisplayTextObj.GetComponent<TextMeshProUGUI>();
        noteTitle = noteTitleObj.GetComponent<TextMeshProUGUI>();

        helperMessage = helperMessageObj.GetComponent<TextMeshProUGUI>();

        defaultMsgText = defaultMsgObj.GetComponent<TextMeshProUGUI>();
        anxietyMsgText = anxietyMsgObj.GetComponent<TextMeshProUGUI>();
        paranoiaMsgText = paranoiaMsgObj.GetComponent<TextMeshProUGUI>();
        guiltMsgText = guiltMsgObj.GetComponent<TextMeshProUGUI>();

        for (int i = 0; i < hearts.Length; i++)
        {
            heartImages[i] = hearts[i].GetComponent<Image>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.pC.pActions.Inventory.WasPressed && (GameController.gC.gState == GameController.GameState.boating || GameController.gC.gState == GameController.GameState.notes))
        {
            switch (noteOpen)
            {
                case true:
                    HideNote();
                    noteOpen = false;
                    GameController.gC.UnPauseGame();
                    GameController.gC.gState = GameController.GameState.boating;
                    break;
                case false:
                    DisplayNote();
                    noteOpen = true;
                    GameController.gC.PauseGame();
                    GameController.gC.gState = GameController.GameState.notes;
                    break;
            }
        }

        if (GameController.gC.gState == GameController.GameState.notes)
        {
            if (PlayerController.pC.pActions.Left.WasPressed)
            {
                PreviousNote();
            }
            else if (PlayerController.pC.pActions.Right.WasPressed)
            {
                NextNote();
            }
        }

        if (helperMessageTimer > 0)
        {
            helperMessageTimer -= Time.deltaTime;
        }
        else if (helperMessageTimer <= 0)
        {
            helperMessage.text = "";
        }

        if (dialogueTimer > 0)
        {
            dialogueTimer -= Time.deltaTime;
        }
        else if (dialogueTimer <= 0)
        {
            dialogueBox.SetActive(false);
        }
    }

    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(resumeButton);
    }

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
    }

    public void DisplayNote()
    {
        AudioController.aC.PlaySFXAtPoint(AudioController.aC.openNotes, Camera.main.transform.position, 0.25f);

        noteDisplay.SetActive(true);
        noteTitle.text = InventoryManager.iM.GetNoteTitle(InventoryManager.iM.currentNote);
        noteDisplayText.text = InventoryManager.iM.GetNoteContent(InventoryManager.iM.currentNote).text;
        GameController.gC.gState = GameController.GameState.notes;
        noteOpen = true;
    }

    public void NextNote()
    {
        AudioController.aC.PlayRandomSFXAtPoint(AudioController.aC.changeNotes, Camera.main.transform.position, 0.3f);

        InventoryManager.iM.currentNote++;
        if (InventoryManager.iM.currentNote > maxNotes)
        {
            InventoryManager.iM.currentNote -= maxNotes;
        }
        noteDisplayText.text = InventoryManager.iM.GetNoteContent(InventoryManager.iM.currentNote).text;
        noteTitle.text = InventoryManager.iM.GetNoteTitle(InventoryManager.iM.currentNote);
    }
    public void PreviousNote()
    {
        AudioController.aC.PlayRandomSFXAtPoint(AudioController.aC.changeNotes, Camera.main.transform.position, 0.4f);

        InventoryManager.iM.currentNote--;
        if (InventoryManager.iM.currentNote < 1)
        {
            InventoryManager.iM.currentNote += maxNotes;
        }
        noteDisplayText.text = InventoryManager.iM.GetNoteContent(InventoryManager.iM.currentNote).text;
        noteTitle.text = InventoryManager.iM.GetNoteTitle(InventoryManager.iM.currentNote);
    }

    public void HideNote()
    {
        AudioController.aC.PlaySFXAtPoint(AudioController.aC.closeNotes, Camera.main.transform.position, 0.25f);

        noteDisplay.SetActive(false);
        GameController.gC.gState = GameController.GameState.boating;
        noteOpen = false;
    }

    public void SetHelperMessageText(string text, float timer)
    {
        helperMessage.text = text;
        helperMessageTimer = timer;
    }

    public void showDialogue(float timer, string content, bool pauseGame = false, int characterId = 1, int barkId = 0, int triggerId = 0)
    {
        dialogueBox.SetActive(true);
        dialogueTimer = timer;

        defaultMsgObj.SetActive(false);
        anxietyMsgObj.SetActive(false);
        paranoiaMsgObj.SetActive(false);
        guiltMsgObj.SetActive(false);

        switch (characterId)
        {
            case 2:
                anxietyMsgObj.SetActive(true);
                anxietyMsgText.text = content;
                break;
            case 3:
                paranoiaMsgObj.SetActive(true);
                paranoiaMsgText.text = content;
                break;
            case 4:
                guiltMsgObj.SetActive(true);
                guiltMsgText.text = content;
                break;
            default:
                defaultMsgObj.SetActive(true);
                defaultMsgText.text = content;
                break;
        }

    }

    public void SetHeartIcon(bool isInverse)
    {
        Sprite icon;
        if (isInverse) icon = heartIconInverse;
        else icon = heartIcon;

        foreach (var image in heartImages)
        {
            image.sprite = icon;
        }
    }
}

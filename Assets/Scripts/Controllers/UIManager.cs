using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    public GameObject dialogueMsgObj;
    public TextMeshProUGUI dialogueMsg;
    private float dialogueTimer;

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
    private bool isInventoryAxisInUse = false;
    private bool isHorizontalAxisInUse = false;
    public int maxNotes = 10;

    // Start is called before the first frame update
    void OnEnable()
    {
        uIM = this;

        noteOpen = false;

        noteDisplayText = noteDisplayTextObj.GetComponent<TextMeshProUGUI>();
        noteTitle = noteTitleObj.GetComponent<TextMeshProUGUI>();
        helperMessage = helperMessageObj.GetComponent<TextMeshProUGUI>();
        dialogueMsg = dialogueMsgObj.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Inventory") == 1 && (GameController.gC.gState == GameController.GameState.boating || GameController.gC.gState == GameController.GameState.notes))
        {
            if (isInventoryAxisInUse) return;

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

            isInventoryAxisInUse = true;
        }
        else
        {
            isInventoryAxisInUse = false;
        }


        if (GameController.gC.gState == GameController.GameState.notes)
        {
            if (Input.GetAxisRaw("Horizontal") == -1f)
            {
                if (isHorizontalAxisInUse) return;
                isHorizontalAxisInUse = true;
                PreviousNote();
            }
            else if (Input.GetAxisRaw("Horizontal") == 1f)
            {
                if (isHorizontalAxisInUse) return;
                isHorizontalAxisInUse = true;
                NextNote();
            }
            else
            {
                isHorizontalAxisInUse = false;
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
        noteDisplayText.text = InventoryManager.iM.ParseNote(InventoryManager.iM.currentNote).text;
        noteTitle.text = InventoryManager.iM.ParseTitle(InventoryManager.iM.currentNote);
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
        noteDisplayText.text = InventoryManager.iM.ParseNote(InventoryManager.iM.currentNote).text;
        noteTitle.text = InventoryManager.iM.ParseTitle(InventoryManager.iM.currentNote);
    }
    public void PreviousNote()
    {
        AudioController.aC.PlayRandomSFXAtPoint(AudioController.aC.changeNotes, Camera.main.transform.position, 0.4f);

        InventoryManager.iM.currentNote--;
        if (InventoryManager.iM.currentNote < 1)
        {
            InventoryManager.iM.currentNote += maxNotes;
        }
        noteDisplayText.text = InventoryManager.iM.ParseNote(InventoryManager.iM.currentNote).text;
        noteTitle.text = InventoryManager.iM.ParseTitle(InventoryManager.iM.currentNote);
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
        dialogueMsg.text = content;
    }
}

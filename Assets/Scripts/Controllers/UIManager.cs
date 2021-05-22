using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using ScriptableObjectArchitecture;

public class UIManager : MonoBehaviour
{
    // Static var
    public static UIManager uIM;

    // UI vars
    public GameObject levelUI;

    public GameObject helperMessageObj;
    private TextMeshProUGUI helperMessage;
    private float helperMessageTimer;
    public GameObject dialogueBox;
    public GameObject defaultMsgObj;
    private TextMeshProUGUI defaultMsgText;
    public GameObject anxietyMsgObj;
    private TextMeshProUGUI anxietyMsgText;
    public GameObject paranoiaMsgObj;
    private TextMeshProUGUI paranoiaMsgText;
    public GameObject guiltMsgObj;
    private TextMeshProUGUI guiltMsgText;
    public GameObject liesMsgObj;
    private TextMeshProUGUI liesMsgText;
    private float dialogueTimer;
    public GameObject[] hearts;
    private Image[] heartImages = new Image[5];
    public Sprite heartIcon;
    public Sprite heartIconInverse;

    public GameObject StormMeterObj;
    public GameObject StormMeterFillObj;
    private Image StormMeterFillImg;
    [SerializeField] private float stormMeterRefillRate = 0.1f;

    [SerializeField] IntVariable playerCurrentHealth;
    [SerializeField] GameEvent callLightning;

    // Pause Menu vars
    public GameObject pauseMenu;
    public GameObject resumeButton;
    public GameObject backToTitleButton;

    // Note menu vars
    public GameObject noteDisplay;
    public GameObject noteDisplayTextObj;
    private TextMeshProUGUI noteDisplayText;
    public GameObject noteTitleObj;
    private TextMeshProUGUI noteTitle;
    private bool noteOpen;
    public int maxNotes = 10;

    void OnEnable()
    {
        uIM = this;

        noteOpen = false;

        helperMessage = helperMessageObj.GetComponent<TextMeshProUGUI>();
        
        noteDisplayText = noteDisplayTextObj.GetComponent<TextMeshProUGUI>();
        noteTitle = noteTitleObj.GetComponent<TextMeshProUGUI>();

        defaultMsgText = defaultMsgObj.GetComponent<TextMeshProUGUI>();
        anxietyMsgText = anxietyMsgObj.GetComponent<TextMeshProUGUI>();
        paranoiaMsgText = paranoiaMsgObj.GetComponent<TextMeshProUGUI>();
        guiltMsgText = guiltMsgObj.GetComponent<TextMeshProUGUI>();
        liesMsgText = liesMsgObj.GetComponent<TextMeshProUGUI>();

        StormMeterFillImg = StormMeterFillObj.GetComponent<Image>();

        for (int i = 0; i < hearts.Length; i++)
        {
            heartImages[i] = hearts[i].GetComponent<Image>();
        }
    }

    private void Start()
    {
        InitializeHearts();
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

        if (StormMeterObj.activeSelf == true)
        {
            // Less than 1 because the meter looks full before the fill reaches 100%
            if (StormMeterFillImg.fillAmount != 1f)
            {
                StormMeterFillImg.fillAmount = Mathf.MoveTowards(StormMeterFillImg.fillAmount, 1, stormMeterRefillRate * Time.deltaTime);
            }
            
            else if (PlayerController.pC.pActions.CallLightning.WasPressed)
            {
                callLightning.Raise();
                StormMeterFillImg.fillAmount = 0;
            }
        }
    }

    // Pause Menu
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

    // Notes
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

    // Helper Message
    public void SetHelperMessageText(string text, float timer)
    {
        helperMessage.text = text;
        helperMessageTimer = timer;
    }

    // Dialogue
    public void ShowDialogue(Dialogue d)
    {
        dialogueBox.SetActive(true);
        dialogueTimer = d.duration;

        defaultMsgObj.SetActive(false);
        anxietyMsgObj.SetActive(false);
        paranoiaMsgObj.SetActive(false);
        guiltMsgObj.SetActive(false);

        switch (d.characterId)
        {
            case 2:
                anxietyMsgObj.SetActive(true);
                anxietyMsgText.text = d.content;
                break;
            case 3:
                paranoiaMsgObj.SetActive(true);
                paranoiaMsgText.text = d.content;
                break;
            case 4:
                guiltMsgObj.SetActive(true);
                guiltMsgText.text = d.content;
                break;
            case 5:
                liesMsgObj.SetActive(true);
                liesMsgText.text = d.content;
                break;
            default:
                defaultMsgObj.SetActive(true);
                defaultMsgText.text = d.content;
                break;
        }

        // Unused: d.barkId, d.triggerId, d.pauseGame
    }

    public void SetHeartIcon(bool lightningIsFlashing)
    {
        Sprite icon;
        if (lightningIsFlashing) icon = heartIcon;
        else icon = heartIconInverse;

        foreach (var image in heartImages)
        {
            image.sprite = icon;
        }
    }

    public void InitializeHearts()
    {
        for (int i = 0; i < playerCurrentHealth.Value; i++)
        {
            hearts[i].SetActive(true);
        }
    }

    public void AddHeart()
    {
        foreach (var heart in hearts)
        {
            if (heart.activeSelf == false)
            {
                heart.SetActive(true);
                break;
            }
        }
    }

    public void RemoveHeart()
    {
        for (int i = hearts.Length - 1; i >= 0; i--)
        {
            if (hearts[i].activeSelf == true)
            {
                hearts[i].SetActive(false);
                break;
            }
        }
    }

    public void DeactivateUI()
    {
        levelUI.SetActive(false);
    }

    public void ShowBottleCollectHelperText()
    {
        SetHelperMessageText("To Read Notes: Press 'i' or the ▲|Y Button", 4f);
    }

    public void StartTutorial()
    {
        // Tutorial for the first level
        IEnumerator Tutorial()
        {
            yield return new WaitForSeconds(4f);

            uIM.helperMessage.text = "Move with WASD or the Controller left-stick";
            uIM.helperMessageTimer = 7f;

            yield return new WaitForSeconds(9f);

            uIM.helperMessage.text = "To turn without moving, press Q and E, or use the Left and Right controller triggers";
            uIM.helperMessageTimer = 12f;

            yield return new WaitForSeconds(14f);

            uIM.helperMessage.text = "To row faster, hold Left-Shift, A (XBox), or X (PlayStation)";
            uIM.helperMessageTimer = 12f;

            yield return new WaitForSeconds(14f);

            uIM.helperMessage.text = "Press F to toggle fullscreen";
            uIM.helperMessageTimer = 12f;
        }

        StartCoroutine(Tutorial());
    }

    public void StartCallLightningHelperText()
    {

        IEnumerator CallLightningHelperText()
        {
            yield return new WaitForSeconds(4f);

            StormMeterObj.SetActive(true);
            uIM.helperMessage.text = "Press Space, X (XBox), or ⬜ (PlayStation) to call lightning.";
            uIM.helperMessageTimer = 10f;

            yield return new WaitForSeconds(12f);
            
            uIM.helperMessage.text = "You may only call lightning when your storm meter is full.";
            uIM.helperMessageTimer = 12f;
        }

        StartCoroutine(CallLightningHelperText());
    }

    public void StartDetachBodiesHelperText()
    {

        IEnumerator DetachBodiesHelperText()
        {
            yield return new WaitForSeconds(2f);

            uIM.helperMessage.text = "Call lightning to detach clinging bodies.";
            uIM.helperMessageTimer = 7f;

            yield return new WaitForSeconds(9f);

            uIM.helperMessage.text = "Jaye is tired. The storm meter will fill up slower than before.";
            uIM.helperMessageTimer = 10f;
        }

        StartCoroutine(DetachBodiesHelperText());
    }
}

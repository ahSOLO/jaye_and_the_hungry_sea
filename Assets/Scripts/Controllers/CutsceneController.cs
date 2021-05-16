using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using InControl;

public class CutsceneController : MonoBehaviour
{
    public GameObject background;
    public GameObject back;
    public GameObject middle;
    public GameObject front;
    public GameObject dialogueBox;
    public GameObject defaultText;
    public GameObject anxietyText;
    public GameObject paranoiaText;
    public GameObject guiltText;
    public GameObject mCText;
    public GameObject mCAvatar;
    public GameObject downTriangle;

    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;
    public Sprite sprite4;
    public Sprite sprite5;
    public Sprite sprite6;
    public Sprite sprite7;
    public Sprite sprite8;

    public TextAsset cutsceneScript;

    private TextMeshProUGUI mcTMP;
    private TextMeshProUGUI defaultTMP;
    private TextMeshProUGUI anxietyTMP;
    private TextMeshProUGUI paranoiaTMP;
    private TextMeshProUGUI guiltTMP;

    private Image backgroundImg;
    private Image backImg;
    private Image middleImg;
    private Image frontImg;

    private Dictionary<int, DialogueNode> dialogueDict = new Dictionary<int, DialogueNode>();
    
    private List<KeyCode> advanceKeys = new List<KeyCode>() { KeyCode.E, KeyCode.Space, KeyCode.Return};
   
    private bool canAdvance;
    private float canAdvanceTimer;

    private int dialogueNodeNum;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
        canAdvanceTimer = 1f;
        canAdvance = false;
        dialogueNodeNum = 0;
    }

    private void Start()
    {
        defaultTMP = defaultText.GetComponent<TextMeshProUGUI>();
        anxietyTMP = anxietyText.GetComponent<TextMeshProUGUI>();
        paranoiaTMP = paranoiaText.GetComponent<TextMeshProUGUI>();
        guiltTMP = guiltText.GetComponent<TextMeshProUGUI>();
        mcTMP = mCText.GetComponent<TextMeshProUGUI>();
        ParseTextToDialogue();
        StartCoroutine(ShowTextAfterWait(3f));
    }

    // Update is called once per frame
    void Update()
    {
        var inputDevice = InputManager.ActiveDevice;

        if (canAdvanceTimer > 0)
        {
            canAdvanceTimer -= Time.deltaTime;
        }
        else
        {
            canAdvance = true;
        }

        if (!canAdvance)
        {
            downTriangle.SetActive(false);
        }
        else
        {
            downTriangle.SetActive(true);

            if ((inputDevice.Action1.WasPressed)
                || (inputDevice.Action3.WasPressed))
            {
                AdvanceDialogue();
            }

            else
            {
                foreach (KeyCode kc in advanceKeys)
                {
                    if (Input.GetKeyDown(kc))
                    {
                        AdvanceDialogue();
                        break;
                    }
                }
            }
        }
    }
    
    // Play music / initial triggers
    public void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "1_Introduction")
        {
         
        }

        else if (SceneManager.GetActiveScene().name == "3_Cutscene1")
        {

        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    private void AdvanceDialogue()
    {
        canAdvance = false;
        canAdvanceTimer = 0.5f;
        dialogueNodeNum++;
        ShowText();
    }

    /* 
        Txt files should have the following format: C1|B0|T0|Text content.
        C#: The number of the character.
        B#: The number of the audio bark, default is 0 (no bark).
        T#: The custom cutscene trigger to initiate, default is 0 (no trigger).
        Text content: The content to show on the cutscene. No quotes are necessary.
    */
    private void ParseTextToDialogue()
    {
        string[] textArr = cutsceneScript.text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        int index = 0;
        foreach (string entry in textArr)
        {
            string[] entryArr = entry.Split("|".ToCharArray());
            DialogueNode dN = new DialogueNode();
            dN.index = index;
            dN.characterId = int.Parse(entryArr[0].Substring(1));
            dN.barkId = int.Parse(entryArr[1].Substring(1));
            dN.triggerId = int.Parse(entryArr[2].Substring(1));
            dN.content = entryArr[3];
            dialogueDict.Add(index, dN);
            index++;
        }
    }

    private IEnumerator ShowTextAfterWait(float timer)
    {
        yield return new WaitForSeconds(timer);
        dialogueBox.SetActive(true);
        ShowText();
    }

    private void ShowText()
    {
        // If reached end of dictionary, go to next scene after a brief fade-out period.
        if (!dialogueDict.ContainsKey(dialogueNodeNum))
        {
            canAdvance = false;
            canAdvanceTimer = 5f;
            StartCoroutine(FadeController.fC.Fade(1f, 2f));
            // StartCoroutine(FadeAudioSource.StartFade(AudioController.aC.musicSource, 2f, 0f));
            StartCoroutine(GameController.gC.LoadNextSceneAsync(2f));
            return;
        }

        if (dialogueDict[dialogueNodeNum].barkId != 0)
        {
            // Play Bark Audio
        }

        if (dialogueDict[dialogueNodeNum].triggerId != 0)
        {
            // Play Trigger
        }

        // Hide all non-MC text
        defaultText.SetActive(false);
        anxietyText.SetActive(false);
        paranoiaText.SetActive(false);
        guiltText.SetActive(false);

        if (dialogueDict[dialogueNodeNum].characterId == 1)
        {
            // Show MC Text
            mCText.SetActive(true);
            mCAvatar.SetActive(true);

            mcTMP.text = dialogueDict[dialogueNodeNum].content;
        }
        else
        {
            // Show other text
            mCText.SetActive(false);
            mCAvatar.SetActive(false);

            switch (dialogueDict[dialogueNodeNum].characterId)
            {
                // Little Light
                case 2:
                    anxietyText.SetActive(true);
                    anxietyTMP.text = dialogueDict[dialogueNodeNum].content;
                    break;
                // Thousand Eyes
                case 3:
                    paranoiaText.SetActive(true);
                    paranoiaTMP.text = dialogueDict[dialogueNodeNum].content;
                    break;
                // The Albatross
                case 4:
                    guiltText.SetActive(true);
                    guiltTMP.text = dialogueDict[dialogueNodeNum].content;
                    break;
                default:
                    defaultText.SetActive(true);
                    defaultTMP.text = dialogueDict[dialogueNodeNum].content;
                    break;
            }
        }
    }

    // Main method for allowing triggers to interact with the cutscene image layers, should be called programmed by a separate script.
    public void SetImage(string layer, Sprite sprite, Dictionary<string, float> options = null)
    {
        switch (layer)
        {
            case "background":
                backgroundImg.sprite = sprite;
                if (options.ContainsKey("xPosition")) 
                {
                    backgroundImg.transform.position = new Vector3 (options["xPosition"], options["yPosition"]);
                }
                if (options.ContainsKey("xScale"))
                {
                    backgroundImg.transform.localScale = new Vector3(options["xScale"], options["yScale"]);
                }
                if (options.ContainsKey("isActive"))
                {
                    if (options["isActive"] == 1)
                    {
                        background.SetActive(true);
                    }
                    else
                    {
                        background.SetActive(false);
                    }
                }
                break;
            case "back":
                backImg.sprite = sprite;
                if (options.ContainsKey("xPosition"))
                {
                    backImg.transform.position = new Vector3(options["xPosition"], options["yPosition"]);
                }
                if (options.ContainsKey("xScale"))
                {
                    backImg.transform.localScale = new Vector3(options["xScale"], options["yScale"]);
                }
                if (options.ContainsKey("isActive"))
                {
                    if (options["isActive"] == 1)
                    {
                        background.SetActive(true);
                    }
                    else
                    {
                        background.SetActive(false);
                    }
                }
                break;
            case "middle":
                middleImg.sprite = sprite;
                if (options.ContainsKey("xPosition"))
                {
                    middleImg.transform.position = new Vector3(options["xPosition"], options["yPosition"]);
                }
                if (options.ContainsKey("xScale"))
                {
                    middleImg.transform.localScale = new Vector3(options["xScale"], options["yScale"]);
                }
                if (options.ContainsKey("isActive"))
                {
                    if (options["isActive"] == 1)
                    {
                        background.SetActive(true);
                    }
                    else
                    {
                        background.SetActive(false);
                    }
                }
                break;
            case "front":
                frontImg.sprite = sprite;
                if (options.ContainsKey("xPosition"))
                {
                    frontImg.transform.position = new Vector3(options["xPosition"], options["yPosition"]);
                }
                if (options.ContainsKey("xScale"))
                {
                    frontImg.transform.localScale = new Vector3(options["xScale"], options["yScale"]);
                }
                if (options.ContainsKey("isActive"))
                {
                    if (options["isActive"] == 1)
                    {
                        background.SetActive(true);
                    }
                    else
                    {
                        background.SetActive(false);
                    }
                }
                break;
        }
    }
}
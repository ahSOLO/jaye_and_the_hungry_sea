using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour
{
    public GameObject background;
    public GameObject back;
    public GameObject middle;
    public GameObject front;
    public GameObject dialogueBox;
    public GameObject otherText;
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

    private TextMeshProUGUI otherTMP;
    private TextMeshProUGUI mcTMP;

    private Image backgroundImg;
    private Image backImg;
    private Image middleImg;
    private Image frontImg;

    private Dictionary<int, DialogueNode> dialogueDict = new Dictionary<int, DialogueNode>();
    
    private List<KeyCode> advanceKeys = new List<KeyCode>() { KeyCode.E, KeyCode.Space, KeyCode.Return, KeyCode.Joystick1Button0, KeyCode.Joystick1Button2, KeyCode.Joystick1Button1 };
   
    private bool canAdvance;
    private float canAdvanceTimer;

    private int dialogueNodeNum;

    // Start is called before the first frame update
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
        canAdvanceTimer = 1f;
        canAdvance = false;
        dialogueNodeNum = 0;
    }

    private void Start()
    {
        otherTMP = otherText.GetComponent<TextMeshProUGUI>();
        mcTMP = mCText.GetComponent<TextMeshProUGUI>();
        ParseTextToDialogue();
        ShowText();
    }

    // Update is called once per frame
    void Update()
    {
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

    private void ShowText()
    {
        // If reached end of dictionary, go to next scene after a brief fade out period.
        if (dialogueDict.ContainsKey(dialogueNodeNum) == false)
        {
            StartCoroutine(FadeAudioSource.StartFade(AudioController.aC.musicSource, 2f, 0f));
            GameController.gC.LoadNextSceneAsync(2f);
        }

        if (dialogueDict[dialogueNodeNum].barkId != 0)
        {
            // Play Bark Audio
        }

        if (dialogueDict[dialogueNodeNum].triggerId != 0)
        {
            // Play Trigger
        }
        
        if (dialogueDict[dialogueNodeNum].characterId == 1)
        {
            // Show main character text
            otherText.SetActive(false);
            mCText.SetActive(true);
            mCAvatar.SetActive(true);

            mcTMP.text = dialogueDict[dialogueNodeNum].content;
        }
        else
        {
            // Show other text
            otherText.SetActive(true);
            mCText.SetActive(false);
            mCAvatar.SetActive(false);

            otherTMP.text = dialogueDict[dialogueNodeNum].content;
        }
    }

    private void SetImage(string layer, Sprite sprite, Dictionary<string, float> options = null)
    {
        switch (layer)
        {
            case "background":
                backgroundImg.sprite = sprite;
                if (options.ContainsKey("xPosition")) 
                {
                    backgroundImg.transform.position = new Vector3 (options["xPosition"], options["yPosition"]);
                }
                else if (options.ContainsKey("xScale"))
                {
                    backgroundImg.transform.localScale = new Vector3(options["xScale"], options["yScale"]);
                }
                break;
            case "back":
                backImg.sprite = sprite;
                if (options.ContainsKey("xPosition"))
                {
                    backImg.transform.position = new Vector3(options["xPosition"], options["yPosition"]);
                }
                else if (options.ContainsKey("xScale"))
                {
                    backImg.transform.localScale = new Vector3(options["xScale"], options["yScale"]);
                }
                break;
            case "middle":
                middleImg.sprite = sprite;
                if (options.ContainsKey("xPosition"))
                {
                    middleImg.transform.position = new Vector3(options["xPosition"], options["yPosition"]);
                }
                else if (options.ContainsKey("xScale"))
                {
                    middleImg.transform.localScale = new Vector3(options["xScale"], options["yScale"]);
                }
                break;
            case "front":
                frontImg.sprite = sprite;
                if (options.ContainsKey("xPosition"))
                {
                    frontImg.transform.position = new Vector3(options["xPosition"], options["yPosition"]);
                }
                else if (options.ContainsKey("xScale"))
                {
                    frontImg.transform.localScale = new Vector3(options["xScale"], options["yScale"]);
                }
                break;
        }
    }
}

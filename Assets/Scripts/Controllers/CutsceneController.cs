using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using InControl;
using ScriptableObjectArchitecture;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] int backgroundCharacterId;
    
    public GameObject background;
    public GameObject back;
    public GameObject middle;
    public GameObject front;

    public GameObject dialogueBox;
    public GameObject defaultText;
    public GameObject avatar;
    public GameObject avatarText;

    public TMP_FontAsset defaultFont;
    public TMP_FontAsset anxietyFont;
    public TMP_FontAsset paranoiaFont;
    public TMP_FontAsset guiltFont;
    public TMP_FontAsset liesFont;

    public GameObject downTriangle;

    public Sprite mCAvatar;
    public Sprite anxietyAvatar;
    public Sprite paranoiaAvatar;
    public Sprite guiltAvatar;
    public Sprite liesAvatar;
    public Sprite adult1Avatar;
    public Sprite adult2Avatar;
    
    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;

    public TextAsset cutsceneScript;

    private Image avatarImg;
    private TextMeshProUGUI defaultTMP;
    private TextMeshProUGUI avatarTMP;

    private Image backgroundImg;
    private Image backImg;
    private Image middleImg;
    private Image frontImg;

    private Dictionary<int, DialogueNode> dialogueDict = new Dictionary<int, DialogueNode>();
    
    private List<KeyCode> advanceKeys = new List<KeyCode>() { KeyCode.E, KeyCode.Space, KeyCode.Return};
   
    private bool canAdvance;
    private float canAdvanceTimer;

    private int dialogueNodeNum;

    // Trigger Events
    [SerializeField] private GameEvent trigger1Event;
    [SerializeField] private GameEvent trigger2Event;
    [SerializeField] private GameEvent trigger3Event;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
        canAdvanceTimer = 1f;
        canAdvance = false;
        dialogueNodeNum = 0;
    }

    private void Start()
    {
        avatarImg = avatar.GetComponent<Image>();
        defaultTMP = defaultText.GetComponent<TextMeshProUGUI>();
        avatarTMP = avatarText.GetComponent<TextMeshProUGUI>();

        ParseTextToDialogue();
        
        StartCoroutine(ShowTextAfterWait(3f));
    }

    // Update is called once per frame
    void Update()
    {
        var inputDevice = InputManager.ActiveDevice;

        switch (canAdvance)
        {
            case false:
                canAdvanceTimer -= Time.deltaTime;
                // Transition: timer reaches zero.
                if (canAdvanceTimer <= 0)
                {
                    canAdvance = true;
                    downTriangle.SetActive(true);
                }
                break;
            case true:
                // Transition: An advance key is pressed.
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
                break;
        }
    }
    
    // Initial triggers
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

        var currentNode = dialogueDict[dialogueNodeNum];

        if (currentNode.barkId != 0)
        {
            AudioController.aC.Play(AudioController.aC.sFXSource, AudioController.aC.cutsceneBarks[currentNode.barkId], 0.4f);
        }

        if (currentNode.triggerId != 0)
        {
            switch (currentNode.triggerId)
            {
                case 1:
                    trigger1Event.Raise();
                    break;
                case 2:
                    trigger2Event.Raise();
                    break;
                case 3:
                    trigger3Event.Raise();
                    break;
            }
        }

        SetAvatarAndText(currentNode.characterId, currentNode.content);
    }

    void SetAvatarAndText(int characterId, string content)
    {
        if (characterId == backgroundCharacterId)
        {
            avatar.SetActive(false);
            avatarText.SetActive(false);

            defaultText.SetActive(true);
            defaultTMP.text = content;
            switch (characterId)
            {
                case 2: // 2 = Anxiety
                    defaultTMP.font = anxietyFont;
                    break;
                case 3: // 3 = Paranoia
                    defaultTMP.font = paranoiaFont;
                    break;
                case 4: // 4 = Guilt
                    defaultTMP.font = guiltFont;
                    break;
                case 5: // 5 = Lies
                    defaultTMP.font = liesFont;
                    break;
                default: // 1 = MC, 6 = Akir, 7 = Isla
                    defaultTMP.font = defaultFont;
                    break;
            }
        }
        else
        {
            defaultText.SetActive(false);

            avatar.SetActive(true);
            avatarText.SetActive(true);
            avatarTMP.text = content;
            switch (characterId)
            {
                case 1: // 1 = MC
                    avatarImg.sprite = mCAvatar;
                    avatarTMP.font = defaultFont;
                    break;
                case 2: // 2 = Anxiety
                    avatarImg.sprite = anxietyAvatar;
                    avatarTMP.font = anxietyFont;
                    break;
                case 3: // 3 = Paranoia
                    avatarImg.sprite = paranoiaAvatar;
                    avatarTMP.font = paranoiaFont;
                    break;
                case 4: // 4 = Guilt
                    avatarImg.sprite = guiltAvatar;
                    avatarTMP.font = guiltFont;
                    break;
                case 5: // 5 = Lies
                    avatarImg.sprite = liesAvatar;
                    avatarTMP.font = liesFont;
                    break;
                case 6: // 6 = Akir
                    avatarImg.sprite = adult1Avatar;
                    avatarTMP.font = defaultFont;
                    break;
                case 7: // 7 = Isla
                    avatarImg.sprite = adult2Avatar;
                    avatarTMP.font = defaultFont;
                    break;
            }
        }
    }

    private IEnumerator ShowTextAfterWait(float timer)
    {
        yield return new WaitForSeconds(timer);
        dialogueBox.SetActive(true);
        ShowText();
    }

    // Main method for allowing triggers to interact with the cutscene image layers, should be called by a separate script.
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using TMPro;
using ScriptableObjectArchitecture;

public class EndingTextController : MonoBehaviour
{
    public GameObject textObj;
    public TextMeshProUGUI textTMP;
    float advanceTimer;
    float advanceTimerMax = 0.75f;

    private List<KeyCode> advanceKeys = new List<KeyCode>() { KeyCode.E, KeyCode.Space, KeyCode.Return };
    int index = 0;

    [SerializeField] GameEvent levelEndEvent;

    // Start is called before the first frame update
    void Start()
    {
        textTMP = textObj.GetComponent<TextMeshProUGUI>();
        Advance();
    }

    // Update is called once per frame
    void Update()
    {
        advanceTimer -= Time.deltaTime;
        if (advanceTimer > 0) return;
        
        var inputDevice = InputManager.ActiveDevice;

        if ((inputDevice.Action1.WasPressed)
            || (inputDevice.Action3.WasPressed))
        {
            Advance();
        }
        else
        {
            foreach (KeyCode kc in advanceKeys)
            {
                if (Input.GetKeyDown(kc))
                {
                    Advance();
                    break;
                }
            }
        }
    }

    void Advance()
    {
        switch (index)
        {
            case 0:
                ShowStats();
                index++;
                break;
            case 1:
                ShowCredits();
                index++;
                break;
            case 2:
                ShowThankYou();
                index++;
                break;
            case 3:
                levelEndEvent.Raise();
                index++;
                break;
        }

        advanceTimer = advanceTimerMax;
    }

    void ShowStats()
    {
        string text = $"Total Bottles Collected: {InventoryManager.iM.inventory.Keys.Count}/12" + System.Environment.NewLine + System.Environment.NewLine +
            $"Level 1 Completion Time: {GetLevelTime(1)}" + System.Environment.NewLine +
            $"Level 2 Completion Time: {GetLevelTime(2)}" + System.Environment.NewLine +
            $"Level 3 Completion Time: {GetLevelTime(3)}" + System.Environment.NewLine +
            $"Level 4 Completion Time: {GetLevelTime(4)}" + System.Environment.NewLine;

        textTMP.text = text;
    }

    void ShowCredits()
    {
        string text = $"Andrew Han: Programmer, Writer, Level Designer" + System.Environment.NewLine + System.Environment.NewLine +
        "Joshua Lee: Composer, Sound Designer" + System.Environment.NewLine + System.Environment.NewLine +
        "Kai 1DR: Artist, Game Designer, Writer, Level Designer" + System.Environment.NewLine + System.Environment.NewLine +
        "Megha: Editor" + System.Environment.NewLine + System.Environment.NewLine +
        "Natalie Davidovic: UI Artist";

        textTMP.text = text;
    }

    void ShowThankYou()
    {
        string text = "Thank you for playing! - Jaye and the Hungry Sea Team.";
        
        textTMP.text = text;
    }

    string GetLevelTime(int level)
    {
        int levelMinutes = (int)GameController.gC.levelTimes[level-1] / 60;
        int levelSeconds = (int)GameController.gC.levelTimes[level-1] - (levelMinutes * 60);

        if (levelSeconds < 10)
            return $"{levelMinutes}:0{levelSeconds}";
        else
            return $"{levelMinutes}:{levelSeconds}";
    }
}

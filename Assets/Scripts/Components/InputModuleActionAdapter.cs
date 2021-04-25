using UnityEngine;
using InControl;


[RequireComponent(typeof(InControlInputModule))]
public class InputModuleActionAdapter : MonoBehaviour
{
    UIActions actions;

    void OnEnable()
    {
        CreateActions();

        var inputModule = GetComponent<InControlInputModule>();
        if (inputModule != null)
        {
            inputModule.SubmitAction = actions.Submit;
            inputModule.CancelAction = actions.Cancel;
            inputModule.MoveAction = actions.Move;
        }
    }

    void OnDisable()
    {
        DestroyActions();
    }

    void CreateActions()
    {
        actions = new UIActions();

        actions.Submit.AddDefaultBinding(InputControlType.Action1);
        actions.Submit.AddDefaultBinding(InputControlType.Action3);
        actions.Submit.AddDefaultBinding(Key.E);
        actions.Submit.AddDefaultBinding(Key.Space);
        actions.Submit.AddDefaultBinding(Key.Return);

        actions.Cancel.AddDefaultBinding(InputControlType.Action2);
        actions.Cancel.AddDefaultBinding(Key.Escape);

        actions.Up.AddDefaultBinding(InputControlType.LeftStickUp);
        actions.Up.AddDefaultBinding(InputControlType.DPadUp);
        actions.Up.AddDefaultBinding(Key.W);
        actions.Up.AddDefaultBinding(Key.UpArrow);

        actions.Down.AddDefaultBinding(InputControlType.LeftStickDown);
        actions.Down.AddDefaultBinding(InputControlType.DPadDown);
        actions.Down.AddDefaultBinding(Key.S);
        actions.Down.AddDefaultBinding(Key.DownArrow);

        actions.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
        actions.Left.AddDefaultBinding(InputControlType.DPadLeft);
        actions.Left.AddDefaultBinding(Key.A);
        actions.Left.AddDefaultBinding(Key.LeftArrow);

        actions.Right.AddDefaultBinding(InputControlType.LeftStickRight);
        actions.Right.AddDefaultBinding(InputControlType.DPadRight);
        actions.Right.AddDefaultBinding(Key.D);
        actions.Right.AddDefaultBinding(Key.RightArrow);
    }

    void DestroyActions()
    {
        actions.Destroy();
    }
}
using InControl;

public class PlayerActions : PlayerActionSet
{
    public PlayerAction Up;
    public PlayerAction Down;
    public PlayerOneAxisAction Vertical;
    public PlayerAction Left;
    public PlayerAction Right;
    public PlayerOneAxisAction Horizontal;
    public PlayerTwoAxisAction Move;

    public PlayerAction RowFast;
    public PlayerAction TurnLeft;
    public PlayerAction TurnRight;
    public PlayerAction CallLightning;

    public PlayerAction PauseMenu;
    public PlayerAction Inventory;
    public PlayerAction Select;

    public PlayerActions()
    {
        Up = CreatePlayerAction("Move Up");
        Down = CreatePlayerAction("Move Down");
        Vertical = CreateOneAxisPlayerAction(Down, Up);
        Left = CreatePlayerAction("Move Left");
        Right = CreatePlayerAction("Move Right");
        Horizontal = CreateOneAxisPlayerAction(Left, Right);
        Move = CreateTwoAxisPlayerAction(Left, Right, Down, Up);

        RowFast = CreatePlayerAction("Row Fast");
        TurnLeft = CreatePlayerAction("Turn Left");
        TurnRight = CreatePlayerAction("Turn Right");
        CallLightning = CreatePlayerAction("Call Lightning");

        Inventory = CreatePlayerAction("Inventory");
        PauseMenu = CreatePlayerAction("Pause Menu");
    }
}

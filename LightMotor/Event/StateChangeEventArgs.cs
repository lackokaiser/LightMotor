namespace LightMotor.Event;

/// <summary>
/// Event arguments for state changed event
/// </summary>
public class StateChangeEventArgs
{
    public GameStatus ChangedTo { get; }

    public StateChangeEventArgs(GameStatus changedTo)
    {
        ChangedTo = changedTo;
    }
}
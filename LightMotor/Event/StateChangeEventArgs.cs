namespace LightMotor.Event;

/// <summary>
/// Event arguments for state changed event
/// </summary>
public class StateChangeEventArgs
{
    /// <summary>
    /// The state that the game changed into
    /// </summary>
    public GameStatus ChangedTo { get; }

    public StateChangeEventArgs(GameStatus changedTo)
    {
        ChangedTo = changedTo;
    }
}
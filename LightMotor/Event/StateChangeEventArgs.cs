using LightMotor.Game;

namespace LightMotor.Event;

public class StateChangeEventArgs
{
    public GameStatus ChangedTo { get; }

    public StateChangeEventArgs(GameStatus changedTo)
    {
        this.ChangedTo = changedTo;
    }
}
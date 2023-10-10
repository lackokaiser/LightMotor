using LightMotor.Entities;

namespace LightMotor.Event;

/// <summary>
/// Event arguments for update event
/// </summary>
public class OnUpdateEventArgs : EventArgs
{
    public Entities.LightMotor PlayerOne { get; }

    public Entities.LightMotor PlayerTwo { get; }

    public LightLine? AddedLightOne { get; }

    public LightLine? AddedLightTwo { get; }

    public OnUpdateEventArgs(Entities.LightMotor playerOne, Entities.LightMotor playerTwo, LightLine? addedLightOne, LightLine? addedLightTwo)
    {
        PlayerOne = playerOne;
        PlayerTwo = playerTwo;
        AddedLightOne = addedLightOne;
        AddedLightTwo = addedLightTwo;
    }
}
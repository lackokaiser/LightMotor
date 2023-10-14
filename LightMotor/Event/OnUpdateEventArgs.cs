using LightMotor.Entities;

namespace LightMotor.Event;

/// <summary>
/// Event arguments for update event
/// </summary>
public class OnUpdateEventArgs : EventArgs
{
    /// <summary>
    /// The first player's motor
    /// </summary>
    public Entities.LightMotor PlayerOne { get; }

    /// <summary>
    /// The second player's motor
    /// </summary>
    public Entities.LightMotor PlayerTwo { get; }

    /// <summary>
    /// The first added light line
    /// </summary>
    public LightLine? AddedLightOne { get; }

    /// <summary>
    /// The second added light line
    /// </summary>
    public LightLine? AddedLightTwo { get; }

    public OnUpdateEventArgs(Entities.LightMotor playerOne, Entities.LightMotor playerTwo, LightLine? addedLightOne, LightLine? addedLightTwo)
    {
        PlayerOne = playerOne;
        PlayerTwo = playerTwo;
        AddedLightOne = addedLightOne;
        AddedLightTwo = addedLightTwo;
    }
}
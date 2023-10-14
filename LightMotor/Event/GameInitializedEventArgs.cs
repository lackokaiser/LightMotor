using LightMotor.Entities;

namespace LightMotor.Event;

/// <summary>
/// Data for the GameInitialized event
/// </summary>
public class GameInitializedEventArgs : EventArgs
{
    /// <summary>
    /// The size of the field
    /// </summary>
    public int Size { get; }
    /// <summary>
    /// The game status of the game
    /// </summary>
    public GameStatus GameStatus { get; }
    
    /// <summary>
    /// The first player's motor
    /// </summary>
    public Entities.LightMotor PlayerOne { get; }

    /// <summary>
    /// The second player's motor
    /// </summary>
    public Entities.LightMotor PlayerTwo { get; }
    
    /// <summary>
    /// contains every light that was added to the game
    /// </summary>
    public Entity[] Lights { get; }

    public GameInitializedEventArgs(int size, Entities.LightMotor playerOne, Entities.LightMotor playerTwo, Entity[] lights, GameStatus gameStatus)
    {
        Size = size;
        PlayerOne = playerOne;
        PlayerTwo = playerTwo;
        Lights = lights;
        GameStatus = gameStatus;
    }
}
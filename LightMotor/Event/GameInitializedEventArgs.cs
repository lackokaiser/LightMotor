using LightMotor.Entities;

namespace LightMotor.Event;

public class GameInitializedEventArgs : EventArgs
{
    public int Size { get; }
    public GameStatus GameStatus { get; }
    public Entities.LightMotor PlayerOne { get; }

    public Entities.LightMotor PlayerTwo { get; }
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
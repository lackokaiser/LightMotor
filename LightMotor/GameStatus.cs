namespace LightMotor;

/// <summary>
/// An abstract status object to hold the status singletons together
/// </summary>
public abstract class GameStatus
{}

/// <summary>
/// Singleton to represent the play status of the current game
/// </summary>
public class PlayStatus : GameStatus
{
    private static PlayStatus? instance;

    private PlayStatus(){}

    public static PlayStatus Get()
    {
        instance ??= new PlayStatus();
        return instance;
    }
}


/// <summary>
/// Singleton to represent the winning case of the first player
/// </summary>
public class FirstPlayerWinStatus : GameStatus
{
    private static FirstPlayerWinStatus? instance;

    private FirstPlayerWinStatus(){}

    public static FirstPlayerWinStatus Get()
    {
        instance ??= new FirstPlayerWinStatus();
        return instance;
    }
}

/// <summary>
/// Singleton to represent the winning case of the second player
/// </summary>
public class SecondPlayerWinStatus : GameStatus
{
    private static SecondPlayerWinStatus? instance;

    private SecondPlayerWinStatus(){}

    public static SecondPlayerWinStatus Get()
    {
        instance ??= new SecondPlayerWinStatus();
        return instance;
    }
}
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
    private static PlayStatus? _instance;

    private PlayStatus(){}

    /// <returns>The singleton instance for <see cref="PlayStatus"/></returns>
    public static PlayStatus Get()
    {
        _instance ??= new PlayStatus();
        return _instance;
    }
}


/// <summary>
/// Singleton to represent the winning case of the first player
/// </summary>
public class FirstPlayerWinStatus : GameStatus
{
    private static FirstPlayerWinStatus? _instance;

    private FirstPlayerWinStatus(){}

    /// <returns>The singleton instance for <see cref="FirstPlayerWinStatus"/></returns>
    public static FirstPlayerWinStatus Get()
    {
        _instance ??= new FirstPlayerWinStatus();
        return _instance;
    }
}

/// <summary>
/// Singleton to represent the winning case of the second player
/// </summary>
public class SecondPlayerWinStatus : GameStatus
{
    private static SecondPlayerWinStatus? _instance;

    private SecondPlayerWinStatus(){}

    /// <returns>The singleton instance for <see cref="SecondPlayerWinStatus"/></returns>
    public static SecondPlayerWinStatus Get()
    {
        _instance ??= new SecondPlayerWinStatus();
        return _instance;
    }
}

/// <summary>
/// Singleton to represent the draw case
/// </summary>
public class DrawStatus : GameStatus
{
    private static DrawStatus? _instance;

    private DrawStatus(){}

    /// <returns>The singleton instance for <see cref="DrawStatus"/></returns>
    public static DrawStatus Get()
    {
        _instance ??= new DrawStatus();
        return _instance;
    }
}
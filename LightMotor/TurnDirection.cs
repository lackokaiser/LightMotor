namespace LightMotor;


/// <summary>
/// Abstract class to hold the turn types
/// </summary>
public abstract class TurnDirection
{
    
}

/// <summary>
/// Singleton representing the "not turning" case
/// </summary>
public class NoTurn : TurnDirection
{
    private static NoTurn? _instance;
    private NoTurn(){}

    public static NoTurn Get()
    {
        _instance ??= new NoTurn();
        return _instance;
    }
}

/// <summary>
/// Singleton for the turn the motor will make
/// </summary>
public class TurnLeft : TurnDirection
{
    private static TurnLeft? _instance;
    private TurnLeft(){}

    public static TurnLeft Get()
    {
        _instance ??= new TurnLeft();
        return _instance;
    }
}

/// <summary>
/// Singleton for the turn the motor will make
/// </summary>
public class TurnRight : TurnDirection
{
    private static TurnRight? _instance;
    private TurnRight(){}

    public static TurnRight Get()
    {
        _instance ??= new TurnRight();
        return _instance;
    }
}
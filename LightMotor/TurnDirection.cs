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
    private static NoTurn? instance;
    private NoTurn(){}

    public static NoTurn Get()
    {
        instance ??= new NoTurn();
        return instance;
    }
}

/// <summary>
/// Singleton for the turn the motor will make
/// </summary>
public class TurnLeft : TurnDirection
{
    private static TurnLeft? instance;
    private TurnLeft(){}

    public static TurnLeft Get()
    {
        instance ??= new TurnLeft();
        return instance;
    }
}

/// <summary>
/// Singleton for the turn the motor will make
/// </summary>
public class TurnRight : TurnDirection
{
    private static TurnRight? instance;
    private TurnRight(){}

    public static TurnRight Get()
    {
        instance ??= new TurnRight();
        return instance;
    }
}
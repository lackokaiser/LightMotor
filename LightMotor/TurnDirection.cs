namespace LightMotor;


/// <summary>
/// Abstract class to hold the turn types
/// <seealso cref="LightMotor.Entities.LightMotor"/>
/// </summary>
public abstract class TurnDirection
{ }

/// <summary>
/// Singleton representing the "not turning" case
/// </summary>
public class NoTurn : TurnDirection
{
    private static NoTurn? _instance;
    private NoTurn(){}

    /// <returns>The singleton instance for <see cref="NoTurn"/></returns>
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

    /// <returns>The singleton instance for <see cref="TurnLeft"/></returns>
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

    /// <returns>The singleton instance for <see cref="TurnRight"/></returns>
    public static TurnRight Get()
    {
        _instance ??= new TurnRight();
        return _instance;
    }
}
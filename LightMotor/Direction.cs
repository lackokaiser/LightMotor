using LightMotor.Entities;

namespace LightMotor;

/// <summary>
/// Abstract class for the direction singleton classes
/// </summary>
public abstract class Direction
{
    protected int Rot;

    /// <summary>
    /// The rotation the <seealso cref="Entity"/> should have
    /// </summary>
    public int Rotation => Rot;

    /// <summary>
    /// Calculates the next direction for the current direction
    /// </summary>
    /// <param name="current">The current direction</param>
    /// <param name="direction">The turn we about to take</param>
    /// <returns>The calculated direction</returns>
    public static Direction GetTurnFor(Direction current, TurnDirection direction)
    {
        if (direction == NoTurn.Get())
            return current;

        if (current == NorthDirection.Get())
        {
            if(direction == TurnLeft.Get())
                return WestDirection.Get();
            return EastDirection.Get();
        }

        if (current == WestDirection.Get())
        {
            if(direction == TurnLeft.Get())
                return SouthDirection.Get();
            return NorthDirection.Get();
        }

        if (current == SouthDirection.Get())
        {
            if (direction == TurnLeft.Get())
                return EastDirection.Get();
            return WestDirection.Get();
        }

        // EastDirection
        if(direction == TurnLeft.Get())
            return NorthDirection.Get();
        return SouthDirection.Get();
            
    }
}

/// <summary>
/// Singleton class representing the west direction
/// </summary>
public class WestDirection : Direction
{
    private static WestDirection? _instance;
    private WestDirection()
    {
        Rot = 180;
    }

    /// <returns>The singleton instance for <see cref="WestDirection"/></returns>
    public static WestDirection Get()
    {
        _instance ??= new WestDirection();
        return _instance;
    }
}

/// <summary>
/// Singleton class representing the east direction
/// </summary>
public class EastDirection : Direction
{
    private static EastDirection? _instance;
    private EastDirection()
    {
        Rot = 0;
    }

    /// <returns>The singleton instance for <see cref="EastDirection"/></returns>
    public static EastDirection Get()
    {
        _instance ??= new EastDirection();
        return _instance;
    }
}

/// <summary>
/// Singleton class representing the south direction
/// </summary>
public class SouthDirection : Direction
{
    private static SouthDirection? _instance;
    private SouthDirection()
    {
        Rot = 90;
    }

    /// <returns>The singleton instance for <see cref="SouthDirection"/></returns>
    public static SouthDirection Get()
    {
        _instance ??= new SouthDirection();
        return _instance;
    }
}

/// <summary>
/// Singleton class representing the north direction
/// </summary>
public class NorthDirection : Direction
{
    private static NorthDirection? _instance;
    private NorthDirection()
    {
        Rot = 270;
    }

    /// <returns>The singleton instance for <see cref="NorthDirection"/></returns>
    public static NorthDirection Get()
    {
        _instance ??= new NorthDirection();
        return _instance;
    }
}
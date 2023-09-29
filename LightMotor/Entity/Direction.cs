namespace LightMotor.Entity;

/// <summary>
/// Abstract class for the direction singleton classes
/// </summary>
public abstract class Direction
{
    protected int rotation;

    /// <summary>
    /// The rotation the <seealso cref="Entity"/> should have
    /// </summary>
    public int Rotation => rotation;
}

/// <summary>
/// Singleton class representing the west direction
/// </summary>
public class WestDirection : Direction
{
    private static WestDirection? instance;
    private WestDirection()
    {
        rotation = 0; // figure out rotation
    }

    public static WestDirection Get()
    {
        instance ??= new WestDirection();
        return instance;
    }
}

/// <summary>
/// Singleton class representing the east direction
/// </summary>
public class EastDirection : Direction
{
    private static EastDirection? instance;
    private EastDirection()
    {
        rotation = 0; // figure out rotation
    }

    public static EastDirection Get()
    {
        instance ??= new EastDirection();
        return instance;
    }
}

/// <summary>
/// Singleton class representing the south direction
/// </summary>
public class SouthDirection : Direction
{
    private static SouthDirection? instance;
    private SouthDirection()
    {
        rotation = 0; // figure out rotation
    }

    public static SouthDirection Get()
    {
        instance ??= new SouthDirection();
        return instance;
    }
}

/// <summary>
/// Singleton class representing the north direction
/// </summary>
public class NorthDirection : Direction
{
    private static NorthDirection? instance;
    private NorthDirection()
    {
        rotation = 0; // figure out rotation
    }

    public static NorthDirection Get()
    {
        instance ??= new NorthDirection();
        return instance;
    }
}
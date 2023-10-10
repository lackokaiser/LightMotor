namespace LightMotor;

/// <summary>
/// Abstract class to hold the input type singletons
/// </summary>
public abstract class InputType
{ }

/// <summary>
/// Singleton representing the left turn's input
/// </summary>
public class LeftInput : InputType
{
    private static LeftInput? _instance;
    
    private LeftInput(){}

    public static LeftInput Get()
    {
        _instance ??= new LeftInput();
        return _instance;
    }
}

/// <summary>
/// Singleton representing the right turn's input
/// </summary>
public class RightInput : InputType
{
    private static RightInput? _instance;
    
    private RightInput(){}

    public static RightInput Get()
    {
        _instance ??= new RightInput();
        return _instance;
    }
}
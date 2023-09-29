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
    private static LeftInput? instance;
    
    private LeftInput(){}

    public static LeftInput Get()
    {
        instance ??= new LeftInput();
        return instance;
    }
}

/// <summary>
/// Singleton representing the right turn's input
/// </summary>
public class RightInput : InputType
{
    private static RightInput? instance;
    
    private RightInput(){}

    public static RightInput Get()
    {
        instance ??= new RightInput();
        return instance;
    }
}
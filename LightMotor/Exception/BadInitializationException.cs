namespace LightMotor.Exception;

public class BadInitializationException : System.Exception
{
    public BadInitializationException(string? message) : base(message)
    {
    }
}
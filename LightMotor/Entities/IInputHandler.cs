namespace LightMotor.Entities;

/// <summary>
/// Interface for input handling on Entities
/// <seealso cref="Entity"/>
/// </summary>
public interface IInputHandler
{
    /// <summary>
    /// Handles a specific input from the view
    /// </summary>
    /// <param name="input">The input itself, can be left or right turn</param>
    /// <seealso cref="InputType"/>
    void AcceptInput(InputType input);
}
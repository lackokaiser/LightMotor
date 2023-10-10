namespace WinPersistance;

/// <summary>
/// Savable interface for savable objects
/// </summary>
public interface ISavable
{
    /// <summary>
    /// Converts the object into a string representation
    /// </summary>
    /// <returns>The string representation</returns>
    string Save(string pre = "");
}
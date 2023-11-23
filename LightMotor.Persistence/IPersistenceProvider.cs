namespace LightMotor.Persistence;

public interface IPersistenceProvider
{
    /// <summary>
    /// Loads data from the disc
    /// </summary>
    /// <param name="fileName">The file to load from</param>
    /// <returns>The loaded string</returns>
    /// <exception cref="FileNotFoundException">When the file does not exists</exception>
    public Task<string> Read(string fileName);

    /// <summary>
    /// Write an object to the destination file, if the file exists, it is overwritten
    /// </summary>
    /// <param name="fileName">The destination file</param>
    /// <param name="obj">The object to write</param>
    /// <seealso cref="ISavable"/>
    public Task Write(string fileName, ISavable obj);
}
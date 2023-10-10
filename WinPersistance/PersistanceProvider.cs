namespace WinPersistance;

/// <summary>
/// Represents a file I/O interface for the program to read and write information to the disc
/// </summary>
public abstract class PersistanceProvider
{
    /// <summary>
    /// Loads data from the disc, then calls the <see cref="Load"/> method to handle that data
    /// </summary>
    /// <param name="file">The file to load from</param>
    /// <exception cref="FileNotFoundException">When the file does not exists</exception>
    public void LoadFile(string file)
    {
        if (!File.Exists(file))
            throw new FileNotFoundException($"File does not exists with name: {file}");
        using StreamReader sr = new StreamReader(file);

        string d = sr.ReadToEnd();
        
        sr.Close();
        
        Load(d);
    }

    /// <summary>
    /// Handles incoming data from the disc
    /// </summary>
    /// <param name="data">The data loaded from the disc</param>
    /// <seealso cref="LoadFile"/>
    protected abstract void Load(string data);

    /// <summary>
    /// Write an object to the destination file, if the file exists, it is overwritten
    /// </summary>
    /// <param name="fileName">The destination file</param>
    /// <param name="obj">The object to write</param>
    /// <seealso cref="ISavable"/>
    protected void Write(string fileName, ISavable obj)
    {
        using StreamWriter sw = new StreamWriter(fileName);
        sw.Write(obj.Save());
            
        sw.Close();
    }
}
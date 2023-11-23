namespace LightMotor.Persistence;

public class WindowsPersistenceProvider : IPersistenceProvider
{
    public async Task<string> Read(string fileName)
    {
        if (!File.Exists(fileName))
            throw new FileNotFoundException($"File does not exists with name: {fileName}");
        using StreamReader sr = new StreamReader(fileName);

        string d = await sr.ReadToEndAsync();
        
        sr.Close();

        return d;
    }

    public async Task Write(string fileName, ISavable obj)
    {
        await using StreamWriter sw = new StreamWriter(fileName);
        await sw.WriteAsync(obj.Save());
            
        sw.Close();
    }
}
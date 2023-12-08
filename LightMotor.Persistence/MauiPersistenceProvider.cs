namespace LightMotor.Persistence;

public class MauiPersistenceProvider : IPersistenceProvider
{
    public async Task<string> Read(string fileName)
    {
        await using FileStream inputStream = File.OpenRead(fileName);  
        using StreamReader reader = new StreamReader(inputStream);
        string res = await reader.ReadToEndAsync();
        reader.Close();
        inputStream.Close();
        return res;
    }

    public async Task Write(string fileName, ISavable obj)
    {
        await using FileStream outputStream = File.OpenWrite(fileName);
        await using StreamWriter streamWriter = new StreamWriter(outputStream);
        await streamWriter.WriteAsync(obj.Save());
        
        streamWriter.Close();
        outputStream.Close();
    }
}
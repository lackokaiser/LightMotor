using LightMotor.Entities;
using LightMotor.Root;

namespace WinPersistance.Tests;

[TestClass]
public class SaveLoadTests
{
    
    [TestMethod]
    public void SaveTest()
    {
        Game g = new ();
        Assert.ThrowsException<ApplicationException>(() => g.SaveGame("failing.txt"));
        
        g.Init(10);
        
        g.SaveGame("file.txt");
        
        Assert.IsTrue(File.Exists("file.txt"));

        using (StreamReader sr = new StreamReader("file.txt"))
        {
            string firstLine = sr.ReadLine()!;
            
            Assert.AreEqual("10 0 2", firstLine);

            string secondLine = sr.ReadLine()!;
            
            Assert.AreEqual("0 4 5 3 0", secondLine);

            string thirdLine = sr.ReadLine()!;
            
            Assert.AreEqual("0 5 6 1 0", thirdLine);
            Assert.IsTrue(sr.EndOfStream);
            
            sr.Close();
        }
        
        File.Delete("file.txt");
    }

    [TestMethod]
    public void LoadTest()
    {
        Game g = new ();
        
        g.Init(10);

        Field f1 = g.GetFieldValue<Field>("_field");
        IInputHandler[] c1 = f1.GetFieldValue<IInputHandler[]>("_playerHandlers");
        
        g.SaveGame("file.txt");

        Game loaded = new();
        
        loaded.LoadFile("file.txt");

        Field f2 = loaded.GetFieldValue<Field>("_field");
        IInputHandler[] c2 = f2.GetFieldValue<IInputHandler[]>("_playerHandlers");
        
        Assert.IsNotNull(f2);
        Assert.AreEqual(f1.Size, f2.Size);
        Assert.AreEqual(f1.Entities.Count, f2.Entities.Count);
        Assert.AreEqual(f1.GameStatus, f2.GameStatus);
        Assert.IsNotNull(c2);
        Assert.AreEqual(c1.Length, c2.Length);
        
        File.Delete("file.txt");
    }
}
using LightMotor.Entities;
using LightMotor.Root;

namespace LightMotor.Persistence.Tests;

[TestClass]
public class SaveLoadTests
{

    [TestMethod]
    public void EntityPersistenceTest()
    {
        LightMotor.Entities.LightMotor motor = new LightMotor.Entities.LightMotor(new Position(10, 13), SouthDirection.Get(), NoTurn.Get());

        string data = motor.Save();
        
        Assert.AreEqual("0 10 13 2 0", data);
        
        Entity? ent = Entity.Load(data);
        Assert.IsNotNull(ent);
        Assert.IsTrue(ent is LightMotor.Entities.LightMotor);
        Assert.AreEqual(new Position(10, 13), ent.Position);
        Assert.AreEqual(SouthDirection.Get(), ent.Direction);

        LightLine line = new LightLine(new Position(10, 0), WestDirection.Get(), TurnRight.Get());

        data = line.Save();
        
        Assert.AreEqual("1 10 0 3 2", data);
        
        Entity? light = Entity.Load(data);
        
        Assert.IsNotNull(light);
        Assert.IsTrue(light is LightLine);
        Assert.AreEqual(new Position(10, 0), light.Position);
        Assert.AreEqual(WestDirection.Get(), light.Direction);
    }
    
    [TestMethod]
    public async Task SaveTest()
    {
        Game g = new (new WindowsPersistenceProvider());
        await Assert.ThrowsExceptionAsync<ApplicationException>( async () => await g.SaveGame("failing.txt"));
        
        g.Init(10);
        
        await g.SaveGame("file.txt");
        
        Assert.IsTrue(File.Exists("file.txt"));

        using (StreamReader sr = new StreamReader("file.txt"))
        {
            string firstLine = (await sr.ReadLineAsync())!;
            
            Assert.AreEqual("10 0 2", firstLine);

            string secondLine = (await sr.ReadLineAsync())!;
            
            Assert.AreEqual("0 4 5 3 0", secondLine);

            string thirdLine = (await sr.ReadLineAsync())!;
            
            Assert.AreEqual("0 5 6 1 0", thirdLine);
            Assert.IsTrue(sr.EndOfStream);
            
            sr.Close();
        }
        
        File.Delete("file.txt");
    }

    [TestMethod]
    public async Task LoadTest()
    {
        Game g = new (new WindowsPersistenceProvider());
        g.Init(10);

        Field f1 = g.GetFieldValue<Field>("_field");
        IInputHandler[] c1 = f1.GetFieldValue<IInputHandler[]>("_playerHandlers");

        await g.SaveGame("file.txt");
        
        Game loaded = new(new WindowsPersistenceProvider());

        await loaded.LoadFile("file.txt");
        
        Field f2 = loaded.GetFieldValue<Field>("_field");
        Assert.IsNotNull(f2);
        Assert.AreEqual(f1.Size, f2.Size);
        Assert.AreEqual(f1.Entities.Count, f2.Entities.Count);
        Assert.AreEqual(f1.GameStatus, f2.GameStatus);

        IInputHandler[] c2 = f2.GetFieldValue<IInputHandler[]>("_playerHandlers");
        Assert.IsNotNull(c2);
        Assert.AreEqual(c1.Length, c2.Length);
        
        File.Delete("file.txt");
        
        await Assert.ThrowsExceptionAsync<FileNotFoundException>(async () => await loaded.LoadFile("file.txt"));

        await using (StreamWriter sw = new StreamWriter("file.txt"))
        {
            await sw.WriteLineAsync("Wrong data");
            await sw.WriteLineAsync("12 wrong bad 0");
            
            sw.Close();
        }
        
        await Assert.ThrowsExceptionAsync<FormatException>( async () => await g.LoadFile("file.txt"));
        
        
        File.Delete("file.txt");
    }
}
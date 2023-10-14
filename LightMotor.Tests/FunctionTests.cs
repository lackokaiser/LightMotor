
using LightMotor.Root;

namespace LightMotor.Tests;


[TestClass]
public class FunctionTests
{
    [TestMethod]
    public void GameInitializationTest()
    {
        Game gm = new Game();

        Assert.ThrowsException<Exception>(() => gm.Run());
        Assert.ThrowsException<ApplicationException>(() => gm.SaveGame("failing.txt"));
    }

    [TestMethod]
    public void FieldInitializationTest()
    {
        Field f = new Field(10);
        
        Assert.AreEqual(10, f.Size);
        Assert.AreEqual(PlayStatus.Get(), f.GameStatus);
        Assert.AreEqual(2, f.Entities.Count);
        Assert.IsTrue(f.Entities[0] is Entities.LightMotor);
        Assert.IsTrue(f.Entities[1] is Entities.LightMotor);
    }

    [TestMethod]
    public void FieldInputHandlingTest()
    {
        Field f = new Field(10);

        var playerOne = (Entities.LightMotor)f.Entities[0];
        
        f.AcceptInput(NorthDirection.Get(), 0);

        Assert.IsTrue(playerOne.NextTurnDirection is TurnRight);

        var playerTwo = (Entities.LightMotor)f.Entities[1];
        
        f.AcceptInput(WestDirection.Get(), 1);
        
        Assert.IsTrue(playerTwo.NextTurnDirection is NoTurn);
        
        Assert.ThrowsException<>()
    }
}
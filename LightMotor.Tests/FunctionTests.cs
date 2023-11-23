using LightMotor.Entities;
using LightMotor.Persistence;
using LightMotor.Root;
using LightMotorClass = LightMotor.Entities.LightMotor;

namespace LightMotor.Tests;


[TestClass]
public class FunctionTests
{
    [TestMethod]
    public void GameInitializationTest()
    {
        Game gm = new Game(new WindowsPersistenceProvider());
        
        Assert.IsFalse(gm.Paused);
        var field = gm.GetFieldValue<Field?>("_field");
        var tokenGen = gm.GetFieldValue<CancellationTokenSource?>("_token");
        Assert.IsNull(field);
        Assert.IsNull(tokenGen);
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

        Assert.ThrowsException<IndexOutOfRangeException>(() => f.AcceptInput(SouthDirection.Get(), 2));
        Assert.ThrowsException<IndexOutOfRangeException>(() => f.AcceptInput(SouthDirection.Get(), -1));
    }

    [TestMethod]
    public void GameFieldInitializationTest()
    {
        Game g = new (new WindowsPersistenceProvider());
        g.Paused = true;
        
        g.Init(12);
        
        Assert.IsFalse(g.Paused);
        Field f = g.GetFieldValue<Field>("_field");
        Assert.AreEqual(12, f.Size);
        Assert.AreEqual(PlayStatus.Get(), f.GameStatus);
        Assert.AreEqual(2, f.Entities.Count);
        Assert.IsTrue(f.Entities[0] is Entities.LightMotor);
        Assert.IsTrue(f.Entities[1] is Entities.LightMotor);
    }

    [TestMethod]
    public void PositionTest()
    {
        Position position = new Position(0, 0);
        position.AddX(1);
        
        Assert.AreEqual(1, position.X);
        Assert.AreEqual(0, position.Y);
        
        position.AddY(-1);
        
        Assert.AreEqual(1, position.X);
        Assert.AreEqual(-1, position.Y);
        
        position.AddX(0);

        Assert.AreEqual(1, position.X);
        Assert.AreEqual(-1, position.Y);

        Assert.IsTrue(position.IsOutOfBounds(0, 0, 10, 10));
        
        position.AddY(1);
        position.AddX(-1);
        
        Assert.IsFalse(position.IsOutOfBounds(0, 0, 10, 10));
        
        position.AddY(5);
        position.AddX(5);

        Assert.IsFalse(position.IsOutOfBounds(0, 0, 10, 10));
        
        position.AddY(5);
        position.AddX(5);

        Assert.IsFalse(position.IsOutOfBounds(0, 0, 10, 10));

        Position newPos = new Position(10, 10);
        
        Assert.IsTrue(newPos == position);
        newPos.AddY(1);
        
        Assert.IsTrue(newPos != position);
    }

    [TestMethod]
    public void EntityTest()
    {
        Entity entity = new LightLine(new Position(0, 0), WestDirection.Get(), NoTurn.Get());
        
        Assert.AreEqual(new Position(0, 0), entity.Position);
        Assert.AreEqual(WestDirection.Get(), entity.Direction);
        Assert.AreEqual(NoTurn.Get(), entity.GetFieldValue<TurnDirection>("TurnDirection"));
        
        LightMotorClass entity2 = new LightMotorClass(new Position(0, 10), SouthDirection.Get(), TurnLeft.Get());
        
        Assert.AreEqual(new Position(0, 10), entity2.Position);
        Assert.AreEqual(SouthDirection.Get(), entity2.Direction);
        Assert.AreEqual(TurnLeft.Get(), entity2.GetFieldValue<TurnDirection>("TurnDirection"));
        
        Assert.IsFalse(entity.IsTouching(entity2));
        
        
        Assert.IsTrue(entity.IsTouching(entity));
        
        entity2.AcceptInput(RightInput.Get());
        
        entity2.Update();
        
        Assert.AreEqual(new Position(-1, 10), entity2.Position);
    }
}
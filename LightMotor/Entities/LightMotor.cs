using System.Text;

namespace LightMotor.Entities;

/// <summary>
/// A player controlled motor on the field
/// </summary>
public class LightMotor : Entity, IInputHandler
{
    public LightMotor(Position pos, Direction dir, TurnDirection turnDirection) : base(pos, dir, turnDirection)
    {
    }

    /// <summary>
    /// The next turn direction of the player <br/>
    /// Accessibility to this value ensures the creations of new light lines
    /// </summary>
    public TurnDirection NextTurnDirection => TurnDirection;

    public override void Update()
    {
        Dir = Direction.GetTurnFor(Dir, TurnDirection);
        TurnDirection = NoTurn.Get(); 
        
        if(Dir == NorthDirection.Get())
            Pos.AddY(1);
        else if(Dir == EastDirection.Get())
            Pos.AddX(1);
        else if(Dir == SouthDirection.Get())
            Pos.AddY(-1);
        else if(Dir == WestDirection.Get())
            Pos.AddX(-1);
    }

    public override string Save(string pre = "")
    {
        StringBuilder stb = new StringBuilder();

        stb.Append(0 + " ");
        
        return base.Save(stb.ToString());
    }

    public void AcceptInput(InputType? input)
    {
        if(input == RightInput.Get())
            TurnDirection = TurnRight.Get();
        else if(input == LeftInput.Get())
            TurnDirection = TurnLeft.Get();
    }
}
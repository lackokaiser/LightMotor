using System.Text;

namespace LightMotor.Entities;

public class LightMotor : Entity, IInputHandler
{
    public LightMotor(Position pos, Direction direction, TurnDirection turnDirection) : base(pos, direction, turnDirection)
    {
    }

    /// <summary>
    /// Accessibility to this value ensures the creations of new light lines
    /// </summary>
    public TurnDirection NextTurnDirection => turnDirection;

    public override void Update()
    {
        // TODO: spawn new light using the direction and the next direction
        
        direction = Direction.GetTurnFor(direction, turnDirection);
        turnDirection = NoTurn.Get(); 
        
        if(direction == NorthDirection.Get())
            pos.AddY(1);
        else if(direction == EastDirection.Get())
            pos.AddX(1);
        else if(direction == SouthDirection.Get())
            pos.AddY(-1);
        else if(direction == WestDirection.Get())
            pos.AddX(-1);
    }

    public override string Save(string pre = "")
    {
        StringBuilder stb = new StringBuilder();

        stb.Append(0 + " ");
        
        return base.Save(stb.ToString());
    }

    public void AcceptInput(InputType input)
    {
        if(input == RightInput.Get())
            turnDirection = TurnRight.Get();
        else if(input == LeftInput.Get())
            turnDirection = TurnLeft.Get();
    }
}
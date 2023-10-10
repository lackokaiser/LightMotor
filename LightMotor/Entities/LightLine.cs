using System.Text;

namespace LightMotor.Entities;

public class LightLine : Entity
{
    public TurnDirection FacingDirection => TurnDirection;
    
    public LightLine(Position pos, Direction direction, TurnDirection turnDirection) : base(pos, direction, turnDirection)
    {
    }

    public override string Save(string pre = "")
    {
        StringBuilder stb = new StringBuilder();

        stb.Append(1 + " ");
        
        return base.Save(stb.ToString());
    }

    public override void Update()
    {
    }
}
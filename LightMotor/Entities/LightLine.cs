using System.Text;

namespace LightMotor.Entities;

/// <summary>
/// Represents a light line on the field spawned by one of the players
/// </summary>
public class LightLine : Entity
{
    /// <summary>
    /// Value to determine which direction the light should face
    /// </summary>
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

    /// <summary>
    /// <inheritdoc cref="Entity.Update"/>. In this instance, update is not needed
    /// </summary>
    public override void Update()
    {
    }
}
namespace LightMotor.Entities;

public class LightMotor : Entity, IInputHandler
{
    private TurnDirection nextTurnDirection = NoTurn.Get();
    
    public LightMotor(Position pos, Direction direction) : base(pos, direction)
    {
    }

    public override void Update()
    {
        direction = Direction.GetTurnFor(direction, nextTurnDirection);
        nextTurnDirection = NoTurn.Get();
        
        
    }

    public void AcceptInput(InputType input)
    {
        throw new NotImplementedException();
    }
}
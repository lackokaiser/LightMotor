using LightMotor;
using LightMotor.Root;

namespace LightMotorViewModel.Command.Movement;

public class RightCommand : MoveCommandBase
{
    public RightCommand(ref Game game, int playerNo) : base(ref game, playerNo)
    {
    }

    protected override Direction GetDirection()
    {
        return EastDirection.Get();
    }
}
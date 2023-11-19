using LightMotor;
using LightMotor.Root;

namespace LightMotorViewModel.Command.Movement;

public class DownCommand : MoveCommandBase
{
    public DownCommand(ref Game game, int playerNo) : base(ref game, playerNo)
    {
    }

    protected override Direction GetDirection()
    {
        return NorthDirection.Get();
    }
}
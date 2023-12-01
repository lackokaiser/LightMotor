using LightMotor.Root;

namespace LightMotor.ViewModel.Command.Movement;

public class UpCommand : MoveCommandBase
{
    public UpCommand(ref Game game, int playerNo) : base(ref game, playerNo)
    {
    }

    protected override Direction GetDirection()
    {
        return SouthDirection.Get();
    }
}
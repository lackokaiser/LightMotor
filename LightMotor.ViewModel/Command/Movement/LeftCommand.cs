using LightMotor.Root;

namespace LightMotor.ViewModel.Command.Movement;
public class LeftCommand : MoveCommandBase
{
    public LeftCommand(ref Game game, int playerNo) : base(ref game, playerNo)
    {
    }

    protected override Direction GetDirection()
    {
        return WestDirection.Get();
    }
}
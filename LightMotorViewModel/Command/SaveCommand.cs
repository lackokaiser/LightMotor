using LightMotor.Root;

namespace LightMotorViewModel.Command;

public class SaveCommand : CommandBase
{
    private readonly Game _game;

    public SaveCommand(ref Game game)
    {
        _game = game;
    }

    public override void Execute(object? parameter)
    {
        // TODO: services
    }
}
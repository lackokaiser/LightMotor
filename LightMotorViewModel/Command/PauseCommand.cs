using LightMotor.Root;

namespace LightMotorViewModel.Command;

public class PauseCommand : CommandBase
{
    private readonly Game _game;

    public PauseCommand(ref Game game)
    {
        _game = game;
    }

    public override void Execute(object? parameter)
    {
        _game.Paused = !_game.Paused;
    }
}
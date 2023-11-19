using LightMotor.Root;
using LightMotorViewModel.ViewModel;

namespace LightMotorViewModel.Command;

public class EscapeCommand : CommandBase
{
    private Game _game;

    public EscapeCommand(ref Game game)
    {
        _game = game;
    }

    public override void Execute(object? parameter)
    {
        _game.Stop();
        Navigation.Get().CurrentViewModel = new MenuViewModel(ref _game);
    }
}
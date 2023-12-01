using LightMotor.Root;
using GameViewModel = LightMotor.ViewModel.ViewModel.GameViewModel;

namespace LightMotor.ViewModel.Command;

public class PauseCommand : CommandBase
{
    private readonly Game _game;
    private readonly GameViewModel _vm;

    public PauseCommand(ref Game game, GameViewModel vm)
    {
        _game = game;
        _vm = vm;
    }

    public override void Execute(object? parameter)
    {
        _game.Paused = !_game.Paused;
        _vm.Status = _game.Paused ? "Paused!" : "";
    }
}
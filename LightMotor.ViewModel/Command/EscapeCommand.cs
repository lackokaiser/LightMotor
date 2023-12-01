using LightMotor.Root;
using LightMotor.ViewModel.ViewModel;
using GameViewModel = LightMotor.ViewModel.ViewModel.GameViewModel;

namespace LightMotor.ViewModel.Command;

public class EscapeCommand : CommandBase
{
    private Game _game;
    private readonly GameViewModel _vm;

    public EscapeCommand(ref Game game, GameViewModel vm)
    {
        _game = game;
        _vm = vm;
    }

    public override void Execute(object? parameter)
    {
        _game.Stop();
        _vm.RemoveListeners();
        Navigation.Get().CurrentViewModel = new MenuViewModel(ref _game);
    }
}
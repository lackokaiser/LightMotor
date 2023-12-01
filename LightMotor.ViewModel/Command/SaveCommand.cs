using LightMotor.Root;
using GameViewModel = LightMotor.ViewModel.ViewModel.GameViewModel;

namespace LightMotor.ViewModel.Command;

public class SaveCommand : CommandBase
{
    private readonly Game _game;
    private readonly GameViewModel _vm;
    public SaveCommand(ref Game game, GameViewModel vm)
    {
        _game = game;
        _vm = vm;
    }

    public override void Execute(object? parameter)
    {
        string prevStatus = _vm.Status;
        bool paused = _game.Paused;
        
        _vm.Status = "Saving!";
        _game.Paused = true;
        
        ViewCallback.Get().SaveFile();
        
        if(!string.IsNullOrEmpty(ViewCallback.Get().OpenedFile))
            _game.SaveGame(ViewCallback.Get().OpenedFile!);

        _vm.Status = prevStatus;
        if (!paused)
            _game.Paused = false;
    }
}
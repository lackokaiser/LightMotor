using LightMotor.Root;
using LightMotorViewModel.ViewModel;

namespace LightMotorViewModel.Command;

public class StartGameCommand : CommandBase
{
    private readonly MenuViewModel _menu;
    private readonly Game _model;

    public StartGameCommand(ref Game model, MenuViewModel menu)
    {
        _model = model;
        _menu = menu;
    }

    public override void Execute(object? parameter)
    {
        _model.Init(_menu.BoardSize);
    }
}
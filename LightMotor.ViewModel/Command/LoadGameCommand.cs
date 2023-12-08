using LightMotor.Root;

namespace LightMotor.ViewModel.Command;

public class LoadGameCommand : CommandBase
{
    private readonly Game _model;

    public LoadGameCommand(ref Game model)
    {
        _model = model;
    }

    public override async void Execute(object? parameter)
    {
        ViewCallback.Get().OpenFile();

        if (!string.IsNullOrEmpty(ViewCallback.Get().OpenedFile))
            await _model.LoadFile(ViewCallback.Get().OpenedFile!);
    }
}
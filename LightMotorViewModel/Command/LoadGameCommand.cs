using LightMotor.Root;

namespace LightMotorViewModel.Command;

public class LoadGameCommand : CommandBase
{
    private readonly Game _model;

    public LoadGameCommand(ref Game model)
    {
        _model = model;
    }

    public override void Execute(object? parameter)
    {
        // Todo: Service
    }
}
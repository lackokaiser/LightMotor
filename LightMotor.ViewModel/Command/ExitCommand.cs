namespace LightMotor.ViewModel.Command;

public class ExitCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        Environment.Exit(0);
    }
}
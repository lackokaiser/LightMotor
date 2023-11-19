using LightMotorViewModel.EventArgs;

namespace LightMotorViewModel.Command;

public class ShowHelpCommand : CommandBase
{
   public override void Execute(object? parameter)
    {
        ViewCallback.Get().ShowMessage(new ShowMessageEventArgs("Press W, A, S, D to control player 1\nPress the arrow keys to control player 2\nPress p to toggle pause", "Help"));
    }
}
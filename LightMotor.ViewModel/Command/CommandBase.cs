using System.Windows.Input;

namespace LightMotorViewModel.Command;

/// <summary>
/// Base class for all commands
/// </summary>
public abstract class CommandBase : ICommand
{
    public virtual bool CanExecute(object? parameter)
    {
        return true;
    }

    public abstract void Execute(object? parameter);

    public event EventHandler? CanExecuteChanged;

    protected void CanExecuteChangedInvoke()
    {
        CanExecuteChanged?.Invoke(this, System.EventArgs.Empty);
    }
}
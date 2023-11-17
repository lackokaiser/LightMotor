﻿using System.Windows.Input;

namespace LightMotorViewModel.Command;

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
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
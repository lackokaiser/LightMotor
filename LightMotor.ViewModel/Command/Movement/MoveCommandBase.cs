using System.ComponentModel.DataAnnotations;
using LightMotor;
using LightMotor.Root;

namespace LightMotorViewModel.Command.Movement;

/// <summary>
/// Base class for all movement related command
/// </summary>
public abstract class MoveCommandBase : CommandBase
{
    private readonly Game _game;
    private readonly int _playerNo;

    public MoveCommandBase(ref Game game, [Range(0, 1)] int playerNo)
    {
        this._game = game;
        _playerNo = playerNo;
    }

    public override void Execute(object? parameter)
    {
        _game.AcceptInput(GetDirection(), _playerNo);
    }

    /// <summary>
    /// Determines the direction the control should pass on to the model
    /// </summary>
    /// <returns>The direction that should be used</returns>
    protected abstract Direction GetDirection();
}
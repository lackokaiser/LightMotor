using LightMotor.Event;
using LightMotor.Exception;

namespace LightMotor.Game;

public delegate void OnGameUpdate(object source, EventArgs e);
public delegate void OnGameStateChanged(object obj, StateChangeEventArgs e);

/// <summary>
/// This class controls the game's state and updates
/// <seealso cref="Field"/>
/// </summary>
public class Game
{
    public event OnGameUpdate? OnUpdate;
    public event OnGameStateChanged? OnStateChanged;
    private Field? field;
    private CancellationTokenSource? _token;
    
    /// <summary>
    /// Represents the paused state of the current game
    /// </summary>
    public bool Paused { get; set; }

    /// <summary>
    /// Initializes the game with the given size, not ideal to call, when a game is already running
    /// </summary>
    /// <param name="n">The size of the new field</param>
    /// <seealso cref="Field"/>
    public void Init(int n)
    {
        // cancel the previous game if possible
        Stop();
        
        _token = new CancellationTokenSource();
        field = new Field(n);
    }
    
    /// <summary>
    /// Handles input from the view
    /// </summary>
    /// <param name="input">The input type that was received</param>
    /// <param name="player">The variant of the input <br/>0 for player-1 <br/>1 for player-2</param>
    public void AcceptInput(InputType input, int player)
    {
        field?.AcceptInput(input, player);
    }

    /// <summary>
    /// Invokes the <see cref="OnGameUpdate"/> event
    /// </summary>
    /// <param name="source">The event's source</param>
    private void OnUpdateInvoke(object source)
    {
        OnUpdate?.Invoke(source, EventArgs.Empty);
    }

    /// <summary>
    /// Invokes the <see cref="OnGameStateChanged"/> event
    /// </summary>
    /// <param name="source">The event's source</param>
    /// <param name="e">Args containing the game's status</param>
    private void OnStateChangedInvoke(object source, StateChangeEventArgs e)
    {
        OnStateChanged?.Invoke(source, e);
    }

    /// <summary>
    /// Starts the execution of the game
    /// <exception cref="BadInitializationException">If the <see cref="Init"/> function has not yet been called</exception>
    /// </summary>
    public async Task Run()
    {
        if (field == null || _token == null)
        {
            throw new BadInitializationException("Can't start an empty game!");
        }

        await Task.Run(() =>
        {
            while (true)
            {
                Thread.Sleep(1000);
                if (_token.Token.IsCancellationRequested)
                    break;
                
                if(Paused)
                    continue;

                if (field.Update())
                {
                    OnStateChangedInvoke(this, new StateChangeEventArgs(field.GameStatus));
                }
                OnUpdateInvoke(this);

                
            }
        }, _token.Token);
    }

    /// <summary>
    /// Called when the game should stop indefinitely, if you prefer to pause the game instead, use the <see cref="Paused"/> property
    /// </summary>
    public void Stop()
    {
        _token?.Cancel();
    }
}
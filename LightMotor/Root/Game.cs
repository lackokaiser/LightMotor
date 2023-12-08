using LightMotor.Entities;
using LightMotor.Event;
using LightMotor.Persistence;

namespace LightMotor.Root;

public delegate void OnGameUpdate(object source, OnUpdateEventArgs e);
public delegate void OnGameStateChanged(object obj, StateChangeEventArgs e);

public delegate void OnGameInitialized(object obj, GameInitializedEventArgs e);

/// <summary>
/// This class controls the game's state and updates
/// <seealso cref="Field"/>
/// </summary>
public class Game : IDisposable
{
    /// <summary>
    /// Event which is called whenever the game updates
    /// </summary>
    public event OnGameUpdate? OnUpdate;
    
    /// <summary>
    /// Event which is called whenever the game's state changes
    /// </summary>
    public event OnGameStateChanged? OnStateChanged;
    
    /// <summary>
    /// Event which is called when the game is initialized
    /// </summary>
    public event OnGameInitialized? OnGameInitialized;
    private Field? _field;
    private CancellationTokenSource? _token;
    private readonly IPersistenceProvider _persistenceProvider;
    
    /// <summary>
    /// Represents the paused state of the current game
    /// </summary>
    public bool Paused { get; set; }

    public Game(IPersistenceProvider persistenceProvider)
    {
        _persistenceProvider = persistenceProvider;
    }

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
        _field = new Field(n);
        var entities = _field.Entities;
        OnGameInitializedInvoke(this, new GameInitializedEventArgs(_field.Size, (Entities.LightMotor)entities[0], (Entities.LightMotor)entities[1], 
            Array.Empty<Entity>(), PlayStatus.Get()));
   }

    /// <summary>
    /// Saves the current game and places it's data into a file
    /// </summary>
    /// <param name="file">The destination file</param>
    /// <exception cref="ApplicationException">If the game is not yet initialized</exception>
    public async Task SaveGame(string file)
    {
        if (_field == null)
            throw new ApplicationException("Cannot save an empty game");
        await _persistenceProvider.Write(file, _field);
    }

    public async Task LoadFile(string file)
    {
        string data = await _persistenceProvider.Read(file);
        Stop();

        _token = new CancellationTokenSource();
        _field = new Field(data);
        
        var entities = _field.Entities;
        var lights = new Entity[entities.Count - 2];
        for (int i = 2; i < entities.Count; i++)
        {
            lights[i - 2] = entities[i];
        }
        OnGameInitializedInvoke(this, new GameInitializedEventArgs(_field.Size, (Entities.LightMotor)entities[0], (Entities.LightMotor)entities[1], 
            lights, _field.GameStatus));
        
    }
    
    /// <summary>
    /// Handles input from the view
    /// </summary>
    /// <param name="direction">The direction that was received</param>
    /// <param name="player">The variant of the input <br/>0 for player-1 <br/>1 for player-2</param>
    public void AcceptInput(Direction direction, int player)
    {
        _field?.AcceptInput(direction, player);
    }

    /// <summary>
    /// Invokes the <see cref="OnGameUpdate"/> event
    /// </summary>
    /// <param name="source">The event's source</param>
    /// <param name="e">Args containing the newly added or updated elements</param>
    private void OnUpdateInvoke(object source, OnUpdateEventArgs e)
    {
        OnUpdate?.Invoke(source, e);
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
    /// Invokes the <see cref="OnGameInitialized"/> event
    /// </summary>
    /// <param name="source">The event's source</param>
    /// <param name="e">Args containing the game's started position</param>
    private void OnGameInitializedInvoke(object source, GameInitializedEventArgs e)
    {
        OnGameInitialized?.Invoke(source, e);
    }

    /// <summary>
    /// Starts the execution of the game, if the game was not initialized yet, nothing will happen
    /// <seealso cref="Init"/>
    /// </summary>
    public async Task Run()
    {
        if (_field == null || _token == null)
        {
            throw new ApplicationException("Can't start an empty game!");
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

                _field.Update();
                
                if(_field.CheckStatus())
                    OnStateChangedInvoke(this, new StateChangeEventArgs(_field.GameStatus));
                
                var ent = _field.Entities;
                OnUpdateInvoke(this, new OnUpdateEventArgs((Entities.LightMotor)ent[0], (Entities.LightMotor)ent[1],
                    (LightLine?)ent[^2], (LightLine?)_field.Entities[^1]));

                
            }
        }, _token.Token);
    }

    /// <summary>
    /// Called when the game should stop indefinitely, if you prefer to pause the game instead, use the <see cref="Paused"/> property
    /// </summary>
    public void Stop()
    {
        _token?.Cancel();
        Paused = false;
    }

    public void Dispose()
    {
        _token?.Dispose();
    }
}
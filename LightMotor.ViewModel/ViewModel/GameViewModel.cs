using System.Windows.Input;
using LightMotor;
using LightMotor.Entities;
using LightMotor.Event;
using LightMotor.Root;
using LightMotorViewModel.Command;
using LightMotorViewModel.Command.Movement;
using LightMotorViewModel.Play;

namespace LightMotorViewModel.ViewModel;

public partial class GameViewModel : ViewModelBase
{
    private readonly Game _game;

    private string _status = String.Empty;
    private int _size;
    public ICommand SaveCommand { get; }
    public ICommand PauseCommand { get; }
    public ICommand EscapeCommand { get; }

    public ICommand LeftPlayerOne { get; }
    public ICommand RightPlayerOne { get; }
    public ICommand UpPlayerOne { get; }
    public ICommand DownPlayerOne { get; }
    
    public ICommand LeftPlayerTwo { get; }
    public ICommand RightPlayerTwo { get; }
    public ICommand UpPlayerTwo { get; }
    public ICommand DownPlayerTwo { get; }

    public string Status
    {
        get => _status;
        set => Set(ref _status, value);
    }

    public int Size
    {
        get => _size;
        set => Set(ref _size, value);
    }

    public Board GameBoard { get; }

    public GameViewModel(ref Game game, GameInitializedEventArgs e)
    {
        _game = game;
        _size = e.Size;
        SaveCommand = new SaveCommand(ref _game, this);
        PauseCommand = new PauseCommand(ref _game, this);
        EscapeCommand = new EscapeCommand(ref _game);

        LeftPlayerOne = new LeftCommand(ref _game, 0);
        RightPlayerOne = new RightCommand(ref _game, 0);
        UpPlayerOne = new UpCommand(ref _game, 0);
        DownPlayerOne = new DownCommand(ref _game, 0);
        
        LeftPlayerTwo = new LeftCommand(ref _game, 1);
        RightPlayerTwo = new RightCommand(ref _game, 1);
        UpPlayerTwo = new UpCommand(ref _game, 1);
        DownPlayerTwo = new DownCommand(ref _game, 1);
        
        _game.OnStateChanged += OnStateChanged;
        _game.OnUpdate += OnUpdate;

        GameBoard = new Board(_size);

        for (int i = 0; i < _size; i++)
        {
            for (int a = 0; a < _size; a++)
            {
                GameBoard.Entities.Add(EntityViewModel.Empty(i, a));
            }
        }

        int indOne = GetLocationFor(e.PlayerOne.Position);
        int indTwo = GetLocationFor(e.PlayerTwo.Position);
        
        if (indOne >= 0 && indOne < GameBoard.Entities.Count && e.PlayerOne.Position.X >= 0 && e.PlayerOne.Position.X < _size)
            GameBoard.Entities[indOne].Update("../Resources/motor_" + GetMotorFacing(e.PlayerOne) + ".png", e.PlayerOne.Position.Y,
                e.PlayerOne.Position.X);
        if(indTwo >= 0 && indTwo < GameBoard.Entities.Count && e.PlayerTwo.Position.X >= 0 && e.PlayerTwo.Position.X < _size)
            GameBoard.Entities[indTwo].Update("../Resources/motor_" + GetMotorFacing(e.PlayerTwo) + ".png", e.PlayerTwo.Position.Y,
                e.PlayerTwo.Position.X);

        for (int i = 0; i < e.Lights.Length; i++)
        {
            GameBoard.Entities[GetLocationFor(e.Lights[i].Position)].Update(
                "../Resources/light_" + GetLightFacing((LightLine)e.Lights[i]) + "_" + (i % 2 == 0 ? "0" : "1") +
                ".png",
                e.Lights[i].Position.Y, e.Lights[i].Position.X);
        }
        if(e.GameStatus is not PlayStatus)
            TriggerGameEnd(e.GameStatus);
    }

    public async Task Start()
    {
        await _game.Run();
    }

    private void OnUpdate(object source, OnUpdateEventArgs e)
    {
        int indOne = GetLocationFor(e.PlayerOne.Position);
        int indTwo = GetLocationFor(e.PlayerTwo.Position);
        if (indOne >= 0 && indOne < GameBoard.Entities.Count && e.PlayerOne.Position.X >= 0 && e.PlayerOne.Position.X < _size)
            GameBoard.Entities[indOne].Update("../Resources/motor_" + GetMotorFacing(e.PlayerOne) + ".png", e.PlayerOne.Position.Y,
                e.PlayerOne.Position.X);
        if(indTwo >= 0 && indTwo < GameBoard.Entities.Count && e.PlayerTwo.Position.X >= 0 && e.PlayerTwo.Position.X < _size)
            GameBoard.Entities[indTwo].Update("../Resources/motor_" + GetMotorFacing(e.PlayerTwo) + ".png", e.PlayerTwo.Position.Y,
                e.PlayerTwo.Position.X);

        GameBoard.Entities[GetLocationFor(e.AddedLightOne!.Position)].Update("../Resources/light_" + GetLightFacing(e.AddedLightOne) + "_0.png", e.AddedLightOne.Position.Y,
            e.AddedLightOne.Position.X);
        GameBoard.Entities[GetLocationFor(e.AddedLightTwo!.Position)].Update("../Resources/light_" + GetLightFacing(e.AddedLightTwo) + "_1.png", e.AddedLightTwo.Position.Y,
            e.AddedLightTwo.Position.X);
    }

    private void OnStateChanged(object obj, StateChangeEventArgs e)
    {
        if (e.ChangedTo is PlayStatus)
            return;
        
        TriggerGameEnd(e.ChangedTo);
    }
    
    private void TriggerGameEnd(GameStatus status)
    {
        _game.Stop();

        if (status is FirstPlayerWinStatus)
            Status = "Player One Wins! ESC To Reload";
        else if(status is SecondPlayerWinStatus)
            Status = "Player Two Wins! ESC To Reload";
        else if (status is DrawStatus)
            Status = "Draw! ESC To Reload";

        _game.OnUpdate -= OnUpdate;
        _game.OnStateChanged -= OnStateChanged;
    }
}
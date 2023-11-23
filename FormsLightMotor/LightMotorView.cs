using LightMotor;
using LightMotor.Entities;
using LightMotor.Event;
using LightMotor.Persistence;
using LightMotor.Root;

namespace FormsLightMotor;

public partial class LightMotorView : Form
{
    private readonly Game _model = new (new WindowsPersistenceProvider());
    private bool HasGameStarted => _players[0].Item1 != null;

    private (PictureBox?, float rotation)[] _players = new (PictureBox?, float rotation)[2];
    
    public LightMotorView()
    {
        InitializeComponent();
        _model.OnUpdate += OnModelUpdate;
        _model.OnStateChanged += OnModelStateChanged;
        _model.OnGameInitialized += OnModelGameInitialized;
    }

    private string GetStatus()
    {
        if (statusLabel.Visible)
            return statusLabel.Text;
        return string.Empty;
    }
    private void SetStatus(string status)
    {
        if (string.IsNullOrEmpty(status))
        {
            ClearStatus();
            return;
        }

        statusLabel.Visible = true;
        statusLabel.Text = status;
    }

    private void ClearStatus()
    {
        statusLabel.Visible = false;
    }

    private void OnSave()
    {
        OpenFileDialog file = new OpenFileDialog();

        file.Multiselect = false;
        file.Filter = "Text files (*.txt)|*txt|All files (*.*)|*.*";
        file.InitialDirectory = "c:\\";
        file.CheckFileExists = false;

        string prevStatus = GetStatus();
        bool paused = _model.Paused;
        _model.Paused = true;
        SetStatus("Saving");
        DialogResult res = file.ShowDialog();

        if (res == DialogResult.OK)
        {
            _model.SaveGame(file.FileName);
        }
        
        if(!paused)
            _model.Paused = false;
        SetStatus(prevStatus);
    }

    private void OnLoad()
    {
        OpenFileDialog file = new OpenFileDialog();

        file.Multiselect = false;
        file.Filter = "Text files (*.txt)|*txt|All files (*.*)|*.*";
        file.InitialDirectory = "c:\\";

        DialogResult res = file.ShowDialog();

        if (res == DialogResult.OK)
        {
            _model.LoadFile(file.FileName);
        }
    }

    private void ShowHelp()
    {
        if(HasGameStarted && !_model.Paused)
            TogglePause();
        MessageBox.Show(
            "Press W, A, S, D to control player 1\nPress the arrow keys to control player 2\nPress p to toggle pause", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        if (HasGameStarted && _model.Paused) 
            TogglePause();
    }

    private void TogglePause()
    {
        _model.Paused = !_model.Paused;
        SetStatus(_model.Paused ? "Paused" : String.Empty);
    }

    private void OnModelStateChanged(object obj, StateChangeEventArgs e)
    {
        Invoke(delegate
        {
            if (e.ChangedTo is PlayStatus)
                return;

            TriggerGameEnd(e.ChangedTo);
        });
    }

    private void OnModelUpdate(object source, OnUpdateEventArgs e)
    {
        Invoke(delegate
        {
            _players[0].Item1!.Location = CalculateLocation(e.PlayerOne.Position);
            _players[1].Item1!.Location = CalculateLocation(e.PlayerTwo.Position);

            float rotationPlayerOne = _players[0].Item2 - e.PlayerOne.Direction.Rotation;
            float rotationPlayerTwo = _players[1].Item2 - e.PlayerTwo.Direction.Rotation;
            
            _players[0].Item1!.Image = RotateImage(_players[0].Item1!.Image, rotationPlayerOne);
            _players[1].Item1!.Image = RotateImage(_players[1].Item1!.Image, rotationPlayerTwo);

            _players[0].Item2 = e.PlayerOne.Direction.Rotation;
            _players[1].Item2 = e.PlayerTwo.Direction.Rotation;
            
            AddLightLine(e.AddedLightOne, Brushes.Aqua);
            AddLightLine(e.AddedLightTwo, Brushes.Orange);
        });
        
    }
    
    private async void OnModelGameInitialized(object obj, GameInitializedEventArgs e)
    {
        Invoke(delegate
        {
            gamePanel.Visible = true;
            
            AddLightMotor(e.PlayerOne, 0);
            AddLightMotor(e.PlayerTwo, 1);

            for (int i = 0; i < e.Lights.Length; i++)
            {
                LightLine? addedLight = (LightLine?)e.Lights[i];
                AddLightLine(addedLight, i % 2 == 0 ? Brushes.Aqua : Brushes.Orange);
            }
            
            startButton.Visible = false;
            fieldSize.Visible = false;
            loadButton.Visible = false;
            gamePanel.Size = new Size(24 * e.Size, 24 * e.Size);
            Size = new Size(new Point(gamePanel.Size + new Size(60, 80)));
            gamePanel.Location =
                new Point(20, 20);
        });
        if (e.GameStatus is not PlayStatus)
            TriggerGameEnd(e.GameStatus);
        else
            await _model.Run();
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {   
        if (e.KeyCode is Keys.H or Keys.Help)
        {
            ShowHelp();
        }
        else if (e.KeyCode is Keys.Escape)
        {
            ResetView();
        }
        else if(HasGameStarted)
        {
            if (e is { KeyCode: Keys.S, Control: true } && !e.Handled)
            {
                OnSave();
            }
            else if (e.KeyCode is Keys.P) // toggle pause
            {
                TogglePause();
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.W: _model.AcceptInput(SouthDirection.Get(), 0);
                        break;
                    case Keys.A: _model.AcceptInput(WestDirection.Get(),  0);
                        break;
                    case Keys.S: _model.AcceptInput(NorthDirection.Get(), 0);
                        break;
                    case Keys.D: _model.AcceptInput(EastDirection.Get(),  0);
                        break;
                    case Keys.Up: _model.AcceptInput(SouthDirection.Get(), 1);
                        break;
                    case Keys.Left: _model.AcceptInput(WestDirection.Get(),  1);
                        break;
                    case Keys.Down: _model.AcceptInput(NorthDirection.Get(), 1);
                        break;
                    case Keys.Right: _model.AcceptInput(EastDirection.Get(),  1);
                        break;
                }
                
                
            }
        }
        e.Handled = true;
    }

    private void StartButtonClick(object? sender, EventArgs e)
    {
        int val = (int)fieldSize.Value;
        _model.Init(val);
        Focus();
    }

    private void LoadButtonClick(object? sender, EventArgs e)
    {
        OnLoad();
    }

    private void ResetView()
    {
        _model.Paused = false;
        _model.Stop();
        _players = new (PictureBox?, float rotation)[2];
        Controls.Clear();
        InitializeComponent();
    }

    private void TriggerGameEnd(GameStatus status)
    {
        if (status is PlayStatus)
            return;
        
        _model.Stop();

        if (status is FirstPlayerWinStatus)
            SetStatus("Player One Wins! ESC To Reload");
        else if(status is SecondPlayerWinStatus)
            SetStatus("Player Two Wins! ESC To Reload");
        else if (status is DrawStatus)
            SetStatus("Draw! ESC To Reload");
    }
}
using System.Diagnostics;
using LightMotor;
using LightMotor.Event;
using LightMotor.Game;

namespace FormsLightMotor;

public partial class LightMotorView : Form
{
    private readonly Game _model = new ();
    private bool HasGameStarted => _players[0] != null;

    private Control?[] _players = new Control?[2];
    private List<Control> _lights = new();
    
    public LightMotorView()
    {
        InitializeComponent();
        _model.OnUpdate += OnModelUpdate;
        _model.OnStateChanged += OnModelStateChanged;
        _model.OnGameInitialized += OnModelGameInitialized;
    }

    private void OnSave()
    {
        OpenFileDialog file = new OpenFileDialog();

        file.Multiselect = false;
        file.Filter = "Text files (*.txt)|*txt|All files (*.*)|*.*";
        file.InitialDirectory = "c:\\";
        file.CheckFileExists = false;
        
        _model.Paused = true;
        DialogResult res = file.ShowDialog();

        if (res == DialogResult.OK)
        {
            _model.SaveGame(file.FileName);
        }
        
        _model.Paused = false;
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
            _players[0]!.Location = CalculateLocation(e.PlayerOne.Position);
            _players[1]!.Location = CalculateLocation(e.PlayerTwo.Position);
        
            // TODO: direction for images

            if (e.AddedLightOne != null)
            {
                Button light = new Button();

                light.Location = CalculateLocation(e.AddedLightOne.Position);
                light.Size = new Size(24, 24);
                light.Enabled = false;
                light.BackColor = Color.Aqua;
                // TODO: calculate rotation
            
                _lights.Add(light);
                gamePanel.Controls.Add(light);
            }

            if (e.AddedLightTwo != null)
            {
                Button light = new Button();

                light.Location = CalculateLocation(e.AddedLightTwo.Position);
                light.Size = new Size(24, 24);
                light.Enabled = false;
                light.BackColor = Color.Aqua;
                // TODO: calculate rotation
            
                _lights.Add(light);
                gamePanel.Controls.Add(light);
            }
            
        });
        
    }
    
    private async void OnModelGameInitialized(object obj, GameInitializedEventArgs e)
    {
        Invoke(delegate
        {
            gamePanel.Visible = true;
            
            Button playerOne = new Button();
            playerOne.Location = CalculateLocation(e.PlayerOne.Position);
            playerOne.Size = new Size(24, 24);
            playerOne.Enabled = false;

            Button playerTwo = new Button();
            playerTwo.Location = CalculateLocation(e.PlayerTwo.Position);
            playerTwo.Size = new Size(24, 24);
            playerTwo.Enabled = false;

            // TODO: direction calc for image

            _players[0] = playerOne;
            _players[1] = playerTwo;

            gamePanel.Controls.Add(_players[0]!);
            gamePanel.Controls.Add(_players[1]!);

            foreach (var addedLight in e.Lights)
            {
                Button light = new Button();

                light.Location = CalculateLocation(addedLight.Position);
                light.Size = new Size(24, 24);
                light.Enabled = false;
                light.BackColor = Color.Aqua;
                // TODO: calculate rotation
            
                _lights.Add(light);
                gamePanel.Controls.Add(light);
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

    private Point CalculateLocation(Position p)
    {
        return new Point(24 * p.X, 24 * p.Y);
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {   
        if (e.KeyCode is Keys.H or Keys.Help)
        {
            ShowHelp();
        }
        else if (e.KeyCode is Keys.Escape)
        {
            _model.Paused = false;
            _model.Stop();
            _lights.Clear();
            _players = new Control[2];
            Controls.Clear();
            InitializeComponent();
        }
        else if(HasGameStarted)
        {
            if (e is { KeyCode: Keys.S, Control: true })
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
    }

    private void LoadButtonClick(object? sender, EventArgs e)
    {
        OnLoad();
    }

    private void TriggerGameEnd(GameStatus status)
    {
        if (status is PlayStatus)
            return;
        
        _model.Stop();

        if (status is FirstPlayerWinStatus)
        {
            
        }
        // TODO: Game ending stuff
        
    }
}
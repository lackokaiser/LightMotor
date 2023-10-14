using System.Drawing.Drawing2D;
using LightMotor;
using LightMotor.Entities;
using LightMotor.Event;
using LightMotor.Root;

namespace FormsLightMotor;

public partial class LightMotorView : Form
{
    private readonly Game _model = new ();
    private bool HasGameStarted => _players[0].Item1 != null;

    private (PictureBox?, float rotation)[] _players = new (PictureBox?, float rotation)[2];
    private readonly List<PictureBox> _lights = new();
    
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
            _players[0].Item1!.Location = CalculateLocation(e.PlayerOne.Position);
            _players[1].Item1!.Location = CalculateLocation(e.PlayerTwo.Position);

            float rotationPlayerOne = _players[0].Item2 - e.PlayerOne.Direction.Rotation;
            float rotationPlayerTwo = _players[1].Item2 - e.PlayerTwo.Direction.Rotation;
            
            _players[0].Item1!.Image = RotateImage(_players[0].Item1!.Image, rotationPlayerOne,
                rotationPlayerOne >= 180);
            _players[1].Item1!.Image = RotateImage(_players[1].Item1!.Image, rotationPlayerTwo,
                rotationPlayerTwo >= 180);

            _players[0].Item2 = e.PlayerOne.Direction.Rotation;
            _players[1].Item2 = e.PlayerTwo.Direction.Rotation;
            
            if (e.AddedLightOne != null)
            {
                PictureBox light = new PictureBox();

                light.Location = CalculateLocation(e.AddedLightOne.Position);
                light.Size = new Size(24, 24);
                light.Enabled = false;

                Bitmap map = new Bitmap(24, 24);

                map = DrawLightLine(map, e.AddedLightOne.Direction, e.AddedLightOne.FacingDirection, Brushes.Orange);
                light.Image = map;
                gamePanel.Controls.Add(light); 
            }

            if (e.AddedLightTwo != null)
            {
                PictureBox light = new PictureBox();

                light.Location = CalculateLocation(e.AddedLightTwo.Position);
                light.Size = new Size(24, 24);
                light.Enabled = false;
            
                Bitmap map = new Bitmap(24, 24);
                
                map = DrawLightLine(map, e.AddedLightTwo.Direction, e.AddedLightTwo.FacingDirection, Brushes.Aqua);
                light.Image = map;

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
            
            PictureBox playerOne = new ();
            playerOne.Location = CalculateLocation(e.PlayerOne.Position);
            playerOne.Size = new Size(24, 24);

            Bitmap map = new Bitmap("./Resources/motor.png");

            map = RotateImage(map, e.PlayerOne.Direction.Rotation, e.PlayerOne.Direction.Rotation >= 180);
            
            playerOne.Image = map;
            playerOne.SizeMode = PictureBoxSizeMode.AutoSize;

            PictureBox playerTwo = new PictureBox();
            playerTwo.Location = CalculateLocation(e.PlayerTwo.Position);
            playerTwo.Size = new Size(24, 24);
            Bitmap map2 = new Bitmap("./Resources/motor.png");

            map2 = RotateImage(map2, e.PlayerTwo.Direction.Rotation, e.PlayerTwo.Direction.Rotation >= 180);

            playerTwo.Image = map2;
            playerTwo.SizeMode = PictureBoxSizeMode.AutoSize;

            _players[0].Item1 = playerOne;
            _players[0].Item2 = e.PlayerOne.Direction.Rotation;
            _players[1].Item1 = playerTwo;
            _players[1].Item2 = e.PlayerTwo.Direction.Rotation;

            gamePanel.Controls.Add(_players[0].Item1!);
            gamePanel.Controls.Add(_players[1].Item1!);

            for (int i = 0; i < e.Lights.Length; i++)
            {
                var addedLight = e.Lights[i];
                PictureBox light = new PictureBox();

                light.Location = CalculateLocation(addedLight.Position);
                light.Size = new Size(24, 24);
                light.Enabled = false;
                Bitmap image = new Bitmap(24, 24);

                image = DrawLightLine(image, addedLight.Direction, ((LightLine)addedLight).FacingDirection, i % 2 == 0 ? Brushes.Aqua : Brushes.Orange);
                light.Image = image;
                
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

    private Bitmap DrawLightLine(Image img, Direction direction, TurnDirection facingDirection, Brush brush, float width = 10)
    {
        Bitmap bmp = new Bitmap(img.Width, img.Height);
        
        Graphics g = Graphics.FromImage(bmp);

        Pen p = new Pen(brush, width);


        if (facingDirection is NoTurn)
        {
            if (direction is SouthDirection or NorthDirection)
                g.DrawLine(p, new Point(12, 0), new Point(12, 24));
            else if (direction is WestDirection or EastDirection)
                g.DrawLine(p, new Point(0, 12), new Point(24, 12));            
        }
        else if (facingDirection is TurnLeft)
        {
            if (direction is SouthDirection)
            {
                g.DrawLine(p, new Point(12, 12), new Point(12, 24));
                g.DrawLine(p, new Point(24, 12), new Point(12, 12));
            }
            else if (direction is NorthDirection)
            {
                g.DrawLine(p, new Point(12, 0), new Point(12, 12));
                g.DrawLine(p, new Point(12, 12), new Point(0, 12));
            }
            else if (direction is WestDirection)
            {
                g.DrawLine(p, new Point(12, 12), new Point(24, 12));
                g.DrawLine(p, new Point(12, 12), new Point(12, 0));
            }
            else // EastDirection
            {
                g.DrawLine(p, new Point(0, 12), new Point(12, 12));
                g.DrawLine(p, new Point(12, 12), new Point(12, 24));
            }    
        }
        else if (facingDirection is TurnRight)
        {
            if (direction is SouthDirection)
            {
                g.DrawLine(p, new Point(12, 12), new Point(12, 24));
                g.DrawLine(p, new Point(0, 12), new Point(12, 12));
            }
            else if (direction is NorthDirection)
            {
                g.DrawLine(p, new Point(12, 0), new Point(12, 12));
                g.DrawLine(p, new Point(12, 12), new Point(24, 12));
            }
            else if (direction is WestDirection)
            {
                g.DrawLine(p, new Point(12, 12), new Point(24, 12));
                g.DrawLine(p, new Point(12, 12), new Point(12, 24));
            }
            else // EastDirection
            {
                g.DrawLine(p, new Point(0, 12), new Point(12, 12));
                g.DrawLine(p, new Point(12, 12), new Point(12, 0));
            }
        }
        
        
        g.DrawImage(img, new Point(0, 0));
        
        g.Dispose();
        p.Dispose();

        return bmp;
    }

    private Bitmap RotateImage(Image img, float rotationAngle, bool flipImage = false)
    {
        //create an empty Bitmap image
        Bitmap bmp = new Bitmap(img.Width, img.Height);

        //turn the Bitmap into a Graphics object
        Graphics gfx = Graphics.FromImage(bmp);

        //now we set the rotation point to the center of our image
        gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

        //now rotate the image
        gfx.RotateTransform(rotationAngle);

        gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

        //set the InterpolationMode to HighQualityBicubic so to ensure a high
        //quality image once it is transformed to the specified size
        gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

        //now draw our new image onto the graphics object
        gfx.DrawImage(img, new Point(0, 0));

        //dispose of our Graphics object
        gfx.Dispose();

        if(flipImage)
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
        
        //return the image
        return bmp;
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
            _players = new (PictureBox?, float rotation)[2];
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
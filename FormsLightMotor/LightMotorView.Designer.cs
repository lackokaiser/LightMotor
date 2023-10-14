namespace FormsLightMotor;

partial class LightMotorView
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private Button startButton;
    private NumericUpDown fieldSize;
    private Button loadButton;
    private Panel gamePanel;
    private Label statusLabel;

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {   
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(200, 300);
        this.Text = "Light Motor";
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.FormBorderStyle = FormBorderStyle.Fixed3D;
        KeyDown += OnKeyDown;
        KeyPreview = true;

        gamePanel = new Panel();
        gamePanel.BorderStyle = BorderStyle.FixedSingle;
        //gamePanel.Bounds = new Rectangle(0, 0, Width, Height);
        gamePanel.Location = new Point(0, 0);
        gamePanel.Padding = new Padding(30);
        gamePanel.Visible = false;
        gamePanel.KeyDown += OnKeyDown;

        startButton = new Button();
        startButton.Size = new Size(100, 30);
        startButton.Location = new Point(50, 100);
        startButton.Text = "New Game";
        startButton.Click += StartButtonClick;

        fieldSize = new NumericUpDown();
        fieldSize.Minimum = 6;
        fieldSize.Maximum = 64;
        fieldSize.Size = new Size(100, 30);
        fieldSize.Location = new Point(50, 140);

        loadButton = new Button();
        loadButton.Size = new Size(100, 30);
        loadButton.Location = new Point(50, 180);
        loadButton.Text = "Load Game";
        loadButton.Click += LoadButtonClick;

        statusLabel = new Label();
        statusLabel.Visible = false;
        statusLabel.TextAlign = ContentAlignment.MiddleCenter;
        statusLabel.Dock = DockStyle.Top;
        
        Controls.Add(startButton);
        Controls.Add(fieldSize);
        Controls.Add(loadButton);
        Controls.Add(gamePanel);
        Controls.Add(statusLabel);
    }

    #endregion
}
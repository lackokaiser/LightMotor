using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;
using LightMotor;
using LightMotor.Entities;

namespace FormsLightMotor;

public partial class LightMotorView
{
    private void AddLightLine(LightLine? lightLine, Brush lightColor)
    {
        if (lightLine != null)
        {
            PictureBox light = new PictureBox();

            light.Location = CalculateLocation(lightLine.Position);
            light.Size = new Size(24, 24);
            light.Enabled = false;
            
            Bitmap map = new Bitmap(24, 24);
                
            map = DrawLightLine(map, lightLine.Direction, lightLine.FacingDirection, lightColor);
            light.Image = map;

            gamePanel.Controls.Add(light);
        }
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

    private void AddLightMotor(LightMotor.Entities.LightMotor motor, [Range(0, 1)] int playerIndex)
    {
        PictureBox player = new ();
        player.Location = CalculateLocation(motor.Position);
        player.Size = new Size(24, 24);

        Bitmap map = new Bitmap("./Resources/motor.png");

        map = RotateImage(map, motor.Direction.Rotation);
        
        player.Image = map;
        player.SizeMode = PictureBoxSizeMode.AutoSize;
        
        _players[playerIndex].Item1 = player;
        _players[playerIndex].Item2 = motor.Direction.Rotation;
        
        gamePanel.Controls.Add(player);
    }
    
    private Point CalculateLocation(Position p)
    {
        return new Point(24 * p.X, 24 * p.Y);
    }

    private Bitmap RotateImage(Image img, float rotationAngle)
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
        
        //return the image
        return bmp;
    }
}
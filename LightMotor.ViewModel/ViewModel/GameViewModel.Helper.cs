using LightMotor.Entities;

namespace LightMotor.ViewModel.ViewModel;

public partial class GameViewModel
{
    private string GetLightFacing(LightLine light)
    {
        if (light.FacingDirection is NoTurn)
        {
            if (light.Direction is SouthDirection or NorthDirection)
                return "vertical";
            if (light.Direction is WestDirection or EastDirection)
                return "horizont";
        }
        if (light.FacingDirection is TurnLeft)
        {
            switch (light.Direction)
            {
                case EastDirection:
                    return "left_down";
                case SouthDirection:
                    return "right_down";
                case NorthDirection:
                    return "left_up";
                case WestDirection:
                    return "right_up";
            }
        }

        if (light.FacingDirection is TurnRight)
        {
            switch (light.Direction)
            {
                case EastDirection:
                    return "left_up";
                case SouthDirection:
                    return "left_down";
                case NorthDirection:
                    return "right_up";
                case WestDirection:
                    return "right_down";
            }
        }

        return String.Empty;
    }

    private string GetMotorFacing(LightMotor.Entities.LightMotor motor)
    {
        switch (motor.Direction)
        {
            case EastDirection: 
                return "right";
            case NorthDirection:
                return "down";
            case SouthDirection:
                return "up";
            case WestDirection:
                return "left";
            default: return string.Empty;
        }
    }
    
    private int GetLocationFor(Position position)
    {
        return position.Y * _size + position.X;
    }
}
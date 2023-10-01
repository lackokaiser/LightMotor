using LightMotor.Entities;

namespace LightMotor.Event;

public class OnUpdateEventArgs : EventArgs
{
    private Entities.LightMotor playerOne;
    private Entities.LightMotor playerTwo;

    private LightLine? addedLightOne;
    private LightLine? addedLightTwo;

    public OnUpdateEventArgs(Entities.LightMotor playerOne, Entities.LightMotor playerTwo, LightLine? addedLightOne, LightLine? addedLightTwo)
    {
        this.playerOne = playerOne;
        this.playerTwo = playerTwo;
        this.addedLightOne = addedLightOne;
        this.addedLightTwo = addedLightTwo;
    }
}
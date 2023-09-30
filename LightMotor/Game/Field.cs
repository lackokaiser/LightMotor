using System.Text;
using LightMotor.Entities;
using LightMotor = LightMotor.Entities.LightMotor;

namespace LightMotor.Game;

public class Field
{
    private int size;
    private List<Entity> entities = new ();
    private IInputHandler[] playerHandlers;
    public GameStatus GameStatus { get; private set; }

    public List<Entity> Entities => new(entities);

    public Field(int size)
    {
        this.size = size;
        GameStatus = PlayStatus.Get();

        Entities.LightMotor playerOne = new (new Position(size / 2, size / 2), WestDirection.Get()), 
            playerTwo = new (new Position(size / 2 + 1, size / 2), EastDirection.Get());
        // create two motor and place them on the field
        playerHandlers = new IInputHandler[] { playerOne, playerTwo };
    }

    public Field(string data)
    {
        string[] split = data.Split('\n');

        string[] main = split[0].Split();

        size = int.Parse(main[0]);
        byte status = byte.Parse(main[1]);
        
        if(status == 0)
            GameStatus = PlayStatus.Get();
        else if (status == 1)
            GameStatus = FirstPlayerWinStatus.Get();
        else if(status == 2)
            GameStatus = SecondPlayerWinStatus.Get();
        
        
    }

    /// <summary>
    /// Handles movement input from the view
    /// </summary>
    /// <param name="input">The input itself</param>
    /// <param name="player">The player index</param>
    /// <exception cref="ArgumentOutOfRangeException">If the player index is not 0 or 1</exception>
    public void AcceptInput(InputType input, int player)
    {
        playerHandlers[player].AcceptInput(input);
    }

    /// <summary>
    /// Updates the game
    /// </summary>
    /// <returns>True if the game status has changed</returns>
    public bool Update()
    {
        foreach (var entity in entities)
        {
            entity.Update();
            
        }
    }
    
    /// <summary>
    /// Converts the entire game state to a string
    /// </summary>
    /// <returns>The string representing the current game</returns>
    public string Save()
    {
        StringBuilder stb = new StringBuilder();

        byte statusByte = 0;

        if (GameStatus == PlayStatus.Get())
            statusByte = 0;
        else if (GameStatus == FirstPlayerWinStatus.Get())
            statusByte = 1;
        else if (GameStatus == SecondPlayerWinStatus.Get())
            statusByte = 2;

        stb.AppendLine(size + " " + statusByte);
        foreach (var entity in entities)
        {
            stb.AppendLine(entity.Save());
        }

        return stb.ToString();
    }


}
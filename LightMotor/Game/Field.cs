using System.Text;
using LightMotor.Entities;

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

        Entities.LightMotor playerOne = new (new Position(size / 2, size / 2), WestDirection.Get(), NoTurn.Get()), 
            playerTwo = new (new Position(size / 2 + 1, size / 2), EastDirection.Get(), NoTurn.Get());
        entities.Add(playerOne);
        entities.Add(playerTwo);
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

        int entSize = int.Parse(main[2]);
        for (int i = 0; i < entSize; i++)
        {
            var ent = Entity.Load(split[i + 1]);
            if(ent != null)
                entities.Add(ent);
        }
        
        playerHandlers = new [] { (IInputHandler)entities[0], (IInputHandler)entities[1] };
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
    public void Update()
    {
        foreach (var entity in entities)
        {
            if (entity is Entities.LightMotor motor)
            {
                LightLine line = new LightLine(motor.Position, motor.Direction, motor.NextTurnDirection);
                entities.Add(line);
            }
            entity.Update();
        }
    }
    
    /// <summary>
    /// Checks the game's current status and alerts when a new game status has been selected
    /// </summary>
    /// <returns>True if the game status has changed</returns>
    public bool CheckStatus()
    {
        LightMotor.Entities.LightMotor playerOne = (Entities.LightMotor)playerHandlers[0];
        LightMotor.Entities.LightMotor playerTwo = (Entities.LightMotor)playerHandlers[1];

        
        
        GameStatus nextStatus = PlayStatus.Get();

        bool oneOut = playerOne.Position.IsOutOfBounds(0, 0, size, size);
        bool twoOut = playerTwo.Position.IsOutOfBounds(0, 0, size, size);

        if (oneOut && twoOut) // draw
        {
            
        }
        else if (oneOut) // player two win
        {
            nextStatus = SecondPlayerWinStatus.Get();
            if (GameStatus != nextStatus)
            {
                GameStatus = nextStatus;
                return true;
            }
        }
        else if (twoOut) // player one win
        {
            nextStatus = FirstPlayerWinStatus.Get();
            if (GameStatus != nextStatus)
            {
                GameStatus = nextStatus;
                return true;
            }
        }
        
        if (playerOne.IsTouching(playerTwo)) // draw
        {
            
        }

        for (int i = 2; i < entities.Count; i++)
        {
            if (entities[i].IsTouching(playerOne)) // collision with light
            {
                nextStatus = SecondPlayerWinStatus.Get();
                break;
            }
            else if (entities[i].IsTouching(playerTwo)) // collision with light
            {
                nextStatus = FirstPlayerWinStatus.Get();
                break;
            }
        }


        if (GameStatus == nextStatus) 
            return false;
        
        GameStatus = nextStatus;
        return true;

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

        stb.AppendLine(size + " " + statusByte + " " + entities.Count);
        foreach (var entity in entities)
        {
            stb.AppendLine(entity.Save());
        }

        return stb.ToString();
    }
}
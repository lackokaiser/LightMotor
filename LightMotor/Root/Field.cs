using System.Text;
using LightMotor.Entities;
using LightMotor.Persistence;

namespace LightMotor.Root;

/// <summary>
/// Represents the game's playing field
/// <seealso cref="Game"/>
/// </summary>
public class Field : ISavable
{
    /// <summary>
    /// The field's size
    /// </summary>
    public int Size { get; }
    private readonly List<Entity> _entities = new ();
    private readonly IInputHandler[] _playerHandlers;
    /// <summary>
    /// The game's current status
    /// </summary>
    public GameStatus GameStatus { get; private set; }

    /// <summary>
    /// Entities that are on the field
    /// </summary>
    public List<Entity> Entities => new(_entities);

    /// <summary>
    /// Constructs a starting field
    /// </summary>
    /// <param name="size">The field's size</param>
    public Field(int size)
    {
        Size = size;
        GameStatus = PlayStatus.Get();

        Entities.LightMotor playerOne = new (new Position(size / 2 - 1, size / 2), WestDirection.Get(), NoTurn.Get()), 
            playerTwo = new (new Position(size / 2, size / 2 + 1), EastDirection.Get(), NoTurn.Get());
        _entities.Add(playerOne);
        _entities.Add(playerTwo);
        // create two motor and place them on the field
        _playerHandlers = new IInputHandler[] { playerOne, playerTwo };
    }

    /// <summary>
    /// Constructs the field object using a data string, usually from a file
    /// </summary>
    /// <param name="data">The data string</param>
    public Field(string data)
    {
        string[] split = data.Split('\n');

        string[] main = split[0].Split();

        Size = int.Parse(main[0]);
        byte status = byte.Parse(main[1]);
        
        if(status == 0)
            GameStatus = PlayStatus.Get();
        else if (status == 1)
            GameStatus = FirstPlayerWinStatus.Get();
        else if(status == 2)
            GameStatus = SecondPlayerWinStatus.Get();
        else if(status == 3)
            GameStatus = DrawStatus.Get();
        else
            GameStatus = PlayStatus.Get();

        int entSize = int.Parse(main[2]);
        for (int i = 0; i < entSize; i++)
        {
            var ent = Entity.Load(split[i + 1]);
            if(ent != null)
                _entities.Add(ent);
        }
        
        _playerHandlers = new [] { (IInputHandler)_entities[0], (IInputHandler)_entities[1] };
    }

    /// <summary>
    /// Handles movement input from the view
    /// </summary>
    /// <param name="direction">The input itself</param>
    /// <param name="player">The player index</param>
    /// <exception cref="IndexOutOfRangeException">If the player index is not 0 or 1</exception>
    public void AcceptInput(Direction direction, int player)
    {
        Entities.LightMotor? motor = _playerHandlers[player] as Entities.LightMotor;

        InputType? input = null;
        
        if (motor?.Direction == NorthDirection.Get())
        {
            if(direction == WestDirection.Get())
                input = LeftInput.Get();
            else if (direction == EastDirection.Get())
                input = RightInput.Get();
        }
        else if (motor?.Direction == WestDirection.Get())
        {
            if(direction == SouthDirection.Get())
                input = LeftInput.Get();
            else if (direction == NorthDirection.Get())
                input = RightInput.Get();
        }
        else if (motor?.Direction == SouthDirection.Get())
        {
            if(direction == EastDirection.Get())
                input = LeftInput.Get();
            else if (direction == WestDirection.Get())
                input = RightInput.Get();
        }
        else if (motor?.Direction == EastDirection.Get())
        {
            if(direction == NorthDirection.Get())
                input = LeftInput.Get();
            else if (direction == SouthDirection.Get())
                input = RightInput.Get();
        }
        
        // calc inputType from direction
        _playerHandlers[player].AcceptInput(input);
    }

    /// <summary>
    /// Updates the game
    /// </summary>
    public void Update()
    {
        foreach (var entity in Entities)
        {
            if (entity is Entities.LightMotor motor)
            {
                LightLine line = new LightLine(motor.Position, motor.Direction, motor.NextTurnDirection);
                _entities.Add(line);
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
        Entities.LightMotor playerOne = (Entities.LightMotor)_playerHandlers[0];
        Entities.LightMotor playerTwo = (Entities.LightMotor)_playerHandlers[1];

        
        
        GameStatus nextStatus = PlayStatus.Get();

        bool oneOut = playerOne.Position.IsOutOfBounds(0, 0, Size - 1, Size - 1);
        bool twoOut = playerTwo.Position.IsOutOfBounds(0, 0, Size - 1, Size - 1);

        if (oneOut && twoOut) // draw
        {
            nextStatus = DrawStatus.Get();
            if (GameStatus != nextStatus)
            {
                GameStatus = nextStatus;
                return true;
            }
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
            nextStatus = DrawStatus.Get();
            if (GameStatus != nextStatus)
            {
                GameStatus = nextStatus;
                return true;
            }
        }

        bool oneTouch = false;
        bool twoTouch = false;
        
        for (int i = 2; i < _entities.Count; i++)
        {
            if (!oneTouch && _entities[i].IsTouching(playerOne)) // collision with light
            {
                oneTouch = true;
            }
            else if (!twoTouch && _entities[i].IsTouching(playerTwo)) // collision with light
            {
                twoTouch = true;
            }
        }
        
        if(oneTouch && twoTouch)
            nextStatus = DrawStatus.Get();
        else if(oneTouch)
            nextStatus = SecondPlayerWinStatus.Get();
        else if(twoTouch)
            nextStatus = FirstPlayerWinStatus.Get();

        if (GameStatus == nextStatus) 
            return false;
        
        GameStatus = nextStatus;
        return true;

    }
    
    /// <summary>
    /// Converts the entire game state to a string
    /// </summary>
    /// <returns>The string representing the current game</returns>
    public string Save(string pre = "")
    {
        StringBuilder stb = new StringBuilder();

        byte statusByte = 0;

        if (GameStatus == PlayStatus.Get())
            statusByte = 0;
        else if (GameStatus == FirstPlayerWinStatus.Get())
            statusByte = 1;
        else if (GameStatus == SecondPlayerWinStatus.Get())
            statusByte = 2;
        else if (GameStatus == DrawStatus.Get())
            statusByte = 3;

        stb.AppendLine(Size + " " + statusByte + " " + _entities.Count);
        foreach (var entity in _entities)
        {
            stb.AppendLine(entity.Save());
        }

        return stb.ToString();
    }
}
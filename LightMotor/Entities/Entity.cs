using System.Text;
using LightMotor.Persistence;

namespace LightMotor.Entities;

/// <summary>
/// Base class for all object on the playing field 
/// </summary>
public abstract class Entity : ISavable
{
    protected Position Pos;
    protected Direction Dir;
    protected TurnDirection TurnDirection;

    /// <summary>
    /// The Entity's position on the field
    /// </summary>
    public Position Position => Pos;
    /// <summary>
    /// The Entity's direction
    /// </summary>
    public Direction Direction => Dir;

    protected Entity(Position pos, Direction dir, TurnDirection turnDirection)
    {
        this.Pos = pos;
        this.Dir = dir;
        this.TurnDirection = turnDirection;
    }

    /// <summary>
    /// Determines if this entity is touching another
    /// </summary>
    /// <param name="entity">The other entity in question</param>
    /// <returns>True if their positions are equal</returns>
    public bool IsTouching(Entity entity)
    {
        return entity.Pos == Pos;
    }

    /// <summary>
    /// Saves the state of this entity
    /// </summary>
    /// <param name="pre">Used by subclasses to append their extra data</param>
    /// <returns>A string representing the entity</returns>
    /// <seealso cref="LightMotor"/>
    /// <seealso cref="LightLine"/>
    public virtual string Save(string pre = "")
    {
        StringBuilder stb = new StringBuilder(pre);

        stb.Append(Pos.X + " " + Pos.Y + " ");
        byte dirByte = 0;
        if (Dir == NorthDirection.Get())
            dirByte = 0;
        else if (Dir == EastDirection.Get())
            dirByte = 1;
        else if (Dir == SouthDirection.Get())
            dirByte = 2;
        else if (Dir == WestDirection.Get())
            dirByte = 3;
        
        byte nextTurn = 0;

        if (TurnDirection == TurnLeft.Get())
            nextTurn = 1;
        else if (TurnDirection == TurnRight.Get())
            nextTurn = 2;

        stb.Append(dirByte + " ");
        stb.Append(nextTurn);

        return stb.ToString();
    }
    
    /// <summary>
    /// Updates the entity
    /// </summary>
    public abstract void Update();
    
    /// <summary>
    /// Loads an entity from a data string
    /// </summary>
    /// <param name="data">The received data</param>
    /// <returns>The entity that was loaded (can be null)</returns>
    public static Entity? Load(string data)
    {
        string[] tmp = data.Split();

        var pos = new Position(int.Parse(tmp[1]), int.Parse(tmp[2]));
        byte directionByte = byte.Parse(tmp[3]);
        byte turnDir = byte.Parse(tmp[4]);

        TurnDirection turnDirection = NoTurn.Get();
        Direction direction = NorthDirection.Get();
        
        if (directionByte == 1)
            direction = EastDirection.Get();
        else if(directionByte == 2)
            direction = SouthDirection.Get();
        else if(directionByte == 3)
            direction = WestDirection.Get();
        if(turnDir == 1)
            turnDirection = TurnLeft.Get();
        else if(turnDir == 2)
            turnDirection = TurnRight.Get();
        
        if (tmp[0] == "0") // motor
        {
            return new LightMotor(pos, direction, turnDirection);
        }
        if (tmp[0] == "1") // light
        {
            return new LightLine(pos, direction, turnDirection);
        }

        return null;
    }
}
using System.Text;

namespace LightMotor.Entities;

public abstract class Entity
{
    protected Position pos;
    protected Direction direction;
    protected TurnDirection turnDirection;

    public Position Position => pos;
    public Direction Direction => direction;

    protected Entity(Position pos, Direction direction, TurnDirection turnDirection)
    {
        this.pos = pos;
        this.direction = direction;
        this.turnDirection = turnDirection;
    }

    /// <summary>
    /// Determines if this entity is touching another
    /// </summary>
    /// <param name="entity">The other entity in question</param>
    /// <returns>True if their positions are equal</returns>
    public bool IsTouching(Entity entity)
    {
        return entity.pos == pos;
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

        stb.Append(pos.X + " " + pos.Y + " ");
        byte dirByte = 0;
        if (direction == NorthDirection.Get())
            dirByte = 0;
        else if (direction == EastDirection.Get())
            dirByte = 1;
        else if (direction == SouthDirection.Get())
            dirByte = 2;
        else if (direction == WestDirection.Get())
            dirByte = 3;
        
        byte nextTurn = 0;

        if (turnDirection == TurnLeft.Get())
            nextTurn = 1;
        else if (turnDirection == TurnRight.Get())
            nextTurn = 2;

        stb.Append(dirByte + " ");
        stb.Append(nextTurn);

        return stb.ToString();
    }
    
    /// <summary>
    /// Updates the entity's state
    /// </summary>
    public abstract void Update();
    
    public static Entity? Load(string data)
    {
        string[] tmp = data.Split();

        var pos = new Position(int.Parse(tmp[2]), int.Parse(tmp[3]));
        byte directionByte = byte.Parse(tmp[4]);
        byte turnDir = byte.Parse(tmp[1]);

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
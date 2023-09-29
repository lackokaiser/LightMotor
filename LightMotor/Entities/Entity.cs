using System.Text;

namespace LightMotor.Entities;

public abstract class Entity
{
    protected Position pos;
    protected Direction direction;

    protected Entity(Position pos, Direction direction)
    {
        this.pos = pos;
        this.direction = direction;
    }

    public bool IsTouching(Entity entity)
    {
        return entity.pos == pos;
    }

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

        stb.Append(dirByte);

        return stb.ToString();
    }
    
    public abstract void Update();
}
namespace LightMotor;

/// <summary>
/// A position representing a 2D coordinate
/// </summary>
public struct Position
{
    private int _x, _y;

    public int X => _x;

    public int Y => _y;

    public Position(int x, int y)
    {
        this._x = x;
        this._y = y;
    }

    /// <summary>
    /// Adds a value to the x position
    /// </summary>
    /// <param name="x">The amount added</param>
    public void AddX(int x)
    {
        this._x += x;
    }

    /// <summary>
    /// Adds a value to the y position
    /// </summary>
    /// <param name="y">The amount added</param>
    public void AddY(int y)
    {
        this._y += y;
    }

    /// <summary>
    /// Determines if this position is out of scope or not
    /// </summary>
    /// <param name="minX">The minimum x</param>
    /// <param name="minY">The minimum y</param>
    /// <param name="maxX">The maximum x</param>
    /// <param name="maxY">The maximum y</param>
    /// <returns>True if out of bounds</returns>
    /// <exception cref="ArgumentException">If minimum values are greater than maximum values</exception>
    public bool IsOutOfBounds(int minX, int minY, int maxX, int maxY)
    {
        if (minY > maxY || minX > maxX)
            throw new ArgumentException("Minimum values can't be greater than maximum values");

        return _x < minX || _x > maxX || _y < minY || _y > maxY;
    }
    
    public static bool operator ==(Position a, Position b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Position a, Position b)
    {
        return !(a == b);
    }
    
    public bool Equals(Position other)
    {
        return _x == other._x && _y == other._y;
    }

    public override bool Equals(object? obj)
    {
        return obj is Position other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_x, _y);
    }
}
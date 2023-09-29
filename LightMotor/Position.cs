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
    /// Clamps the position into a box that was given
    /// </summary>
    /// <param name="minX">The minimum x coordinate</param>
    /// <param name="minY">The minimum y coordinate</param>
    /// <param name="maxX">The maximum x coordinate</param>
    /// <param name="maxY">The maximum y coordinate</param>
    /// <returns>True if anything was done to x or y</returns>
    /// <exception cref="ArgumentException">If minimum values are greater than maximum values</exception>
    public bool Clamp(int minX, int minY, int maxX, int maxY)
    {
        if (minY > maxY || minX > maxX)
            throw new ArgumentException("Minimum values can't be greater than maximum values");

        int xTmp = _x;
        int yTmp = _y;
        _x = Math.Clamp(_x, minX, maxX);
        _y = Math.Clamp(_y, minY, maxY);

        return _x != xTmp || _y != yTmp;
    }

    /// <summary>
    /// Sets the position
    /// </summary>
    /// <param name="x">The new x position</param>
    /// <param name="y">The new y position</param>
    public void Set(int x, int y)
    {
        _x = x;
        _y = y;
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
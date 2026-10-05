using UnityEngine;
public enum Direction { Up, Down, Left, Right }
public static class DirectionExtensions 
{
    public static Vector2Int ConvertVector2Int(this Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                return new Vector2Int(0, 1);
            case Direction.Down:
                return new Vector2Int(0, -1);
            case Direction.Left:
                return new Vector2Int(-1, 0);
            case Direction.Right:
                return new Vector2Int(1, 0);
            default: return Vector2Int.zero;
        }      
    }
    public static Direction GetOppositeDir(this Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                return Direction.Down;
            case Direction.Down:
                return Direction.Up;
            case Direction.Left:
                return Direction.Right;
            case Direction.Right:
                return Direction.Left;
            default: return dir;
        }
    }
}

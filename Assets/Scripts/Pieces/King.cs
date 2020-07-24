using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class King : ChessPiece
{
    public override void Initialize(Position position, Color color)
    {
        Initialize(position, ChessPiece.Symbol.K, float.PositiveInfinity, color);
    }

    public override void SetSight()
    {
        sight = new List<Position>();

        CanSeeThrough((int)position.col + 1, position.row + 1);
        CanSeeThrough((int)position.col + 1, position.row);
        CanSeeThrough((int)position.col + 1, position.row - 1);
        CanSeeThrough((int)position.col, position.row + 1);
        CanSeeThrough((int)position.col, position.row - 1);
        CanSeeThrough((int)position.col - 1, position.row + 1);
        CanSeeThrough((int)position.col - 1, position.row);
        CanSeeThrough((int)position.col - 1, position.row - 1);
    }
}

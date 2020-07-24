using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Knight : ChessPiece
{
    public override void Initialize(Position position, Color color)
    {
        Initialize(position, Symbol.N, 3, color);
    }

    public override void SetSight()
    {
        sight = new List<Position>();

        CanSeeThrough((int)position.col - 2, position.row + 1);
        CanSeeThrough((int)position.col - 2, position.row - 1);

        CanSeeThrough((int)position.col - 1, position.row + 2);
        CanSeeThrough((int)position.col - 1, position.row - 2);

        CanSeeThrough((int)position.col + 1, position.row + 2);
        CanSeeThrough((int)position.col + 1, position.row - 2);

        CanSeeThrough((int)position.col + 2, position.row + 1);
        CanSeeThrough((int)position.col + 2, position.row - 1);
    }
}

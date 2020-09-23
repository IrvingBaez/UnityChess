using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Knight : ChessPiece
{
    public Knight(Position position, Color color, Board board)
        : base(position, Symbol.N, 3, color, board){}

    protected override void SetSight()
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

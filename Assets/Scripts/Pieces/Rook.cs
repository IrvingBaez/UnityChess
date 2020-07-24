using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rook : ChessPiece
{
    public override void Initialize(Position position, Color color)
    {
        this.Initialize(position, ChessPiece.Symbol.R, 5, color);
    }
    public override void SetSight()
    {
        sight = new List<Position>();

        for (int i = 1; i < 8; i++)
        {
            if (!CanSeeThrough((int)position.col, position.row + i))
            {
                break;
            }
        }

        for (int i = 1; i < 8; i++)
        {
            if (!CanSeeThrough((int)position.col, position.row - i))
            {
                break;
            }
        }

        for (int i = 1; i < 8; i++)
        {
            if (!CanSeeThrough((int)position.col + i, position.row))
            {
                break;
            }
        }

        for (int i = 1; i < 8; i++)
        {
            if (!CanSeeThrough((int)position.col - i, position.row))
            {
                break;
            }
        }
    }
}

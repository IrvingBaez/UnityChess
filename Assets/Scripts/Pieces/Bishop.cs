using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bishop : ChessPiece
{
    public override void Initialize(Position position, Color color)
    {
        Initialize(position, ChessPiece.Symbol.B, 3, color);
    }

    public override void SetSight()
    {
        sight = new List<Position>();

        for (int i = 1; i < 8; i++)
        {
            if (!CanSeeThrough((int)position.col + i, position.row + i))
            {
                break;
            }
        }

        for (int i = 1; i < 8; i++)
        {
            if (!CanSeeThrough((int)position.col + i, position.row - i))
            {
                break;
            }
        }

        for (int i = 1; i < 8; i++)
        {
            if (!CanSeeThrough((int)position.col - i, position.row + i))
            {
                break;
            }
        }

        for (int i = 1; i < 8; i++)
        {
            if (!CanSeeThrough((int)position.col - i, position.row - i))
            {
                break;
            }
        }
    }
}

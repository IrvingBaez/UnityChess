using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Queen : ChessPiece
{
    public Queen(Position position, Color color, Board board)
        : base(position, ChessPiece.Symbol.Q, 9, color, board){ }
    
    protected override void SetSight()
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

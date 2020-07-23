using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Queen : ChessPiece
{
    public override void Initialize(Position position, Color color)
    {
        Initialize(position, ChessPiece.Symbol.Q, 9, color);
    }
    
    public override void SetSight()
    {
        sight = new List<Position>();

        for (int i = 1; i < 8; i++)
        {
            ChessPiece.Position testing = controller.IndexToPosition((int)position.col + i, position.row + i);
            if (testing != null)
            {
                sight.Add(testing);
                if (controller.GetOnPosition(testing) != null)
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        for (int i = 1; i < 8; i++)
        {
            ChessPiece.Position testing = controller.IndexToPosition((int)position.col + i, position.row - i);
            if (testing != null)
            {
                sight.Add(testing);
                if (controller.GetOnPosition(testing) != null)
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        for (int i = 1; i < 8; i++)
        {
            ChessPiece.Position testing = controller.IndexToPosition((int)position.col - i, position.row + i);
            if (testing != null)
            {
                sight.Add(testing);
                if (controller.GetOnPosition(testing) != null)
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        for (int i = 1; i < 8; i++)
        {
            ChessPiece.Position testing = controller.IndexToPosition((int)position.col - i, position.row - i);
            if (testing != null)
            {
                sight.Add(testing);
                if (controller.GetOnPosition(testing) != null)
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        for (int i = 1; i < 8; i++)
        {
            ChessPiece.Position testing = controller.IndexToPosition((int)position.col, position.row + i);
            if (testing != null)
            {
                sight.Add(testing);
                if (controller.GetOnPosition(testing) != null)
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        for (int i = 1; i < 8; i++)
        {
            ChessPiece.Position testing = controller.IndexToPosition((int)position.col, position.row - i);
            if (testing != null)
            {
                sight.Add(testing);
                if (controller.GetOnPosition(testing) != null)
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        for (int i = 1; i < 8; i++)
        {
            ChessPiece.Position testing = controller.IndexToPosition((int)position.col + i, position.row);
            if (testing != null)
            {
                sight.Add(testing);
                if (controller.GetOnPosition(testing) != null)
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        for (int i = 1; i < 8; i++)
        {
            ChessPiece.Position testing = controller.IndexToPosition((int)position.col - i, position.row);
            if (testing != null)
            {
                sight.Add(testing);
                if (controller.GetOnPosition(testing) != null)
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }
    }

    public override void SetMoves()
    {
        moves = new List<Position>();
        SetSight();

        foreach (ChessPiece.Position pos in sight)
        {
            ChessPiece capture = controller.GetOnPosition(pos);
            if (capture == null || capture.GetColor() != this.color)
            {
                moves.Add(pos);
            }
        }
    }
}

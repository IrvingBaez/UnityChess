using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public abstract class Strategy : MonoBehaviour
{
    protected Board board;
    protected string name;
    public Stopwatch checkmateWatch = new Stopwatch();

    public abstract float Evaluate(Board board);

    protected float FilterCheckMate(float value)
    {
        checkmateWatch.Start();
        switch (board.GetGameState())
        {
            case Board.GameState.WHITE_MATE:
                checkmateWatch.Stop();
                return float.PositiveInfinity;
            case Board.GameState.BLACK_MATE:
                checkmateWatch.Stop();
                return float.NegativeInfinity;
            case Board.GameState.STALE_MATE:
                checkmateWatch.Stop();
                return 0;
            default:
                checkmateWatch.Stop();
                return value;
        }
    }

    protected float Material()
    {
        float value = 0;

        foreach (ChessPiece piece in board.GetWhitePieces())
        {
            if (!(piece is King))
                value += piece.GetValue();
        }

        foreach (ChessPiece piece in board.GetBlackPieces())
        {
            if (!(piece is King))
                value -= piece.GetValue();
        }

        return value;
    }

    protected float Space()
    {
        List<Position> whiteSight = new List<Position>();
        List<Position> blackSight = new List<Position>();

        foreach (ChessPiece piece in board.GetWhitePieces())
        {
            foreach (Position pos in piece.GetSight())
            {
                if (!whiteSight.Contains(pos))
                {
                    whiteSight.Add(pos);
                }
            }
        }

        foreach (ChessPiece piece in board.GetBlackPieces())
        {
            foreach (Position pos in piece.GetSight())
            {
                if (!blackSight.Contains(pos))
                {
                    blackSight.Add(pos);
                }
            }
        }

        return whiteSight.Count - blackSight.Count;
    }

    protected float DistanceToKing()
    {
        float result = 0;

        Position whiteKing = board.GetWhiteKing().GetPosition();
        Position blackKing = board.GetBlackKing().GetPosition();

        foreach (ChessPiece piece in board.GetWhitePieces())
        {
            if(!(piece is King))
                result += Position.SquareDistance(blackKing, piece.GetPosition());
        }

        foreach (ChessPiece piece in board.GetBlackPieces())
        {
            if (!(piece is King))
                result -= Position.SquareDistance(whiteKing, piece.GetPosition());
        }

        return result;
    }

    public override string ToString()
    {
        return $"Strategy {name}";
    }
}
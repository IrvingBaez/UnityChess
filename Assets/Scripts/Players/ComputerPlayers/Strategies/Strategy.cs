using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public abstract class Strategy : MonoBehaviour
{
    protected Board board;
    protected string name;
    public Stopwatch checkmateWatch = new Stopwatch();

    protected Dictionary<char, float> pieceValues = new Dictionary<char, float>() {
        { 'K', 1000 },
        { 'Q', 9 },
        { 'R', 5 },
        { 'B', 3 },
        { 'N', 3 },
        { 'P', 1 },
        { 'k', -1000 },
        { 'q', -9 },
        { 'r', -5 },
        { 'b', -3 },
        { 'n', -3 },
        { 'p', -1 }
    };

    public abstract float Evaluate(Board board);

    protected float FilterCheckMate(float value)
    {
        checkmateWatch.Start();
        switch (board.State)
        {
            case Board.GameState.CHECKMATE_TO_BLACK:
                checkmateWatch.Stop();
                return float.PositiveInfinity;
            case Board.GameState.CHECKMATE_TO_WHITE:
                checkmateWatch.Stop();
                return float.NegativeInfinity;
            case Board.GameState.STALEMATE:
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
        float holder;

        foreach (Position piece in board.WhitePieces.Concat(board.BlackPieces))
        {
            //UnityEngine.Debug.Log(piece);
            pieceValues.TryGetValue(board.GetOnPosition(piece).Value, out holder);
            value += holder;
        }

        return value;
    }

    protected float RelativeMaterial()
    {
        float whiteMaterial = 0;
        float blackMaterial = 0;
    /*
        foreach (ChessPiece piece in board.GetWhitePieces())
        {
            if (!(piece is King))
                whiteMaterial += piece.GetValue();
        }

        foreach (ChessPiece piece in board.GetBlackPieces())
        {
            if (!(piece is King))
                blackMaterial += piece.GetValue();
        }
    */
        return -1f / (1f + Mathf.Exp(whiteMaterial - blackMaterial)) + 0.5f;
    }

    protected float Space()
    {
        return board.WhiteSight.Count - board.BlackSight.Count;
    }

    protected float DistanceToKing()
    {
        float result = 0;
/*
        OldPosition whiteKing = board.GetWhiteKing().GetPosition();
        OldPosition blackKing = board.GetBlackKing().GetPosition();

        foreach (ChessPiece piece in board.GetWhitePieces())
        {
            if(!(piece is King))
                result += OldPosition.SquareDistance(blackKing, piece.GetPosition());
        }

        foreach (ChessPiece piece in board.GetBlackPieces())
        {
            if (!(piece is King))
                result -= OldPosition.SquareDistance(whiteKing, piece.GetPosition());
        }
*/
        return result;
    }

    public override string ToString()
    {
        return $"Strategy {name}";
    }
}
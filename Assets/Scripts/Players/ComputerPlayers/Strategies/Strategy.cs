using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Strategy : MonoBehaviour
{
    protected Board board;

    public abstract float Evaluate(Board board);

    protected float Material()
    {
        float value = 0;

        foreach(ChessPiece piece in board.GetWhitePieces())
        {
            if(piece != null && !(piece is King))
            {
                value += piece.GetValue();
            }
        }

        foreach (ChessPiece piece in board.GetBlackPieces())
        {
            if (piece != null && !(piece is King))
            {
                value -= piece.GetValue();
            }
        }

        return value;
    }
}

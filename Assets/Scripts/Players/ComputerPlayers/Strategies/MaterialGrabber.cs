using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialGrabber : Strategy
{
    public override float Evaluate(Board board)
    {
        this.board = board;
        
        if(board.GetBlackKing().GetGameState() == Game.GameState.WHITE_MATE)
        {
            return float.PositiveInfinity;
        }

        if (board.GetWhiteKing().GetGameState() == Game.GameState.BLACK_MATE)
        {
            return float.NegativeInfinity;
        }

        return Material();
    }
}

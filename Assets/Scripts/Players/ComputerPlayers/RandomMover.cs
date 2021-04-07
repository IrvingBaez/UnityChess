using System;
using System.Collections.Generic;
using System.Linq;

public class RandomMover : ChessPlayer
{
    public override void Move()
    {
        List<Move> legalMoves = board.LegalMoves.Values.SelectMany(x => x).ToList();
        
        Random random = new Random();
        int index = random.Next(legalMoves.Count);
        game.Move(legalMoves[index]);
    }
}

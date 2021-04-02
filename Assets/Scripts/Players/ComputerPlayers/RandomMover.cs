using UnityEngine;

public class RandomMover : ChessPlayer
{
    public override void Move()
    {
        Board.Position piece;
        Board.Move move = null;

        switch (color)
        {
            case 1:
                do
                {
                    piece = board.WhitePieces[Random.Range(0, board.WhitePieces.Count)];
                }
                while (piece == null || board.LegalMoves(piece).Count == 0);

                move = board.LegalMoves(piece)[Random.Range(0, board.LegalMoves(piece).Count)];
                break;
            case -1:
                do
                {
                    piece = board.BlackPieces[Random.Range(0, board.BlackPieces.Count)];
                }
                while (piece == null || board.LegalMoves(piece).Count == 0);

                move = board.LegalMoves(piece)[Random.Range(0, board.LegalMoves(piece).Count)];
                break;
        }

        game.Move(move);
    }
}

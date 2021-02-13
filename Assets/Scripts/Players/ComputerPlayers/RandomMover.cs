using UnityEngine;

public class RandomMover : ChessPlayer
{
    public override void Move()
    {
        ChessPiece piece;
        Move move = null;

        switch (this.color)
        {
            case ChessPiece.Color.WHITE:
                do
                {
                    piece = board.GetWhitePieces()[Random.Range(0, board.GetWhitePieces().Count)];
                }
                while (piece == null || piece.GetMoves().Count == 0);

                move = piece.GetMoves()[Random.Range(0, piece.GetMoves().Count)];
                break;
            case ChessPiece.Color.BLACK:
                do
                {
                    piece = board.GetBlackPieces()[Random.Range(0, board.GetBlackPieces().Count)];
                }
                while (piece == null || piece.GetMoves().Count == 0);

                move = piece.GetMoves()[Random.Range(0, piece.GetMoves().Count)];
                break;
        }

        game.Move(move);
    }
}

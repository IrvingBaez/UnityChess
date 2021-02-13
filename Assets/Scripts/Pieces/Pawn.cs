using UnityEngine;
using System.Collections;
using System.Transactions;
using System.Collections.Generic;

public class Pawn : ChessPiece
{
    public Pawn(Position position, ChessPiece.Color color, Board board)
        : base(position, ChessPiece.Symbol.P, 1, color, board){ }

    public List<Move> RawMoves()
    {
        List<Move> rawMoves = new List<Move>();
        Move testing;
        bool freeAhead = false;

        if (this.color == Color.WHITE)
        {
            testing = new Move(this, Position.IndexToPosition((int)position.col, position.row + 1));
            if (testing != null && board.GetOnPosition(testing.destiny) == null)
            {
                rawMoves.Add(testing);
                freeAhead = true;
            }

            testing = new Move(this, Position.IndexToPosition((int)position.col, position.row + 2));
            if (testing != null && position.row == 2 && freeAhead && board.GetOnPosition(testing.destiny) == null)
            {
                rawMoves.Add(testing);
            }
        }
        else
        {
            testing = new Move(this, Position.IndexToPosition((int)position.col, position.row - 1));
            if (testing != null && board.GetOnPosition(testing.destiny) == null)
            {
                rawMoves.Add(testing);
                freeAhead = true;
            }

            testing = new Move(this, Position.IndexToPosition((int)position.col, position.row - 2));
            if (testing != null && position.row == 7 && freeAhead && board.GetOnPosition(testing.destiny) == null)
            {
                rawMoves.Add(testing);
            }
        }

        SetSight();
        foreach (Position pos in sight)
        {
            Move move = new Move(this, pos);
            ChessPiece capture = board.GetOnPosition(pos);

            if (capture != null && capture.GetColor() != this.color && capture.GetType() != typeof(King))
            {
                rawMoves.Add(move);
            }
        }

        return rawMoves;
    }

    protected override void SetSight()
    {
        this.sight = new List<Position>();
        Position testing;
        
        switch (this.color)
        {
            case Color.WHITE:
                testing = Position.IndexToPosition((int)position.col - 1, position.row + 1);
                if (testing != null)
                {
                    this.sight.Add(testing);
                }

                testing = Position.IndexToPosition((int)position.col + 1, position.row + 1);
                if (testing != null)
                {
                    this.sight.Add(testing);
                }
                break;
            case Color.BLACK:
                testing = Position.IndexToPosition((int)position.col - 1, position.row - 1);
                if (testing != null)
                {
                    this.sight.Add(testing);
                }

                testing = Position.IndexToPosition((int)position.col + 1, position.row - 1);
                if (testing != null)
                {
                    this.sight.Add(testing);
                }
                break;
        }
    }

    protected override void SetMoves()
    {
        SetSight();
        this.moves = new List<Move>();
        Move testing;
        bool freeAhead = false;

        switch (this.color)
        {
            case Color.WHITE:
                testing = new Move(this, Position.IndexToPosition((int)position.col, position.row + 1));
                if (testing != null && board.GetOnPosition(testing.destiny) == null && board.IsValidMove(testing))
                {
                    this.moves.Add(testing);
                    freeAhead = true;
                }

                testing = new Move(this, Position.IndexToPosition((int)position.col, position.row + 2));
                if (testing != null && position.row == 2 && freeAhead && board.GetOnPosition(testing.destiny) == null && board.IsValidMove(testing))
                {
                    this.moves.Add(testing);
                }
                break;
            case Color.BLACK:
                testing = new Move(this, Position.IndexToPosition((int)position.col, position.row - 1));
                if (testing != null && board.GetOnPosition(testing.destiny) == null && board.IsValidMove(testing))
                {
                    this.moves.Add(testing);
                    freeAhead = true;
                }
                
                testing = new Move(this, Position.IndexToPosition((int)position.col, position.row - 2));
                if (testing != null && position.row == 7 && freeAhead && board.GetOnPosition(testing.destiny) == null && board.IsValidMove(testing))
                {
                    this.moves.Add(testing);
                }
                break;
        }

        foreach (Position pos in sight)
        {
            Move move = new Move(this, pos);
            ChessPiece capture = board.GetOnPosition(pos);

            if (capture != null && capture.GetColor() != this.color && capture.GetType() != typeof(King) && board.IsValidMove(move))
            {
                moves.Add(move);
            }
        }
    }
}

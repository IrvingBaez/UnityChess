using System;
using System.Collections.Generic;

public class King : ChessPiece
{
    private bool canShortCastle = true;
    private bool canLongCastle = true;

    public King(Position position, Color color, Board board)
        : base(position, ChessPiece.Symbol.K, float.PositiveInfinity, color, board){}

    protected override void SetSight()
    {
        sight = new List<Position>();

        CanSeeThrough((int)position.col + 1, position.row + 1);
        CanSeeThrough((int)position.col + 1, position.row);
        CanSeeThrough((int)position.col + 1, position.row - 1);
        CanSeeThrough((int)position.col, position.row + 1);
        CanSeeThrough((int)position.col, position.row - 1);
        CanSeeThrough((int)position.col - 1, position.row + 1);
        CanSeeThrough((int)position.col - 1, position.row);
        CanSeeThrough((int)position.col - 1, position.row - 1);
    }

    protected override void SetMoves()
    {
        base.SetMoves();
        
        if (board.IsInCheck(this))
        {
            return;
        }

        if (canShortCastle)
        {
            bool canShortCastle = true;
            canShortCastle = canShortCastle && board.GetOnPosition(new Position(position.col + 1, position.row)) == null;
            canShortCastle = canShortCastle && board.GetOnPosition(new Position(position.col + 2, position.row)) == null;

            canShortCastle = canShortCastle && !board.IsInCheck(color, new Position(position.col + 1, position.row));
            canShortCastle = canShortCastle && !board.IsInCheck(color, new Position(position.col + 2, position.row));

            if (canShortCastle)
            {
                moves.Add(new Move(this, new Position(position.col + 2, position.row), null, true));
            }
        }

        if (canLongCastle)
        {
            bool canLongCastle = true;
            canLongCastle = canLongCastle && board.GetOnPosition(new Position(position.col - 1, position.row)) == null;
            canLongCastle = canLongCastle && board.GetOnPosition(new Position(position.col - 2, position.row)) == null;
            canLongCastle = canLongCastle && board.GetOnPosition(new Position(position.col - 3, position.row)) == null;

            canLongCastle = canLongCastle && !board.IsInCheck(color, new Position(position.col - 1, position.row));
            canLongCastle = canLongCastle && !board.IsInCheck(color, new Position(position.col - 2, position.row));

            if (canLongCastle)
            {
                moves.Add(new Move(this, new Position(position.col - 2, position.row), null, true));
            }
        }
    }

    public void SetLongCastlingRight(bool canLongCastle)
    {
        this.canLongCastle = canLongCastle;
    }

    public void SetShortCastlingRight(bool canShortCastle)
    {
        this.canShortCastle = canShortCastle;
    }

    public void RemoveCastlingRights()
    {
        canShortCastle = false;
        canLongCastle = false;
    }

    public void RemoveCastlingRights(Rook rook)
    {
        switch (rook.GetPosition().col)
        {
            case Board.Column.A:
                canLongCastle = false;
                break;
            case Board.Column.H:
                canShortCastle = false;
                break;
        }
    }

    public bool CanShortCastle()
    {
        return canShortCastle;
    }

    public bool CanLongCastle()
    {
        return canLongCastle;
    }
}

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
        
        if (IsInCheck())
        {
            return;
        }

        if (canShortCastle)
        {
            bool canShortCastle = true;
            canShortCastle = canShortCastle && board.GetOnPosition(new Position(position.col + 1, position.row)) == null;
            canShortCastle = canShortCastle && board.GetOnPosition(new Position(position.col + 2, position.row)) == null;

            canShortCastle = canShortCastle && !IsInCheck(new Position(position.col + 1, position.row));
            canShortCastle = canShortCastle && !IsInCheck(new Position(position.col + 2, position.row));

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

            canLongCastle = canLongCastle && !IsInCheck(new Position(position.col - 1, position.row));
            canLongCastle = canLongCastle && !IsInCheck(new Position(position.col - 2, position.row));

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

    public bool IsInCheck()
    {
        return IsInCheck(this.position);
    }

    private bool IsInCheck(Position position)
    {
        Color oposite = ChessPiece.OpositeColor(color);

        //Rooks and Queen
        for (int i = 1; i < 8; i++)
        {
            Position pos = new Position(position.col + i, position.row);
            if (board.LookFor(oposite, typeof(Rook), pos)) { return true; }
            if (board.LookFor(oposite, typeof(Queen), pos)) { return true; }
            if (board.GetOnPosition(pos) != null) { break; }
        }

        for (int i = 1; i < 8; i++)
        {
            Position pos = new Position(position.col - i, position.row );
            if (board.LookFor(oposite, typeof(Rook), pos)) { return true; }
            if (board.LookFor(oposite, typeof(Queen), pos)) { return true; }
            if (board.GetOnPosition(pos) != null) { break; }
        }

        for (int i = 1; i < 8; i++)
        {
            Position pos = new Position(position.col, position.row + i);
            if (board.LookFor(oposite, typeof(Rook), pos)) { return true; }
            if (board.LookFor(oposite, typeof(Queen), pos)) { return true; }
            if (board.GetOnPosition(pos) != null) { break; }
        }

        for (int i = 1; i < 8; i++)
        {
            Position pos = new Position(position.col, position.row - i);
            if (board.LookFor(oposite, typeof(Rook), pos)) { return true; }
            if (board.LookFor(oposite, typeof(Queen), pos)) { return true; }
            if (board.GetOnPosition(pos) != null) { break; }
        }

        //Bishops and Queen 
        for (int i = 1; i < 8; i++)
        {
            Position pos = new Position(position.col + i, position.row + i);
            if (board.LookFor(oposite, typeof(Bishop), pos)) { return true; }
            if (board.LookFor(oposite, typeof(Queen), pos)) { return true; }
            if (board.GetOnPosition(pos) != null) { break; }
        }

        for (int i = 1; i < 8; i++)
        {
            Position pos = new Position(position.col + i, position.row - i);
            if (board.LookFor(oposite, typeof(Bishop), pos)) { return true; }
            if (board.LookFor(oposite, typeof(Queen), pos)) { return true; }
            if (board.GetOnPosition(pos) != null) { break; }
        }

        for (int i = 1; i < 8; i++)
        {
            Position pos = new Position(position.col - i, position.row + i);
            if (board.LookFor(oposite, typeof(Bishop), pos)) { return true; }
            if (board.LookFor(oposite, typeof(Queen), pos)) { return true; }
            if (board.GetOnPosition(pos) != null) { break; }
        }

        for (int i = 1; i < 8; i++)
        {
            Position pos = new Position(position.col - i, position.row - i);
            if (board.LookFor(oposite, typeof(Bishop), pos)) { return true; }
            if (board.LookFor(oposite, typeof(Queen), pos)) { return true; }
            if (board.GetOnPosition(pos) != null) { break; }
        }

        bool check = false;

        //Knights
        check = check || board.LookFor(oposite, typeof(Knight), new Position(position.col - 2, position.row + 1));
        check = check || board.LookFor(oposite, typeof(Knight), new Position(position.col - 2, position.row - 1));
        check = check || board.LookFor(oposite, typeof(Knight), new Position(position.col - 1, position.row + 2));
        check = check || board.LookFor(oposite, typeof(Knight), new Position(position.col - 1, position.row - 2));
        check = check || board.LookFor(oposite, typeof(Knight), new Position(position.col + 1, position.row + 2));
        check = check || board.LookFor(oposite, typeof(Knight), new Position(position.col + 1, position.row - 2));
        check = check || board.LookFor(oposite, typeof(Knight), new Position(position.col + 2, position.row + 1));
        check = check || board.LookFor(oposite, typeof(Knight), new Position(position.col + 2, position.row - 1));
        if (check) { return true; }

        //King
        check = check || board.LookFor(oposite, typeof(Knight), new Position(position.col - 1, position.row - 1));
        check = check || board.LookFor(oposite, typeof(Knight), new Position(position.col - 1, position.row));
        check = check || board.LookFor(oposite, typeof(Knight), new Position(position.col - 1, position.row + 1));
        check = check || board.LookFor(oposite, typeof(Knight), new Position(position.col, position.row - 1));
        check = check || board.LookFor(oposite, typeof(Knight), new Position(position.col, position.row + 1));
        check = check || board.LookFor(oposite, typeof(Knight), new Position(position.col + 1, position.row - 1));
        check = check || board.LookFor(oposite, typeof(Knight), new Position(position.col + 1, position.row));
        check = check || board.LookFor(oposite, typeof(Knight), new Position(position.col + 1, position.row + 1));
        if (check) { return true; }

        //Pawns
        switch (color)
        {
            case Color.WHITE:
                check = check || board.LookFor(oposite, typeof(Pawn), new Position(position.col - 1, position.row + 1));
                check = check || board.LookFor(oposite, typeof(Pawn), new Position(position.col + 1, position.row + 1));
                break;
            case Color.BLACK:
                check = check || board.LookFor(oposite, typeof(Pawn), new Position(position.col - 1, position.row - 1));
                check = check || board.LookFor(oposite, typeof(Pawn), new Position(position.col + 1, position.row - 1));
                break;
        }

        return check;
    }

    private bool IsStaleMate()
    {
        switch (this.color)
        {
            case Color.WHITE:
                foreach (ChessPiece piece in board.GetWhitePieces())
                {
                    if (piece.GetMoves().Count > 0)
                    {
                        return false;
                    }
                }
                break;
            case Color.BLACK:
                foreach (ChessPiece piece in board.GetBlackPieces())
                {
                    if (piece.GetMoves().Count > 0)
                    {
                        return false;
                    }
                }
                break;
        }

        return true;
    }

    public Game.GameState GetGameState()
    {
        if (IsInCheck())
        {
            if (IsStaleMate())
            {
                //Checkmate
                switch (color)
                {
                    case Color.WHITE:
                        return Game.GameState.BLACK_MATE;
                    case Color.BLACK:
                        return Game.GameState.WHITE_MATE;
                }
            }
            else
            {
                //Check
                switch (color)
                {
                    case Color.WHITE:
                        return Game.GameState.BLACK_CHECK;
                    case Color.BLACK:
                        return Game.GameState.WHITE_CHECK;
                }
            }
        }
        else
        {
            if (IsStaleMate())
            {
                //Stalemate
                return Game.GameState.STALE_MATE;
            }
            else
            {
                //Live
                return Game.GameState.ALIVE;
            }
        }

        return Game.GameState.INVALID;
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

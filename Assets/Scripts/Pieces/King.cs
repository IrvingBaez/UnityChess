using System;
using System.Collections.Generic;

public class King : ChessPiece
{
    public override void Initialize(Position position, Color color, Board board)
    {
        Initialize(position, ChessPiece.Symbol.K, float.PositiveInfinity, color, board);
    }

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

    public bool IsInCheck()
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
}

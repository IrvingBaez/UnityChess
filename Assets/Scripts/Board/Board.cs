using System;
using System.Collections.Generic;

public class Board
{
    public event Action OnBoardChanged;

    private ChessPiece[,] positions = new ChessPiece[8, 8];
    private Position enPassant;
    private ChessPiece.Color turn;
    private int halfTurn;
    private int fullTurn;

    private King whiteKing;
    private King blackKing;
    private List<ChessPiece> whitePieces = new List<ChessPiece>();
    private List<ChessPiece> blackPieces = new List<ChessPiece>();

    public enum Column { A, B, C, D, E, F, G, H };
    public bool isCopy = false;


    public Board() : this("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1") { }

    public Board(string fen)
    {
        int index = 0;

        //board
        while(fen[0] != ' ')
        {
            int leap;

            if (int.TryParse(fen[0].ToString(), out leap))
            {
                index += leap;
            }
            else
            {
                if(fen[0] != '/')
                {
                    ChessPiece piece = null;
                    
                    switch (fen[0])
                    {
                        case 'K':
                            piece = new King(IntToPosition(index), ChessPiece.Color.WHITE, this);
                            break;
                        case 'Q':
                            piece = new Queen(IntToPosition(index), ChessPiece.Color.WHITE, this);
                            break;
                        case 'R':
                            piece = new Rook(IntToPosition(index), ChessPiece.Color.WHITE, this);
                            break;
                        case 'B':
                            piece = new Bishop(IntToPosition(index), ChessPiece.Color.WHITE, this);
                            break;
                        case 'N':
                            piece = new Knight(IntToPosition(index), ChessPiece.Color.WHITE, this);
                            break;
                        case 'P':
                            piece = new Pawn(IntToPosition(index), ChessPiece.Color.WHITE, this);
                            break;
                        case 'k':
                            piece = new King(IntToPosition(index), ChessPiece.Color.BLACK, this);
                            break;
                        case 'q':
                            piece = new Queen(IntToPosition(index), ChessPiece.Color.BLACK, this);
                            break;
                        case 'r':
                            piece = new Rook(IntToPosition(index), ChessPiece.Color.BLACK, this);
                            break;
                        case 'b':
                            piece = new Bishop(IntToPosition(index), ChessPiece.Color.BLACK, this);
                            break;
                        case 'n':
                            piece = new Knight(IntToPosition(index), ChessPiece.Color.BLACK, this);
                            break;
                        case 'p':
                            piece = new Pawn(IntToPosition(index), ChessPiece.Color.BLACK, this);
                            break;
                    }

                    AddPiece(piece);
                    index++;
                }
            }

            fen = fen.Substring(1);
        }

        //turn
        fen = fen.Substring(1);
        switch (fen[0])
        {
            case 'w':
                turn = ChessPiece.Color.WHITE;
                break;
            case 'b':
                turn = ChessPiece.Color.BLACK;
                break;
            default:
                return;
        }
        fen = fen.Substring(1);

        //castling rights
        fen = fen.Substring(1);
        whiteKing.RemoveCastlingRights();
        blackKing.RemoveCastlingRights();
        
        while (fen[0] != ' ')
        {
            switch (fen[0])
            {
                case 'K':
                    whiteKing.SetShortCastlingRight(true);
                    break;
                case 'Q':
                    whiteKing.SetLongCastlingRight(true);
                    break;
                case 'k':
                    blackKing.SetShortCastlingRight(true);
                    break;
                case 'q':
                    blackKing.SetLongCastlingRight(true);
                    break;
            }

            fen = fen.Substring(1);
        }

        //En passant
        fen = fen.Substring(1);
        if(fen[0] != '-')
        {
            enPassant = Position.StringToPosition(fen.Substring(0,2));
            fen = fen.Substring(1);
        }
        fen = fen.Substring(2);

        //Halfmove count
        if(fen[1] == ' ')
        {
            int.TryParse(fen[0].ToString(), out halfTurn);
            fen = fen.Substring(2);
        }
        else
        {
            int.TryParse(fen.Substring(1,2), out halfTurn);
            fen = fen.Substring(3);
        }

        //Fullmove count
        int.TryParse(fen, out fullTurn);
    }

    public string Fen()
    {
        string fen = "";
        int freeSpaces = 0;

        for (int i = 0; i < 64; i++)
        {
            ChessPiece piece = GetOnPosition(IntToPosition(i));

            if (piece == null)
            {
                freeSpaces++;
            }
            else
            {
                if(freeSpaces > 0)
                {
                    fen += freeSpaces;
                    freeSpaces = 0;
                }

                switch (piece.GetColor())
                {
                    case ChessPiece.Color.WHITE:
                        fen += piece.GetSymbol().ToString();
                        break;
                    case ChessPiece.Color.BLACK:
                        fen += piece.GetSymbol().ToString().ToLower();
                        break;
                }
            }

            if((i + 1) % 8 == 0)
            {
                if (freeSpaces > 0)
                {
                    fen += freeSpaces;
                    freeSpaces = 0;
                }

                if(i < 63)
                {
                    fen += "/";
                }
            }
        }

        switch (turn)
        {
            case ChessPiece.Color.WHITE:
                fen += " w ";
                break;
            case ChessPiece.Color.BLACK:
                fen += " b ";
                break;
        }

        bool castling = false;

        if (whiteKing.CanShortCastle())
        {
            fen += "K";
            castling = true;
        }

        if (whiteKing.CanLongCastle())
        { 
            fen += "Q"; 
            castling = true;
        }

        if (blackKing.CanShortCastle())
        {
            fen += "k";
            castling = true;
        }

        if (blackKing.CanLongCastle())
        {
            fen += "q";
            castling = true;
        }

        if (!castling)
        {
            fen += "-";
        }

        if(enPassant != null)
        {
            fen += $" {enPassant.ToString().ToLower()} ";
        }
        else
        {
            fen += " - ";
        }

        fen += $" {halfTurn} {fullTurn}";

        return fen;
    }

    private Position IntToPosition(int index)
    {
        return new Position((Board.Column)(index % 8), 8 - (int)(index / 8));
    }

    public void AddPiece(ChessPiece piece)
    {
        ChessPiece captured = GetOnPosition(piece.GetPosition());

        if (captured != null)
        {
            whitePieces.Remove(captured);
            blackPieces.Remove(captured);
            positions[(int)captured.GetPosition().col, captured.GetPosition().row - 1] = null;
        }

        switch (piece.GetColor())
        {
            case ChessPiece.Color.WHITE:
                whitePieces.Add(piece);
                if (piece is King)
                    whiteKing = piece as King;
                break;
            case ChessPiece.Color.BLACK:
                blackPieces.Add(piece);
                if (piece is King)
                    blackKing = piece as King;
                break;
        }

        positions[(int)piece.GetPosition().col, piece.GetPosition().row - 1] = piece;
    }

    public void PerformMove(Move move)
    {
        if(move.piece.GetColor() != turn)
        {
            return;
        }

        Position origin = move.piece.GetPosition();

        ChessPiece captured = GetOnPosition(move.destiny);
        if (captured != null)
        {
            whitePieces.Remove(captured);
            blackPieces.Remove(captured);
            positions[(int)captured.GetPosition().col, captured.GetPosition().row - 1] = null;
        }

        if(move.piece is King)
        {
            if (move.isCastling)
            {
                PerformCastling(move);
            }
            (move.piece as King).RemoveCastlingRights();
        }

        if(move.piece is Rook)
        {
            Rook rook = move.piece as Rook;
            switch (rook.GetColor())
            {
                case ChessPiece.Color.WHITE:
                    whiteKing.RemoveCastlingRights(rook);
                    break;
                case ChessPiece.Color.BLACK:
                    blackKing.RemoveCastlingRights(rook);
                    break;
            }
        }

        if (move.piece is Pawn && (move.destiny.row == 8 || move.destiny.row == 1))
        {
            Pawn pawn = (Pawn)move.piece;
            Queen queen = new Queen(pawn.GetPosition(), pawn.GetColor(), this);

            switch (move.piece.GetColor())
            {
                case ChessPiece.Color.WHITE:
                    whitePieces.Remove(move.piece);
                    whitePieces.Add(queen);
                    break;
                case ChessPiece.Color.BLACK:
                    blackPieces.Remove(move.piece);
                    blackPieces.Add(queen);
                    break;
            }
            positions[(int)pawn.GetPosition().col, pawn.GetPosition().row - 1] = null;
            move.piece = queen;
        }

        positions[(int)origin.col, origin.row - 1] = null;
        positions[(int)move.destiny.col, move.destiny.row - 1] = move.piece;
        move.piece.SetBoardPosition(move.destiny);
        
        halfTurn++;

        if(turn == ChessPiece.Color.BLACK)
        {
            fullTurn++;
        }
        
        turn = (ChessPiece.Color)(((int)turn + 1) % 2);

        OnBoardChanged?.Invoke();
    }

    private void PerformCastling(Move move)
    {
        Rook rook = null;
        Position rookDestiny = null;

        switch (move.destiny.col)
        {
            case Column.C:
                rook = (Rook)GetOnPosition(new Position(move.destiny.col - 2, move.destiny.row));
                rookDestiny = new Position(move.destiny.col + 1, move.destiny.row);

                break;
            case Column.G:
                rook = (Rook)GetOnPosition(new Position(move.destiny.col + 1, move.destiny.row));
                rookDestiny = new Position(move.destiny.col - 1, move.destiny.row);
                break;
        }
        PerformMove(new Move(rook, rookDestiny));
    }

    public bool IsValidMove(Move move)
    {
        if (move.piece.GetColor() != turn)
        {
            return false;
        }

        Board copy = CopyAndMove(move);
        bool valid = true;

        switch (move.piece.GetColor())
        {
            case ChessPiece.Color.WHITE:
                if (copy.whiteKing.IsInCheck())
                    valid = false;
                break;
            case ChessPiece.Color.BLACK:
                if (copy.blackKing.IsInCheck())
                    valid = false;
                break;
        }

        return valid;
    }

    public Board CopyAndMove(Move move)
    {
        Board boardCopy = new Board(Fen());
        ChessPiece movingPiece = boardCopy.GetOnPosition(move.piece.GetPosition());

        boardCopy.PerformMove(new Move(movingPiece, move.destiny, move.promoteTo, move.isCastling));

        return boardCopy;
    }

    public ChessPiece GetOnPosition(Position position)
    {
        if (position.row < 1 || position.row > 8 || (int)position.col < 0 || (int)position.col > 7)
        {
            return null;
        }

        return positions[(int)position.col, position.row - 1];
    }

    public bool LookFor(ChessPiece.Color color, Type type, Position position)
    {
        ChessPiece piece = GetOnPosition(position);

        if(piece == null)
        {
            return false;
        }

        return type.Equals(piece.GetType()) && piece.GetColor() == color;
    }

    public King GetWhiteKing()
    {
        return whiteKing;
    }

    public King GetBlackKing()
    {
        return blackKing;
    }

    public List<ChessPiece> GetWhitePieces()
    {
        return whitePieces;
    }

    public List<ChessPiece> GetBlackPieces()
    {
        return blackPieces;
    }

    public ChessPiece.Color GetTurn()
    {
        return this.turn;
    }

    public int GetHalfTurn()
    {
        return halfTurn;
    }

    public override string ToString()
    {
        string boardString = "";

        for(int row = 8; row >= 1; row--)
        {
            for(int col = 0; col <= 7; col++)
            {
                ChessPiece piece = null;
                Position pos = Position.IndexToPosition(col, row);
                if(pos != null)
                {
                    piece = GetOnPosition(pos);
                }

                if(piece != null)
                {
                    if(piece.GetColor() == ChessPiece.Color.WHITE)
                        boardString += piece.GetSymbol().ToString();
                    else
                        boardString += piece.GetSymbol().ToString().ToLower();
                }
                else
                {
                    boardString += "_ ";
                }
                boardString += " ";
            }
            boardString += "\n";
        }

        return boardString;
    }
}
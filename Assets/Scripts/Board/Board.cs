using System;
using System.Collections.Generic;
using System.Linq;

public class Board
{
    public GameState State { get; private set; }

    public Position WhiteKing { get; private set; }

    public Position BlackKing { get; private set; }

    public List<Position> WhitePieces { get; private set; } = new List<Position>();

    public List<Position> BlackPieces { get; private set; } = new List<Position>();

    public List<Position> WhiteSight { get; private set; }

    public List<Position> BlackSight { get; private set; }

    public int Turn { get; private set; }

    private char?[,] tiles = new char?[8, 8];
    private int halfTurn;
    private int fullTurn;
    private Position enPassant;
    private bool isCopy;
    private bool[] whiteCastling = new bool[] { false, false };
    private bool[] blackCastling = new bool[] { false, false };
    public static char[] whiteSymbols = new char[] { 'K', 'Q', 'R', 'B', 'N', 'B', 'P' };
    public static char[] blackSymbols = new char[] { 'k', 'q', 'r', 'b', 'n', 'b', 'p' };

    public enum GameState { UNDEFINED, ALIVE, CHECK_TO_WHITE, CHECK_TO_BLACK, CHECKMATE_TO_WHITE, CHECKMATE_TO_BLACK, STALEMATE, INVALID };

    public class Position : IEquatable<Position>
    {
        public int col;
        public int row;

        private Position(int col, int row)
        {
            this.col = col;
            this.row = row;
        }

        public Position Clone()
        {
            return new Position(col, row);
        }

        public static Position Create(int col, int row)
        {
            if(0 <= col && col <= 7 && 0 <= row && row <= 7)
            {
                return new Position(col, row);
            }
            else
            {
                return null;
            }
        }

        public override string ToString()
        {
            return $"[{col}, {row}]";
        }

        public bool Equals(Position other)
        {
            return other != null && col == other.col && row == other.row;
        }

        public override bool Equals(object obj) => Equals(obj as Position);
        public override int GetHashCode() => (col, row).GetHashCode();
    }

    public class Move
    {
        public Position origin;
        public Position destiny;
        public char? promotion;

        internal Move(Position origin, Position destiny, char? promotion = null)
        {
            this.origin = origin;
            this.destiny = destiny;
            this.promotion = promotion;
        }

        public override string ToString()
        {
            return $"From {origin} to {destiny}";
        }

        public static Move sampleMove = new Move(Position.Create(0, 6), Position.Create(0, 5));
    }

    public Board(){}

    public Board(string fen)
    {
        this.isCopy = false;
        int index = 0;

        //board
        while (fen[0] != ' ')
        {
            int leap;

            if (int.TryParse(fen[0].ToString(), out leap))
            {
                index += leap;
            }
            else
            {
                if (fen[0] != '/')
                {
                    tiles[index % 8, index / 8] = fen[0];

                    //Save pieces and kings positions
                    if (whiteSymbols.Contains(fen[0]))
                    {
                        WhitePieces.Add(Position.Create(index % 8, index / 8));
                        if(fen[0] == 'K')
                        {
                            WhiteKing = Position.Create(index % 8, index / 8);
                        }
                    }
                    else
                    {
                        BlackPieces.Add(Position.Create(index % 8, index / 8));
                        if (fen[0] == 'k')
                        {
                            BlackKing = Position.Create(index % 8, index / 8);
                        }
                    }
                    
                    index++;
                }
            }

            fen = fen.Substring(1);
        }

        //turn
        fen = fen.Substring(1);
        Turn = fen[0] == 'w' ? 1 : -1;

        fen = fen.Substring(1);

        //castling rights
        fen = fen.Substring(1);
        while (fen[0] != ' ')
        {
            switch (fen[0])
            {
                case 'K':
                    whiteCastling[1] = true;
                    break;
                case 'Q':
                    whiteCastling[0] = true;
                    break;
                case 'k':
                    blackCastling[1] = true;
                    break;
                case 'q':
                    blackCastling[0] = true;
                    break;
            }
            fen = fen.Substring(1);
        }

        //En passant
        fen = fen.Substring(1);
        if (fen[0] != '-')
        {
            int.TryParse(fen[1].ToString(), out int col);
            enPassant = Position.Create((int)fen[0] - 97, col - 1);
            fen = fen.Substring(1);
        }
        fen = fen.Substring(2);

        //Halfmove count
        if (fen[1] == ' ')
        {
            int.TryParse(fen[0].ToString(), out halfTurn);
            fen = fen.Substring(2);
        }
        else
        {
            int.TryParse(fen.Substring(1, 2), out halfTurn);
            fen = fen.Substring(3);
        }

        //Fullmove count
        int.TryParse(fen, out fullTurn);

        FindGameState();
    }

    public string Fen()
    {
        string fen = "";
        int freeSpaces = 0;

        for (int row = 0; row < 8; row++)
        {
            for(int col = 0; col < 8; col++)
            {
                if(tiles[col, row] == null)
                {
                    freeSpaces++;
                }
                else
                {
                    if (freeSpaces > 0)
                    {
                        fen += freeSpaces;
                        freeSpaces = 0;
                    }
                    fen += tiles[col, row];
                }
            }

            if (freeSpaces > 0)
            {
                fen += freeSpaces;
                freeSpaces = 0;
            }

            if(row < 7)
            {
                fen += "/";
            }
        }

        switch (Turn)
        {
            case 1:
                fen += " w ";
                break;
            case -1:
                fen += " b ";
                break;
        }

        bool castling = false;

        if (whiteCastling[1])
        {
            fen += "K";
            castling = true;
        }

        if (whiteCastling[0])
        {
            fen += "Q";
            castling = true;
        }

        if (blackCastling[1])
        {
            fen += "k";
            castling = true;
        }

        if (blackCastling[0])
        {
            fen += "q";
            castling = true;
        }

        if (!castling)
        {
            fen += "-";
        }

        if (enPassant != null)
        {
            fen += $" {(char)(enPassant.col + 97)}{enPassant.row + 1} ";
        }
        else
        {
            fen += " - ";
        }

        fen += $"{halfTurn} {fullTurn}";

        return fen;
    }

    public void PerformMove(Move move)
    {
        WhiteSight = null;
        BlackSight = null;

        char? moving = GetOnPosition(move.origin);
        char? captured = GetOnPosition(move.destiny);

        // Forget captured piece
        if (captured != null)
        {
            halfTurn = -1;

            WhitePieces.Remove(move.destiny);
            BlackPieces.Remove(move.destiny);
        }

        // Manage castling on capture
        if (captured == 'R')
        {
            if (move.destiny.col == 0)
            {
                whiteCastling[0] = false;
            }
            else if (move.destiny.col == 7)
            {
                whiteCastling[1] = false;
            }
        }
        else if (captured == 'r')
        {
            if (move.destiny.col == 0)
            {
                blackCastling[0] = false;
            }
            else if (move.destiny.col == 7)
            {
                blackCastling[1] = false;
            }
        }

        // Manage castling on move
        if (moving == 'R')
        {
            if (move.origin.Equals(Position.Create(0, 7)))
            {
                whiteCastling[0] = false;
            }
            else if (move.origin.Equals(Position.Create(7, 7)))
            {
                whiteCastling[1] = false;
            }
        }
        else if (moving == 'r')
        {
            if (move.origin.Equals(Position.Create(0, 0)))
            {
                blackCastling[0] = false;
            }
            else if (move.origin.Equals(Position.Create(7, 0)))
            {
                blackCastling[1] = false;
            }
        }

        // detect and perform castling
        if (moving == 'K')
        {
            if (whiteCastling[0] && move.destiny.col == 2)
            {
                tiles[0, 7] = null;
                tiles[3, 7] = 'R';
            }
            else if (whiteCastling[1] && move.destiny.col == 6)
            {
                tiles[7, 7] = null;
                tiles[5, 7] = 'R';
            }
            whiteCastling = new bool[] { false, false };
        }
        else if (moving == 'k')
        {
            if (blackCastling[0] && move.destiny.col == 2)
            {
                tiles[0, 0] = null;
                tiles[3, 0] = 'r';
            }
            else if (blackCastling[1] && move.destiny.col == 6)
            {
                tiles[7, 0] = null;
                tiles[5, 0] = 'r';
            }
            blackCastling = new bool[] { false, false };
        }

        // detect pawn
        if (moving == 'P' || moving == 'p')
        {
            halfTurn = -1;
            int color = GetPieceColor(move.origin);
            if (move.destiny.Equals(enPassant))
            {
                tiles[enPassant.col, enPassant.row - color] = null;
            }

            if (Math.Abs(move.origin.row - move.destiny.row) == 2)
            {
                enPassant = Position.Create(move.destiny.col, move.destiny.row + color);
            }
            else
            {
                enPassant = null;
            }
        }
        else
        {
            enPassant = null;
        }

        tiles[move.origin.col, move.origin.row] = null;
        tiles[move.destiny.col, move.destiny.row] = move.promotion == null ? moving : move.promotion;

        if (moving == 'K')
            WhiteKing = move.destiny;
        if (moving == 'k')
            BlackKing = move.destiny;

        List<Position> changing = char.IsUpper(moving.Value) ? WhitePieces : BlackPieces;
        changing.Remove(move.origin);
        changing.Add(move.destiny);

        halfTurn++;
        fullTurn += (-Turn + 1) / 2;
        Turn = -Turn;

        if (!isCopy)
        {
            FindGameState();
        }
    }

    public Board CopyAndMove(Move move)
    {
        Board boardCopy = new Board();
        boardCopy.Turn = Turn;
        boardCopy.tiles = tiles.Clone() as char?[,];
        boardCopy.enPassant = enPassant?.Clone();
        boardCopy.WhiteKing = WhiteKing.Clone();
        boardCopy.BlackKing = BlackKing.Clone();
        boardCopy.WhitePieces = WhitePieces.Select(item => (Position)item.Clone()).ToList();
        boardCopy.BlackPieces = BlackPieces.Select(item => (Position)item.Clone()).ToList();
        boardCopy.whiteCastling = new bool[] { whiteCastling[0], whiteCastling[1] };
        boardCopy.blackCastling = new bool[] { blackCastling[0], blackCastling[1] };
        boardCopy.WhiteSight = null;
        boardCopy.isCopy = true;
        boardCopy.PerformMove(move);

        return boardCopy;
    }

    private bool IsValidMove(Move move)
    {
        Board copy = CopyAndMove(move);
        copy.FindSight();
        
        Position king = char.IsUpper(GetOnPosition(move.origin).Value) ? copy.WhiteKing : copy.BlackKing;
        List<Position> sight = char.IsUpper(GetOnPosition(move.origin).Value) ? copy.BlackSight : copy.WhiteSight;

        return !sight.Contains(king);
    }

    private void FindSight()
    {
        WhiteSight = new List<Position>();
        BlackSight = new List<Position>();

        foreach(Position position in WhitePieces)
        {
            WhiteSight = WhiteSight.Union(FindSight(position)).ToList();
        }

        foreach (Position position in BlackPieces)
        {
            BlackSight = BlackSight.Union(FindSight(position)).ToList();
        }
    }

    private List<Position> FindSight(Position position)
    {
        switch (GetOnPosition(position))
        {
            case 'K':
            case 'k':
                return KingSight(position);
            case 'Q':
            case 'q':
                return QueenSight(position);
            case 'R':
            case 'r':
                return RookSight(position);
            case 'B':
            case 'b':
                return BishopSight(position);
            case 'N':
            case 'n':
                return KnightSight(position);
            case 'P':
            case 'p':
                return PawnSight(position);
            default:
                return new List<Position>();
        }
    }

    private List<Position> KingSight(Position position)
    {
        List<Position> kingSight = new List<Position>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (j == 0 && i == 0)
                    continue;
                Position positionInSight = Position.Create(position.col + i, position.row + j);
                if (positionInSight != null)
                    kingSight.Add(positionInSight);
            }
        }

        return kingSight;
    }

    private List<Position> QueenSight(Position position)
    {
        return RookSight(position).Concat(BishopSight(position)).ToList();
    }

    private List<Position> RookSight(Position position)
    {
        List<Position> rookSight = new List<Position>();
        Position checking;

        for(int i = 1; i <= 7; i++)
        {
            checking = Position.Create(position.col, position.row + i);
            if (checking != null)
            {
                rookSight.Add(checking);
                if(GetOnPosition(checking) != null)
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        for (int i = 1; i <= 7; i++)
        {
            checking = Position.Create(position.col, position.row - i);
            if (checking != null)
            {
                rookSight.Add(checking);
                if (GetOnPosition(checking) != null)
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        for (int i = 1; i <= 7; i++)
        {
            checking = Position.Create(position.col + i, position.row);
            if (checking != null)
            {
                rookSight.Add(checking);
                if (GetOnPosition(checking) != null)
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        for (int i = 1; i <= 7; i++)
        {
            checking = Position.Create(position.col - i, position.row);
            if (checking != null)
            {
                rookSight.Add(checking);
                if (GetOnPosition(checking) != null)
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        return rookSight;
    }

    private List<Position> BishopSight(Position position)
    {
        List<Position> bishopSight = new List<Position>();

        Position checking;

        for (int i = 1; i <= 7; i++)
        {
            checking = Position.Create(position.col + i, position.row + i);
            if (checking != null)
            {
                bishopSight.Add(checking);
                if (GetOnPosition(checking) != null)
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        for (int i = 1; i <= 7; i++)
        {
            checking = Position.Create(position.col - i, position.row - i);
            if (checking != null)
            {
                bishopSight.Add(checking);
                if (GetOnPosition(checking) != null)
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        for (int i = 1; i <= 7; i++)
        {
            checking = Position.Create(position.col + i, position.row - i);
            if (checking != null)
            {
                bishopSight.Add(checking);
                if (GetOnPosition(checking) != null)
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        for (int i = 1; i <= 7; i++)
        {
            checking = Position.Create(position.col - i, position.row + i);
            if (checking != null)
            {
                bishopSight.Add(checking);
                if (GetOnPosition(checking) != null)
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        return bishopSight;
    }

    private List<Position> KnightSight(Position position)
    {
        List<Position> knightSight = new List<Position>();
        Position checking;

        int[] skips = new int[] { -2, -1, 1, 2 };
        
        foreach (int i in skips){
            foreach(int j in skips)
            {
                if((i + j) % 2 != 0)
                {
                    checking = Position.Create(position.col + i, position.row + j);
                    if (checking != null)
                        knightSight.Add(checking);
                }
            }
        }

        return knightSight;
    }

    private List<Position> PawnSight(Position position)
    {
        List<Position> pawnSight = new List<Position>();
        Position checking;

        int color = -GetPieceColor(position);
        
        checking = Position.Create(position.col - 1, position.row + color);
        if (checking != null)
            pawnSight.Add(checking);

        checking = Position.Create(position.col + 1, position.row + color);
        if (checking != null)
            pawnSight.Add(checking);

        return pawnSight;
    }

    public List<Move> LegalMoves(Position position)
    {
        if(GetPieceColor(position) != Turn)
        {
            return new List<Move>();
        }

        if (WhiteSight == null || BlackSight == null)
            FindSight();

        switch (GetOnPosition(position))
        {
            case 'K':
            case 'k':
                return KingMoves(position);
            case 'Q':
            case 'q':
                return QueenMoves(position);
            case 'R':
            case 'r':
                return RookMoves(position);
            case 'B':
            case 'b':
                return BishopMoves(position);
            case 'N':
            case 'n':
                return KnightMoves(position);
            case 'P':
            case 'p':
                return PawnMoves(position);
            default:
                return new List<Move>();
        }
    }

    private List<Move> KingMoves(Position position)
    {
        List<Move> kingMoves = new List<Move>();

        List<Position> partners = char.IsUpper(GetOnPosition(position).Value) ? WhitePieces : BlackPieces;
        List<Position> opponentSight = char.IsUpper(GetOnPosition(position).Value) ? BlackSight : WhiteSight;
        bool[] castlig = char.IsUpper(GetOnPosition(position).Value) ? whiteCastling : blackCastling;

        foreach (Position checking in KingSight(position))
        {
            if (!opponentSight.Contains(checking) && !partners.Contains(checking))
            {
                kingMoves.Add(new Move(position, checking));
            }
        }

        if (castlig[0] &&
            !partners.Contains(Position.Create(position.col - 1, position.row)) &&
            !partners.Contains(Position.Create(position.col - 2, position.row)) &&
            !partners.Contains(Position.Create(position.col - 3, position.row)) &&
            !opponentSight.Contains(Position.Create(position.col - 1, position.row)) &&
            !opponentSight.Contains(Position.Create(position.col - 2, position.row))
            )
        {
            kingMoves.Add(new Move(position, Position.Create(position.col - 2, position.row)));
        }

        if (castlig[1] &&
            !partners.Contains(Position.Create(position.col + 1, position.row)) &&
            !partners.Contains(Position.Create(position.col + 2, position.row)) &&
            !opponentSight.Contains(Position.Create(position.col + 1, position.row)) &&
            !opponentSight.Contains(Position.Create(position.col + 2, position.row))
            )
        {
            kingMoves.Add(new Move(position, Position.Create(position.col + 2, position.row)));
        }

        return kingMoves;
    }

    private List<Move> QueenMoves(Position position)
    {
        List<Move> queenMoves = new List<Move>();

        List<Position> partners = char.IsUpper(GetOnPosition(position).Value) ? WhitePieces : BlackPieces;

        foreach (Position checking in QueenSight(position))
        {
            if (!partners.Contains(checking))
            {
                Move move = new Move(position, checking);
                if(IsValidMove(move))
                    queenMoves.Add(move);
            }
        }

        return queenMoves;
    }

    private List<Move> RookMoves(Position position)
    {
        List<Move> rookMoves = new List<Move>();

        List<Position> partners = char.IsUpper(GetOnPosition(position).Value) ? WhitePieces : BlackPieces;

        foreach (Position checking in RookSight(position))
        {
            if (!partners.Contains(checking))
            {
                Move move = new Move(position, checking);
                if (IsValidMove(move))
                    rookMoves.Add(move);
            }
        }

        return rookMoves;
    }

    private List<Move> BishopMoves(Position position)
    {
        List<Move> bishopMoves = new List<Move>();

        List<Position> partners = char.IsUpper(GetOnPosition(position).Value) ? WhitePieces : BlackPieces;

        foreach (Position checking in BishopSight(position))
        {
            if (!partners.Contains(checking))
            {
                Move move = new Move(position, checking);
                if (IsValidMove(move))
                    bishopMoves.Add(move);
            }
        }

        return bishopMoves;
    }

    private List<Move> KnightMoves(Position position)
    {
        List<Move> knightMoves = new List<Move>();

        List<Position> partners = char.IsUpper(GetOnPosition(position).Value) ? WhitePieces : BlackPieces;

        foreach (Position checking in KnightSight(position))
        {
            if (!partners.Contains(checking))
            {
                Move move = new Move(position, checking);
                if (IsValidMove(move))
                    knightMoves.Add(move);
            }
        }

        return knightMoves;
    }

    private List<Move> PawnMoves(Position position)
    {
        List<Move> pawnMoves = new List<Move>();

        //Captures and en passant
        List<Position> opponents = char.IsUpper(GetOnPosition(position).Value) ? BlackPieces : WhitePieces;
        foreach (Position checking in PawnSight(position))
        {
            if (opponents.Contains(checking) || checking.Equals(enPassant))
            {
                Move move = new Move(position, checking);
                if (IsValidMove(move))
                {
                    if (checking.row == 0)
                    {
                        pawnMoves.Add(new Move(position, checking, 'Q'));
                        pawnMoves.Add(new Move(position, checking, 'R'));
                        pawnMoves.Add(new Move(position, checking, 'B'));
                        pawnMoves.Add(new Move(position, checking, 'N'));
                    }
                    else if (checking.row == 7)
                    {
                        pawnMoves.Add(new Move(position, checking, 'q'));
                        pawnMoves.Add(new Move(position, checking, 'r'));
                        pawnMoves.Add(new Move(position, checking, 'b'));
                        pawnMoves.Add(new Move(position, checking, 'n'));
                    }
                    else
                    {
                        pawnMoves.Add(move);
                    }
                }
            }
        }

        //Steps, double steps and promotions
        int step = -GetPieceColor(position);
        Position forward = Position.Create(position.col, position.row + step);
        
        if(forward != null && GetOnPosition(forward) == null)
        {
            Move move = new Move(position, forward);
            if(IsValidMove(move))
            {
                if (forward.row == 0)
                {
                    pawnMoves.Add(new Move(position, forward, 'Q'));
                    pawnMoves.Add(new Move(position, forward, 'R'));
                    pawnMoves.Add(new Move(position, forward, 'B'));
                    pawnMoves.Add(new Move(position, forward, 'N'));
                }
                else if (forward.row == 7)
                {
                    pawnMoves.Add(new Move(position, forward, 'q'));
                    pawnMoves.Add(new Move(position, forward, 'r'));
                    pawnMoves.Add(new Move(position, forward, 'b'));
                    pawnMoves.Add(new Move(position, forward, 'n'));
                }
                else
                {
                    pawnMoves.Add(move);
                }
            }

            if (position.row == (1 - step) * 2.5 + 1)
            {
                forward = Position.Create(position.col, position.row + 2 * step);
                if (forward != null && GetOnPosition(forward) == null)
                {
                    move = new Move(position, forward);
                    if (IsValidMove(move))
                        pawnMoves.Add(move);
                }
            }
        }

        return pawnMoves;
    }

    private bool AnyLegalMoves()
    {
        List<Position> pieces = Turn > 0 ? WhitePieces : BlackPieces;

        foreach (Position piece in pieces)
        {
            if(LegalMoves(piece).Count > 0)
            {
                return true;
            }
        }

        return false;
    }

    private void FindGameState()
    {
        FindSight();
        Position king = Turn > 0 ? WhiteKing : BlackKing;
        List<Position> enemySight = Turn > 0 ? BlackSight : WhiteSight;

        if (enemySight.Contains(king))
        {
            State = Turn > 0 ? GameState.CHECK_TO_WHITE : GameState.CHECK_TO_BLACK;
            if (!AnyLegalMoves())
            {
                State = Turn > 0 ? GameState.CHECKMATE_TO_WHITE : GameState.CHECKMATE_TO_BLACK;
                return;
            }
            return;
        }
        
        if (!AnyLegalMoves())
        {
            State = GameState.STALEMATE;
            return;
        }

        State = GameState.ALIVE;
    }

    public char? GetOnPosition(Position position)
    {
        return tiles[position.col, position.row];
    }

    public int GetPieceColor(Position position)
    {
        return char.IsUpper(GetOnPosition(position).Value) ? 1 : -1;
    }

    public override string ToString()
    {
        string boardString = "";

        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                if (tiles[col, row] != null)
                {
                    boardString += tiles[col, row];
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
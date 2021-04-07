using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

public partial class Board : ScriptableObject
{
    public GameState State { get; private set; }
    public Position WhiteKing { get; private set; }
    public Position BlackKing { get; private set; }
    public List<Position> WhitePieces { get; private set; } = new List<Position>();
    public List<Position> BlackPieces { get; private set; } = new List<Position>();
    public Dictionary<Position, List<Position>> WhiteSight { get; private set; }
    public Dictionary<Position, List<Position>> BlackSight { get; private set; }
    public Dictionary<Position, List<Position>> WhitePins { get; private set; }
    public Dictionary<Position, List<Position>> BlackPins { get; private set; }
    public Dictionary<Position, List<Position>> WhiteChecks { get; private set; }
    public Dictionary<Position, List<Position>> BlackChecks { get; private set; }
    public Dictionary<Position, List<Move>> LegalMoves { get; private set; }

    public int Turn { get; private set; }

    private char?[,] tiles = new char?[8, 8];
    private int halfTurn;
    private int fullTurn;
    private Position enPassant;
    private bool[] whiteCastling = new bool[] { false, false };
    private bool[] blackCastling = new bool[] { false, false };
    public static char[] whiteSymbols = new char[] { 'K', 'Q', 'R', 'B', 'N', 'B', 'P' };
    public static char[] blackSymbols = new char[] { 'k', 'q', 'r', 'b', 'n', 'b', 'p' };

    public enum GameState { UNDEFINED, ALIVE, CHECK_TO_WHITE, CHECK_TO_BLACK, CHECKMATE_TO_WHITE, CHECKMATE_TO_BLACK, STALEMATE, INVALID };

    public void PerformMove(Move move)
    {
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

        FindSight();
        FindLegalMoves();

        FindGameState();
    }

    private List<Position> FlatDict(Dictionary<Position, List<Position>> dict){
        return dict.Values.SelectMany(x => x).ToList();
    }

    private bool AnyLegalMoves(){
        return LegalMoves.Values.SelectMany(x => x).Count() > 0;
    }

    private void FindGameState()
    {
        Position king = Turn == 1 ? WhiteKing : BlackKing;
        List<Position> enemySight = FlatDict(Turn > 0 ? BlackSight : WhiteSight);

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
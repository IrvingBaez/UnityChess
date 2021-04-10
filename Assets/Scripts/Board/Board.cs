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

    private char?[] tiles = new char?[64];
    private int halfTurn;
    private int fullTurn;
    private Position enPassant;
    private bool[] whiteCastling = new bool[] { false, false };
    private bool[] blackCastling = new bool[] { false, false };
    public static char[] whiteSymbols = new char[] { 'K', 'Q', 'R', 'B', 'N', 'B', 'P' };
    public static char[] blackSymbols = new char[] { 'k', 'q', 'r', 'b', 'n', 'b', 'p' };

    public enum GameState { UNDEFINED, ALIVE, CHECK_TO_WHITE, CHECK_TO_BLACK, CHECKMATE_TO_WHITE, CHECKMATE_TO_BLACK, STALEMATE, INVALID };

    public void PerformMove(Move move){
        performeMoveWatch.Start();

        char? moving = GetOnPosition(move.origin);

        bool[] castling = char.IsUpper(moving.Value) ? whiteCastling : blackCastling;
        move.castling = new bool[]{ castling[0], castling[1] };
        move.enPassant = enPassant;
        move.halfturn = halfTurn;

        // Forget captured piece
        if (move.capture != null){
            halfTurn = -1;

            WhitePieces.Remove(move.destiny);
            BlackPieces.Remove(move.destiny);
        }

        // Manage castling on capture
        if (move.capture == 'R'){
            whiteCastling[0] = whiteCastling[0] && move.destiny.col != 0;
            whiteCastling[1] = whiteCastling[1] && move.destiny.col != 7;
        } else if (move.capture == 'r'){
            blackCastling[0] = blackCastling[0] && move.destiny.col != 0;
            blackCastling[1] = blackCastling[1] && move.destiny.col != 7;
        }

        // Manage castling on move
        if (moving == 'R'){
            whiteCastling[0] = whiteCastling[0] && move.origin.col != 0;
            whiteCastling[1] = whiteCastling[1] && move.origin.col != 7;
        } else if (moving == 'r') {
            blackCastling[0] = blackCastling[0] && move.origin.col != 0;
            blackCastling[1] = blackCastling[1] && move.origin.col != 7;
        }

        // detect and perform castling
        if (moving == 'K'){
            if (whiteCastling[0] && move.destiny.col == 2){
                SetOnPosition(0, 7, null);
                SetOnPosition(3, 7, 'R');
            } else if (whiteCastling[1] && move.destiny.col == 6){
                SetOnPosition(7, 7, null);
                SetOnPosition(5, 7, 'R');
            }

            whiteCastling = new bool[] { false, false };
        } else if (moving == 'k'){
            if (blackCastling[0] && move.destiny.col == 2){
                SetOnPosition(0, 0, null);
                SetOnPosition(3, 0, 'r');
            } else if (blackCastling[1] && move.destiny.col == 6){
                SetOnPosition(7, 0, null);
                SetOnPosition(5, 0, 'r');
            }

            blackCastling = new bool[] { false, false };
        }

        // detect pawn
        if (moving == 'P' || moving == 'p'){
            halfTurn = -1;
            int color = GetPieceColor(move.origin);

            if (move.destiny.Equals(enPassant)){
                SetOnPosition(enPassant.col, enPassant.row - color, null);
            }

            enPassant = null;
            if (Math.Abs(move.origin.row - move.destiny.row) == 2){
                enPassant = Position.Create(move.destiny.col, move.destiny.row + color);
            }
        } else {
            enPassant = null;
        }

        SetOnPosition(move.origin.col, move.origin.row, null);
        SetOnPosition(move.destiny.col, move.destiny.row, move.promotion == null ? moving : move.promotion);

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

        performeMoveWatch.Stop();

        FindSight();
        FindLegalMoves();
        FindGameState();
    }

    public void undoMove(Move move){
        undoMoveWatch.Start();

        char moving = GetOnPosition(move.destiny).Value;

        halfTurn = move.halfturn;
        enPassant = move.enPassant;

        // Restore captured piece
        if (move.capture != null){
            // halfTurn = -1; Debería recuperar el valor anterior.

            if(char.IsUpper(move.capture.Value))
                WhitePieces.Add(move.destiny);
            else
                BlackPieces.Remove(move.destiny);
        }

        // Restore castling
        if (char.IsUpper(moving)){
            whiteCastling = move.castling;
        } else {
            blackCastling = move.castling;
        }

        // Detect and undo castling
        if (moving == 'K'){
            if (move.castling[0] && move.destiny.col == 2){
                SetOnPosition(0, 7, 'R');
                SetOnPosition(3, 7, null);
            } else if (whiteCastling[1] && move.destiny.col == 6){
                SetOnPosition(7, 7, 'R');
                SetOnPosition(5, 7, null);
            }
        } else if (moving == 'k'){
            if (blackCastling[0] && move.destiny.col == 2){
                SetOnPosition(0, 0, 'r');
                SetOnPosition(3, 0, null);
            } else if (blackCastling[1] && move.destiny.col == 6){
                SetOnPosition(7, 0, 'r');
                SetOnPosition(5, 0, null);
            }
        }

        // restore pawn on promotion
        if (move.promotion != null){
            moving = char.IsUpper(moving) ? 'P' : 'p';
        }

        SetOnPosition(move.destiny.col, move.destiny.row, move.capture);
        SetOnPosition(move.origin.col, move.origin.row, moving);

        if (moving == 'K')
            WhiteKing = move.origin;
        if (moving == 'k')
            BlackKing = move.origin;

        List<Position> changing = char.IsUpper(moving) ? WhitePieces : BlackPieces;
        changing.Remove(move.destiny);
        changing.Add(move.origin);

        fullTurn -= (-Turn + 1) / 2;
        Turn = -Turn;

        undoMoveWatch.Stop();

        FindSight();
        FindLegalMoves();
        FindGameState();
    }

    private List<Position> FlatDict(Dictionary<Position, List<Position>> dict){
        return dict.Values.SelectMany(x => x).ToList();
    }

    public List<Move> AllLegalMoves(){
        return LegalMoves.Values.SelectMany(x => x).ToList();
    }

    private bool AnyLegalMoves(){
        return AllLegalMoves().Count() > 0;
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

    public void SetOnPosition(int col, int row, char? value){
        tiles[col * 8 + row] = value;
    }

    public char? GetOnPosition(Position position)
    {
        return tiles[position.col * 8 + position.row];
    }

    public char? GetOnPosition(int col, int row)
    {
        return tiles[col * 8 + row];
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
                if (GetOnPosition(col, row) != null)
                {
                    boardString += GetOnPosition(col, row);
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
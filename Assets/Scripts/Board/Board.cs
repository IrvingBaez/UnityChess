using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

public partial class Board : ScriptableObject
{
    public GameState State { get; private set; }

    public long whitePawns;
    public long blackPawns;
    public long whiteKnights;
    public long blackKnights;
    public long whiteBishops;
    public long blackBishops;
    public long whiteRooks;
    public long blackRooks;
    public long whiteQueens;
    public long blackQueens;
    public long whiteKing;
    public long blackKing;
    public long WhiteSight;
    public long WhiteBlack;
    public int Turn { get; private set; }
    private int halfTurn;
    private int fullTurn;
    private long enPassant;
    private bool[] whiteCastling = new bool[] { false, false };
    private bool[] blackCastling = new bool[] { false, false };
    public static char[] whiteSymbols = new char[] { 'K', 'Q', 'R', 'B', 'N', 'B', 'P' };
    public static char[] blackSymbols = new char[] { 'k', 'q', 'r', 'b', 'n', 'b', 'p' };

    public enum GameState { UNDEFINED, ALIVE, CHECK_TO_WHITE, CHECK_TO_BLACK, CHECKMATE_TO_WHITE, CHECKMATE_TO_BLACK, STALEMATE, INVALID };

    public Board(){
        whitePawns =    11111111 << 8;
        blackPawns =    11111111 << 48;
        whiteRooks =    10000001;
        blackRooks =    10000001 << 56;
        whiteKnights =  01000010;
        blackKnights =  01000010 << 56;
        whiteBishops =  00100100;
        blackBishops =  00100100 << 56;
        whiteQueens =   00010000;
        blackQueens =   00010000 << 56;
        whiteKing =     00001000;
        blackKing =     00001000 << 56;

        State = GameState.ALIVE;
        Turn = 1;
        halfTurn = 0;
        fullTurn = 1;
    }
    public void PerformMove(Move move){
        performeMoveWatch.Start();

        char? moving = GetOnPosition(move.origin);
        int color = GetPieceColor(move.origin);

        bool[] castling = color == 1 ? whiteCastling : blackCastling;
        move.castling = new bool[]{ castling[0], castling[1] };
        move.enPassant = enPassant;
        move.halfturn = halfTurn;

        // Forget captured piece
        move.capture = RemoveFromPosition(move.destiny);

        // Manage castling on capture
        /*
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
        */

        RemoveFromPosition(move.origin);
        SetOnPosition(move.destiny, move.promotion == null ? moving : move.promotion);

        if (moving == 'K')
            whiteKing = (long) 1 << move.destiny;
        if (moving == 'k')
            blackKing = (long) 1 << move.destiny;

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

        // Restore castling
        /*
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
        */

        SetOnPosition(move.destiny, move.capture);
        SetOnPosition(move.origin, moving);

        if (moving == 'K')
            whiteKing = (long) 1 << move.origin;
        if (moving == 'k')
            blackKing = (long) 1 << move.origin;

        fullTurn -= (-Turn + 1) / 2;
        Turn = -Turn;

        undoMoveWatch.Stop();

        FindSight();
        FindLegalMoves();
        FindGameState();
    }

    public List<Move> AllLegalMoves(){
        return legalMoves.Values.SelectMany(x => x).ToList();
    }

    private bool AnyLegalMoves(){
        return AllLegalMoves().Count() > 0;
    }

    private void FindGameState()
    {
        long king = Turn == 1 ? whiteKing : blackKing;

        if ((enemySight & king) != 0)
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

    private void SetOnPosition(int position, char? value){
        long positionMask = (long)1 << position;

        switch(value){
            case 'K':
                whiteKing |= positionMask;
                break;
            case 'Q':
                whiteQueens |= positionMask;
                break;
            case 'B':
                whiteBishops |= positionMask;
                break;
            case 'N':
                whiteKnights |= positionMask;
                break;
            case 'R':
                whiteRooks |= positionMask;
                break;
            case 'P':
                whitePawns |= positionMask;
                break;
            case 'k':
                blackKing |= positionMask;
                break;
            case 'q':
                blackQueens |= positionMask;
                break;
            case 'b':
                blackBishops |= positionMask;
                break;
            case 'n':
                blackKnights |= positionMask;
                break;
            case 'r':
                blackRooks |= positionMask;
                break;
            case 'p':
                blackPawns |= positionMask;
                break;
        }
    }

    private char? RemoveFromPosition(int position){
        long positionMask = (long)1 << position;

        if(IsBitSet(whitePawns, position)){
            whitePawns ^= positionMask;
            return 'P';
        }

        if(IsBitSet(blackPawns, position)){
            blackPawns ^= positionMask;
            return 'p';
        }

        if(IsBitSet(whiteKnights, position)){
            whiteKnights ^= positionMask;
            return 'N';
        }

        if(IsBitSet(blackKnights, position)){
            blackKnights ^= positionMask;
            return 'n';
        }

        if(IsBitSet(whiteBishops, position)){
            whiteBishops ^= positionMask;
            return 'B';
        }

        if(IsBitSet(blackBishops, position)){
            blackBishops ^= positionMask;
            return 'b';
        }

        if(IsBitSet(whiteRooks, position)){
            whiteRooks ^= positionMask;
            return 'R';
        }

        if(IsBitSet(blackRooks, position)){
            blackRooks ^= positionMask;
            return 'r';
        }

        if(IsBitSet(whiteQueens, position)){
            whiteQueens ^= positionMask;
            return 'Q';
        }

        if(IsBitSet(blackQueens, position)){
            blackQueens ^= positionMask;
            return 'q';
        }

        return null;
    }

    public char? GetOnPosition(int position)
    {
        long positionMask = (long)1 << position;
        
        if((whiteKing & positionMask) != 0)
            return 'K';
        if((whiteQueens & positionMask) != 0)
            return 'Q';
        if((whiteBishops & positionMask) != 0)
            return 'B';
        if((whiteKnights & positionMask) != 0)
            return 'N';
        if((whiteRooks & positionMask) != 0)
            return 'R';
        if((whitePawns & positionMask) != 0)
            return 'P';
        if((blackKing & positionMask) != 0)
            return 'K';
        if((blackQueens & positionMask) != 0)
            return 'Q';
        if((blackBishops & positionMask) != 0)
            return 'B';
        if((blackKnights & positionMask) != 0)
            return 'N';
        if((blackRooks & positionMask) != 0)
            return 'R';
        if((blackPawns & positionMask) != 0)
            return 'P';
        
        return null;
    }

    public int GetPieceColor(int position)
    {
        long positionMask = (long)1 << position;
        long blackPieces = blackKing | blackQueens | blackBishops | blackKnights | blackRooks | blackPawns;
        long whitePieces = whiteKing | whiteQueens | whiteBishops | whiteKnights | whiteRooks | whitePawns;

        if((whitePieces & positionMask) != 0)
            return 1;

        if((blackPieces & positionMask) != 0)
            return -1;
        
        return 0;
    }

    public override string ToString()
    {
        string boardString = "";

        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                if (GetOnPosition(col * 8 + row) != null)
                {
                    boardString += GetOnPosition(col * 8 + row);
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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class Game : MonoBehaviour
{
    public Pawn pawn;
    public Rook rook;
    public Bishop bishop;
    public Queen queen;

    public Board board;

    public Player white;
    public Player black;
    private Player currentTurn;

    private ChessPiece[,] positions = new ChessPiece[8, 8];
    private ChessPiece[] whitePieces = new ChessPiece[16];
    private ChessPiece[] blackPieces = new ChessPiece[16];

    private GameState gameState = GameState.ALIVE;

    public enum GameState { ALIVE, WHITE_CHECK, WHITE_MATE, BLACK_CHECK, BLACK_MATE, STALE_MATE, INVALID };

    public Board GetBoard()
    {
        return board;
    }

    void Start()
    {
        whitePieces = new ChessPiece[]{
            Manufacture(ChessPiece.Color.WHITE, ChessPiece.Symbol.K, ChessPiece.Column.E, 1),
            Manufacture(ChessPiece.Color.WHITE, ChessPiece.Symbol.Q, ChessPiece.Column.D, 1),
            
            Manufacture(ChessPiece.Color.WHITE, ChessPiece.Symbol.R, ChessPiece.Column.A, 1),
            Manufacture(ChessPiece.Color.WHITE, ChessPiece.Symbol.R, ChessPiece.Column.H, 1),
            
            Manufacture(ChessPiece.Color.WHITE, ChessPiece.Symbol.B, ChessPiece.Column.C, 1),
            Manufacture(ChessPiece.Color.WHITE, ChessPiece.Symbol.B, ChessPiece.Column.F, 1),
            Manufacture(ChessPiece.Color.WHITE, ChessPiece.Symbol.N, ChessPiece.Column.B, 1),
            Manufacture(ChessPiece.Color.WHITE, ChessPiece.Symbol.N, ChessPiece.Column.G, 1),
            
            Manufacture(ChessPiece.Color.WHITE, ChessPiece.Symbol.P, ChessPiece.Column.A, 2),
            Manufacture(ChessPiece.Color.WHITE, ChessPiece.Symbol.P, ChessPiece.Column.B, 2),
            Manufacture(ChessPiece.Color.WHITE, ChessPiece.Symbol.P, ChessPiece.Column.C, 2),
            Manufacture(ChessPiece.Color.WHITE, ChessPiece.Symbol.P, ChessPiece.Column.D, 2),
            Manufacture(ChessPiece.Color.WHITE, ChessPiece.Symbol.P, ChessPiece.Column.E, 2),
            Manufacture(ChessPiece.Color.WHITE, ChessPiece.Symbol.P, ChessPiece.Column.F, 2),
            Manufacture(ChessPiece.Color.WHITE, ChessPiece.Symbol.P, ChessPiece.Column.G, 2),
            Manufacture(ChessPiece.Color.WHITE, ChessPiece.Symbol.P, ChessPiece.Column.H, 2),
        };

        blackPieces = new ChessPiece[]{
            Manufacture(ChessPiece.Color.BLACK, ChessPiece.Symbol.K, ChessPiece.Column.E, 8),
            Manufacture(ChessPiece.Color.BLACK, ChessPiece.Symbol.Q, ChessPiece.Column.D, 8),

            Manufacture(ChessPiece.Color.BLACK, ChessPiece.Symbol.R, ChessPiece.Column.A, 8),
            Manufacture(ChessPiece.Color.BLACK, ChessPiece.Symbol.R, ChessPiece.Column.H, 8),

            Manufacture(ChessPiece.Color.BLACK, ChessPiece.Symbol.B, ChessPiece.Column.C, 8),
            Manufacture(ChessPiece.Color.BLACK, ChessPiece.Symbol.B, ChessPiece.Column.F, 8),
            Manufacture(ChessPiece.Color.BLACK, ChessPiece.Symbol.N, ChessPiece.Column.B, 8),
            Manufacture(ChessPiece.Color.BLACK, ChessPiece.Symbol.N, ChessPiece.Column.G, 8),

            Manufacture(ChessPiece.Color.BLACK, ChessPiece.Symbol.P, ChessPiece.Column.A, 7),
            Manufacture(ChessPiece.Color.BLACK, ChessPiece.Symbol.P, ChessPiece.Column.B, 7),
            Manufacture(ChessPiece.Color.BLACK, ChessPiece.Symbol.P, ChessPiece.Column.C, 7),
            Manufacture(ChessPiece.Color.BLACK, ChessPiece.Symbol.P, ChessPiece.Column.D, 7),
            Manufacture(ChessPiece.Color.BLACK, ChessPiece.Symbol.P, ChessPiece.Column.E, 7),
            Manufacture(ChessPiece.Color.BLACK, ChessPiece.Symbol.P, ChessPiece.Column.F, 7),
            Manufacture(ChessPiece.Color.BLACK, ChessPiece.Symbol.P, ChessPiece.Column.G, 7),
            Manufacture(ChessPiece.Color.BLACK, ChessPiece.Symbol.P, ChessPiece.Column.H, 7),
        };

        foreach(ChessPiece piece in whitePieces)
        {
            positions[(int)piece.GetPosition().col, piece.GetPosition().row - 1] = piece;
        }

        foreach (ChessPiece piece in blackPieces)
        {
            positions[(int)piece.GetPosition().col, piece.GetPosition().row - 1] = piece;
        }
    }

    public void Move(ChessPiece piece, ChessPiece.Position destiny)
    {
        ChessPiece.Position origin = piece.GetPosition();

        ChessPiece captured = GetOnPosition(destiny);
        if (captured != null)
        {
            print(captured);
            Destroy(captured.gameObject);
        }

        positions[(int)origin.col, origin.row - 1] = null;
        positions[(int)destiny.col, destiny.row - 1] = piece;
        piece.SetBoardPosition(destiny);
    }

    private ChessPiece Manufacture(ChessPiece.Color color, ChessPiece.Symbol symbol, ChessPiece.Column col, int row)
    {
        ChessPiece piece;
        ChessPiece.Position position = new ChessPiece.Position(col, row);

        switch (symbol)
        {
            case ChessPiece.Symbol.K:
                piece = Instantiate(pawn);
                break;
            case ChessPiece.Symbol.Q:
                piece = Instantiate(queen);
                break;
            case ChessPiece.Symbol.R:
                piece = Instantiate(rook);
                break;
            case ChessPiece.Symbol.B:
                piece = Instantiate(bishop);
                break;
            case ChessPiece.Symbol.N:
                piece = Instantiate(pawn);
                break;
            case ChessPiece.Symbol.P:
                piece = Instantiate(pawn);
                break;
            default:
                piece = null;
                break;
        }

        piece.Initialize(position, color);
        return piece;
    }

    public ChessPiece.Position IndexToPosition(int col, int row)
    {
        if (row < 1 || row > 8 || col < 0 || col > 7)
        {
            return null;
        }

        try
        {
            return new ChessPiece.Position((ChessPiece.Column)col, row);
        }
        catch (System.Exception)
        {
            return null;
        }
    }

    public ChessPiece GetOnPosition(ChessPiece.Position position)
    {
        if (position.row < 1 || position.row > 8)
        {
            return null;
        }

        try
        {
            return positions[(int)position.col, position.row - 1];
        }
        catch (System.Exception)
        {
            return null;
        }
    }
}

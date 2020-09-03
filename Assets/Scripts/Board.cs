using System;
using UnityEngine;

public class Board : MonoBehaviour
{
    public event Action OnBoardChanged;

    [SerializeField] private King king;
    [SerializeField] private Queen queen;
    [SerializeField] private Rook rook;
    [SerializeField] private Bishop bishop;
    [SerializeField] private Knight knight;
    [SerializeField] private Pawn pawn;

    private ChessPiece[,] positions = new ChessPiece[8, 8];
    private ChessPiece[] whitePieces = new ChessPiece[16];
    private ChessPiece[] blackPieces = new ChessPiece[16];

    public void Initialize()
    {
        InitializePieces();
    }

    public void Move(ChessPiece piece, ChessPiece.Position destiny)
    {
        ChessPiece.Position origin = piece.GetPosition();

        ChessPiece captured = GetOnPosition(destiny);
        if (captured != null)
        {
            print("Destroyed on capture: " + captured);
            Destroy(captured.gameObject);
            Destroy(captured);
        }

        positions[(int)origin.col, origin.row - 1] = null;
        positions[(int)destiny.col, destiny.row - 1] = piece;
        piece.SetBoardPosition(destiny);

        OnBoardChanged?.Invoke();
    }

    public bool IsValidMove(ChessPiece piece, ChessPiece.Position destiny)
    {
        Board copy = this.CopyAndMove(piece, destiny);
        bool valid = true;

        switch (piece.GetColor())
        {
            case ChessPiece.Color.WHITE:
                if ((copy.getWhitePieces()[0] as King).IsInCheck())
                    valid = false;
                break;
            case ChessPiece.Color.BLACK:
                if ((copy.getBlackPieces()[0] as King).IsInCheck())
                    valid = false;
                break;
        }

        DestroyBoardCopy(copy);
        return valid;
    }

    public Board CopyAndMove(ChessPiece piece, ChessPiece.Position destiny)
    {
        // TODO, dejar más bonita esta madre.

        Board copy = Instantiate(this);
        for (int i = 0; i < 16; i++)
        {
            if (whitePieces[i] != null)
            {
                ChessPiece pieceCopy = Instantiate(whitePieces[i]);
                pieceCopy.Initialize(whitePieces[i].GetPosition(), whitePieces[i].GetColor(), copy);

                if(piece == whitePieces[i])
                {
                    pieceCopy.SetBoardPosition(destiny);
                }

                copy.whitePieces[i] = pieceCopy;
                ChessPiece.Position pos = pieceCopy.GetPosition();
                copy.positions[(int)pos.col, pos.row - 1] = pieceCopy;
            }

            if (blackPieces[i] != null)
            {
                ChessPiece pieceCopy = Instantiate(blackPieces[i]);
                pieceCopy.Initialize(blackPieces[i].GetPosition(), blackPieces[i].GetColor(), copy);

                if (piece == blackPieces[i])
                {
                    pieceCopy.SetBoardPosition(destiny);
                }

                copy.blackPieces[i] = pieceCopy;
                ChessPiece.Position pos = pieceCopy.GetPosition();
                copy.positions[(int)pos.col, pos.row - 1] = pieceCopy;
            }
        }

        return copy;
    }

    public void DestroyBoardCopy(Board copy)
    {
        for (int i = 0; i < 16; i++)
        {
            if (copy.whitePieces[i] != null)
            {
                Destroy(copy.whitePieces[i].gameObject);
            }

            if (copy.blackPieces[i] != null)
            {
                Destroy(copy.blackPieces[i].gameObject);
            }
        }

        Destroy(copy.gameObject);
    }

    public ChessPiece GetOnPosition(ChessPiece.Position position)
    {
        if (position.row < 1 || position.row > 8 || (int)position.col < 0 || (int)position.col > 7)
        {
            return null;
        }

        return positions[(int)position.col, position.row - 1];
    }

    public bool LookFor(ChessPiece.Color color, Type type, ChessPiece.Position position)
    {
        ChessPiece piece = GetOnPosition(position);

        if(piece == null)
        {
            return false;
        }

        return type.Equals(piece.GetType()) && piece.GetColor() == color;
    }

    private void InitializePieces()
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

        foreach (ChessPiece piece in whitePieces)
        {
            positions[(int)piece.GetPosition().col, piece.GetPosition().row - 1] = piece;
        }

        foreach (ChessPiece piece in blackPieces)
        {
            positions[(int)piece.GetPosition().col, piece.GetPosition().row - 1] = piece;
        }
    }

    private ChessPiece Manufacture(ChessPiece.Color color, ChessPiece.Symbol symbol, ChessPiece.Column col, int row)
    {
        ChessPiece piece;
        ChessPiece.Position position = new ChessPiece.Position(col, row);

        switch (symbol)
        {
            case ChessPiece.Symbol.K:
                piece = Instantiate(king);
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
                piece = Instantiate(knight);
                break;
            case ChessPiece.Symbol.P:
                piece = Instantiate(pawn);
                break;
            default:
                piece = null;
                break;
        }

        piece.Initialize(position, color, this);
        return piece;
    }

    public ChessPiece[] getWhitePieces()
    {
        return whitePieces;
    }

    public ChessPiece[] getBlackPieces()
    {
        return blackPieces;
    }
}

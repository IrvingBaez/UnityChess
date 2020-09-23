using System;
using System.Collections.Generic;

public abstract class ChessPiece
{
    public enum Color { BLACK, WHITE };
    public enum Symbol { K, Q, R, B, N, P };

    protected static int nextId = 0;

    protected Color color;
    protected Symbol symbol;
    protected float value;
    protected Position position;
    protected readonly int id;

    protected List<Position> sight;
    protected List<Move> moves;

    public Board board;
    public bool isCopy = false;

    protected ChessPiece(Position position, Symbol symbol, float value, Color color, Board board)
    {
        this.SetBoardPosition(position);
        this.symbol = symbol;
        this.value = value;
        this.color = color;
        this.board = board;
        this.id = nextId;
        nextId++;
    }
    protected abstract void SetSight();

    public static Color OpositeColor(Color color)
    {
        if (color == Color.WHITE)
            return Color.BLACK;

        return Color.WHITE;
    }

    public void SetBoardPosition(Position position)
    {
        this.position = position;
    }

    protected bool CanSeeThrough(int col, int row)
    {
        Position testing = Position.IndexToPosition(col, row);
        if (testing != null)
        {
            sight.Add(testing);
            if (board.GetOnPosition(testing) != null)
            {
                return false;
            }
            return true;
        }

        return false;
    }

    protected virtual void SetMoves()
    {
        moves = new List<Move>();
        SetSight();

        foreach (Position pos in sight)
        {
            ChessPiece capture = board.GetOnPosition(pos);
            if (capture == null || (capture.color != this.color && capture.GetType() != typeof(King)))
            {
                Move move = new Move(this, pos);
                if (board.IsValidMove(move))
                {
                    moves.Add(move);
                }
            }
        }
    }

    public override string ToString()
    {
        return $"{id}: {color} {symbol} in {position}";
    }

    public float GetValue()
    {
        return this.value;
    }

    public Color GetColor()
    {
        return this.color;
    }

    public Symbol GetSymbol()
    {
        return this.symbol;
    }

    public Position GetPosition()
    {
        return this.position;
    }

    public List<Move> GetMoves()
    {
        SetMoves();
        return this.moves;
    }
}

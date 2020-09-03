using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChessPiece : MonoBehaviour
{
    public static event EventHandler PieceClicked;

    public enum Color { BLACK, WHITE };
    public enum Symbol { K, Q, R, B, N, P };
    public enum Column { A, B, C, D, E, F, G, H };

    public Sprite black_sprite;
    public Sprite white_sprite;
    
    protected Color color;
    protected Symbol symbol;
    protected float value;
    protected Position position;

    protected List<Position> sight;
    protected List<Position> moves;

    protected Game controller;
    protected Board board;

    public class Position
    {
        public readonly ChessPiece.Column col;
        public readonly int row;
        
        public Position(ChessPiece.Column col, int row)
        {
            this.col = col;
            this.row = row;
        }

        public override string ToString()
        {
            return col + row.ToString();
        }
    }

    public static Color OpositeColor(Color color)
    {
        if (color == Color.WHITE)
            return Color.BLACK;

        return Color.WHITE;
    }

    public abstract void Initialize(Position position, Color color, Board board);
    protected abstract void SetSight();

    void Awake()
    {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
    }

    protected void Initialize(Position position, Symbol symbol, float value, Color color, Board board)
    {
        this.SetBoardPosition(position);
        this.symbol = symbol;
        this.value = value;
        this.color = color;
        this.board = board;

        switch (this.color)
        {
            case Color.WHITE:
                this.GetComponent<SpriteRenderer>().sprite = white_sprite;
                break;
            case Color.BLACK:
                this.GetComponent<SpriteRenderer>().sprite = black_sprite;
                break;
        }

        this.gameObject.AddComponent<BoxCollider2D>();
    }

    public void SetBoardPosition(Position position)
    {
        this.position = position;
    }

    protected bool CanSeeThrough(int col, int row)
    {
        ChessPiece.Position testing = controller.IndexToPosition(col, row);
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
        moves = new List<Position>();
        SetSight();

        foreach (ChessPiece.Position pos in sight)
        {
            if (!board.IsValidMove(this, pos))
            {
                continue;
            }

            ChessPiece capture = board.GetOnPosition(pos);
            if (capture == null || (capture.color != this.color && capture.GetType() != typeof(King)))
            {
                moves.Add(pos);
            }
        }
    }

    public override string ToString()
    {
        return color + " " + symbol + " in " + position;
    }

    private void OnMouseDown()
    {
        PieceClicked?.Invoke(this, null);
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

    public List<Position> GetMoves()
    {
        SetMoves();
        return this.moves;
    }
}

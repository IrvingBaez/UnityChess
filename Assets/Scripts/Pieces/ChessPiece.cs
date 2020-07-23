using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor;
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
    private Board board;

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

    public abstract void Initialize(Position position, Color color);
    public abstract void SetSight();
    public abstract void SetMoves();

    void Awake()
    {
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
        this.board = controller.GetBoard();
    }

    protected void Initialize(Position position, Symbol symbol, float value, Color color)
    {
        this.SetBoardPosition(position);
        this.symbol = symbol;
        this.value = value;
        this.color = color;

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
        transform.position = board.SolveWorldPosition(position);
        transform.position = new Vector3(transform.position.x, transform.position.y, -0.5f);
    }

    private void OnMouseDown()
    {
        this.SetMoves();
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
        print(moves.Count);
        return this.moves;
    }
}

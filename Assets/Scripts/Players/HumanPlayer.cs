using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayer : ChessPlayer
{
    public event System.Action pieceSelected;

    public Game controller;
    public MoveTile tile;
    private ChessPiece selected;

    public override void Move()
    {
        ChessPiece.PieceClicked += ChessPiece_PieceClicked;
    }

    private void ChessPiece_PieceClicked(object sender, System.EventArgs e)
    {
        ChessPiece clicked = (ChessPiece)sender;

        if (clicked.GetColor() != this.color)
        {
            return;
        }

        this.selected = clicked;
        pieceSelected?.Invoke();

        foreach(ChessPiece.Position pos in this.selected.GetMoves())
        {
            MoveTile newTile = Instantiate(tile);
            newTile.Initialize(this, new ChessPiece.Position(pos.col, pos.row));
        }
    }

    public void SetDestiny(ChessPiece.Position position)
    {
        this.controller.Move(selected, position);
        this.selected = null;
        ChessPiece.PieceClicked -= ChessPiece_PieceClicked;
        RaisePlayerMoved();
    }
}

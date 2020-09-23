using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPieceView : MonoBehaviour
{
    public event System.Action OnPieceClick;

    private ChessPiece piece;
    private BoardView boardView;

    public void Initialize(Sprite sprite, ChessPiece piece, BoardView boardView)
    {
        if (piece.board.isCopy)
        {
            Debug.Log("A copied pieces has a view!");
        }

        this.piece = piece;
        this.boardView = boardView;
        this.boardView.DrawingBoard += DrawingNewBoard;

        transform.localScale *= 0.6f;
        GetComponent<SpriteRenderer>().sprite = sprite;
        GetComponent<BoxCollider2D>().size = sprite.bounds.size;
    }

    private void OnMouseDown()
    {
        boardView.NotifyClick(piece);
    }

    private void DrawingNewBoard()
    {
        this.boardView.DrawingBoard -= DrawingNewBoard;
        Destroy(gameObject);
        Destroy(this);
    }
}

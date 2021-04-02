using UnityEngine;
using System.Collections;

public class PieceView : MonoBehaviour
{
    public event System.Action OnPieceClick;

    private BoardView boardView;
    private int col;
    private int row;

    public void Initialize(Sprite sprite, int col, int row, BoardView boardView)
    {
        this.boardView = boardView;
        boardView.DrawingBoard += DestroyThis;
        this.col = col;
        this.row = row;

        transform.localScale *= 0.6f;
        GetComponent<SpriteRenderer>().sprite = sprite;
        GetComponent<BoxCollider2D>().size = sprite.bounds.size;
    }

    private void DestroyThis()
    {
        boardView.DrawingBoard -= DestroyThis;
        Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        boardView.NotifyClick(col, row);
    }
}

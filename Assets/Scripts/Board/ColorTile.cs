using UnityEngine;
using System.Collections;

public class ColorTile : MonoBehaviour
{
    BoardView boardView;

    public void Initialize(Board.Position position, BoardView boardView, Color color)
    {
        this.boardView = boardView;
        boardView.PieceClicked += DestroyColorTiles;
        boardView.DrawingBoard += DestroyColorTiles;

        GetComponent<SpriteRenderer>().color = color;
        gameObject.transform.position = boardView.SolveWorldPosition(position);
        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, -1);
    }

    public void DestroyColorTiles()
    {
        boardView.PieceClicked -= DestroyColorTiles;
        boardView.DrawingBoard -= DestroyColorTiles;
        Destroy(gameObject);
    }
}

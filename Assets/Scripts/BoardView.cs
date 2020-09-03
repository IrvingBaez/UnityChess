using UnityEngine;
using System.Collections;
using System;

public class BoardView : MonoBehaviour
{
    private Board board;
    private bool flipped;

    public void setBoard(Board board)
    {
        this.board = board;
        board.OnBoardChanged += drawBoard;
        drawBoard();
    }

    private void drawBoard()
    {
        foreach (ChessPiece piece in board.getWhitePieces())
        {
            piece.transform.position = SolveWorldPosition(piece.GetPosition());
            piece.transform.position = new Vector3(piece.transform.position.x, piece.transform.position.y, -0.5f);
        }

        foreach (ChessPiece piece in board.getBlackPieces())
        {
            try
            {
                piece.transform.position = SolveWorldPosition(piece.GetPosition());
                piece.transform.position = new Vector3(piece.transform.position.x, piece.transform.position.y, -0.5f);
            }
            catch (MissingReferenceException e)
            {
                continue;
            }
        }
    }

    public Vector3 SolveWorldPosition(ChessPiece.Position boardPosition)
    {
        Vector3 deface = transform.parent.position;
        Vector3 size = this.GetComponent<Renderer>().bounds.size;

        float tileXSize = size.x / 8;
        float tileYSize = size.x / 8;

        Vector3 origin = deface;
        origin += (3.5f * tileXSize * Vector3.left);
        origin += (3.5f * tileYSize * Vector3.down);

        Vector3 position = origin;
        position += (boardPosition.row - 1) * tileXSize * Vector3.up;
        position += ((int)boardPosition.col) * tileYSize * Vector3.right;

        if (flipped)
        {
            position = 2 * deface - position;
        }

        return position + Vector3.back;
    }

    public void flip()
    {
        this.flipped = !flipped;
    }

    public Board getBoard()
    {
        return board;
    }
}

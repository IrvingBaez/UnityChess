using UnityEngine;

public abstract class ChessPlayer : MonoBehaviour
{
    public Game game;
    protected Board board;
    protected ChessPiece.Color color;

    public abstract void Move();

    public void SetBoard(Board board)
    {
        this.board = board;
    }

    public void SetColor(ChessPiece.Color color)
    {
        this.color = color;
    }

    public ChessPiece.Color GetColor()
    {
        return this.color;
    }
}

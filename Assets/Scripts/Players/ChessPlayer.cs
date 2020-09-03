using UnityEngine;
using System.Collections;

public abstract class ChessPlayer : MonoBehaviour
{
    public event System.Action PlayerMoved;

    protected ChessPiece.Color color;

    public abstract void Move();
    public void SetColor(ChessPiece.Color color)
    {
        this.color = color;
    }

    public ChessPiece.Color GetColor()
    {
        return this.color;
    }

    protected void RaisePlayerMoved()
    {
        PlayerMoved?.Invoke();
    }
}
